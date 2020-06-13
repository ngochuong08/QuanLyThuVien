using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL
{
    class NhanVien
    {
        ConnectionClass connClass;

        public NhanVien()
        {
            connClass = new ConnectionClass();
        }
        public int TotalNhanvien()
        {
            string sql = "SELECT count(*) AS TongNV FROM NHANVIEN";
            DataTable dt = connClass.Execute(sql);
            return dt.Rows.Count ;
        }
        public DataTable GetDSNhanVien()
        {
            string sql = "SELECT MaNhanVien, HoTenNhanVien, NgaySinh, DiaChi, DienThoai, b.TenBangCap " +
                         "FROM NHANVIEN n INNER JOIN BANGCAP b ON n.MaBangCap = b.MaBangCap ORDER BY MaNhanVien DESC";
            DataTable dt = connClass.Execute(sql);
            return dt;
        }

        //Lấy tên các nhóm NV
        public DataTable GetDSBangCap()
        {
            string sql = "SELECT * FROM BANGCAP";
            DataTable dt = connClass.Execute(sql);
            return dt;
        }

        public void DeleteNhanVien(string index_NV)
        {
            string sql = "DELETE FROM NHANVIEN WHERE MaNhanVien = " + index_NV;
            connClass.ExecutiNonQuery(sql);
        }

        public void AddNhanVien(string fullname, string birthday, string address, string phone, int idBangCap)
        {
            string str = string.Format("INSERT INTO NHANVIEN ([HoTenNhanVien] ,[NgaySinh] ,[DiaChi] ,[DienThoai] ,[MaBangCap]) " +
                " VALUES (N'{0}',N'{1}',N'{2}',N'{3}',{4})", fullname, birthday, address, phone, idBangCap);
            connClass.ExecutiNonQuery(str);
        }

        public void UpdateNhanVien(int idNV, string fullname, string birthday, string address, string phone, int idBangCap)
        {
            string sql = string.Format("UPDATE NHANVIEN SET [HoTenNhanVien] = N'{0}', " +
                "[NgaySinh] = N'{1}', [DiaChi] = N'{2}',[DienThoai] = N'{3}', [MaBangCap] = {4} " +
                "WHERE [MaNhanVien] = {5}", fullname, birthday, address, phone, idBangCap, idNV);
            connClass.ExecutiNonQuery(sql);
        }

        //  conString = ConfigurationManager.ConnectionStrings["cnstr"].ConnectionString;
        // sqlConn = new SqlConnection(conString);
        //private int GetStoredProcedured(string procedureName)
        //{
        //    int gt = 0;
        //    SqlCommand sqlCom = new SqlCommand();
        //    try
        //    {
        //        sqlConn.Open();
        //        sqlCom.Connection = sqlConn;
        //        sqlCom.CommandText = procedureName;
        //        sqlCom.CommandType = CommandType.StoredProcedure;
        //        gt = (int)sqlCom.ExecuteScalar();
        //    }
        //    finally
        //    {
        //        sqlConn.Close();
        //    }
        //    return gt;
        //}

        //// Gọi SP có tham số cần truyền vào 3 giá trị: tên biến tham số, kiểu DL của tham số, giá trị của tham số
        //private int GetStoredProcedured(string procedureName, string tenThamSo, string kieuDuLieu, string giaTriThamSo)
        //{
        //    //GetStoredProcedured("FindProductID", "@name", "NVarChar(20)", "Huong");
        //    int gt = 0;
        //    SqlCommand sqlCom = new SqlCommand();
        //    try
        //    {
        //        sqlConn.Open();
        //        sqlCom.Connection = sqlConn;
        //        sqlCom.CommandText = procedureName;
        //        sqlCom.CommandType = CommandType.StoredProcedure;
        //        //Add tham số
        //        sqlCom.Parameters.Clear();
        //        sqlCom.Parameters.Add(tenThamSo, kieuDuLieu);
        //        //Gán gia strij cho tham số
        //        sqlCom.Parameters[tenThamSo].Value = giaTriThamSo;
        //        gt = (int)sqlCom.ExecuteScalar();//Trả về giá trị
        //    }
        //    finally
        //    {
        //        sqlConn.Close();
        //    }
        //    return gt;
        //}

    }

}
