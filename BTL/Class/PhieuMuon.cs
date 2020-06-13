using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Transactions;
using System.Data;
using System.Windows.Forms;

namespace BTL
{
    public class PhieuMuonModel
    {
        public int MaPhieuMuon { get; set; }
        public System.Nullable<DateTime> NgayMuon { get; set; }
        public string TenDocGia { get; set; }
    }
    public class ChiTietPhieuMuonModel
    {
        public int MaPhieuMuon { get; set; }
        public string TenSach { get; set; }
        public int MaSach { get; set; }
        public System.Nullable<DateTime> NgayMuon { get; set; }
        public System.Nullable<DateTime> NgayTra { get; set; }
    }
    class PhieuMuon
    {
        SqlConnection sqlConn;
        string cnStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        DBQuanLyThuVienDataContext QLThuVienDC;

        public PhieuMuon()
        {
            sqlConn = new SqlConnection(cnStr);
            QLThuVienDC = new DBQuanLyThuVienDataContext(sqlConn);
        }

        public List<PhieuMuonModel> GetDSPhieuMuon()
        {
            List<PhieuMuonModel> list = (from pm in QLThuVienDC.PHIEUMUONSACHes
                                               join d in QLThuVienDC.DOCGIAs
                                               on pm.MaDocGia equals d.MaDocGia 
                                               orderby pm.MaPhieuMuon descending
                                               select new PhieuMuonModel
                                               {
                                                    MaPhieuMuon = pm.MaPhieuMuon,
                                                    NgayMuon = pm.NgayMuon,
                                                    TenDocGia = d.HoTenDocGia
                                                }
                                     ).ToList();
            return list;
        }
        public PhieuMuonModel GetPhieuMuon(int idPhieuMuon)
        {
            PhieuMuonModel e = QLThuVienDC.PHIEUMUONSACHes.Join(
                QLThuVienDC.GetTable<DOCGIA>(), 
                pm=>pm.MaDocGia,
                dg=>dg.MaDocGia,
                (pm,dg)=> new PhieuMuonModel
                {
                    MaPhieuMuon = pm.MaPhieuMuon,
                    TenDocGia = dg.HoTenDocGia,
                    NgayMuon = pm.NgayMuon
                }
            ).FirstOrDefault(p=>p.MaPhieuMuon == idPhieuMuon);
            return e;
        }
        public List<ChiTietPhieuMuonModel> GetChiTietPhieuMuon(int idPhieuMuon)
        {
            List<ChiTietPhieuMuonModel> list = (from ct in QLThuVienDC.CHITIETPHIEUMUONs
                                         join s in QLThuVienDC.SACHes
                                         on ct.MaSach equals s.MaSach
                                         join pm in QLThuVienDC.PHIEUMUONSACHes
                                         on ct.MaPhieuMuon equals pm.MaPhieuMuon
                                         where pm.MaPhieuMuon == idPhieuMuon
                                         select new ChiTietPhieuMuonModel
                                         {
                                             MaPhieuMuon = ct.MaPhieuMuon,
                                             TenSach = s.TenSach,
                                             MaSach = s.MaSach,
                                             NgayTra = ct.NgayTra,
                                             NgayMuon = pm.NgayMuon
                                         }
                                     ).ToList();
            return list;
        }
        public IEnumerable<CHITIETPHIEUMUON> GetChiTietMotPhieuMuon(int idPhieuMuon, int idSach)
        {
            IEnumerable<CHITIETPHIEUMUON> r = from ct in QLThuVienDC.CHITIETPHIEUMUONs
                                              where ct.MaPhieuMuon == idPhieuMuon && ct.MaSach == idSach
                                              select ct;
            return r;
        }
        public IEnumerable<DOCGIA> GetDSDocGia()
        {
            IEnumerable<DOCGIA> dg = QLThuVienDC.DOCGIAs.OrderByDescending(d=>d.MaDocGia);
            return dg;
        }
        public IEnumerable<SACH> GetListSach()
        {
            IEnumerable<SACH> sach = QLThuVienDC.SACHes.OrderByDescending(s=>s.MaSach);
            return sach;
        }
       
