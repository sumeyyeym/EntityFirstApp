using System;
using System.Linq;
using System.Windows.Forms;

namespace PhoneBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        PhoneBookEntities db = new PhoneBookEntities();


        #region Kişi Listesi
        void GetList()
        {

            lviPeople.Items.Clear();

            var people = db.People.ToList();

            foreach (Person person in people)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = person.FirstName;
                lvi.SubItems.Add(person.LastName);
                lvi.SubItems.Add(person.PhoneNumber);
                lvi.SubItems.Add(person.Mail);
                lvi.Tag = person.Id; 

                lviPeople.Items.Add(lvi);
            }
        }
        #endregion

        void GetList(string param)
        {

            lviPeople.Items.Clear();

            var people = db.People.Where(x => x.FirstName.Contains(param)   ||
                                              x.LastName.Contains(param)    ||
                                              x.PhoneNumber.Contains(param) ||
                                              x.Mail.Contains(param)).ToList();

            foreach (Person person in people)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = person.FirstName;
                lvi.SubItems.Add(person.LastName);
                lvi.SubItems.Add(person.PhoneNumber);
                lvi.SubItems.Add(person.Mail);
                lvi.Tag = person.Id;

                lviPeople.Items.Add(lvi);
            }
        }

        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            GetList();
            GetList(txtSearch.Text);
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            Person person = new Person();
            person.FirstName = txtFirstName.Text;
            person.LastName = txtLastName.Text;
            person.PhoneNumber = txtPhone.Text;
            person.Mail = txtMail.Text;

            db.People.Add(person);
            bool result = db.SaveChanges() > 0;
            MessageBox.Show(result ? "Kayıt Eklendi" : "Kayıt Ekleme Hatası");
            GetList();
            Clean();
        }
         

        void Clean()
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }
        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            txtFirstName.Text = FakeData.NameData.GetFirstName();
            txtLastName.Text = FakeData.NameData.GetSurname();
            txtPhone.Text = FakeData.PhoneNumberData.GetPhoneNumber();
            txtMail.Text = FakeData.NetworkData.GetEmail().ToLower();
        }

        private void ToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void TsmSil_Click(object sender, EventArgs e)
        {
            if (lviPeople.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silme işlemi için bir eleman seçiniz");
                return;
            }
            DialogResult dr = MessageBox.Show("Kayıt kalıcı olarak silinecektir. İşleme devam etmek istiyor musunuz?", "Kayıt Silme Bildirimi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dr == DialogResult.Yes)
            {
                int id = (int)lviPeople.SelectedItems[0].Tag;
                Person person = db.People.FirstOrDefault(x => x.Id == id);
                if (person != null)
                {
                    db.People.Remove(person);
                    db.SaveChanges();
                    GetList();
                }
            }
        }

        Person guncellenecek;
        private void TsmDuzenle_Click(object sender, EventArgs e)
        {
            if (lviPeople.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen güncelleme için bir eleman seçiniz!");
                return;
            }

            int id = (int)lviPeople.SelectedItems[0].Tag;
            guncellenecek = db.People.FirstOrDefault(x => x.Id == id);
            if (guncellenecek != null)
            {
                txtFirstName.Text = guncellenecek.FirstName;
                txtLastName.Text = guncellenecek.LastName;
                txtMail.Text = guncellenecek.Mail;
                txtPhone.Text = guncellenecek.PhoneNumber;
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (guncellenecek == null)
            {
                MessageBox.Show("Bu kaydı güncelleyemezsiniz!");
                return;
            }

            guncellenecek.FirstName = txtFirstName.Text;
            guncellenecek.LastName = txtLastName.Text;
            guncellenecek.Mail = txtMail.Text;
            guncellenecek.PhoneNumber = txtPhone.Text;

            db.SaveChanges();
            GetList();
            Clean();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
































/*
  
CREATE DATABASE PhoneBook;
GO

USE PhoneBook;
GO

CREATE TABLE People ( 
             Id          INT PRIMARY KEY IDENTITY(1 , 1) , 
             FirstName   NVARCHAR(50) NOT NULL , 
             LastName    NVARCHAR(50) NOT NULL , 
             PhoneNumber NVARCHAR(24) NULL , 
             Mail        NVARCHAR(150) NULL
                    );    
    
 */
