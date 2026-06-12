using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSinhvien
{
    public partial class UCQL_lophoc : UserControl
    {
        DatabaseDataContext db = new DatabaseDataContext();

        int currentPage = 1;
        int pageSize = 5;
        int totalRecords = 0;
        int totalPages = 1;
        string keyword = "";

        public UCQL_lophoc()
        {
            InitializeComponent();

            dgv_QLLH.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_QLLH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv_QLLH.CellClick += dgv_QLLH_CellClick;

            btn_add.Click += btn_add_Click;
            btn_update.Click += btn_update_Click;
            btn_delete.Click += btn_delete_Click;
            btn_reload.Click += btn_reload_Click;
            btn_listSV.Click += btn_listSV_Click;

            button5.Click += button5_Click;

            button6.Click += button6_Click; // <<
            button7.Click += button7_Click; // <
            button9.Click += button9_Click; // >
            button8.Click += button8_Click; // >>
        }

        private void UCQL_lophoc_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var query = db.Lops.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                int idSearch;
                bool isNumber = int.TryParse(keyword, out idSearch);

                query = query.Where(l =>
                    l.malop.Contains(keyword) ||
                    l.tenlop.Contains(keyword) ||
                    (isNumber && l.id == idSearch)
                );
            }

            totalRecords = query.Count();

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

            var ds = query
                .OrderBy(l => l.id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new
                {
                    l.id,
                    l.malop,
                    l.tenlop,
                    l.ghichu
                })
                .ToList();

            dgv_QLLH.DataSource = ds;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                label7.Text = $"Trang {currentPage}/{totalPages} | {totalRecords} bản ghi";
            }
            else
            {
                label7.Text = $"Trang {currentPage}/{totalPages} | Tìm thấy {totalRecords} bản ghi";
            }

            button6.Enabled = currentPage > 1;
            button7.Enabled = currentPage > 1;
            button9.Enabled = currentPage < totalPages;
            button8.Enabled = currentPage < totalPages;
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_id.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập mã ID!");
                    return;
                }

                if (txt_malop.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập mã lớp!");
                    return;
                }

                if (txt_tenlop.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập tên lớp!");
                    return;
                }

                int id;

                if (!int.TryParse(txt_id.Text.Trim(), out id))
                {
                    MessageBox.Show("Mã ID phải là số!");
                    return;
                }

                Lop checkId = db.Lops.FirstOrDefault(l => l.id == id);

                if (checkId != null)
                {
                    MessageBox.Show("Mã ID này đã tồn tại!");
                    return;
                }

                Lop checkMaLop = db.Lops.FirstOrDefault(l => l.malop == txt_malop.Text.Trim());

                if (checkMaLop != null)
                {
                    MessageBox.Show("Mã lớp này đã tồn tại!");
                    return;
                }

                Lop lop = new Lop();

                lop.id = id;
                lop.malop = txt_malop.Text.Trim();
                lop.tenlop = txt_tenlop.Text.Trim();
                lop.ghichu = txt_ghichu.Text.Trim();

                db.Lops.InsertOnSubmit(lop);
                db.SubmitChanges();

                totalRecords = db.Lops.Count();
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                currentPage = totalPages;

                keyword = "";
                textBox3.Clear();

                MessageBox.Show("Thêm lớp học thành công!");

                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_id.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng chọn lớp học cần sửa!");
                    return;
                }

                int id = Convert.ToInt32(txt_id.Text.Trim());

                Lop lop = db.Lops.FirstOrDefault(l => l.id == id);

                if (lop == null)
                {
                    MessageBox.Show("Không tìm thấy lớp học cần sửa!");
                    return;
                }

                if (txt_malop.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập mã lớp!");
                    return;
                }

                if (txt_tenlop.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập tên lớp!");
                    return;
                }

                Lop checkMaLop = db.Lops.FirstOrDefault(l =>
                    l.malop == txt_malop.Text.Trim() && l.id != id
                );

                if (checkMaLop != null)
                {
                    MessageBox.Show("Mã lớp này đã tồn tại ở lớp khác!");
                    return;
                }

                lop.malop = txt_malop.Text.Trim();
                lop.tenlop = txt_tenlop.Text.Trim();
                lop.ghichu = txt_ghichu.Text.Trim();

                db.SubmitChanges();

                MessageBox.Show("Sửa lớp học thành công!");

                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_id.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng chọn lớp học cần xóa!");
                    return;
                }

                int id = Convert.ToInt32(txt_id.Text.Trim());

                Lop lop = db.Lops.FirstOrDefault(l => l.id == id);

                if (lop == null)
                {
                    MessageBox.Show("Không tìm thấy lớp học cần xóa!");
                    return;
                }

                int soSinhVien = db.SinhViens.Count(sv => sv.malop == id);

                if (soSinhVien > 0)
                {
                    MessageBox.Show("Không thể xóa lớp này vì đang có sinh viên thuộc lớp này!");
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn xóa lớp học này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    db.Lops.DeleteOnSubmit(lop);
                    db.SubmitChanges();

                    MessageBox.Show("Xóa lớp học thành công!");

                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            keyword = "";
            textBox3.Clear();

            currentPage = 1;

            LoadData();
            ClearForm();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            keyword = textBox3.Text.Trim();

            if (keyword == "")
            {
                MessageBox.Show("Vui lòng nhập từ khóa cần tìm!");
                return;
            }

            currentPage = 1;
            LoadData();

            if (totalRecords == 0)
            {
                MessageBox.Show("Không tìm thấy lớp học phù hợp!");
            }
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

        private void dgv_QLLH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_QLLH.Rows[e.RowIndex];

                txt_id.Text = row.Cells["id"].Value.ToString();
                txt_malop.Text = row.Cells["malop"].Value.ToString();
                txt_tenlop.Text = row.Cells["tenlop"].Value.ToString();

                if (row.Cells["ghichu"].Value != null)
                {
                    txt_ghichu.Text = row.Cells["ghichu"].Value.ToString();
                }
                else
                {
                    txt_ghichu.Text = "";
                }

                txt_id.ReadOnly = true;
            }
        }

        private void ClearForm()
        {
            txt_id.Clear();
            txt_malop.Clear();
            txt_tenlop.Clear();
            txt_ghichu.Clear();

            txt_id.ReadOnly = false;
        }

        private void btn_listSV_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_id.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng chọn lớp học để xem danh sách sinh viên!");
                    return;
                }

                int idLop = Convert.ToInt32(txt_id.Text.Trim());

                var dsSinhVien = db.SinhViens
                    .Where(sv => sv.malop == idLop)
                    .Select(sv => new
                    {
                        sv.id,
                        sv.hoten,
                        sv.gioitinh,
                        sv.ngaysinh,
                        sv.malop
                    })
                    .ToList();

                dgv_QLLH.DataSource = dsSinhVien;

                label7.Text = $"Danh sách sinh viên | {dsSinhVien.Count} bản ghi";

                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem danh sách sinh viên: " + ex.Message);
            }
        }
    }
}
