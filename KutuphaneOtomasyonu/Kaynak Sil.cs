using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Kaynak_Sil : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Kaynak_Sil()
        {
            InitializeComponent();
            Location = new System.Drawing.Point(35, 35); // Initialize form location
        }

        private void Kaynak_Sil_Load(object sender, EventArgs e)
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynaklar yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Lütfen silmek istediğiniz bir kaynağı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                // Delete related records from the "kayitlar" table first
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Remove related records from the "kayitlar" table
                    string deleteKayitlarQuery = "DELETE FROM kayitlar WHERE kitap_id = @id";
                    using (SqlCommand cmd = new SqlCommand(deleteKayitlarQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                    // Now delete the main record from the "Kaynaklar" table
                    string deleteKaynakQuery = "DELETE FROM Kaynaklar WHERE kaynak_id = @id";
                    using (SqlCommand cmd = new SqlCommand(deleteKaynakQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Kaynak ve ilişkili kayıtlar başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload the DataGridView
                LoadKaynaklar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Silme işlemi sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
