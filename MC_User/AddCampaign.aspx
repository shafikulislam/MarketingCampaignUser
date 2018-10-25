<%@ Page Title="Campaign" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCampaign.aspx.cs" Inherits="MC_User.AddCampaign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container" style="margin-top: 30px;">
        <div class="col-sm-12 form-group">
            <div class="col-sm-4">
                <label class="control-label label label-primary">Campaign Name</label>
                <asp:TextBox ID="campaignNameTextBox" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-4">
                <label class="control-label label label-primary">Campaign Type</label>
                <asp:DropDownList ID="campaignTypeDropDownList" runat="server" CssClass="form-control">
                    <asp:ListItem Text="SMS Campaign" Value="SMS" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Email Campaign" Value="EMAIL"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-sm-4">
                <label class="control-label label label-primary">Campaign Messeage</label>
                <asp:TextBox ID="campaignMesseageTextBox" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div class="col-sm-5">
                <label class="control-label label label-primary list-group-item">Campaign Customers</label>
                <ul id="selectedCustomers" class="col-sm-12">
                </ul>
            </div>

            <div class="col-sm-7">
                <div>
                    <ul id="allCustomers" class="col-sm-6 list-group-item">
                    </ul>
                    <div class="col-sm-6">
                        <label class="control-label label label-primary">Search Customers</label>
                        <input type="text" id="cusNEMTextBox" class="form-control" />
                        <button type="button" class="btn btn-sm btn-primary" id="searchCustomerBtn">Search</button>
                    </div>
                </div>
            </div>

            <div class="col-sm-4">
                <label class="control-label" style="color: transparent;">Action</label>
                <button type="button" class="btn btn-sm btn-success" id="saveCampaign">Save Campaign</button>
            </div>
        </div>

    </div>

    <script>
        function removeFromSelected(id) {
            var taId = "#cusA_" + id;
            var tsId = "#cusS_" + id;

            $(taId).parent().show();
            $(tsId).parent().remove();

        }

        function addToSelected(id) {
            var taId = "#cusA_" + id;
            var tsId = "cusS_" + id;

            $("#selectedCustomers").append('<li><button type="button" id="' + tsId + '" onClick="removeFromSelected(\'' + id + '\');">' + $(taId).text() + '</button></li>');

            $(taId).parent().hide();
        }

        function search(searchKey) {
            $("#allCustomers").empty();
            $.ajax({
                type: 'Post',
                contentType: "application/json; charset=utf-8",
                url: "AddCampaign.aspx/SearchCustomer",
                data: "{'searchKey':'" + searchKey + "'}",
                async: false,
                success: function (response) {
                    var customers = response.d;

                    for (var i = 0; i < customers.length; i++) {
                        $("#allCustomers").append('<li><button type="button" id="cusA_' + customers[i].Id + '" onClick="addToSelected(\'' + customers[i].Id + '\');">' + customers[i].FName + ' ' + customers[i].LName + '</button></li>');
                    }
                },
                error: function () {
                    alert("failed");
                }
            });
        }

        $(document).on('click', '#searchCustomerBtn', function () {
            search($("#cusNEMTextBox").val());
            return false;
        });

        $(document).on('click', '#saveCampaign', function () {
            var name = $("#MainContent_campaignNameTextBox").val();
            var type = $("#MainContent_campaignTypeDropDownList").val();
            var msg = $("#MainContent_campaignMesseageTextBox").val();
            var tC = $("#selectedCustomers li");
            var cArr = [];
            for (var i = 0; i < tC.length; i++) {
                var c = $(tC[i]).find('button').attr('id');
                cArr.push(c.split('_')[1]);
            }
            $.ajax({
                type: 'Post',
                contentType: "application/json; charset=utf-8",
                url: "AddCampaign.aspx/AddNewCampaign",
                data: "{'name':'" + name + "','type':'" + type + "','msg':'" + msg + "','customers':'" + cArr.join(",") + "'}",
                async: false,
                success: function (response) {
                    alert(response.d);
                },
                error: function () {
                    alert("failed");
                }
            });
            return false;
        });

    </script>
</asp:Content>
