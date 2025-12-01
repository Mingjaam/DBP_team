using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public static class MultiProfileService
    {
        public static void EnsureTables()
        {
            try
            {
                DBManager.Instance.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS multi_profile_map ("+
                    "  id INT AUTO_INCREMENT PRIMARY KEY,\n"+
                    "  owner_user_id INT NOT NULL,\n"+
                    "  target_user_id INT NOT NULL,\n"+
                    "  display_name VARCHAR(255) NULL,\n"+
                    "  photo LONGBLOB NULL,\n"+
                    "  created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,\n"+
                    "  updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,\n"+
                    "  UNIQUE KEY uk_owner_target (owner_user_id, target_user_id),\n"+
                    "  INDEX idx_owner (owner_user_id),\n"+
                    "  INDEX idx_target (target_user_id)\n"+
                    ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");
            }
            catch { /* ignore create errors */ }
        }

        // 목록: 대상자 이름 포함(회사 사용자 이름과 조인)
        public static DataTable GetMappings(int ownerUserId)
        {
            EnsureTables();
            var sql = "SELECT mpm.id, mpm.display_name, mpm.target_user_id, COALESCE(u.full_name,u.email) AS target_name, mpm.created_at, mpm.updated_at " +
                      "FROM multi_profile_map mpm LEFT JOIN users u ON u.id = mpm.target_user_id " +
                      "WHERE mpm.owner_user_id = @owner ORDER BY mpm.updated_at DESC, mpm.created_at DESC";
            return DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@owner", ownerUserId));
        }

        public static DataTable GetCompanyUsersExceptOwner(int ownerUserId)
        {
            var dt = DBManager.Instance.ExecuteDataTable("SELECT company_id FROM users WHERE id = @id", new MySqlParameter("@id", ownerUserId));
            int companyId = 0;
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value) companyId = Convert.ToInt32(dt.Rows[0][0]);
            if (companyId <= 0) return new DataTable();

            var sql = "SELECT id, COALESCE(full_name, email) AS name FROM users WHERE company_id = @cid AND id <> @owner ORDER BY name";
            return DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@cid", companyId), new MySqlParameter("@owner", ownerUserId));
        }

        public static (string DisplayName, byte[] Photo, int TargetUserId) GetMapping(int mappingId)
        {
            EnsureTables();
            var dt = DBManager.Instance.ExecuteDataTable("SELECT display_name, photo, target_user_id FROM multi_profile_map WHERE id = @id", new MySqlParameter("@id", mappingId));
            if (dt != null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                string name = row["display_name"] == DBNull.Value ? null : row["display_name"].ToString();
                byte[] photo = row["photo"] == DBNull.Value ? null : (byte[])row["photo"];
                int target = row["target_user_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["target_user_id"]);
                return (name, photo, target);
            }
            return (null, null, 0);
        }

        // mappingId가 null이면 선택된 대상자마다 새 매핑 생성(중복은 업데이트), 있으면 해당 매핑을 기준으로 저장
        public static void SaveMapping(int ownerUserId, int? mappingId, string displayName, byte[] photoBytes, IEnumerable<int> selectedTargets)
        {
            EnsureTables();
            if (selectedTargets == null) selectedTargets = new int[0];

            // helper upsert per target
            Action<int> upsert = targetId =>
            {
                try
                {
                    // try insert first (if unique violation, fall back to update)
                    var inserted = DBManager.Instance.ExecuteNonQuery(
                        "INSERT INTO multi_profile_map(owner_user_id, target_user_id, display_name, photo) VALUES(@owner, @target, @name, @photo)",
                        new MySqlParameter("@owner", ownerUserId),
                        new MySqlParameter("@target", targetId),
                        new MySqlParameter("@name", string.IsNullOrWhiteSpace(displayName) ? (object)DBNull.Value : displayName),
                        new MySqlParameter("@photo", photoBytes == null ? (object)DBNull.Value : photoBytes));
                }
                catch
                {
                    // on duplicate, update
                    DBManager.Instance.ExecuteNonQuery(
                        "UPDATE multi_profile_map SET display_name = @name, photo = @photo WHERE owner_user_id = @owner AND target_user_id = @target",
                        new MySqlParameter("@owner", ownerUserId),
                        new MySqlParameter("@target", targetId),
                        new MySqlParameter("@name", string.IsNullOrWhiteSpace(displayName) ? (object)DBNull.Value : displayName),
                        new MySqlParameter("@photo", photoBytes == null ? (object)DBNull.Value : photoBytes));
                }
            };

            if (mappingId == null)
            {
                foreach (var target in selectedTargets)
                {
                    upsert(target);
                }
                return;
            }

            // existing mapping row -> may change to multiple targets; update/insert for selected; if original target removed, delete it
            var existing = GetMapping(mappingId.Value);
            var selectedSet = new HashSet<int>(selectedTargets);

            foreach (var target in selectedSet)
            {
                upsert(target);
            }

            if (existing.TargetUserId != 0 && !selectedSet.Contains(existing.TargetUserId))
            {
                try
                {
                    DBManager.Instance.ExecuteNonQuery("DELETE FROM multi_profile_map WHERE id = @id AND owner_user_id = @owner",
                        new MySqlParameter("@id", mappingId.Value), new MySqlParameter("@owner", ownerUserId));
                }
                catch { }
            }
        }

        public static void DeleteMapping(int ownerUserId, int mappingId)
        {
            EnsureTables();
            DBManager.Instance.ExecuteNonQuery("DELETE FROM multi_profile_map WHERE id = @id AND owner_user_id = @owner",
                new MySqlParameter("@id", mappingId), new MySqlParameter("@owner", ownerUserId));
        }

        public static string GetDisplayNameForViewer(int ownerUserId, int viewerUserId)
        {
            EnsureTables();
            var sql = "SELECT COALESCE(mpm.display_name, u.full_name, u.email) AS name " +
                      "FROM users u " +
                      "LEFT JOIN multi_profile_map mpm ON mpm.owner_user_id = u.id AND mpm.target_user_id = @viewer " +
                      "WHERE u.id = @owner";
            var obj = DBManager.Instance.ExecuteScalar(sql, new MySqlParameter("@viewer", viewerUserId), new MySqlParameter("@owner", ownerUserId));
            return obj == null || obj == DBNull.Value ? null : obj.ToString();
        }

        public static byte[] GetProfileImageForViewer(int ownerUserId, int viewerUserId)
        {
            EnsureTables();
            var sql = "SELECT COALESCE(mpm.photo, u.profile_image) AS photo " +
                      "FROM users u " +
                      "LEFT JOIN multi_profile_map mpm ON mpm.owner_user_id = u.id AND mpm.target_user_id = @viewer " +
                      "WHERE u.id = @owner";
            var dt = DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@viewer", viewerUserId), new MySqlParameter("@owner", ownerUserId));
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                return (byte[])dt.Rows[0][0];
            return null;
        }
    }
}
