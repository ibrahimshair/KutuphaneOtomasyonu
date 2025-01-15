using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Kaynak_Liste : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        public Kaynak_Liste()
        {
            InitializeComponent();
            Location = new System.Drawing.Point(35, 35); // Initialize form location
        }

        private void Kaynak_Liste_Load(object sender, EventArgs e)
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

            // Hide unnecessary columns (adjust based on your data model)
            dataGridView1.Columns["kaynak_id"].Visible = false;

            // Configure headers
            dataGridView1.Columns["kaynak_ad"].HeaderText = "Kaynak Adı";
            dataGridView1.Columns["kaynak_yazar"].HeaderText = "Yazar";
            dataGridView1.Columns["kaynak_yayıncı"].HeaderText = "Yayıncı";
            dataGridView1.Columns["kaynak_sayfasi"].HeaderText = "Sayfa Sayısı";
            dataGridView1.Columns["kaynak_basimtarihi"].HeaderText = "Basım Tarihi";

            // Adjust column widths (optional)
            dataGridView1.Columns["kaynak_ad"].Width = 150;
            dataGridView1.Columns["kaynak_yazar"].Width = 120;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Future functionality can be added here, if needed
        }
    }
}
