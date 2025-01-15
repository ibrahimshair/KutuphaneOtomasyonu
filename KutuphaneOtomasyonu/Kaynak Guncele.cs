using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Kaynak_Guncele : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Kaynak_Guncele()
        {
            InitializeComponent();
            Location = new System.Drawing.Point(35, 35);
        }

        private void Kaynak_Guncele_Load(object sender, EventArgs e)
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

            // Configure column headers and visibility
            dataGridView1.Columns["kaynak_id"].Visible = false;  // Hide 'kaynak_id' column
            dataGridView1.Columns["kaynak_ad"].HeaderText = "Kaynak Adı";
            dataGridView1.Columns["kaynak_yazar"].HeaderText = "Yazar";
            dataGridView1.Columns["kaynak_yayıncı"].HeaderText = "Yayıncı";
            dataGridView1.Columns["kaynak_sayfasi"].HeaderText = "Sayfa Sayısı";
            dataGridView1.Columns["kaynak_basimtarihi"].HeaderText = "Basım Tarihi";
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            try
            {
                // Populate textboxes with selected row's data
                AdKaynaktext.Text = dataGridView1.CurrentRow.Cells["kaynak_ad"].Value.ToString();
                YazarKaynaktext.Text = dataGridView1.CurrentRow.Cells["kaynak_yazar"].Value.ToString();
                YayıncıKaynaktext.Text = dataGridView1.CurrentRow.Cells["kaynak_yayıncı"].Value.ToString();
                numericUpDown1.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["kaynak_sayfasi"].Value);
                dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["kaynak_basimtarihi"].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hücre verisi işlenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz bir kaynak seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get the selected 'kaynak_id' from the row
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["kaynak_id"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Kaynaklar SET kaynak_ad = @kaynak_ad, kaynak_yazar = @kaynak_yazar, kaynak_yayıncı = @kaynak_yayıncı, " +
                                   "kaynak_sayfasi = @kaynak_sayfasi, kaynak_basimtarihi = @kaynak_basimtarihi WHERE kaynak_id = @kaynak_id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@kaynak_id", id);
                    cmd.Parameters.AddWithValue("@kaynak_ad", AdKaynaktext.Text.Trim());
                    cmd.Parameters.AddWithValue("@kaynak_yazar", YazarKaynaktext.Text.Trim());
                    cmd.Parameters.AddWithValue("@kaynak_yayıncı", YayıncıKaynaktext.Text.Trim());
                    cmd.Parameters.AddWithValue("@kaynak_sayfasi", Convert.ToInt16(numericUpDown1.Value));
                    cmd.Parameters.AddWithValue("@kaynak_basimtarihi", dateTimePicker1.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                // Reload data grid to reflect updated source list
                LoadKaynaklar();

                MessageBox.Show("Kaynak başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynak güncellenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Not used but left for future functionality if needed
        }
    }
}
