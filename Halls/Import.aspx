<%@ Page Title="Import" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="Halls.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- File upload div -->
            <div id="FileUploadDiv">
                <br />
                <asp:FileUpload ID="XMLFileUpload" runat="server" />
                <asp:Button ID="ImportXMLButton" runat="server" Text="Import" OnClick="ImportXMLButton_Click" />
            </div>
            <asp:Label ID="ImportStatusLabel" runat="server" Text="" ForeColor="LimeGreen"></asp:Label>
            <asp:Label ID="ErrorLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImportXMLButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
