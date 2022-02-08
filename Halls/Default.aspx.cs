using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Xml;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Halls
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HallGroupListBindData();
                HallListBindData();
                SeatRowBind();
                SeatNumberBind();
            }
        }

        public static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new StringWriter();
                var serializer = new XmlSerializer(typeof(T));

                var xns = new XmlSerializerNamespaces();
                xns.Add(string.Empty, string.Empty);

                serializer.Serialize(stringwriter, dataToSerialize, xns);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }

        HallSeat SelectHallSeat(int row, int number)
        {

            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            HallSeat seat = new HallSeat();
            List<HallSeat> seats = new List<HallSeat>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallSeat_Select", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExternalHallGroupID", SqlDbType.Int).Value = HallGroupDropDownList.SelectedValue;
                cmd.Parameters.AddWithValue("@SeatRow", SqlDbType.Int).Value = row;
                cmd.Parameters.AddWithValue("@SeatNumber", SqlDbType.Int).Value = number;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ShowSeatID"]);
                    int hallGroupId = Convert.ToInt32(reader["ExternalHallGroupID"]);
                    string color = reader["Color"].ToString();
                    double price = Convert.ToDouble(reader["Price"]);
                    int seatRow = Convert.ToInt32(reader["SeatRow"]);
                    string seatRowLetter = (reader["SeatRowLetter"] as string == null) ? reader["SeatRowLetter"] as string : string.Empty;
                    int seatNumber = Convert.ToInt32(reader["SeatNumber"]);
                    string seatNumberLetter = (reader["SeatNumberLetter"] as string == null) ? reader["SeatNumberLetter"] as string : string.Empty;
                    bool isReserved = Convert.ToBoolean(reader["IsReserved"]);

                    seat = new HallSeat(id, hallGroupId, color, price, seatRow, seatRowLetter, seatNumber, seatNumberLetter, isReserved);
                }
                cnn.Close();

                return seat;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return null; //Error accessing database
            }
        }

        List<HallGroup> SelectHallGroups()
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            List<HallGroup> hallGroups = new List<HallGroup>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallGroup_Select", cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ExternalHallGroupID"]);
                    int hallId = Convert.ToInt32(reader["ExternalHallID"]);
                    string name = reader["Name"].ToString();
                    int az = Convert.ToInt32(reader["AZ"]);

                    HallGroup hallGroup = new HallGroup(id, hallId, name, az);
                    hallGroups.Add(hallGroup);
                }
                cnn.Close();

                return hallGroups;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return null; //Error accessing database
            }
        }

        List<Hall> SelectHalls()
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            List<Hall> halls = new List<Hall>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Hall_Select", cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ExternalHallID"]);
                    string name = reader["Name"].ToString();
                    int ticketLimit = Convert.ToInt32(reader["TicketLimit"]);

                    Hall hall = new Hall(id, name, ticketLimit);
                    halls.Add(hall);
                }
                cnn.Close();

                return halls;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return null; //Error accessing database
            }
        }

        bool UpdateReservation(int showSeatId, bool isReserved)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallSeat_Update_Reservation", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShowSeatID", SqlDbType.Int).Value = showSeatId;
                cmd.Parameters.AddWithValue("@IsReserved", SqlDbType.Bit).Value = isReserved;
                cmd.ExecuteNonQuery();
                cnn.Close();

                return true; //Success
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return false; //Error accessing database
            }
        }

        protected void ImportXMLButton_Click(object sender, EventArgs e)
        {
            
        }

        protected void SearchSeatButton_Click(object sender, EventArgs e)
        {
            ErrorLabel.Text = string.Empty;
            ReservationStatusLabel.Visible = false;

            int row = Convert.ToInt32(SeatRowDropDownList.SelectedValue);
            int number = Convert.ToInt32(SeatNumberDropDownList.SelectedValue);

            HallSeat seat = SelectHallSeat(row, number); //FIX THIS SHIT

            if (seat.Id == -1)
            {
                IsReservedLabel.ForeColor = Color.OrangeRed;
                IsReservedLabel.Text = "This seat is doesn't exist!";
                ReserveDiv.Visible = false;
                return;
            }

            if (seat.IsReserved)
            {
                IsReservedLabel.ForeColor = Color.Red;
                IsReservedLabel.Text = "This seat is already reserved!";
                ReserveDiv.Visible = false;
            }
            else
            {
                IsReservedLabel.ForeColor = Color.LimeGreen;
                IsReservedLabel.Text = "This seat is not reserved!";
                ReserveDiv.Visible = true;
            }
            ViewState["SearchedSeatId"] = seat.Id;
        }

        protected void ReserveButton_Click(object sender, EventArgs e)
        {
            int seatId = (int)ViewState["SearchedSeatId"];
            UpdateReservation(seatId, true);
            ReserveDiv.Visible = false;
            ReservationStatusLabel.ForeColor = Color.LimeGreen;
            IsReservedLabel.Text = "";
            ReservationStatusLabel.Text = "Reservation successful!";
            ReservationStatusLabel.Visible = true;
        }

        void SeatNumberBind()
        {
            SeatNumberDropDownList.DataSource = SelectSeatNumber(Convert.ToInt32(HallGroupDropDownList.SelectedValue), SeatRowDropDownList.SelectedValue);
            HallGroupDropDownList.DataTextField = "EntireNumber";
            HallGroupDropDownList.DataValueField = "Number"; //FIX
            SeatNumberDropDownList.DataBind();
        }

        void SeatRowBind()
        {
            SeatRowDropDownList.DataSource = SelectSeatRow(Convert.ToInt32(HallGroupDropDownList.SelectedValue));
            HallGroupDropDownList.DataTextField = "EntireRow";
            HallGroupDropDownList.DataValueField = "EntireRow";
            SeatRowDropDownList.DataBind();
        }

        void HallGroupListBindData()
        {
            ReserveDiv.Visible = false;
            HallGroupDropDownList.DataSource = SelectHallGroups();
            HallGroupDropDownList.DataTextField = "Name";
            HallGroupDropDownList.DataValueField = "Id";
            HallGroupDropDownList.DataBind();
        }

        void HallListBindData()
        {
            ReserveDiv.Visible = false;
            HallDropDownList.DataSource = SelectHalls();
            HallDropDownList.DataTextField = "Name";
            HallDropDownList.DataValueField = "Id";
            HallDropDownList.DataBind();
        }

        List<FullSeatRow> SelectSeatRow(int hallGroupId)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            List<FullSeatRow> rows = new List<FullSeatRow>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallSeat_Select_SeatRow", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExternalHallGroupID", SqlDbType.Int).Value = hallGroupId;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int row = Convert.ToInt32(reader["SeatRow"]);
                    string rowLetter = (!DBNull.Value.Equals(reader["SeatRowLetter"])) ? reader["SeatRowLetter"].ToString() : string.Empty;
                    rows.Add(new FullSeatRow(row, rowLetter));
                    ErrorLabel.Text += row + rowLetter + ", ";
                }
                cnn.Close();
               
                return rows;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return null; //Error accessing database
            }
        }

        List<FullSeatNumber> SelectSeatNumber(int hallGroupId, string seatRow)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            List<FullSeatNumber> numbers = new List<FullSeatNumber>();

            Regex regex = new Regex(@"(\d+)([a-zA-Z]+)");
            Match result = regex.Match(seatRow);

            string seatNumber = string.Empty;
            string seatNumberLetter = string.Empty;
            if (result.Success)
            {
                seatNumber = result.Groups[1].Value;
                seatNumberLetter = result.Groups[2].Value;
            }
            else
            {
                seatNumber = seatRow;
            }

            try
            {
                cnn.Open();
                SqlCommand cmd;
                if (seatNumberLetter != null && seatNumberLetter.Length > 0)
                {
                    cmd = new SqlCommand("HallSeat_Select_SeatNumber_WithLetter", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExternalHallGroupID", SqlDbType.Int).Value = hallGroupId;
                    cmd.Parameters.AddWithValue("@SeatRow", SqlDbType.Int).Value = Convert.ToInt32(seatNumber);
                    cmd.Parameters.AddWithValue("@SeatRowLetter", SqlDbType.NVarChar).Value = seatNumberLetter;
                }
                else
                {
                    cmd = new SqlCommand("HallSeat_Select_SeatNumber", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExternalHallGroupID", SqlDbType.Int).Value = hallGroupId;
                    cmd.Parameters.AddWithValue("@SeatRow", SqlDbType.Int).Value = Convert.ToInt32(seatNumber);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int number = Convert.ToInt32(reader["SeatNumber"]);
                    string rowLetter = (!DBNull.Value.Equals(reader["SeatNumberLetter"])) ? reader["SeatNumberLetter"].ToString() : string.Empty;
                    numbers.Add(new FullSeatNumber(number, rowLetter));
                }
                cnn.Close();

                return numbers;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
                return null; //Error accessing database
            }
        }

        protected void HallGroupDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeatRowBind();
        }

        protected void SeatRowDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeatNumberBind();
        }
    }
}