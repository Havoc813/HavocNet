using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HavocNet.Web.Puzzles
{
    public partial class MorseCode : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            this.cmdMorseToText.Click += cmdConvert_Click;
            this.cmdTextToMorse.Click += cmdConvert2_Click;
        }

        protected void cmdConvert_Click(object sender, EventArgs e)
        {
            var referenceArray = new Dictionary<string, string>();
            var aServer = new HavocServer();
            aServer.Open();

            var aReader = aServer.ExecuteReader("SELECT * FROM dbo.tblCharacterReference WHERE Morse IS NOT NULL ORDER BY Ordering");

            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    referenceArray.Add(aReader["CharacterName"].ToString(), aReader["Morse"].ToString().Replace(" ", ""));
                }
            }

            aReader.Close();
            aServer.Close();

            var aTxtbox = new TextBox
                {
                    Rows = 5,
                    Columns = 95,
                    TextMode = TextBoxMode.MultiLine,
                    BorderColor = Color.Gray,
                    BorderWidth = 1
                };

            var txtPlain = Request["ctl00$cphMain$txtPlainText"];

            for (var i = 0; i <= txtPlain.Length - 1; i++)
            {
                if (referenceArray.ContainsKey(txtPlain.Substring(i, 1)))
                {
                    aTxtbox.Text += referenceArray[txtPlain.Substring(i, 1)] + @" ";
                }
                else if (txtPlain.Substring(i, 1) != " ")
                {
                    aTxtbox.Text += @"? ";
                }
                else
                {
                    aTxtbox.Text += @"/ ";
                }
            }

            this.phMorseCode.Controls.Clear();
            this.phMorseCode.Controls.Add(aTxtbox);
        }

        protected void cmdConvert2_Click(object sender, EventArgs e)
        {
            var referenceArray = new Dictionary<string, string>();
            var aServer = new HavocServer();
            aServer.Open();

            var aReader = aServer.ExecuteReader("SELECT * FROM dbo.tblCharacterReference WHERE Morse IS NOT NULL AND CharacterType <> 'Upper Case Characters' ORDER BY Ordering");

            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    referenceArray.Add(aReader["Morse"].ToString().Replace(" ", ""), aReader["CharacterName"].ToString());
                }
            }

            aReader.Close();
            aServer.Close();

            var aTxtbox = new TextBox
                {
                    Rows = 5,
                    Columns = 95,
                    TextMode = TextBoxMode.MultiLine,
                    BorderColor = Color.Gray,
                    BorderWidth = 1
                };

            foreach (var aWord in Request["ctl00$cphMain$txtMorseCode"].Split('/'))
            {
                foreach (var aLetter in aWord.Split(' '))
                {
                    //Create the letters
                    if (referenceArray.ContainsKey(aLetter))
                    {
                        aTxtbox.Text += referenceArray[aLetter];
                    }
                    else if (!string.IsNullOrEmpty(aLetter))
                    {
                        aTxtbox.Text += @"?";
                    }
                }
                aTxtbox.Text += @" ";
            }

            this.phPlainText.Controls.Clear();
            this.phPlainText.Controls.Add(aTxtbox);
        }
    }
}