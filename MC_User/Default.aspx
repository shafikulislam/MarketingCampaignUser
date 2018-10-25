<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MarketingCampaign._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-sm-12">
            <h3 id="msgShow" runat="server"></h3>
        </div>
        <div class="col-sm-12">
            <table id="campaignListTable" class="table table-responsive table-striped">
                <thead>
                    <tr>
                        <th>SL#
                        </th>
                        <th>Campaign Name
                        </th>
                        <th>Campaign Type
                        </th>
                        <th>Total Customers
                        </th>
                        <th>Action
                        </th>
                    </tr>
                </thead>
                <tbody id="campaignListTableTbody" runat="server">
                    
                </tbody>
            </table>
        </div>
    </div>
    
    
    <div class="modal"></div>
    <script>
        function sendMesseage(campaignId) {
            var cus = $("#camp_" + campaignId).find(".customers").text().split(',');

            for (var i = 0; i < cus.length; i++) {
                $.ajax({
                    type: 'Post',
                    contentType: "application/json; charset=utf-8",
                    url: "Default.aspx/SendMsgForCampaign",
                    data: "{'customerId':'" + cus[i] + "','campaignId':'" + campaignId + "'}",
                    async: false,
                    success: function (response) {
                        var xx = i + 1;
                        $("#MainContent_msgShow").text(xx+" of "+cus.length+" send.");
                    },
                    error: function () {
                        alert("failed");
                    }
                });
            }

            
        }
    </script>
</asp:Content>
