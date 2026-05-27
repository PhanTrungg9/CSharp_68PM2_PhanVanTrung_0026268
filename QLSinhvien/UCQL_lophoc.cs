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
    public partial class UCQL_lophoc : UserControl
    {
        DatabaseDataContext db = new DatabaseDataContext();
        public UCQL_lophoc()
        {
            InitializeComponent();
        }

        private void UCQL_lophoc_Load(object sender, EventArgs e)
        {
            List<Lop> ds = db.Lops.ToList();
            dgv_QLLH.DataSource = ds;
            dgv_QLLH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
