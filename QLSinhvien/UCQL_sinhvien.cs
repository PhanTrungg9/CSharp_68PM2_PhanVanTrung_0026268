using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSinhvien
{
    public partial class UCQL_sinhvien : UserControl
    {
        DatabaseDataContext db = new DatabaseDataContext();
        int currentPage = 1;
        int pageSize = 5;
        int totalRecords = 0;
        int totalPages = 1;
        public UCQL_sinhvien()
        {
            InitializeComponent();
            button6.Click += button6_Click;
            button7.Click += button7_Click;
            button9.Click += button9_Click;
            button8.Click += button8_Click;
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
            totalRecords = db.SinhViens.Count();

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (currentPage < 1)
            {
                currentPage = 1;
            }

            if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            var ds = db.SinhViens
                .OrderBy(sv => sv.id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
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

            label7.Text = $"Trang {currentPage}/{totalPages} | {totalRecords} bản ghi";

            button6.Enabled = currentPage > 1;
            button7.Enabled = currentPage > 1;
            button9.Enabled = currentPage < totalPages;
            button8.Enabled = currentPage < totalPages;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            LoadData();
        }
        private void LoadDSLHCBX()
        {
            List<Lop> lop = db.Lops.ToList();
            cb_Lop.DataSource = lop;
            cb_Lop.DisplayMember = "tenlop"; 
            cb_Lop.ValueMember = "id";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadData();
            txt_MSSV.ReadOnly = false;
            txtSearch.Clear();
        }
        private void ClearForm()
        {
            txt_MSSV.Clear();
            txt_HoTen.Clear();
            cb_Gt.SelectedIndex = -1;
            cb_Lop.SelectedIndex = -1;
            dtp_NgaySinh.Value = DateTime.Now;
        }

        private void dgv_DSSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_DSSV.Rows[e.RowIndex];

                txt_MSSV.Text = row.Cells["id"].Value.ToString();
                txt_HoTen.Text = row.Cells["hoten"].Value.ToString();
                cb_Gt.Text = row.Cells["gioitinh"].Value.ToString();

                if (row.Cells["ngaysinh"].Value != null)
                {
                    dtp_NgaySinh.Value = Convert.ToDateTime(row.Cells["ngaysinh"].Value);
                }

                if (row.Cells["malop"].Value != null)
                {
                    cb_Lop.SelectedValue = Convert.ToInt32(row.Cells["malop"].Value);
                }
                txt_MSSV.ReadOnly = true;
            }
        }
        

    }
}
