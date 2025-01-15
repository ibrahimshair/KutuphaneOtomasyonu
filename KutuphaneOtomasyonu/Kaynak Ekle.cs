using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Kaynak_Ekle : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Kaynak_Ekle()
        {
            InitializeComponent();
            Location = new System.Drawing.Point(35, 35);
        }

        private void Kaynak_Ekle_Load(object sender, EventArgs e)
        {
            LoadKaynaklar();
        }

        private void LoadKaynaklar()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Kaynaklar", conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    ConfigureDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynaklar yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Rename headers for better readability
            dataGridView1.Columns["kaynak_ad"].HeaderText = "Kaynak Adı";
            dataGridView1.Columns["kaynak_yazar"].HeaderText = "Yazar";
            dataGridView1.Columns["kaynak_yayıncı"].HeaderText = "Yayıncı";
            dataGridView1.Columns["kaynak_sayfasi"].HeaderText = "Sayfa Sayısı";
            dataGridView1.Columns["kaynak_basimtarihi"].HeaderText = "Basım Tarihi";

            // Hide unnecessary columns (optional, based on your actual data)
            if (dataGridView1.Columns.Contains("SensitiveColumn"))
            {
                dataGridView1.Columns["SensitiveColumn"].Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AdKaynaktext.Text) ||
                    string.IsNullOrWhiteSpace(YazarKaynaktext.Text) ||
                    string.IsNullOrWhiteSpace(YayıncıKaynaktext.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert the new record into the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Kaynaklar (kaynak_ad, kaynak_yazar, kaynak_yayıncı, kaynak_sayfasi, kaynak_basimtarihi) " +
                                   "VALUES (@kaynak_ad, @kaynak_yazar, @kaynak_yayıncı, @kaynak_sayfasi, @kaynak_basimtarihi)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@kaynak_ad", AdKaynaktext.Text);
                    cmd.Parameters.AddWithValue("@kaynak_yazar", YazarKaynaktext.Text);
                    cmd.Parameters.AddWithValue("@kaynak_yayıncı", YayıncıKaynaktext.Text);
                    cmd.Parameters.AddWithValue("@kaynak_sayfasi", numericUpDown1.Value);
                    cmd.Parameters.AddWithValue("@kaynak_basimtarihi", dateTimePicker1.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Kaynak başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadKaynaklar(); // Refresh the DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynak eklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Placeholder for future enhancements
        }
    }
}
