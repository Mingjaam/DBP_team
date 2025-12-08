using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBP_team
{
    public partial class AddressSearchForm : Form
    {
        public string SelectedPostalCode { get; private set; }
        public string SelectedAddress { get; private set; }
        private const string JusoConfmKey = "devU01TX0FVVEgyMDI1MTEwNjE1MzQwOTExNjQxMTc=";

        // 누락된 컨트롤 필드 추가
        private TextBox txtQuery;
        private Button btnSearch;
        private ListBox listResults;
        private Label lblStatus;
        private Button btnOk;
        private Button btnCancel;

        public AddressSearchForm(string initialQuery = null)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(initialQuery)) this.txtQuery.Text = initialQuery;
        }

        // 최소한의 InitializeComponent 구현
        private void InitializeComponent()
        {
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.listResults = new System.Windows.Forms.ListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(12, 12);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(360, 21);
            this.txtQuery.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(378, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "검색";
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // listResults
            // 
            this.listResults.ItemHeight = 12;
            this.listResults.Location = new System.Drawing.Point(12, 39);
            this.listResults.Name = "listResults";
            this.listResults.Size = new System.Drawing.Size(441, 196);
            this.listResults.TabIndex = 2;
            this.listResults.DoubleClick += new System.EventHandler(this.ListResults_DoubleClick);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(12, 245);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(300, 23);
            this.lblStatus.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(297, 274);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "확인";
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(378, 274);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "취소";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // AddressSearchForm
            // 
            this.ClientSize = new System.Drawing.Size(465, 309);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.listResults);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Name = "AddressSearchForm";
            this.Text = "주소 검색";
            this.Load += new System.EventHandler(this.AddressSearchForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            await DoSearchAsync();
        }

        private void ListResults_DoubleClick(object sender, EventArgs e)
        {
            AcceptSelection();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            AcceptSelection();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AcceptSelection()
        {
            if (this.listResults.SelectedItem == null) return;
            var item = this.listResults.SelectedItem as AddressResult;
            if (item == null) return;
            SelectedPostalCode = item.PostalCode;
            SelectedAddress = item.Address;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private async Task DoSearchAsync()
        {
            var q = this.txtQuery.Text?.Trim();
            if (string.IsNullOrWhiteSpace(q))
            {
                MessageBox.Show("검색어를 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.lblStatus.Text = "검색 중...";
                this.btnSearch.Enabled = false;
                this.listResults.Items.Clear();

                var results = await SearchAddressAsync(q);
                foreach (var r in results)
                {
                    this.listResults.Items.Add(r);
                }

                this.lblStatus.Text = results.Count == 0 ? "결과가 없습니다." : $"결과 {results.Count}건";
            }
            catch (Exception ex)
            {
                MessageBox.Show("주소 검색 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.lblStatus.Text = "검색 실패";
            }
            finally
            {
                this.btnSearch.Enabled = true;
            }
        }

        private async Task<List<AddressResult>> SearchAddressAsync(string query)
        {
            var list = new List<AddressResult>();

            using (var http = new HttpClient())
            {
                var url = "https://www.juso.go.kr/addrlink/addrLinkApi.do?confmKey=" + Uri.EscapeDataString(JusoConfmKey)
                          + "&currentPage=1&countPerPage=20&keyword=" + Uri.EscapeDataString(query) + "&resultType=json";

                var resp = await http.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                {
                    var txt = await resp.Content.ReadAsStringAsync();
                    throw new Exception($"API error: {resp.StatusCode} - {txt}");
                }

                var json = await resp.Content.ReadAsStringAsync();

                var jusoArrayPos = json.IndexOf("\"juso\":", StringComparison.OrdinalIgnoreCase);
                if (jusoArrayPos >= 0)
                {
                    var objPattern = new Regex("\\{[^}]*\\}", RegexOptions.Multiline);
                    var roadPattern = new Regex("\"roadAddr\"\\s*:\\s*\"([^\"]*)\"", RegexOptions.Compiled);
                    var jibunPattern = new Regex("\"jibunAddr\"\\s*:\\s*\"([^\"]*)\"", RegexOptions.Compiled);
                    var zipPattern = new Regex("\"zipNo\"\\s*:\\s*\"([^\"]*)\"", RegexOptions.Compiled);

                    var arrStart = json.IndexOf('[', jusoArrayPos);
                    if (arrStart >= 0)
                    {
                        var arrEnd = json.IndexOf(']', arrStart);
                        if (arrEnd > arrStart)
                        {
                            var arrContent = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
                            foreach (Match m in objPattern.Matches(arrContent))
                            {
                                var objText = m.Value;
                                string addr = null;
                                string zip = null;
                                var rm = roadPattern.Match(objText);
                                if (rm.Success) addr = rm.Groups[1].Value;
                                var jm = jibunPattern.Match(objText);
                                if (jm.Success && string.IsNullOrWhiteSpace(addr)) addr = jm.Groups[1].Value;
                                var zm = zipPattern.Match(objText);
                                if (zm.Success) zip = zm.Groups[1].Value;

                                if (!string.IsNullOrWhiteSpace(addr))
                                {
                                    list.Add(new AddressResult { PostalCode = zip ?? string.Empty, Address = addr });
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        private class AddressResult
        {
            public string PostalCode { get; set; }
            public string Address { get; set; }
            public override string ToString() => $"[{PostalCode}] {Address}";
        }

        private void AddressSearchForm_Load(object sender, EventArgs e)
        {

        }
    }
}