using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarsDB
{
    public partial class Form1 : Form
    {
        string connectionString;
        SqlConnection connection;
        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["CarsDB.Properties.Settings.CarsConnectionString"].ConnectionString;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateCarsTable();
        }
        private void PopulateCarsTable()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM CarMark", connection))
            {
                DataTable carTable = new DataTable();
                adapter.Fill(carTable);

                listCars.DisplayMember = "CarMarkName";
                listCars.ValueMember = "Id";
                listCars.DataSource = carTable;
            }
            
        }

        private void listCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateCarMarks();
        }
        private void PopulateCarMarks()
        {
            string query = "SELECT Car.Name FROM CarMark INNER JOIN Car ON Car.MarkId = CarMark.Id WHERE CarMark.Id = @MarkId";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@MarkId", listCars.SelectedValue);
                DataTable carmarkTable = new DataTable();
                adapter.Fill(carmarkTable);

                listModelName.DisplayMember = "CarModelName";
                listModelName.ValueMember = "Id";
                listModelName.DataSource = carmarkTable;
            }
        }
    }
}
