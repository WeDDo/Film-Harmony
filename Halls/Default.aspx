<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Halls._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <!-- File upload div -->
            <div id="FileUploadDiv">
                <br />
                <asp:FileUpload ID="XMLFileUpload" runat="server" />
                <asp:Button ID="ImportXMLButton" runat="server" Text="Import" OnClick="ImportXMLButton_Click" />
            </div>
            <asp:Label ID="ErrorLabel" runat="server" Text="" ForeColor="Red"></asp:Label>

            <br />
            <!-- Seat search div -->
            <div id="SeatSearchDiv">
                <div style="display: inline-block">
                    <asp:Label ID="HallDropDownListLabel" CssClass="label-width-m" runat="server" Text="Hall:"></asp:Label>
                    <asp:DropDownList ID="HallDropDownList" runat="server"></asp:DropDownList>
                </div>

                <div style="display: inline-block">
                    <asp:Label ID="HallGroupDropDownListLabel" CssClass="label-width-m" runat="server" Text="Hall group:"></asp:Label>
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

            <!-- Seat info div -->
            <div id="SeatInfoDiv">
                <asp:Label ID="SeatInfoLabel" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="IsReservedLabel" runat="server" Text=""></asp:Label>

                <!-- Seat reservation div -->
                <div id="ReserveDiv" runat="server">
                    <asp:Label ID="ReserveInstructionsLabel" runat="server" Text="Reserve this seat by clicking 'Reserve'"></asp:Label>
                    <br />
                    <asp:Button ID="ReserveButton" runat="server" Text="Reserve" OnClick="ReserveButton_Click" OnClientClick="return confirm('Are you sure you want to reserve this seat?');"  />
                </div>
                <asp:Label ID="ReservationStatusLabel" runat="server" Text=" " Visible="false"></asp:Label>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImportXMLButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
