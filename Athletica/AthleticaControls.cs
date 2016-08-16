using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Athletica.Repositories;
using Core;
using Phoenix.Core;

namespace Athletica
{
    public class AthleticaControls
    {
        private readonly AthleticaServer _aServer;
        private int _sportID = 0;
        private int _userID;
        private string _sportClass;

        public readonly ArrayList Hiddens = new ArrayList();
        public readonly Dictionary<string, string> Settings = new Dictionary<string, string>();

        public AthleticaControls(User aUser, AthleticaServer aServer, int sportID, string activityClass)
        {
            _userID = aUser.ID;
            _sportID = sportID;
            _sportClass = activityClass;
            
            _aServer = aServer;

            _aServer.SQLParams.Add(aUser.ID);
            _aServer.SQLParams.Add(activityClass);

            const string strSQL = @"SELECT 
                    SettingName, 
                    isnull(SettingValue, SettingDefault) AS SettingValue 
                    FROM 
                    dbo.UserSettings a 
                    RIGHT OUTER JOIN dbo.Settings b ON a.SettingID = b.SettingID 
                    WHERE 
                    UserID = @Param0";
            
            var aReader = _aServer.ExecuteReader(strSQL);

            while (aReader.Read())
            {
                this.Settings.Add(aReader["SettingName"].ToString(), aReader["SettingValue"].ToString());
            }
            aReader.Close();
        }

        public static void BindDate(TextBox txtDate)
        {
            txtDate.Text = HttpContext.Current.Request["ctl00$cphMain$" + txtDate.ID] ?? DateTime.Now.ToString(FSFormat.BasicDateFormat);
        }

        public static void BindTimeOfDay(DropDownList cboTimeOfDay)
        {
            cboTimeOfDay.Items.Clear();
            cboTimeOfDay.Items.Add(new ListItem("Morning"));
            cboTimeOfDay.Items.Add(new ListItem("Afternoon"));
            cboTimeOfDay.Items.Add(new ListItem("Evening"));

            if (HttpContext.Current.Request["ctl00$cphMain$" + cboTimeOfDay] != null)
            {
                cboTimeOfDay.Items.FindByValue(HttpContext.Current.Request["ctl00$cphMain$" + cboTimeOfDay.ID]).Selected = true;
            }
            else
            {
                if (int.Parse(DateTime.Now.ToString("HH")) > 11 && int.Parse(DateTime.Now.ToString("HH")) <= 18) cboTimeOfDay.Items.FindByValue("Afternoon").Selected = true;
                if (int.Parse(DateTime.Now.ToString("HH")) > 18) cboTimeOfDay.Items.FindByValue("Evening").Selected = true;
            }
        }

        public void BindLocation(DropDownList cbo, TextBox newValue)
        {
            const string strSQL = @"SELECT 
                    ActivityID,
                    LocationID, 
                    LocationName
                    FROM 
                    dbo.Location 
                    WHERE 
                    UserID = @Param0
                    AND IsActive = 1
                    ORDER BY 
                    Ordering";

            var locations = new Dictionary<string, string>();
            var aReader = _aServer.ExecuteReader(strSQL);

            cbo.Items.Clear();
            while (aReader.Read())
            {
                if (int.Parse(aReader["ActivityID"].ToString()) == _sportID)
                    cbo.Items.Add(new ListItem(aReader["LocationName"].ToString(), aReader["LocationID"].ToString()));

                if (!locations.ContainsKey(aReader["ActivityID"] + "_location")) 
                    locations.Add(aReader["ActivityID"] + "_location", aReader["LocationID"] + ";" + aReader["LocationName"]);
                else
                    locations[aReader["ActivityID"] + "_location"] += "|" + aReader["LocationID"] + ";" + aReader["LocationName"];
            }
            aReader.Close();

            foreach (var location in locations)
            {
                this.Hiddens.Add(new HiddenField { ID = location.Key, Value = location.Value});
            }

            SetValue(cbo);

            //New Location
            if (HttpContext.Current.Request["ctl00$cphMain$" + newValue.ID] != null && HttpContext.Current.Request["ctl00$cphMain$" + newValue.ID] != "")
            {
                newValue.Text = HttpContext.Current.Request["ctl00$cphMain$" + newValue.ID];
                newValue.Style["display"] = "inline";
            }
            else
            {
                newValue.Style["display"] = "none";
            }
        }

