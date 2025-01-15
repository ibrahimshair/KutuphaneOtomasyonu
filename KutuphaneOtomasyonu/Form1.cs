using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private void Giris_Click(object sender, EventArgs e)
        {
            string ad = AdGiris.Text;
            string sifre = SifreGiris.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Buradaki sütun adlarını veritabanınızdaki doğru isimlerle değiştirdiğinizden emin olun
                string query = "SELECT * FROM Personeller WHERE personel_ad = @ad AND personel_sifre = @sifre";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ad", ad);
                command.Parameters.AddWithValue("@sifre", sifre);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // Başarılı giriş
                        MessageBox.Show("Başarılı");
                        Form2 form2 = new Form2();
                        this.Hide();
                        form2.ShowDialog();
                    }
                    else
                    {
                        // Geçersiz kullanıcı adı veya şifre
                        MessageBox.Show("Kullanıcı Adı Yada Şifre Hatalı");
                    }
                }
                catch (Exception ex)
                {
                    // SQL veya bağlantı hatalarını yakalama
                    MessageBox.Show("Bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde çalışması gereken kodlar (isteğe bağlı)
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Form kapandığında uygulamanın düzgün kapanmasını sağla
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Etiket tıklama işlemi için kod (gerektiğinde)
        }
    }
}
