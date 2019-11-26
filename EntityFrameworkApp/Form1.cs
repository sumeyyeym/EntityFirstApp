using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityFrameworkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        NorthwindEntities db = new NorthwindEntities();
        private void btnKategori_Click(object sender, EventArgs e)
        {
            dgvKategoriler.DataSource = db.Categories.ToList();
        }

        private void btnPersonel_Click(object sender, EventArgs e)
        {
            dgvPersoneller.DataSource = db.Employees.ToList();
        }
    }
}
