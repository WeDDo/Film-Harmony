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

namespace Halls
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReserveDiv.Visible = false;
                HallGroupDropDownList.DataSource = SelectHallGroup();
                HallGroupDropDownList.DataTextField = "Name";
                HallGroupDropDownList.DataValueField = "Id";
                HallGroupDropDownList.DataBind();
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

        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool InsertHallToDatabase(Hall hall)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Hall_Insert", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HallID", SqlDbType.Int).Value = hall.Id;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = hall.Name;
                cmd.Parameters.AddWithValue("@TicketLimit", SqlDbType.Int).Value = hall.TicketLimit;
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

        bool InsertHallGroupToDatabase(HallGroup hallGroup)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallGroup_Insert", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HallGroupID", SqlDbType.Int).Value = hallGroup.Id;
                cmd.Parameters.AddWithValue("@HallID", SqlDbType.Int).Value = hallGroup.HallID;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = hallGroup.Name;
                cmd.Parameters.AddWithValue("@AZ", SqlDbType.Int).Value = hallGroup.AZ;
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

        bool InsertHallSeatToDatabase(HallSeat hallSeat)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallSeat_Insert", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShowSeatID", SqlDbType.Int).Value = hallSeat.Id;
                cmd.Parameters.AddWithValue("@HallGroupID", SqlDbType.Int).Value = hallSeat.HallGroupId;
                cmd.Parameters.AddWithValue("@Color", SqlDbType.NVarChar).Value = hallSeat.Color;
                cmd.Parameters.AddWithValue("@Price", SqlDbType.Float).Value = hallSeat.Price;
                cmd.Parameters.AddWithValue("@SeatRow", SqlDbType.Int).Value = hallSeat.Row;
                cmd.Parameters.AddWithValue("@SeatRowLetter", SqlDbType.NVarChar).Value = hallSeat.RowLetter;
                cmd.Parameters.AddWithValue("@SeatNumber", SqlDbType.Int).Value = hallSeat.Number;
                cmd.Parameters.AddWithValue("@SeatNumberLetter", SqlDbType.NVarChar).Value = hallSeat.NumberLetter;
                cmd.Parameters.AddWithValue("@IsReserved", SqlDbType.Bit).Value = hallSeat.IsReserved;
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
                cmd.Parameters.AddWithValue("@HallGroupID", SqlDbType.Int).Value = HallGroupDropDownList.SelectedValue;
                cmd.Parameters.AddWithValue("@SeatRow", SqlDbType.Int).Value = row;
                cmd.Parameters.AddWithValue("@SeatNumber", SqlDbType.Int).Value = number;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ShowSeatID"]);
                    int hallGroupId = Convert.ToInt32(reader["HallGroupID"]);
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

        List<HallGroup> SelectHallGroup()
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            HallGroup hallGroup = new HallGroup();
            List<HallGroup> hallGroups = new List<HallGroup>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallGroup_Select", cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["HallGroupID"]);
                    int hallId = Convert.ToInt32(reader["HallID"]);
                    string name = reader["Name"].ToString();
                    int az = Convert.ToInt32(reader["AZ"]);

                    hallGroup = new HallGroup(id, hallId, name, az);
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
            if (!XMLFileUpload.HasFile)
            {
                ErrorLabel.Text = "You need to select a file!";
                return;
            }
            ErrorLabel.Text = string.Empty;

            string fileName = XMLFileUpload.PostedFile.FileName;
            string filePath = Server.MapPath("~/upload/") + fileName;
            XMLFileUpload.SaveAs(filePath);
            string xmlFile = File.ReadAllText(filePath);

            //Viskas nuskaityta i sita kintamaji
            Filharmonija result = Deserialize<Filharmonija>(xmlFile);

            //InsertHallToDatabase(result.Hall);
            ErrorLabel.Text += ("<br />- " + result.Hall + "<br />");

            for (int i = 0; i < result.Hall.hallGroups.Count; i++)
            {
                //InsertHallGroupToDatabase(result.Hall.hallGroups[i]);
                ErrorLabel.Text += ("- " + result.Hall.hallGroups[i] + "<br />");

                for (int j = 0; j < result.Hall.hallGroups[i].HallSeats.Count; j++)
                {
                    //InsertHallSeatToDatabase(result.Hall.hallGroups[i].HallSeats[j]);
                    ErrorLabel.Text += ("-- " + result.Hall.hallGroups[i].HallSeats[j] + "<br />");
                }
            }
        }

        protected void SearchSeatButton_Click(object sender, EventArgs e)
        {
            int row = Convert.ToInt32(SeatRowTextBox.Text);
            int number = Convert.ToInt32(SeatNumberTextBox.Text);

            HallSeat seat = SelectHallSeat(row, number);

            if(seat.Id == -1)
            {
                IsReservedLabel.ForeColor = Color.OrangeRed;
                IsReservedLabel.Text = "This seat is doesn't exist!";
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
            SeatInfoLabel.Text = seat.ToString();
            ViewState["SearchedSeatId"] = seat.Id;
        }

        protected void ReserveButton_Click(object sender, EventArgs e)
        {
            int seatId = (int)ViewState["SearchedSeatId"];
            UpdateReservation(seatId, true);
        }
    }
}