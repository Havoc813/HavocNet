﻿using System;

namespace HavocNet.Web.Athletica
{
    public partial class MenuMaintenance : System.Web.UI.Page
    {
        private Web.Athletica.Athletica _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Web.Athletica.Athletica)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainTools");

        }
    }
}