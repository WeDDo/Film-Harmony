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

        public static T Deserialize<T>(string xmlText, string root)
        {
            try
            {
                var stringReader = new StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(root));
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

        int SelectHallSystemId(int hallId)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Hall_Select_System_Id", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HallID", SqlDbType.Int).Value = hallId;
                SqlDataReader reader = cmd.ExecuteReader();

                int id = -1;
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["Id"]);
                }
                cnn.Close();

                return id;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return -1; //Error accessing database
            }
        }

        bool InsertHallGroupToDatabase(HallGroup hallGroup)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);

            int hallId = SelectHallSystemId(hallGroup.HallID);

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallGroup_Insert", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HallGroupID", SqlDbType.Int).Value = hallGroup.Id;
                cmd.Parameters.AddWithValue("@HallID", SqlDbType.Int).Value = hallGroup.HallID;
                cmd.Parameters.AddWithValue("@Hall_Id", SqlDbType.Int).Value = hallId;
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

        int SelectHallGroupSystemId(int hallGroupId)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallGroup_Select_System_Id", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HallGroupID", SqlDbType.Int).Value = hallGroupId;
                SqlDataReader reader = cmd.ExecuteReader();

                int id = -1;
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["Id"]);
                }
                cnn.Close();

                return id;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return -1; //Error accessing database
            }
        }

        bool InsertHallSeatToDatabase(HallSeat hallSeat)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);

            int hallGroupId = SelectHallGroupSystemId(hallSeat.HallGroupId);
            
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallSeat_Insert", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShowSeatID", SqlDbType.Int).Value = hallSeat.Id;
                cmd.Parameters.AddWithValue("@HallGroupID", SqlDbType.Int).Value = hallSeat.HallGroupId;
                cmd.Parameters.AddWithValue("@HallGroup_Id", SqlDbType.Int).Value = hallGroupId;
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
                    int id = Convert.ToInt32(reader["HallGroupID"]);
                    int hallId = Convert.ToInt32(reader["HallID"]);
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
                    int id = Convert.ToInt32(reader["HallID"]);
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
            if (!XMLFileUpload.HasFile)
            {
                ErrorLabel.Text = "You need to select a file!";
                return;
            }
            ErrorLabel.Text = string.Empty;
            ReservationStatusLabel.Visible = false;

            string fileName = XMLFileUpload.PostedFile.FileName;
            string filePath = Server.MapPath("~/upload/") + fileName;
            XMLFileUpload.SaveAs(filePath);
            string xmlFile = File.ReadAllText(filePath);
            string rootName = File.ReadLines(filePath).ElementAtOrDefault(1).Trim(new char[] { '<', '>' });

            //Viskas nuskaityta i sita kintamaji
            Organization result = Deserialize<Organization>(xmlFile, rootName);

            List<int> hallIds = GetHallExistingIds();
            List<int> hallGroupIds = GetHallGroupExistingIds();
            List<int> hallSeatIds = GetHallSeatExistingIds();

            for (int i = 0; i < result.Hall.Count; i++)
            {
                if (hallIds.Contains(result.Hall[i].Id))
                {
                    UpdateHall(result.Hall[i]);
                    //ErrorLabel.Text += ("<br />- " + result.Hall + " !Updated! <br />");
                }
                else
                {
                    InsertHallToDatabase(result.Hall[i]);
                    //ErrorLabel.Text += ("<br />- " + result.Hall + "<br />");
                }
                
                for (int j = 0; j < result.Hall[i].hallGroups.Count; j++)
                {
                    if (hallGroupIds.Contains(result.Hall[i].hallGroups[j].Id))
                    {
                        UpdateHallGroup(result.Hall[i].hallGroups[j]);
                        //ErrorLabel.Text += ("- " + result.Hall.hallGroups[i] + " !Updated! <br />");
                    }
                    else
                    {
                        InsertHallGroupToDatabase(result.Hall[i].hallGroups[j]);
                        //ErrorLabel.Text += ("- " + result.Hall.hallGroups[i] + "<br />");
                    }
                    
                    for (int k = 0; k < result.Hall[i].hallGroups[j].HallSeats.Count; k++)
                    {
                        if (hallSeatIds.Contains(result.Hall[i].hallGroups[j].HallSeats[k].Id))
                        {
                            UpdateHallSeat(result.Hall[i].hallGroups[j].HallSeats[k]);
                            //ErrorLabel.Text += ("-- " + result.Hall.hallGroups[i].HallSeats[j] + " !Updated! <br />");
                        }
                        else
                        {
                            InsertHallSeatToDatabase(result.Hall[i].hallGroups[j].HallSeats[k]);
                            //ErrorLabel.Text += ("-- " + result.Hall.hallGroups[i].HallSeats[j] + "<br />");
                        }
                    }
                }
            }

            HallGroupListBindData();
            HallListBindData();
        }

        protected void SearchSeatButton_Click(object sender, EventArgs e)
        {
            ErrorLabel.Text = string.Empty;
            ReservationStatusLabel.Visible = false;

            int row = Convert.ToInt32(SeatRowDropDownList.SelectedValue);
            int number = Convert.ToInt32(SeatNumberDropDownList.SelectedValue);

            HallSeat seat = SelectHallSeat(row, number);

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
            SeatNumberDropDownList.DataSource = SelectSeatNumber(Convert.ToInt32(HallGroupDropDownList.SelectedValue), Convert.ToInt32(SeatRowDropDownList.SelectedValue));
            SeatNumberDropDownList.DataBind();
        }

        void SeatRowBind()
        {
            SeatRowDropDownList.DataSource = SelectSeatRow(Convert.ToInt32(HallGroupDropDownList.SelectedValue));
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

        List<int> GetHallExistingIds()
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            List<int> ids = new List<int>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Hall_Select_Ids", cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["HallID"]);
                    ids.Add(id);
                }
                cnn.Close();

                return ids;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return null; //Error accessing database
            }
        }

        List<int> GetHallGroupExistingIds()
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            List<int> ids = new List<int>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallGroup_Select_Ids", cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["HallGroupId"]);
                    ids.Add(id);
                }
                cnn.Close();

                return ids;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return null; //Error accessing database
            }
        }

        List<int> GetHallSeatExistingIds()
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            List<int> ids = new List<int>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallSeat_Select_Ids", cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ShowSeatId"]);
                    ids.Add(id);
                }
                cnn.Close();

                return ids;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                return null; //Error accessing database
            }
        }

        bool UpdateHall(Hall hall)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Hall_Update", cnn);
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

        bool UpdateHallGroup(HallGroup hallGroup)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallGroup_Update", cnn);
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

        bool UpdateHallSeat(HallSeat hallSeat)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallSeat_Update", cnn);
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

        List<int> SelectSeatRow(int hallGroupId)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            List<int> rows = new List<int>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallSeat_Select_SeatRow", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HallGroupId", SqlDbType.Int).Value = hallGroupId;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int row = Convert.ToInt32(reader["SeatRow"]);
                    rows.Add(row);
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

        List<int> SelectSeatNumber(int hallGroupId, int seatRow)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            List<int> numbers = new List<int>();

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallSeat_Select_SeatNumber", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HallGroupId", SqlDbType.Int).Value = hallGroupId;
                cmd.Parameters.AddWithValue("@SeatRow", SqlDbType.Int).Value = seatRow;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int number = Convert.ToInt32(reader["SeatNumber"]);
                    numbers.Add(number);
                }
                cnn.Close();

                return numbers;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
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