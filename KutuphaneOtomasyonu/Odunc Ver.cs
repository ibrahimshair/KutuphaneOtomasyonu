using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Odunc_Ver : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Odunc_Ver()
        {
            InitializeComponent();
            Location = new System.Drawing.Point(35, 35); // Initialize form location
        }

        private void Odunc_Ver_Load(object sender, EventArgs e)
        {
            LoadKayitlar();
            LoadKaynaklar();
        }

        // Method to load records (kayitlar) from the database
        private void LoadKayitlar()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM kayitlar"; // Example query to fetch all records from kayitlar

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt; // Bind data to DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayitlar yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to load available books (kaynaklar) from the database
        private void LoadKaynaklar()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Kaynaklar"; // Query to fetch all resources
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt; // Bind data to second DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynaklar yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to search users by TC (ID)
        private void button1_Click(object sender, EventArgs e)
        {
            string tc = TCtext.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT kullanici_ad, kullanici_soyad FROM kullanicilar WHERE kullanici_tc = @tc";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tc", tc);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        label1.Text = $"{reader["kullanici_ad"]} {reader["kullanici_soyad"]}";
                    }
                    else
                    {
                        label1.Text = "Böyle bir kullanıcı bulunmamaktadır";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcı sorgulama sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to filter books as you type in the search textbox
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string kitapAd = textBox1.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Kaynaklar WHERE kaynak_ad LIKE @kitapAd"; // Query to search books by name
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@kitapAd", "%" + kitapAd + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt; // Bind filtered data to DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kitap arama sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to issue a book to a user
        // Method to issue a book to a user
        private void button2_Click(object sender, EventArgs e)
        {
            string tc = TCtext.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT kullanici_id FROM kullanicilar WHERE kullanici_tc = @tc";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tc", tc);

                    int kullaniciId = Convert.ToInt32(cmd.ExecuteScalar());

                    if (kullaniciId == 0)
                    {
                        MessageBox.Show("Kullanıcı bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int kitapId = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);
                    string kitapQuery = "SELECT kaynak_id FROM Kaynaklar WHERE kaynak_id = @kitapId";
                    SqlCommand kitapCmd = new SqlCommand(kitapQuery, conn);
                    kitapCmd.Parameters.AddWithValue("@kitapId", kitapId);
                    int kaynakId = Convert.ToInt32(kitapCmd.ExecuteScalar());

                    if (kaynakId == 0)
                    {
                        MessageBox.Show("Seçilen kitap bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Insert the loan data here (e.g., add to a 'Loans' table, if applicable)
                    string insertLoanQuery = "INSERT INTO Kayitlar (kallanici_id, kitap_id, bitis_tarih) VALUES (@kullaniciId, @kaynakId, @oduncTarihi)";
                    SqlCommand insertLoanCmd = new SqlCommand(insertLoanQuery, conn);
                    insertLoanCmd.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                    insertLoanCmd.Parameters.AddWithValue("@kaynakId", kaynakId);
                    insertLoanCmd.Parameters.AddWithValue("@oduncTarihi", DateTime.Now);

                    int rowsAffected = insertLoanCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kitap başarıyla ödünç verildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh kayitlar and kaynaklar tables
                        LoadKayitlar();  // Refresh the kayitlar table
                        LoadKaynaklar(); // Refresh the Kaynaklar table
                    }
                    else
                    {
                        MessageBox.Show("Kitap ödünç verme sırasında bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödünç verme sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Optional: Handle cell click events for DataGridViews if needed
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Add any necessary logic if required on cell click
        }
    }
}
