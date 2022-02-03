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

namespace Halls
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
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
            catch(Exception ex)
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
    }
}