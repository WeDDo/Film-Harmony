using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace Halls
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ImportStatusLabel.Text = string.Empty;
        }

        public T Deserialize<T>(string xmlText, string root)
        {
            try
            {
                var stringReader = new StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(root));
                return (T)serializer.Deserialize(stringReader);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if(ex.Message.Contains("There is an error in XML document"))
                {
                    errorMessage = errorMessage.Replace("There is an error in XML document (", string.Empty);
                    errorMessage = errorMessage.Replace(").", "");
                    string[] values = errorMessage.Split(new char[] { ',' });
                    errorMessage = string.Format("There is an error in XML document line {0}! Fix this error and try importing again.", Convert.ToInt32(values[0]) - 1);
                }

                ErrorLabel.Text = errorMessage;
                return default(T);
            }
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
                    int id = Convert.ToInt32(reader["ExternalHallID"]);
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
                    int id = Convert.ToInt32(reader["ExternalHallGroupID"]);
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
                    int id = Convert.ToInt32(reader["ShowSeatID"]);
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
                cmd.Parameters.AddWithValue("@ExternalHallID", SqlDbType.Int).Value = hall.Id;
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
                cmd.Parameters.AddWithValue("@ExternalHallGroupID", SqlDbType.Int).Value = hallGroup.Id;
                cmd.Parameters.AddWithValue("@ExternalHallID", SqlDbType.Int).Value = hallGroup.HallID;
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
                cmd.Parameters.AddWithValue("@ExternalHallGroupID", SqlDbType.Int).Value = hallSeat.HallGroupId;
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

        bool InsertHallToDatabase(Hall hall)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Hall_Insert", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExternalHallID", SqlDbType.Int).Value = hall.Id;
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

            int hallId = SelectHallSystemId(hallGroup.HallID);

            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallGroup_Insert", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExternalHallGroupID", SqlDbType.Int).Value = hallGroup.Id;
                cmd.Parameters.AddWithValue("@ExternalHallID", SqlDbType.Int).Value = hallGroup.HallID;
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

        int SelectHallSystemId(int hallId)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Hall_Select_System_Id", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExternalHallID", SqlDbType.Int).Value = hallId;
                SqlDataReader reader = cmd.ExecuteReader();

                int id = -1;
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["Hall_Id"]);
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

        int SelectHallGroupSystemId(int hallGroupId)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("HallGroup_Select_System_Id", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExternalHallGroupID", SqlDbType.Int).Value = hallGroupId;
                SqlDataReader reader = cmd.ExecuteReader();

                int id = -1;
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["HallGroup_Id"]);
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
                cmd.Parameters.AddWithValue("@ExternalHallGroupID", SqlDbType.Int).Value = hallSeat.HallGroupId;
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


        protected void ImportXMLButton_Click(object sender, EventArgs e)
        {
            if (!XMLFileUpload.HasFile)
            {
                ErrorLabel.Text = "You need to select a file!";
                return;
            }

            string fileName = XMLFileUpload.PostedFile.FileName;
            string filePath = Server.MapPath("~/upload/") + fileName;
            XMLFileUpload.SaveAs(filePath);
            string xmlFile = File.ReadAllText(filePath);
            string rootName = File.ReadLines(filePath).ElementAtOrDefault(1).Trim(new char[] { '<', '>' });

            //Deserialized data structure
            Organization result = Deserialize<Organization>(xmlFile, rootName);

            //If an error was caught in the XML file don't continue
            if (result == default(Organization))
                return;

            List<int> hallIds = GetHallExistingIds();
            List<int> hallGroupIds = GetHallGroupExistingIds();
            List<int> hallSeatIds = GetHallSeatExistingIds();

            for (int i = 0; i < result.Hall.Count; i++)
            {
                if (hallIds.Contains(result.Hall[i].Id))
                {
                    UpdateHall(result.Hall[i]);
                }
                else
                {
                    InsertHallToDatabase(result.Hall[i]);
                }

                for (int j = 0; j < result.Hall[i].hallGroups.Count; j++)
                {
                    if (hallGroupIds.Contains(result.Hall[i].hallGroups[j].Id))
                    {
                        UpdateHallGroup(result.Hall[i].hallGroups[j]);
                    }
                    else
                    {
                        InsertHallGroupToDatabase(result.Hall[i].hallGroups[j]);
                    }

                    for (int k = 0; k < result.Hall[i].hallGroups[j].HallSeats.Count; k++)
                    {
                        if (hallSeatIds.Contains(result.Hall[i].hallGroups[j].HallSeats[k].Id))
                        {
                            UpdateHallSeat(result.Hall[i].hallGroups[j].HallSeats[k]);
                        }
                        else
                        {
                            InsertHallSeatToDatabase(result.Hall[i].hallGroups[j].HallSeats[k]);
                        }
                    }
                }
            }
            ImportStatusLabel.Text = "Successfully imported!";
            ErrorLabel.Text = string.Empty;
        }
    }
}