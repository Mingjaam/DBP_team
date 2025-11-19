using DBP_team.Models;

namespace DBP_team
{
    public static class AdminGuard
    {
        public static bool IsAdmin(User user)
        {
            if (user == null) return false;
            if (!string.IsNullOrWhiteSpace(user.Role) && user.Role.Trim().ToLowerInvariant() == "admin")
                return true;
            if (!string.IsNullOrWhiteSpace(user.Email) && user.Email.Trim().ToLowerInvariant() == "admin")
                return true;
            return false;
        }
    }
}
