using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing; // Ensure you have this using directive for Image

namespace Quanlyview
{
    public partial class Quanly : Form
    {
        public List<Employee> lstEmp = new List<Employee>();
        private BindingSource bs = new BindingSource();
        public bool isThoat = true;
        public event EventHandler DangXuat;
        private string employeeImagePath = string.Empty; // Store the image path

        public Quanly()
        {
            InitializeComponent();
            SetupImageList();

            //ngay sinh
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMMM yyyy";
            // Handle value changes (optional)
            dateTimePicker1.ShowUpDown = true;
        }

        private void Quanly_Load(object sender, EventArgs e)
        {
            lstEmp = GetData();
            bs.DataSource = lstEmp;
            dgvEmployee.DataSource = bs;
            SetupDataGridView(); // Setup DataGridView columns
            dateTimePicker1.Value = DateTime.Now; // Set the default date to now

        }

        public List<Employee> GetData()
        {
            // Sample data can be added here if needed
            return lstEmp;
        }

        private void SetupDataGridView()
        {
            dgvEmployee.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployee.Columns[0].HeaderText = "Mã";
            dgvEmployee.Columns[1].HeaderText = "Tên";
            dgvEmployee.Columns[2].HeaderText = "Giới Tính";
            dgvEmployee.Columns[3].HeaderText = "Mã Lớp";
            dgvEmployee.Columns[4].HeaderText = "Số Điện Thoại";
            dgvEmployee.Columns[5].HeaderText = "Địa Chỉ";
            dgvEmployee.Columns[6].HeaderText = "Ảnh";
            dgvEmployee.Columns[7].HeaderText = "Ngày Sinh";

            // Đặt toàn bộ DataGridView thành chỉ đọc
            dgvEmployee.ReadOnly = true;

            // Tùy chọn: Thay đổi màu ô để dễ nhận biết
            dgvEmployee.DefaultCellStyle.BackColor = Color.LightGray;

            // Thêm xử lý khi người dùng cố gắng chỉnh sửa ô
            dgvEmployee.CellBeginEdit += (s, e) =>
            {
                MessageBox.Show("Lỗi: Không thể chỉnh sửa dữ liệu trực tiếp trong bảng.");
                e.Cancel = true; // Hủy bỏ việc chỉnh sửa
            };
        }


        private void btThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btDangXuat_Click(object sender, EventArgs e)
        {
            DangXuat?.Invoke(this, EventArgs.Empty);
        }

