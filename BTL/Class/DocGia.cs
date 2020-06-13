using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL
{
    class DocGia
    {
        SqlConnection sqlConn;
        string cnStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        DBQuanLyThuVienDataContext QLThuVienDC;

        public DocGia()
        {
            sqlConn = new SqlConnection(cnStr);
            QLThuVienDC = new DBQuanLyThuVienDataContext(sqlConn);
        }

        public IEnumerable<DOCGIA> GetDSDocGia()
        {
            IEnumerable<DOCGIA> emp = QLThuVienDC.DOCGIAs.OrderByDescending(d=>d.MaDocGia);
            return emp;
        }

        public bool CheckDocGiaHasForeignKey(int idDocGia)
        {
            int data = (from pm in QLThuVienDC.PHIEUMUONSACHes
                        where pm.MaDocGia == idDocGia
                        select pm
                    ).Count();
            if (data > 0) return true;
            data = (from pm in QLThuVienDC.PHIEUTHUTIENs
                        where pm.MaDocGia == idDocGia
                        select pm
                    ).Count();
            if (data > 0) return true;
            return false;
        }
        public void DeleteDocGia(int idDocGia)
        {
            DOCGIA docgia = QLThuVienDC.DOCGIAs.FirstOrDefault(s => s.MaDocGia.Equals(idDocGia));
            QLThuVienDC.DOCGIAs.DeleteOnSubmit(docgia);
            QLThuVienDC.SubmitChanges();
        }

        public void AddDocGia(string hoten, string bd, string add, string email, string ngayLapthe, string ngayHethan, int tienNo)
        {
            DOCGIA e = new DOCGIA();
            e.HoTenDocGia = hoten;
            e.NgaySinh = DateTime.Parse(bd);
            e.DiaChi = add;
            e.Email = email;
            e.TienNo = tienNo;
            e.NgayLapThe = DateTime.Parse(ngayLapthe);
            e.NgayHetHan = DateTime.Parse(ngayHethan);
            e.isActive = 1;
            QLThuVienDC.DOCGIAs.InsertOnSubmit(e);
            QLThuVienDC.SubmitChanges();
        }

        public void UpdateDocGia(int idDocGia, string hoten, string bd, string add, string email, string ngayLapthe, string ngayHethan, int tienNo)
        {
            DOCGIA e = QLThuVienDC.DOCGIAs.FirstOrDefault(s => s.MaDocGia.Equals(idDocGia));
            e.HoTenDocGia = hoten;
            e.NgaySinh = DateTime.Parse(bd);
            e.DiaChi = add;
            e.Email = email;
            e.NgayLapThe = DateTime.Parse(ngayLapthe);
            e.NgayHetHan = DateTime.Parse(ngayHethan);
            e.TienNo = tienNo;
            QLThuVienDC.SubmitChanges();
        }
    }
}
