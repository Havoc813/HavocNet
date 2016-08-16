using System;
using System.Web.UI;
using Phoenix.Core;

namespace HavocNet.Web.Administration
{
    public partial class ConnectionStrings : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainAdministration");

            LoadPage();
        }

        private void LoadPage()
        {
            this.txtResponse.Visible = false;

            this.cmdDecrypt.Click += cmdDecrypt_Click;
            this.cmdDecrypt.Visible = _myMaster.WriteAccess();
            this.cmdEncrypt.Click += cmdEncrypt_Click;
        }

        void cmdEncrypt_Click(object sender, EventArgs e)
        {
            this.txtResponse.Text = FSEncrypt.Encrypt(this.txtConnectionString.Text);
            this.txtResponse.Visible = true;
        }

        void cmdDecrypt_Click(object sender, EventArgs e)
        {
            this.txtResponse.Text = FSEncrypt.Decrypt(this.txtConnectionString.Text);
            this.txtResponse.Visible = true;
        }
    }
}