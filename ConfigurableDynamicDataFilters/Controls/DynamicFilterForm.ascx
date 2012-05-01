<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicFilterForm.ascx.cs" Inherits="ConfigurableDynamicDataFilters.Controls.DynamicFilterForm" %>
<%@ Register TagPrefix="dd" Namespace="ConfigurableDynamicDataFilters.Controls" Assembly="ConfigurableDynamicDataFilters" %>

<asp:Label runat="server" Text="Add fitler" AssociatedControlID="ddlFilterableColumns" />
<asp:DropDownList runat="server" ID="ddlFilterableColumns" CssClass="ui-widget"
    AutoPostBack="True"
    ItemType="<%$ Code: typeof(KeyValuePair<string, string>) %>"
    DataValueField="Key"
    DataTextField="Value"
	SelectMethod="GetFilterableColumns"
    OnSelectedIndexChanged="ddlFilterableColumns_SelectedIndexChanged">
</asp:DropDownList>

<input type="hidden" runat="server" ID="FilterColumns" />
<dd:DynamicFilterRepeater runat="server" ID="FilterRepeater">
	<ItemTemplate>
		<div>
			<asp:Label ID="lblDisplayName" runat="server" 
				Text='<%# Eval("DisplayName") %>'
				OnPreRender="lblDisplayName_PreRender" />
			<asp:DynamicFilter runat="server" ID="DynamicFilter" />
		</div>
	</ItemTemplate>
</dd:DynamicFilterRepeater>