using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Configuration;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Geri_Alma : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Geri_Alma()
        {
            InitializeComponent();
        }

        private void Geri_Alma_Load(object sender, EventArgs e)
        {
            // Kayitlar tablosunu yükle
            LoadData();
        }

        // Kayitlar tablosundaki veriyi yüklemek için metod
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kayitlar tablosundaki veriyi almak için SQL sorgusu
                    string query = "SELECT * FROM Kayitlar"; // 'Kayitlar' tablosunu kullanıyoruz
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable); // Veriyi DataTable'a doldur

                    // DataGridView'e veri kaynağını ata
                    dataGridView1.DataSource = dataTable;

                    ConfigureDataGridView(); // DataGridView ayarlarını yap
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıtları yüklerken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DataGridView'i yapılandırma metodu
        private void ConfigureDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 'Durum' kolonu varsa gizle (opsiyonel)
            if (dataGridView1.Columns.Contains("Durum"))
            {
                dataGridView1.Columns["Durum"].Visible = false;
            }
        }

        // Kitap geri alma işlemi butonunun tıklanması
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Lütfen geri almak istediğiniz bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Seçilen satırdaki 'Kayıt ID' değerini almak için dinamik kolon adını kullan
                string kayitIdColumnName = "kayit_id"; // Kolon adını burada güncelleyin
                if (dataGridView1.Columns.Contains(kayitIdColumnName))
                {
                    int kayitId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[kayitIdColumnName].Value);

                    // Veritabanı bağlantısı
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // SQL DELETE komutu
                        string query = "DELETE FROM Kayitlar WHERE kayit_id = @KayitID";

                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@KayitID", kayitId);

                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                // Kitap geri alındı mesajını göster
                                MessageBox.Show("Kitap başarıyla geri alındı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Veriyi yeniden yükle
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Kayıt silinemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seçilen satırda geçerli bir Kayıt ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Geri alma işlemi sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
    }
