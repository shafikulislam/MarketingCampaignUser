using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using MC_User;

namespace MarketingCampaign
{
    public partial class SiteMaster : MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            User logUser = (User)Session["loguser"];
            string currentPageName = Path.GetFileName(Request.PhysicalPath);
            if (logUser == null && currentPageName != "Login" && currentPageName != "Login.aspx")
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }

    }

}