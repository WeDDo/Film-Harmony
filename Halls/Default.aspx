<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Halls._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:FileUpload ID="XMLFileUpload" runat="server" />
            <asp:Button ID="ImportXMLButton" runat="server" Text="Import" OnClick="ImportXMLButton_Click" />
            <asp:Label ID="ErrorLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImportXMLButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
