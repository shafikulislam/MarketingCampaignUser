using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using MC_User;
using MC_User.localhost;
using WebGrease.Css.Extensions;

namespace MarketingCampaign
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User logUser = (User)Session["loguser"];
            if (!IsPostBack)
            {
                if (logUser != null) LoadAllCampaignsByUser(logUser.Id);
            }
        }

        private void LoadAllCampaignsByUser(long id)
        {
            MCampDataContext db = new MCampDataContext();
            var campaigns = (from c in db.Campaigns
                where c.UserId == id
                select c).ToList();

            campaignListTableTbody.InnerHtml = MakeCampaignTableHtml(campaigns);
        }

        private string MakeCampaignTableHtml(List<Campaign> campaigns)
        {
            string html = "";
            int i = 1;
            foreach (Campaign c in campaigns)
            {
                List<long> s = new List<long>();
                c.CampaignCustomers.ForEach(j => s.Add(j.CustomerId));
                html += "<tr id='camp_"+c.Id+"'>";
                html += "<td>"+i+"</td>";
                html += "<td>"+c.Title+"</td>";
                html += "<td>"+c.Type+"</td>";
                html += "<td>"+c.CustomerCount+"</td>";
                html += "<td class='hidden customers'>"+string.Join(",",s)+"</td>";
                html += "<td><button type='button' class='btn btn-sm btn-primary' onClick='sendMesseage(\""+c.Id+"\");'>Send</button></td>";
                html += "</tr>";
                i++;
            }
            return html;
        }

        User aUser = new User();
        MCampDataContext db  = new MCampDataContext();


        [WebMethod]
        public static string SendMsgForCampaign(long campaignId, string messeage)
        {
            MCampDataContext db = new MCampDataContext();
            var cus = (from u in db.CampaignCustomers where u.CampaignId == campaignId select u.Customer).ToList();

            List<SingleMesseage> messeages = new List<SingleMesseage>();
            cus.ForEach(i=>
            {
                var f = i.CampaignCustomers.FirstOrDefault(j=>j.CampaignId==campaignId);
                if (f != null)
                    messeages.Add(new SingleMesseage()
                    {
                        MsgType = f.Campaign.Type,
                        MsgTo = f.Campaign.Type=="SMS"?i.Mobile:i.Email,
                        Messeage = f.Campaign.Messeage
                    });
            });
            MC_User.localhost.SendMesseage api = new SendMesseage();
            return api.SendMultiple(messeages.ToArray());
        }

        [WebMethod]
        public static string SendMsgForCampaign(long customerId, long campaignId)
        {
            MCampDataContext db = new MCampDataContext();
            var customer = (from u in db.Customers where u.Id == customerId select u).FirstOrDefault();
            var res = "";
            SendMesseage x = new SendMesseage();
            if (customer != null)
            {
                var f = customer.CampaignCustomers.FirstOrDefault(i => i.CampaignId == campaignId);
                if (f != null)
                {
                    res = x.SendSingle(new SingleMesseage()
                    {
                        MsgTo =  f.Campaign.Type=="SMS"?customer.Mobile:customer.Email,
                        MsgType = f.Campaign.Type,
                        Messeage = f.Campaign.Messeage
                    });
                }
            }
            return res;
        }
    }
}