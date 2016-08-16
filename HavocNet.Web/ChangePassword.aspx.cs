using System;
using System.Drawing;
using Core.Repositories;
using Phoenix.Core.Servers;

namespace HavocNet.Web
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPageNoAuth("");
            _myMaster.ClearMenus();

            this.cmdChange.Click += cmdChange_click;
        }

        private void cmdChange_click(object sender, EventArgs e)
        {
            var aRepository = new UserRepository(new HavocServer());

            if (Request["ctl00$cphMain$txtNewPassword"] != "" && Request["ctl00$cphMain$txtNewPasswordConfirm"] != "" &&
                Request["ctl00$cphMain$txtOldPassword"] != "")
            {
                if (Request["ctl00$cphMain$txtNewPassword"] == Request["ctl00$cphMain$txtNewPasswordConfirm"])
                {
                    if (aRepository.TryGet(_myMaster.aUser.Username, Request["ctl00$cphMain$txtOldPassword"], out _myMaster.aUser))
                    {
                        aRepository.ResetPassword(_myMaster.aUser.ID, Request["ctl00$cphMain$txtNewPassword"]);

                        Response.Redirect(_myMaster.aUser.HomePage);
                    }
                    else
                    {
                        this.lblStatus.Text = "Old password is incorrect.";
                    }
                }
                else
                {
                    this.lblStatus.Text = "New passwords must be identical.";
                }
            }
            else
            {
                this.lblStatus.Text = "Passwords cannot be blank.";
            }
        }
    }
}