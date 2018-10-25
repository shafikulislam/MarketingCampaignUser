using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Css.Extensions;

namespace MC_User
{
    public partial class AddCampaign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadAllCustomers();
            }
        }

        private void LoadAllCustomers()
        {
            MCampDataContext db = new MCampDataContext();
            var x = (from u in db.Customers select u).ToList();

        }


        [WebMethod]
        public static List<Customer> SearchCustomer(string searchKey)
        {
            MCampDataContext db = new MCampDataContext();
            var customers = (from u in db.Customers where u.Mobile.Contains(searchKey) || u.Email.Contains(searchKey) || u.FName.Contains(searchKey) || u.LName.Contains(searchKey) select u).ToList();
            customers.ForEach(i=> { i.CampaignCustomers = null; });
            return customers;
        }

        [WebMethod]
        public static string AddNewCampaign(string name, string type, string msg, string customers)
        {
            try
            {

                MCampDataContext db = new MCampDataContext();

                User logUser = (User)HttpContext.Current.Session["loguser"];

                List<long> customersList = new List<long>();
                customers.Split(',').ForEach(i => customersList.Add(Convert.ToInt64(i)));

                Campaign aCampaign = new Campaign();
                aCampaign.Title = name;
                aCampaign.Type = type;
                aCampaign.Messeage = msg;
                aCampaign.CustomerCount = customersList.Count;
                aCampaign.UserId = logUser.Id;

                db.Campaigns.InsertOnSubmit(aCampaign);
                db.SubmitChanges();

                if (aCampaign.Id == 0)
                {
                    aCampaign =
                        (from u in db.Campaigns where u.UserId == logUser.Id orderby u.Id descending select u)
                            .FirstOrDefault();
                }

                if (aCampaign != null)
                {

                    List<CampaignCustomer> campaignCustomers = customersList.Select(l => new CampaignCustomer()
                    {
                        CampaignId = aCampaign.Id,
                        CustomerId = l
                    }).ToList();

                    db.CampaignCustomers.InsertAllOnSubmit(campaignCustomers);
                    db.SubmitChanges();

                }

                return "Success";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }
    }
}