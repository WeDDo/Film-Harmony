<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Halls._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div id="FileUploadDiv">
                <asp:FileUpload ID="XMLFileUpload" runat="server" />
                <asp:Button ID="ImportXMLButton" runat="server" Text="Import" OnClick="ImportXMLButton_Click" />
            </div>
            <asp:Label ID="ErrorLabel" runat="server" Text="" ForeColor="Red"></asp:Label>

            <div id="SeatSearchDiv">
                <div style="display: inline-block">
                    <asp:Label ID="Label1" CssClass="label-width-m" runat="server" Text="Hall group:"></asp:Label>
                    <asp:DropDownList ID="HallGroupDropDownList" runat="server"></asp:DropDownList>
                </div>

                <div style="display: inline-block">
                    <asp:Label ID="SeatRowLabel" CssClass="label-width-m" runat="server" Text="Seat Row:"></asp:Label>
                    <asp:TextBox ID="SeatRowTextBox" CssClass="input-width-m" runat="server"></asp:TextBox>
                </div>

                <div style="display: inline-block">
                    <asp:Label ID="SeatNumberLabel" CssClass="label-width-m" runat="server" Text="Seat Number:"></asp:Label>
                    <asp:TextBox ID="SeatNumberTextBox" CssClass="input-width-m" runat="server"></asp:TextBox>
                </div>
                <br />
                <asp:Button ID="SearchSeatButton" runat="server" Text="Search" OnClick="SearchSeatButton_Click" />
            </div>

            <div id="SeatInfoDiv">
                
                <asp:Label ID="SeatInfoLabel" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="IsReservedLabel" runat="server" Text=""></asp:Label>

                <div ID="ReserveDiv" runat="server">
                    <asp:Label ID="ReserveInstructionsLabel" runat="server" Text="Reserve this eat by checking the box and clicking the button"></asp:Label>
                    <asp:CheckBox ID="IsReservedCheckBox" runat="server" />
                    <asp:Button ID="ReserveButton" runat="server" Text="Reserve" OnClick="ReserveButton_Click" OnClientClick="return confirm('Are you sure you want to reserve this seat?');"  />
                </div>
                
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImportXMLButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
