using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class Form2 : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\KutuphaneOtomasyon.mdf;Integrated Security=True";

        private enum PanelType { User, Resource, Loan, Return }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            HideAllPanels();
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Veritabanı bağlantısı başarılı.", "Bağlantı Testi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı bağlantısı başarısız: {ex.Message}", "Bağlantı Testi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HideAllPanels()
        {
            KUEklebtn.Visible = false;
            KUGuncellebtn.Visible = false;
            KUSilbtn.Visible = false;
            EkleKayanakbtn.Visible = false;
            GunceleKaynakbtn.Visible = false;
            SilKaynakbtn.Visible = false;
        }

        private void TogglePanel(PanelType panel)
        {
            HideAllPanels();

            switch (panel)
            {
                case PanelType.User:
                    KUEklebtn.Visible = true;
                    KUGuncellebtn.Visible = true;
                    KUSilbtn.Visible = true;
                    break;
                case PanelType.Resource:
                    EkleKayanakbtn.Visible = true;
                    GunceleKaynakbtn.Visible = true;
                    SilKaynakbtn.Visible = true;
                    break;
                case PanelType.Loan:
                    // Additional code for loan panel (if any)
                    break;
                case PanelType.Return:
                    // Additional code for return panel (if any)
                    break;
            }
        }

        private void CloseAllChildForms()
        {
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }
        }

        private void ShowChildForm<T>() where T : Form, new()
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is T)
                {
                    child.Close();
                    return;
                }
            }

            CloseAllChildForms();
            T form = new T
            {
                MdiParent = this,
                StartPosition = FormStartPosition.Manual,
                Location = new Point(0, 0)
            };
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TogglePanel(PanelType.User);
            ShowChildForm<Form3>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TogglePanel(PanelType.Resource);
            ShowChildForm<Kaynak_Liste>();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TogglePanel(PanelType.Loan);
            ShowChildForm<Odunc_Ver>();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TogglePanel(PanelType.Return);
            ShowChildForm<Geri_Alma>();
        }

        private void KUEklebtn_Click(object sender, EventArgs e)
        {
            ShowChildForm<Form4>();
        }

        private void KUGuncellebtn_Click(object sender, EventArgs e)
        {
            ShowChildForm<Form6>();
        }

        private void KUSilbtn_Click(object sender, EventArgs e)
        {
            ShowChildForm<Form5>();
        }

        private void EkleKayanakbtn_Click(object sender, EventArgs e)
        {
            ShowChildForm<Kaynak_Ekle>();
        }

        private void GunceleKaynakbtn_Click(object sender, EventArgs e)
        {
            ShowChildForm<Kaynak_Guncele>();
        }

        private void SilKaynakbtn_Click(object sender, EventArgs e)
        {
            ShowChildForm<Kaynak_Sil>();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
