<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextFilter.ascx.cs" Inherits="ConfigurableDynamicDataFilters.DynamicData.Filters.TextFilter" %>
<asp:DropDownList runat="server" ID="ddlOperator" CssClass="ui-widget">
    <asp:ListItem Text="Contains" Value="contains" />
    <asp:ListItem Text="Starts with" Value="start" />
    <asp:ListItem Text="Ends with" Value="end" />
</asp:DropDownList>
<asp:TextBox runat="server" ID="tbFilter" />
