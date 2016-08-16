using System;
using Core;
using Core.Repositories;
using Phoenix.Core.Servers;

namespace HavocNet.Web
{
    public partial class Login : System.Web.UI.Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPageNoAuth("");
            _myMaster.ClearMenus();

            this.cmdSignIn.Click += cmdSignIn_Click;

            if(_myMaster.aUser.Username != "public") Response.Redirect("Home.aspx");
        }

        private void cmdSignIn_Click(object sender, EventArgs e)
        {
            var aRepository = new UserRepository(new HavocServer());

            if (Request["ctl00$cphMain$txtUsername"] != "" && Request["ctl00$cphMain$txtPassword"] != "")
            {
                if (aRepository.TryGet(Request["ctl00$cphMain$txtUsername"], Request["ctl00$cphMain$txtPassword"], out _myMaster.aUser))
                {
                    if (_myMaster.aUser.MustChangePassword)
                    {
                        Session.Clear();
                        Session.Add("HavocNetUser", _myMaster.aUser);

                        Response.Redirect("ChangePassword.aspx");
                    }
                    else
                    {
                        Session.Clear();
                        Session.Add("HavocNetUser", _myMaster.aUser);

                        Response.Redirect(_myMaster.aUser.HomePage);
                    }
                }
            }
            
            this.lblStatus.Text = @"The username and password combination is invalid.";
        }
    }
}