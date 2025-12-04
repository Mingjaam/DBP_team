using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBP_team
{
    public class AddressSearchForm : Form
    {
        private TextBox txtQuery;
        private Button btnSearch;
        private ListBox listResults;
        private Label lblStatus;
        private Button btnOk;
        private Button btnCancel;
        private Panel pnlTop;

        public string SelectedPostalCode { get; private set; }
        public string SelectedAddress { get; private set; }

        private const string JusoConfmKey = "devU01TX0FVVEgyMDI1MTEwNjE1MzQwOTExNjQxMTc=";

        public AddressSearchForm(string initialQuery = null)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(initialQuery)) txtQuery.Text = initialQuery;
        }

        private void InitializeComponent()
        {
            this.Text = "ÁÖ¼Ò °Ë»ö";
            this.ClientSize = new Size(600, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("¸¼Àº °íµñ", 9F);

            pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(15)
            };

            var lblTitle = new Label
            {
                Text = "ÁÖ¼Ò °Ë»ö",
                Font = new Font("¸¼Àº °íµñ", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                Location = new Point(15, 10),
                AutoSize = true
            };

            txtQuery = new TextBox
            {
                Location = new Point(15, 38),
                Size = new Size(450, 25),
                Font = new Font("¸¼Àº °íµñ", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnSearch = new Button
            {
                Location = new Point(475, 36),
                Size = new Size(100, 29),
                Text = "°Ë»ö",
                Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(74, 144, 226),
                ForeColor = Color.White
            };
            btnSearch.FlatAppearance.BorderSize = 0;

            listResults = new ListBox
            {
                Location = new Point(15, 85),
                Size = new Size(570, 300),
                Font = new Font("¸¼Àº °íµñ", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblStatus = new Label
            {
                Location = new Point(15, 390),
                Size = new Size(570, 20),
                Text = "",
                Font = new Font("¸¼Àº °íµñ", 9F),
                ForeColor = Color.FromArgb(120, 120, 120)
            };

            btnOk = new Button
            {
                Location = new Point(395, 415),
                Size = new Size(90, 32),
                Text = "È®ÀÎ",
                Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(74, 144, 226),
                ForeColor = Color.White
            };
            btnOk.FlatAppearance.BorderSize = 0;

            btnCancel = new Button
            {
                Location = new Point(495, 415),
                Size = new Size(90, 32),
                Text = "Ãë¼Ò",
                Font = new Font("¸¼Àº °íµñ", 9F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnSearch.Click += BtnSearch_Click;
            listResults.DoubleClick += ListResults_DoubleClick;
            btnOk.Click += BtnOk_Click;
            btnCancel.Click += BtnCancel_Click;

            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(txtQuery);
            pnlTop.Controls.Add(btnSearch);

            this.Controls.Add(pnlTop);
            this.Controls.Add(listResults);
            this.Controls.Add(lblStatus);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
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
            if (listResults.SelectedItem == null) return;
            var item = listResults.SelectedItem as AddressResult;
            if (item == null) return;
            SelectedPostalCode = item.PostalCode;
            SelectedAddress = item.Address;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private async Task DoSearchAsync()
        {
            var q = txtQuery.Text?.Trim();
            if (string.IsNullOrWhiteSpace(q))
            {
                MessageBox.Show("°Ë»ö¾î¸¦ ÀÔ·ÂÇÏ¼¼¿ä.", "ÀÔ·Â ¿À·ù", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                lblStatus.Text = "°Ë»ö Áß...";
                btnSearch.Enabled = false;
                listResults.Items.Clear();

                var results = await SearchAddressAsync(q);
                foreach (var r in results)
                {
                    listResults.Items.Add(r);
                }

                lblStatus.Text = results.Count == 0 ? "°á°ú°¡ ¾ø½À´Ï´Ù." : $"°á°ú {results.Count}°Ç";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ÁÖ¼Ò °Ë»ö ¿À·ù: " + ex.Message, "¿À·ù", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "°Ë»ö ½ÇÆÐ";
            }
            finally
            {
                btnSearch.Enabled = true;
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
    }
}