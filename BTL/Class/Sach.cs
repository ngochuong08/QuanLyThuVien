using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BTL
{
    class Sach
    {
        SqlConnection sqlConn;
        string cnStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        DBQuanLyThuVienDataContext QLThuVienDC;

        public Sach()
        {
            sqlConn = new SqlConnection(cnStr);
            QLThuVienDC = new DBQuanLyThuVienDataContext(sqlConn);
        }

        public IEnumerable<SACH> GetListBook(int position, int nRecord)
        {
            IEnumerable<SACH> sach = QLThuVienDC.GetTable<SACH>()
                                    .OrderByDescending(s=>s.MaSach)
                                    .Skip(position)
                                    .Take(nRecord).ToList(); ;
            return sach;
        }
        public int TotalBook()
        {
            return QLThuVienDC.GetTable<SACH>().Count();
        }
        public IEnumerable<SACH> FindBook(string keyword="", string namXBFrom = "", string namXBTo = "", double giaFrom = -1, double giaTo = -1, string ngayNhapFrom ="", string ngayNhapTo="")
        {
            IEnumerable<SACH> sach = QLThuVienDC.SACHes;
            if (keyword != "")
            {
                sach = sach.Where(s => s.TenSach.ToLower().Contains(keyword.ToLower()) ||
                                s.TacGia.ToLower().Contains(keyword.ToLower()) ||
                                s.NhaXuatBan.ToLower().Contains(keyword.ToLower())
                          ).Select(s => s);
            }
            if (namXBFrom != "" && namXBTo != "")
            {
                sach = sach.Where(s => s.NamXuatBan >= DateTime.Parse(namXBFrom) && 
                                        s.NamXuatBan <= DateTime.Parse(namXBTo)
                                ).Select(s => s);
            }
            if (ngayNhapFrom != "" && ngayNhapTo != "")
            {
                sach = sach.Where(s => s.NgayNhap >= DateTime.Parse(ngayNhapFrom) && 
                                    s.NamXuatBan <= DateTime.Parse(ngayNhapTo)
                                ).Select(s => s);
            }
            if (giaFrom != -1 && giaTo != -1)
            {
                sach = sach.Where(s => Convert.ToDouble(s.TriGia) >= giaFrom &&
                                        Convert.ToDouble(s.TriGia) <= giaTo
                                 ).Select(s => s);
            }
            // sach.ToList().ForEach(i => Debug.WriteLine(i.TenSach));
            return sach;
        }
        public bool CheckBookExistInChiTietPhieuMuon(int idSach)
        {
            int data = (from pm in QLThuVienDC.CHITIETPHIEUMUONs
                        where pm.MaSach == idSach
                        select pm
                    ).Count();
            if (data > 0) return true;
            return false;
        }
        public void DeleteBook(int idSach)
        {
            SACH sach = QLThuVienDC.SACHes.FirstOrDefault(s => s.MaSach.Equals(idSach));
            // Debug.WriteLine(sach.MaSach);
            QLThuVienDC.SACHes.DeleteOnSubmit(sach);
            QLThuVienDC.SubmitChanges();
        }

        public void AddBook(string tenSach, string tacGia, string namXB, string NXB, string gia, string ngayNhap)
        {
            SACH e = new SACH();
            e.TenSach = tenSach;
            e.TacGia = tacGia;
            e.NhaXuatBan = NXB;
            e.TriGia = gia;
            e.NamXuatBan = DateTime.Parse(namXB);
            e.NgayNhap = DateTime.Parse(ngayNhap);
            e.isActive = 1;
            QLThuVienDC.SACHes.InsertOnSubmit(e);
            QLThuVienDC.SubmitChanges();
        }

        public void UpdateBook(int idSach, string tenSach, string tacGia, string namXB, string NXB, string gia, string ngayNhap)
        {
            SACH e = QLThuVienDC.SACHes.FirstOrDefault(s => s.MaSach.Equals(idSach));
            e.TenSach = tenSach;
            e.TacGia = tacGia;
            e.NhaXuatBan = NXB;
            e.TriGia = gia;
            e.NamXuatBan = DateTime.Parse(namXB);
            e.NgayNhap = DateTime.Parse(ngayNhap);
            QLThuVienDC.SubmitChanges();
        }
    }
}
