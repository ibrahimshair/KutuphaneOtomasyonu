using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Form6 : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Form6()
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
                    string query = "SELECT * FROM Kullanicilar";
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

        private void Form6_Load(object sender, EventArgs e)
        {
            Listele(); // Load data when form loads
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null) return;

                // Uncomment and set the values if needed to fill textboxes
                // kullaniciAdtext.Text = dataGridView1.CurrentRow.Cells["Ad"].Value.ToString();
                // kullaniciSoyadtext.Text = dataGridView1.CurrentRow.Cells["Soyad"].Value.ToString();
                // kullaniciTctext.Text = dataGridView1.CurrentRow.Cells["TC"].Value.ToString();
                // kullaniciTeltext.Text = dataGridView1.CurrentRow.Cells["Telefon"].Value.ToString();
                // kullaniciMailtxt.Text = dataGridView1.CurrentRow.Cells["Mail"].Value.ToString();
                // kullaniciCezatext.Text = dataGridView1.CurrentRow.Cells["Ceza"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcı seçilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Lütfen güncellemek istediğiniz bir kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["kullanici_id"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to update user information
                    string updateQuery = @"UPDATE kullanicilar
                                           SET kullanici_ad = @Ad, 
                                               kullanici_soyad = @Soyad, 
                                               kullanici_tc = @TC, 
                                               kullanici_tel = @Telefon, 
                                               kullanici_mail = @Mail, 
                                               kullanici_ceza = @Ceza
                                           WHERE kullanici_id = @Id";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Ad", kullaniciAdtext.Text.Trim());
                        command.Parameters.AddWithValue("@Soyad", kullaniciSoyadtext.Text.Trim());
                        command.Parameters.AddWithValue("@TC", kullaniciTctext.Text.Trim());
                        command.Parameters.AddWithValue("@Telefon", kullaniciTeltext.Text.Trim());
                        command.Parameters.AddWithValue("@Mail", kullaniciMailtxt.Text.Trim());
                        command.Parameters.AddWithValue("@Ceza", double.TryParse(kullaniciCezatext.Text.Trim(), out var ceza) ? ceza : 0);
                        command.Parameters.AddWithValue("@Id", id);

                        // Execute the update query
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Kullanıcı bilgileri başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele(); // Refresh the data grid after update
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcıyı güncellerken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
