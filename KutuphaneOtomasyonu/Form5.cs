using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Form5 : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Form5()
        {
            InitializeComponent();
            Location = new Point(35, 35); // Set location during initialization
        }

        private void Listele()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to get all columns from the "kullanicilar" table
                    string query = "SELECT * FROM kullanicilar";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();

                    // Fill the DataTable with data
                    dataAdapter.Fill(dataTable);

                    // Bind data to DataGridView
                    dataGridView1.DataSource = dataTable;

                    ConfigureDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcıları listelerken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            // Hide unnecessary columns
            if (dataGridView1.Columns.Contains("Id"))
            {
                dataGridView1.Columns["Id"].Visible = false;
            }

            // Adjust column width to fit content
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Listele(); // Load data when form loads
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Lütfen silmek istediğiniz bir kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["kullanici_id"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Remove related records from the "kayitlar" table
                    string deleteKayitlarQuery = "DELETE FROM Kayitlar WHERE kallanici_id = @Id";
                    using (SqlCommand deleteKayitlarCommand = new SqlCommand(deleteKayitlarQuery, connection))
                    {
                        deleteKayitlarCommand.Parameters.AddWithValue("@Id", id);
                        deleteKayitlarCommand.ExecuteNonQuery();
                    }

                    // Remove user from the "kullanicilar" table
                    string deleteKullaniciQuery = "DELETE FROM Kullanicilar WHERE kullanici_id = @Id";
                    using (SqlCommand deleteKullaniciCommand = new SqlCommand(deleteKullaniciQuery, connection))
                    {
                        deleteKullaniciCommand.Parameters.AddWithValue("@Id", id);
                        deleteKullaniciCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Kullanıcı ve ilişkili kayıtlar başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele(); // Refresh the data grid after deletion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcıyı silerken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Handle cell clicks if needed (no functionality needed here)
        }
    }
}
