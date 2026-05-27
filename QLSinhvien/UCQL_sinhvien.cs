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
    public partial class UCQL_sinhvien : UserControl
    {
        DatabaseDataContext db = new DatabaseDataContext();
        public UCQL_sinhvien()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void UCQL_sinhvien_Load_1(object sender, EventArgs e)
        {
            dgv_DSSV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadData();
            LoadDSLHCBX();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mssv = txt_MSSV.Text;
            string hoTen = txt_HoTen.Text;
            string gioiTinh = cb_Gt.Text;
            string datetime = dtp_NgaySinh.Text;
            string lop = cb_Lop.SelectedValue.ToString();
            SinhVien sv = new SinhVien();
            sv.id = Convert.ToInt32(mssv);
            sv.hoten = hoTen;
            sv.gioitinh = gioiTinh;
            sv.malop = Convert.ToInt32(lop);
            sv.ngaysinh = DateTime.Parse(datetime);
            try
            {
                db.SinhViens.InsertOnSubmit(sv);
                db.SubmitChanges();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);


            }
        }
        private void LoadData()
        {
            var ds = db.SinhViens
           .Select(sv => new
           {
               sv.id,
               sv.hoten,
               sv.gioitinh,
               sv.ngaysinh,
               sv.malop
           })
           .ToList();
            dgv_DSSV.DataSource = ds;
        }
        private void LoadDSLHCBX()
        {
            List<Lop> lop = db.Lops.ToList();
            cb_Lop.DataSource = lop;
            cb_Lop.DisplayMember = "tenlop"; 
            cb_Lop.ValueMember = "id";
        }
    }
}
