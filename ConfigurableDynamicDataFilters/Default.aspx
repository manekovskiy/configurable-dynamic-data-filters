<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ConfigurableDynamicDataFilters._Default" %>
<%-- ReSharper disable RedundantUsingDirective --%>
<%@ Import Namespace="ConfigurableDynamicDataFilters.Model" %>
<%-- ReSharper restore RedundantUsingDirective --%>
<%@ Register tagName="DynamicFilterForm" tagPrefix="dd" src="~/Controls/DynamicFilterForm.ascx" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Page.Title %>.</h1>
                <h2>Configurable dynamic data filters in action.</h2>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="features" data-ui-fn="collapsible" data-ui-collapsible-active="1, 2">
    	<h3><a href="#">Filters</a></h3>
        <div class="feature">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <dd:DynamicFilterForm runat="server" ID="dffCustomers"
                        FitlerType="<%$ Code: typeof(Customer) %>" 
                        QueryExtenderID="GridQueryExtender"
                        GridViewID="gvCustomer" />        
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <br />
            <asp:Button runat="server" ID="btnApply" data-ui-fn="button"
                Text="Apply"
                OnClick="btnApply_Click" />
        </div>

		<h3><a href="#">Search results</a></h3>
        <div class="feature">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" ID="gvCustomer" 
			            DataSourceID="ldsCustomers"
			            AutoGenerateColumns="True"
			            AllowPaging="True">
		            </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnApply" EventName="Click"/>
                </Triggers>
		    </asp:UpdatePanel>

            <asp:LinqDataSource runat="server" ID="ldsCustomers"
			    ContextTypeName="<%$ Code: typeof(EntitiesContext).AssemblyQualifiedName %>"
			    TableName="Customers"
                OrderBy="CustomerID"
                AutoPage="True">
		    </asp:LinqDataSource>
            
            <asp:QueryExtender TargetControlID="ldsCustomers" ID="GridQueryExtender" runat="server">
	            <asp:DynamicFilterExpression ControlID="<%$ Code: dffCustomers.FilterRepeater.UniqueID  %>" />
            </asp:QueryExtender>
        </div>
    </div>
</asp:Content>