        public void BindRoute(DropDownList cbo)
        { 
            const string strSQL = @"SELECT 
                    RouteID, 
                    RouteName,
                    ActivityID 
                    FROM 
                    dbo.Route 
                    WHERE 
                    UserID = @Param0
                    AND IsActive = 1
                    ORDER BY 
                    Ordering";

            var routes = new Dictionary<string, string>();
            var aReader = _aServer.ExecuteReader(strSQL);

            cbo.Items.Clear();
            while (aReader.Read())
            {
                if (int.Parse(aReader["ActivityID"].ToString()) == _sportID)
                    cbo.Items.Add(new ListItem(aReader["RouteName"].ToString(), aReader["RouteID"].ToString()));

                if (!routes.ContainsKey(aReader["ActivityID"] + "_route"))
                    routes.Add(aReader["ActivityID"] + "_route", aReader["RouteID"] + ";" + aReader["RouteName"]);
                else
                    routes[aReader["ActivityID"] + "_route"] += "|" + aReader["RouteID"] + ";" + aReader["RouteName"];
            }
            aReader.Close();

            foreach (var route in routes)
            {
                this.Hiddens.Add(new HiddenField { ID = route.Key, Value = route.Value });
            }

            SetValue(cbo);
        }

        public void BindKit(DropDownList cbo, TextBox newValue)
        {
            const string strSQL = @"SELECT 
                    KitID, 
                    KitName,
                    ActivityID,
                    IsDefault 
                    FROM 
                    dbo.Kit 
                    WHERE 
                    UserID = @Param0
                    AND IsActive = 1
                    ORDER BY 
                    Ordering";

            var kits = new Dictionary<string, string>();
            var aReader = _aServer.ExecuteReader(strSQL);

            cbo.Items.Clear();
            while (aReader.Read())
            {
                if (int.Parse(aReader["ActivityID"].ToString()) == _sportID)
                {
                    cbo.Items.Add(new ListItem(aReader["KitName"].ToString(), aReader["KitID"].ToString()));

                    if (int.Parse(aReader["IsDefault"].ToString()) == 1) cbo.Items.FindByValue(aReader["KitID"].ToString()).Selected = true;                    
                }

                if (!kits.ContainsKey(aReader["ActivityID"] + "_kit"))
                    kits.Add(aReader["ActivityID"] + "_kit", aReader["KitID"] + ";" + aReader["KitName"]);
                else
                    kits[aReader["ActivityID"] + "_kit"] += "|" + aReader["KitID"] + ";" + aReader["KitName"];
            }
            aReader.Close();

            foreach (var kit in kits)
            {
                this.Hiddens.Add(new HiddenField { ID = kit.Key, Value = kit.Value });
            }

            SetValue(cbo);

            //New Kit
            if (HttpContext.Current.Request["ctl00$cphMain$" + newValue.ID] != null && HttpContext.Current.Request["ctl00$cphMain$" + newValue.ID] != "")
            {
                newValue.Text = HttpContext.Current.Request["ctl00$cphMain$" + newValue.ID];
                newValue.Style["display"] = "inline";
            }
            else
            {
                newValue.Style["display"] = "none";
            }
        }

        public void BindUnits(Label lblUnits)
        {
            lblUnits.Text = this.Settings.ContainsKey("Units") ? this.Settings["Units"] : "||units not set||";
        }

        private void SetValue(DropDownList cbo)
        {
            if (HttpContext.Current.Request["ctl00$cphMain$" + cbo.ID] != null)
            {
                cbo.Items.FindByValue(HttpContext.Current.Request["ctl00$cphMain$" + cbo.ID]).Selected = true;
            }
            else
            {
                if (cbo.Items.Count > 0) cbo.Items[0].Selected = true;
            }
        }

        public static void BindTime(TextBox txtHours, TextBox txtMinutes, TextBox txtSeconds)
        {
            BindTextBox(txtHours);
            BindTextBox(txtMinutes);
            BindTextBox(txtSeconds);
        }

        public static void BindTextBox(TextBox txt)
        {
            txt.Text = HttpContext.Current.Request["ctl00$cphMain$" + txt.ID] ?? "";
        }

        public static void BindTextBoxes(IEnumerable<TextBox> textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                BindTextBox(textBox);
            }
        }
    }
}
