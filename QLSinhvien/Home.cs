using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSinhvien
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            UCQL_sinhvien uCQL = new UCQL_sinhvien();
            LoadUserControl(uCQL);

        }

        
        private void QLSinhVien_Load(object sender, EventArgs e)
        {

        }
        private void LoadUserControl(UserControl uc)
        {
            pn_main.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pn_main.Controls.Add(uc);
        }

        private void pn_main_Click(object sender, EventArgs e)
        {
            UCQL_sinhvien uCQL = new UCQL_sinhvien();
            pn_main.Controls.Clear();
            pn_main.Controls.Add(uCQL);
        }

        private void homeToolStripMenuItem_DoubleClick(object sender, EventArgs e)
        {
            UCQL_sinhvien uCQL = new UCQL_sinhvien();
            pn_main.Controls.Clear();
            pn_main.Controls.Add(uCQL);
        }
        private void sinhVienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UCQL_lophoc uCLH = new UCQL_lophoc();
            pn_main.Controls.Clear();
            pn_main.Controls.Add(uCLH);
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UCQL_sinhvien uCQL = new UCQL_sinhvien();
            pn_main.Controls.Clear();
            pn_main.Controls.Add(uCQL);
        }
    }
}