        private void Quanly_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isThoat) Application.Exit();
        }

    

        private void btAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbId.Text) ||
                    string.IsNullOrWhiteSpace(tbName.Text) ||
                    string.IsNullOrWhiteSpace(tbAddress.Text) ||
                    string.IsNullOrWhiteSpace(tbMaduan.Text) ||
                    string.IsNullOrWhiteSpace(cbMaphongban.Text))
                {
                    MessageBox.Show("Lỗi: Bạn cần nhập đầy đủ tất cả các trường.");
                    return;
                }

                if (!int.TryParse(tbId.Text, out int newId))
                {
                    MessageBox.Show("Lỗi: ID phải là chữ số.");
                    return;
                }

                if (!IsAlphabetic(tbName.Text))
                {
                    MessageBox.Show("Lỗi: Họ tên phải chỉ chứa các ký tự chữ cái.");
                    return;
                }

                if (!IsValidPhoneNumber(tbMaduan.Text))
                {
                    MessageBox.Show("Lỗi: Số điện thoại phải bắt đầu bằng 0 và có 10 chữ số.");
                    return;
                }

                if (lstEmp.Any(emp => emp.Id == newId))
                {
                    MessageBox.Show("Lỗi: Mã ID đã tồn tại. Vui lòng nhập ID khác.");
                    return;
                }

                Employee newEmp = new Employee
                {
                    Id = newId,
                    Name = tbName.Text,
                    Gender = ckGender.Checked,
                    Address = tbAddress.Text,
                    Maduan = tbMaduan.Text,
                    Maphongban = cbMaphongban.Text,
                    ImagePath = employeeImagePath,
                    BirthDate = dateTimePicker1.Value.Date
                };

                lstEmp.Add(newEmp);
                bs.ResetBindings(false);
                ClearInputFields();
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi: Nhập số hợp lệ cho ID.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            if (dgvEmployee.CurrentRow == null) return;

            int idx = dgvEmployee.CurrentRow.Index;
            Employee em = lstEmp[idx];

            try
            {
                if (string.IsNullOrWhiteSpace(tbId.Text) ||
                    string.IsNullOrWhiteSpace(tbName.Text) ||
                    string.IsNullOrWhiteSpace(tbAddress.Text) ||
                    string.IsNullOrWhiteSpace(tbMaduan.Text) ||
                    string.IsNullOrWhiteSpace(cbMaphongban.Text))
                {
                    MessageBox.Show("Lỗi: Bạn cần nhập đầy đủ tất cả các trường.");
                    return;
                }

                if (!int.TryParse(tbId.Text, out int id))
                {
                    MessageBox.Show("Lỗi: ID phải là chữ số.");
                    return;
                }

                if (!IsAlphabetic(tbName.Text))
                {
                    MessageBox.Show("Lỗi: Họ tên phải chỉ chứa các ký tự chữ cái.");
                    return;
                }

                if (!IsValidPhoneNumber(tbMaduan.Text))
                {
                    MessageBox.Show("Lỗi: Số điện thoại phải bắt đầu bằng 0 và có 10 chữ số.");
                    return;
                }

                em.Id = id;
                em.Name = tbName.Text;
                em.Gender = ckGender.Checked;
                em.Address = tbAddress.Text;
                em.Maduan = tbMaduan.Text;
                em.Maphongban = cbMaphongban.Text;
                em.ImagePath = employeeImagePath;
                em.BirthDate = dateTimePicker1.Value.Date;
                bs.ResetBindings(false);
                ClearInputFields();
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi: Nhập số hợp lệ cho ID.");
            }
        }

        // Hàm kiểm tra nếu chuỗi chỉ chứa chữ cái
        private bool IsAlphabetic(string text)
        {
            return text.All(char.IsLetter);
        }

        // Hàm kiểm tra số điện thoại hợp lệ
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) &&
                   phoneNumber.Length == 10 &&
                   phoneNumber.StartsWith("0") &&
                   phoneNumber.All(char.IsDigit);
        }







        private void btDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployee.CurrentRow == null) return;

            int idx = dgvEmployee.CurrentRow.Index;
            lstEmp.RemoveAt(idx);
            bs.ResetBindings(false);
        }

        private void dgvEmployee_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= lstEmp.Count) return;

            Employee em = lstEmp[e.RowIndex];

            tbId.Text = em.Id.ToString();
            tbName.Text = em.Name;
            ckGender.Checked = em.Gender;
            tbAddress.Text = em.Address;
            tbMaduan.Text = em.Maduan;
            cbMaphongban.Text = em.Maphongban;

            // Đảm bảo BirthDate nằm trong phạm vi của DateTimePicker
            DateTime validDate = em.BirthDate < dateTimePicker1.MinDate ? dateTimePicker1.MinDate
                               : em.BirthDate > dateTimePicker1.MaxDate ? dateTimePicker1.MaxDate
                               : em.BirthDate;

            dateTimePicker1.Value = validDate;

            // Tải ảnh của nhân viên nếu tồn tại
            if (!string.IsNullOrEmpty(em.ImagePath) && System.IO.File.Exists(em.ImagePath))
            {
                pbEmployeeImage.Image = Image.FromFile(em.ImagePath);
            }
            else
            {
                pbEmployeeImage.Image = null; // Xóa ảnh nếu không có
            }
        }

        private void ClearInputFields()
        {
            tbId.Text = "";
            tbName.Text = "";
            tbAddress.Text = "";
            tbMaduan.Text = "";
            cbMaphongban.Text = "";
            ckGender.Checked = false;
            pbEmployeeImage.Image = null; // Clear image display
            dateTimePicker1.Value = DateTime.Now; // Reset DateTimePicker to current date
        }

        private void SetupImageList()
        {
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(24, 24);


            btAddNew.ImageList = imageList;
            btAddNew.ImageIndex = 0;

            btEdit.ImageList = imageList;
            btEdit.ImageIndex = 1;

            btDelete.ImageList = imageList;
            btDelete.ImageIndex = 2;
        }

        private void btSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    employeeImagePath = ofd.FileName; // Store the image path
                    pbEmployeeImage.Image = Image.FromFile(employeeImagePath); // Show the image
                }
            }
        }

        // Method to set a specific date for the DateTimePicker (if needed)
        private void SetDateForDateTimePicker(DateTime date)
        {
            dateTimePicker1.Value = date;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value;

            this.Text = dateTimePicker1.Value.ToString("dd MMMM yyyy");
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void label8_Click(object sender, EventArgs e)
        {
            // Code xử lý khi nhấp vào label8
        }

        private void tbMaduan_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
