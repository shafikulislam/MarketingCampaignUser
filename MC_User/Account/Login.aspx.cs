using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using MC_User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;

namespace MarketingCampaign.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                MCampDataContext db = new MCampDataContext();
                var user = (from u in db.Users where u.UserId == UserId.Text && u.Password == Password.Text select u).FirstOrDefault();
                if (user != null)
                {
                    if (Session["user"] != null)
                    {
                        Session["loguser"] = user;
                    }
                    else
                    {
                        Session.Add("user", user);
                    }

                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    FailureText.Text = "Invalid user id or password.";
                    ErrorMessage.Visible = true;
                }
            }
        }
    }
}