        public void AddPhieuMuon(string ngayMuon, int idDocGia)
        {
            if (this.QLThuVienDC.Connection.State == System.Data.ConnectionState.Closed)
            {
                this.QLThuVienDC.Connection.Open();
            }
            QLThuVienDC.Transaction = QLThuVienDC.Connection.BeginTransaction();

            try
            {
                PHIEUMUONSACH e = new PHIEUMUONSACH();
                e.NgayMuon = DateTime.Parse(ngayMuon);
                e.MaDocGia = idDocGia;
                QLThuVienDC.PHIEUMUONSACHes.InsertOnSubmit(e);
                QLThuVienDC.SubmitChanges();
                QLThuVienDC.Transaction.Commit();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                QLThuVienDC.Transaction.Rollback();
            }
            finally
            {
                QLThuVienDC.Transaction.Dispose();
                this.QLThuVienDC.Connection.Close();
            }
        }
        public void AddChiTietPhieuMuon(int idPhieuMuon, int idSach)
        {
            if (this.QLThuVienDC.Connection.State == System.Data.ConnectionState.Closed)
            {
                this.QLThuVienDC.Connection.Open();
            }
            QLThuVienDC.Transaction = QLThuVienDC.Connection.BeginTransaction();
            try
            {
                CHITIETPHIEUMUON e = new CHITIETPHIEUMUON();
                e.MaPhieuMuon = idPhieuMuon;
                e.MaSach = idSach;
                QLThuVienDC.CHITIETPHIEUMUONs.InsertOnSubmit(e);
                QLThuVienDC.SubmitChanges();
                QLThuVienDC.Transaction.Commit();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                QLThuVienDC.Transaction.Rollback();
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message);
                QLThuVienDC.Transaction.Rollback();
            }
        }
        public int ThemPhieuMuon(string ngayMuon, int idDocGia, List<ChiTietPhieuMuonModel> chiTietPhieuMuon, int idPhieuMuon = -1)
        {
            try
            {
                using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(60)))
                {
                    QLThuVienDC.CommandTimeout = 60;
                    if (idPhieuMuon == -1)
                    {
                        AddPhieuMuon(ngayMuon, idDocGia);
                        idPhieuMuon = int.Parse(QLThuVienDC.GETMAXPHIEUMUON().FirstOrDefault().MaPhieuMuon.ToString());
                    }
                    foreach (ChiTietPhieuMuonModel r in chiTietPhieuMuon)
                    {
                        AddChiTietPhieuMuon(idPhieuMuon, r.MaSach);
                    }
                    t.Complete();
                }
            }
            catch (SqlException ex1)
            {
                QLThuVienDC.Transaction.Rollback();
                MessageBox.Show(ex1.Message);

            }
            catch (Exception ex)
            {
                QLThuVienDC.Transaction.Rollback();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                QLThuVienDC.Transaction.Dispose();
                this.QLThuVienDC.Connection.Close();
            }
            return idPhieuMuon;
        }

        public void UpdatePhieuMuon(int idPhieuMuon, string ngayMuon, int idDocGia)
        {
            PHIEUMUONSACH e = QLThuVienDC.PHIEUMUONSACHes.FirstOrDefault(s => s.MaPhieuMuon.Equals(idPhieuMuon));
            e.NgayMuon = DateTime.Parse(ngayMuon);
            e.MaDocGia = idDocGia;
            QLThuVienDC.SubmitChanges();
        }
        public void UpdateNgayTraSach(int idPhieuMuon, int idSach, string ngayTra)
        {
            CHITIETPHIEUMUON e = QLThuVienDC.CHITIETPHIEUMUONs.FirstOrDefault(s => 
                                s.MaPhieuMuon.Equals(idPhieuMuon) &&
                                s.MaSach.Equals(idSach)
                            );
            e.NgayTra = DateTime.Parse(ngayTra);
            QLThuVienDC.SubmitChanges();
        }
        public void DeleteChitietPhieuMuon(int idSach, int idPhieuMuon)
        {
            CHITIETPHIEUMUON ct = QLThuVienDC.CHITIETPHIEUMUONs.FirstOrDefault(s => s.MaSach.Equals(idSach) && s.MaPhieuMuon.Equals(idPhieuMuon));
            QLThuVienDC.CHITIETPHIEUMUONs.DeleteOnSubmit(ct);
            QLThuVienDC.SubmitChanges();
        }
        public int CountChiTietPhieuMuon(int idPhieuMuon)
        {
            int total = QLThuVienDC.CHITIETPHIEUMUONs.Count(c=>c.MaPhieuMuon== idPhieuMuon);
            return total;
        }
    }

}
