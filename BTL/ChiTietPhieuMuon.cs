using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace BTL
{
    public partial class ChiTietPhieuMuon : Form
    {

        int idPhieuMuon = -1;
        int idSach = -1;
        public ChiTietPhieuMuon(int idPM = -1)
        {
            InitializeComponent();
            idPhieuMuon = idPM;
        }
        public void ShowDocGia()
        {
            PhieuMuon list = new PhieuMuon();
            cbDocgia.DataSource = list.GetDSDocGia();
            cbDocgia.DisplayMember = "HoTenDocGia";
            cbDocgia.ValueMember = "MaDocGia";
        }
        public void ShowSach()
        {
            PhieuMuon list = new PhieuMuon();
            clbSach.DataSource = list.GetListSach();
            clbSach.DisplayMember = "TenSach";
            clbSach.ValueMember = "MaSach";
        }
        void ShowChiTietPhieuMuon(int idPhieuMuon)
        {
            PhieuMuon list = new PhieuMuon();
            dgvThongTin.DataSource = list.GetChiTietPhieuMuon(idPhieuMuon);
        }
        void setNull()
        {
            for (int i = 0; i < clbSach.Items.Count; i++)
            {
                clbSach.SetItemCheckState(i, CheckState.Unchecked);
                clbSach.SetSelected(i, false);
            }

            dtpNgaytra.Format = DateTimePickerFormat.Custom;
            dtpNgaytra.CustomFormat = " ";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (clbSach.CheckedItems.Count > 0)
                {
                    PhieuMuon pm = new PhieuMuon();
                    List<ChiTietPhieuMuonModel> list = new List<ChiTietPhieuMuonModel>();
                    foreach (object itemChecked in clbSach.CheckedItems)
                    {
                        SACH sach = (SACH)itemChecked;
                        int idSach = sach.MaSach;
                        // kiem tra chi tiet phieu muon bi trung ma sach
                        if (pm.GetChiTietMotPhieuMuon(idPhieuMuon, idSach).Count() <= 0)
                        {
                            ChiTietPhieuMuonModel item = new ChiTietPhieuMuonModel
                            {
                                MaSach = idSach
                            };
                            list.Add(item);
                        }
                    }
                    if (list.Count > 0)
                    {
                        if (idPhieuMuon == -1)
                        {
                            idPhieuMuon = pm.ThemPhieuMuon(
                                                dtpNgayMuon.Value.ToShortDateString(),
                                                Int16.Parse(cbDocgia.SelectedValue.ToString()),
                                                list
                                            );
                            MessageBox.Show("Thêm phiếu mượn thành công");

                        }
                        else
                        {
                            pm.ThemPhieuMuon(
                                            dtpNgayMuon.Value.ToShortDateString(),
                                            Int16.Parse(cbDocgia.SelectedValue.ToString()),
                                            list,
                                            idPhieuMuon
                                        );
                            MessageBox.Show("Thêm sách vào phiếu mượn thành công");
                        }
                    }
                    ShowChiTietPhieuMuon(idPhieuMuon);
                    setNull();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sách", "Thêm chi tiết phiếu mượn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clbSach.Focus();
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thêm chi tiết phiếu mượn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    
        private void ChiTietPhieuMuon_Load(object sender, EventArgs e)
        {
            dtpNgaytra.Format = DateTimePickerFormat.Custom;
            dtpNgaytra.CustomFormat = " ";

            ShowSach();
            ShowDocGia();

            if (idPhieuMuon != -1)
            {
                PhieuMuon pm = new PhieuMuon();
                PhieuMuonModel f = pm.GetPhieuMuon(idPhieuMuon);
                cbDocgia.Text = f.TenDocGia;
                dtpNgayMuon.Text = f.NgayMuon.ToString();
            }
            ShowChiTietPhieuMuon(idPhieuMuon);
        }

        private void dtpNgaytra_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker d = sender as DateTimePicker;
            d.CustomFormat = "MM/dd/yyyy";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (clbSach.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn sách cần xóa", "Xóa chi tiết phiếu mượn");
                    return;
                }
                PhieuMuon f = new PhieuMuon();

                if (f.CountChiTietPhieuMuon(idPhieuMuon) <= 1)
                {
                    MessageBox.Show(
                        "Phiếu mượn phải có ít nhất 1 sách được mượn. Vui lòng kiểm tra lại",
                        "Xóa chi tiết phiếu mượn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
                if (dgvThongTin.SelectedCells.Count > 0)
                {
                    DialogResult dr = MessageBox.Show(
                        "Bạn có chắc chắn xóa chi tiết phiếu mượn không?",
                        "Xóa chi tiết phiếu mượn",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question
                    );
                    if (dr == DialogResult.OK)
                    {
                        foreach (object itemChecked in clbSach.CheckedItems)
                        {
                            SACH sach = (SACH)itemChecked;
                            int idSach = sach.MaSach;
                            f.DeleteChitietPhieuMuon(idSach, idPhieuMuon);
                        }
                        ShowChiTietPhieuMuon(idPhieuMuon);
                        setNull();
                    }
                }
                else MessageBox.Show("Vui lòng chọn chi tiết phiếu mượn cần xóa");
            }
            catch(Exception exx)
            {
                MessageBox.Show(
                        exx.Message,
                        "Xóa chi tiết phiếu mượn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
            }
        }

        private void dgvThongTin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int numrow = e.RowIndex;
                if (numrow >= 0)
                {
                    if (dgvThongTin.Rows[numrow].Cells[4].Value != null)
                    {
                        dtpNgaytra.Text = dgvThongTin.Rows[numrow].Cells[4].Value.ToString();
                    }
                    else
                    {
                        dtpNgaytra.Format = DateTimePickerFormat.Custom;
                        dtpNgaytra.CustomFormat = " ";
                    }
                    for (int i = 0; i < clbSach.Items.Count; i++)
                    {
                        SACH sach = (SACH)clbSach.Items[i];
                        if (sach.TenSach == dgvThongTin.Rows[numrow].Cells[1].Value.ToString())
                        {
                            clbSach.SetItemCheckState(i, CheckState.Checked);
                            idSach = sach.MaSach;
                        }
                        else
                            clbSach.SetItemCheckState(i, CheckState.Unchecked);
                    }
                    btnSua.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            if (idPhieuMuon == -1 || idSach == -1)
            {
                MessageBox.Show("Vui lòng chọn chi tiết phiếu mượn cần sửa", "Sửa phiếu mượn", MessageBoxButtons.OK);
                return;
            }
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (idPhieuMuon == -1 || idSach == -1)
                {
                    MessageBox.Show(
                        "Vui lòng chọn chi tiết phiếu mượn cần sửa",
                        "Sửa chi tiết phiếu mượn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }
                if (dtpNgaytra.Text == " ")
                {
                    MessageBox.Show(
                        "Vui lòng chọn ngày trả sách",
                        "Sửa chi tiết phiếu mượn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    dtpNgaytra.Focus();
                    return;
                }
                // kiem tra ngay tra sau ngay muon
                if (dtpNgaytra.Value < dtpNgayMuon.Value)
                {
                    MessageBox.Show(
                       "Ngày trả sách không được trước ngày mượn",
                       "Sửa chi tiết phiếu mượn",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information
                   );
                    dtpNgaytra.Focus();
                    return;
                }
                PhieuMuon pm = new PhieuMuon();
                pm.UpdateNgayTraSach(idPhieuMuon, idSach, dtpNgaytra.Value.ToShortDateString());
                ShowChiTietPhieuMuon(idPhieuMuon);
                setNull();
                btnLuu.Enabled = false;
                btnHuy.Enabled = false;
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                      ex.Message,
                      "Sửa chi tiết phiếu mượn",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error
                );
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
        }

    }
}
