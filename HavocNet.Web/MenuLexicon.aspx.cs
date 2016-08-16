using System;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Phoenix.Core.Servers;

namespace HavocNet.Web
{
    public partial class MenuLexicon : Page
    {
        private HavocNetMaster _myMaster;
        private int _maxLength;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            this.results.Visible = false;

            BindDictionaries();

            this.cmdRun.Click+=cmdRun_Click;
        }

        protected void cmdRun_Click(object sender, EventArgs eventArgs)
        {
            var aServer = new HavocServer();
            aServer.Open();

            this.phResults.Controls.Clear();
            this.phResults.Controls.Add(MakeAnalysis(aServer));

            this.phStats.Controls.Clear();
            this.phStats.Controls.Add(MakeStats(((Table)this.phResults.Controls[0]).Rows.Count));

            aServer.Close();

            this.results.Visible = true;
        }

        private Table MakeAnalysis(HavocServer aServer)
        {
            var tbl = new Table();

            var aWord = Request["ctl00$cphMain$txtLetterOptions"].ToUpper();
            int[] aCode;
            var strSQL = "";
            int i;
            var wildcards = 0;
            var charArr = aWord.ToCharArray();
            _maxLength = 0;

            aServer.SQLParams.Add(Request["ctl00$cphMain$cboDictionaries"]);

            if (!aWord.Contains("?"))
            {
                aCode = CreateCode(aWord);

                if (!string.IsNullOrEmpty(aWord))
                {
                    for (i = 0; i <= 25; i++)
                    {
                        aServer.SQLParams.Add(aCode[i]);
                        if (this.rdoSetType.SelectedValue == "Sub")
                        {
                            strSQL += " AND " + ((char)(i + 65)).ToString(CultureInfo.InvariantCulture).ToUpper() + " <= @Param" + (i + 1);
                        }
                        else
                        {
                            strSQL += " AND " + ((char)(i + 65)).ToString(CultureInfo.InvariantCulture).ToUpper() + " >= @Param" + (i + 1);
                        }

                    }
                }

                strSQL = "SELECT Word,Score FROM dbo.tblWords WHERE DictionaryID = @Param0" + strSQL + MakeInputMask() + " ORDER BY " + rdoOrderBy.SelectedValue + " DESC, Word";
            }
            else
            {
                int j;
                for (j = 0; j <= aWord.Length - 1; j++)
                {
                    if (charArr[j] == '?')
                        wildcards += 1;
                }

                aCode = CreateCode(aWord.Replace("?", ""));

                string strSQL2;
                int k;
                if (wildcards == 1)
                {
                    for (i = 0; i <= 25; i++)
                    {
                        strSQL2 = "";
                        for (k = 0; k <= 25; k++)
                        {
                            var adj = k == i ? 1 : 0;
                            if (this.rdoSetType.SelectedValue == "Sub")
                            {
                                strSQL2 += " AND " + ((char)(k + 65)).ToString(CultureInfo.InvariantCulture).ToUpper() + " <= " + aCode[k] + adj;
                            }
                            else
                            {
                                strSQL2 += " AND " + ((char)(k + 65)).ToString(CultureInfo.InvariantCulture).ToUpper() + " >= " + aCode[k] + adj;
                            }
                        }
                        strSQL += " OR (" + strSQL2.Substring(5) + ")";
                    }
                }
                else
                {
                    for (j = 0; j <= 25; j++)
                    {
                        strSQL2 = "";
                        for (i = 0; i <= 25; i++)
                        {
                            var strSQL3 = "";
                            for (k = 0; k <= 25; k++)
                            {
                                var adj = k == i ? 1 : 0;
                                var adj1 = k == j ? 1 : 0;

                                if (this.rdoSetType.SelectedValue == "Sub")
                                {
                                    strSQL3 += " AND " + ((char)(k + 65)).ToString(CultureInfo.InvariantCulture).ToUpper() + " <= " + aCode[k] + adj + adj1;
                                }
                                else
                                {
                                    strSQL3 += " AND " + ((char)(k + 65)).ToString(CultureInfo.InvariantCulture).ToUpper() + " >= " + aCode[k] + adj + adj1;
                                }
                            }
                            strSQL2 += " OR (" + strSQL3.Substring(5) + ")";
                        }
                        strSQL += " OR " + strSQL2.Substring(4) + "";
                    }
                }

                strSQL = "SELECT * FROM dbo.tblWords WHERE Length > 4 AND DictionaryID = @Param0 AND (" + strSQL.Substring(4) + ") " + MakeInputMask() + " ORDER BY " + this.rdoOrderBy.SelectedValue + " DESC, Word";
            }

            tbl.Rows.Add(MakeHeader());

            var aReader = aServer.ExecuteReader(strSQL);

            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    tbl.Rows.Add(MakeRow(aReader));

                    if (aReader["Word"].ToString().Length > _maxLength)
                        _maxLength = aReader["Word"].ToString().Length;
                }
            }
            aReader.Close();

            return tbl;
        }

        private Table MakeStats(int wordCount)
        {
            var tbl = new Table();

            var aRow = new TableRow();
            aRow.Cells.Add(new TableCell {Text = "Count"});
            var aCell2 = new TableCell();
            aCell2.Style["padding-left"] = "4px";
            var txt = new TextBox {Width = 50, Text = wordCount.ToString(CultureInfo.InvariantCulture), ReadOnly = true};
            txt.Style["text-align"] = "center";
            aCell2.Controls.Add(txt);
            aRow.Cells.Add(aCell2);

            var aCell21 = new TableCell {Text = "Max Length"};
            aCell21.Style["padding-left"] = "10px";
            aRow.Cells.Add(aCell21);
            var aCell22 = new TableCell();
            aCell22.Style["padding-left"] = "4px";
            var txt2 = new TextBox {Width = 50, Text = _maxLength.ToString(CultureInfo.InvariantCulture), ReadOnly = true};
            txt2.Style["text-align"] = "center";
            aCell22.Controls.Add(txt2);
            aRow.Cells.Add(aCell22);

            tbl.Rows.Add(aRow);

            return tbl;
        }

        private TableRow MakeHeader()
        {
            var aRow = new TableRow {BackColor = Color.PaleGoldenrod};

            var aCell1 = new TableCell
                {
                    Text = "Word",
                    BorderColor = Color.Gray,
                    BorderStyle = BorderStyle.Solid,
                    BorderWidth = 1,
                    Width = 100,
                    Font = {Bold = true}
                };
            aCell1.Style["padding"] = "5px";
            aRow.Cells.Add(aCell1);

            var aCell2 = new TableCell
                {
                    Text = "Score",
                    BorderColor = Color.Gray,
                    BorderStyle = BorderStyle.Solid,
                    BorderWidth = 1,
                    Width = 100,
                    HorizontalAlign = HorizontalAlign.Center,
                    Font = {Bold = true}
                };
            aCell2.Style["padding"] = "5px";
            aRow.Cells.Add(aCell2);

            var aCell3 = new TableCell
            {
                Text = "Length",
                BorderColor = Color.Gray,
                BorderStyle = BorderStyle.Solid,
                BorderWidth = 1,
                Width = 100,
                HorizontalAlign = HorizontalAlign.Center,
                Font = { Bold = true }
            };
            aCell3.Style["padding"] = "5px";
            aRow.Cells.Add(aCell3);

            return aRow;
        }

        private TableRow MakeRow(DbDataReader aReader)
        {
            var aRow = new TableRow();

            var aCell1 = new TableCell
                {
                    Text = aReader["Word"].ToString(),
                    BorderColor = Color.Gray,
                    BorderStyle = BorderStyle.Solid,
                    BorderWidth = 1
                };
            aCell1.Style["padding"] = "5px";
            aRow.Cells.Add(aCell1);

            var aCell2 = new TableCell
                {
                    Text = aReader["Score"].ToString(),
                    BorderColor = Color.Gray,
                    BorderStyle = BorderStyle.Solid,
                    BorderWidth = 1,
                    HorizontalAlign = HorizontalAlign.Center
                };
            aCell2.Style["padding"] = "5px";
            aRow.Cells.Add(aCell2);

            var aCell3 = new TableCell
            {
                Text = aReader["Word"].ToString().Length.ToString(""),
                BorderColor = Color.Gray,
                BorderStyle = BorderStyle.Solid,
                BorderWidth = 1,
                HorizontalAlign = HorizontalAlign.Center
            };
            aCell3.Style["padding"] = "5px";
            aRow.Cells.Add(aCell3);

            return aRow;
        }

        private int[] CreateCode(string aWord)
        {
            var aArr = new int[26];

            foreach (var aChr in aWord)
            {
                aArr[aChr - 65] += 1;
            }

            return aArr;
        }

        private void BindDictionaries()
        {
            var aServer = new HavocServer();
            aServer.Open();

            var aReader = aServer.ExecuteReader("SELECT DictionaryID, DictionaryName, DictionaryDescription FROM dbo.tblDictionaries WHERE Loaded = 1 ORDER BY Ordering");
            this.cboDictionaries.DataSource = aReader;
            this.cboDictionaries.DataValueField = "DictionaryID";
            this.cboDictionaries.DataTextField = "DictionaryName";
            this.cboDictionaries.DataBind();

            aReader.Close();

            if (!Page.IsPostBack)
            {
                this.cboDictionaries.Items[0].Selected = true;
            }
            else
            {
                if (this.cboDictionaries.Items.FindByValue(Request["ctl00$cphMain$cboDictionaries"]) == null)
                {
                    this.cboDictionaries.Items[0].Selected = true;
                }
                else
                {
                    this.cboDictionaries.Items.FindByValue(Request["ctl00$cphMain$cboDictionaries"]).Selected = true;
                }
            }

            aServer.Close();
        }

        private string MakeInputMask()
        {
            var inputMask = "";

            if (ValidateInput(Request["ctl00$cphMain$txtInputMask"]))
            {
                inputMask = " AND Word LIKE '" + Request["ctl00$cphMain$txtInputMask"] + "' ";
            }

            return inputMask;
        }

        private bool ValidateInput(string inputMask)
        {
            var aServer = new HavocServer();
            aServer.Open();

            if (inputMask.Contains(" "))
            {
                this.lblStatus.Text = "Input mask not valid SQL. SQL Injection detected.";

                return false;
            }

            try
            {
                aServer.ExecuteScalar("SELECT TOP 1 * FROM dbo.tblWords WHERE Word LIKE '" + inputMask + "'");
                aServer.Close();
            }
            catch (Exception ex)
            {
                this.lblStatus.Text = "Input mask not valid SQL. The query was run without";
                aServer.Close();

                return false;
            }

            return !string.IsNullOrEmpty(inputMask);
        }
    }
}