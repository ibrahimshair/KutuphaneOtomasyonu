using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Form4 : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Form4()
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcıları listelerken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Query to add a new user
                        string query = @"INSERT INTO kullanicilar (kullanici_ad, kullanici_soyad, kullanici_tc, kullanici_tel, kullanici_mail, kullanici_ceza) 
                                         VALUES (@Ad, @Soyad, @TC, @Tel, @Mail, @Ceza)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Adding parameters for insertion
                            command.Parameters.AddWithValue("@Ad", kullaniciAdtext.Text.Trim());
                            command.Parameters.AddWithValue("@Soyad", kullaniciSoyadtext.Text.Trim());
                            command.Parameters.AddWithValue("@TC", kullaniciTctext.Text.Trim());
                            command.Parameters.AddWithValue("@Tel", kullaniciTeltext.Text.Trim());
                            command.Parameters.AddWithValue("@Mail", kullaniciMailtxt.Text.Trim());
                            command.Parameters.AddWithValue("@Ceza", Convert.ToDouble(kullaniciCezatext.Text.Trim()));

                            // Execute the query
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Kullanıcı başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Listele(); // Refresh the data grid
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kullanıcı eklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInputs()
        {
            // Validate if any required fields are empty
            if (string.IsNullOrWhiteSpace(kullaniciAdtext.Text) ||
                string.IsNullOrWhiteSpace(kullaniciSoyadtext.Text) ||
                string.IsNullOrWhiteSpace(kullaniciTctext.Text) ||
                string.IsNullOrWhiteSpace(kullaniciTeltext.Text) ||
                string.IsNullOrWhiteSpace(kullaniciMailtxt.Text) ||
                string.IsNullOrWhiteSpace(kullaniciCezatext.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if the "Ceza" field is a valid number
            if (!double.TryParse(kullaniciCezatext.Text.Trim(), out _))
            {
                MessageBox.Show("Ceza değeri geçersiz. Lütfen geçerli bir sayı girin.", "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Handle cell clicks if needed
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Optional: Handle label clicks if needed
        }
    }
}
