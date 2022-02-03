<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Halls._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:FileUpload ID="XMLFileUpload" runat="server" />
                <asp:Button ID="ImportXMLButton" runat="server" Text="Import" OnClick="ImportXMLButton_Click" />
            </div>
            <asp:Label ID="ErrorLabel" runat="server" Text="" ForeColor="Red"></asp:Label>

            <div id="SeatSearch">
                <asp:TextBox ID="SeatRowTextBox" runat="server"></asp:TextBox>
                <asp:TextBox ID="SeatNumber" runat="server"></asp:TextBox>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImportXMLButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
