using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BTL
{
    class Users
    {
        SqlConnection sqlConn;
        string cnStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        DBQuanLyThuVienDataContext QLThuVienDC;

        public Users()
        {
            sqlConn = new SqlConnection(cnStr);
            QLThuVienDC = new DBQuanLyThuVienDataContext(sqlConn);
        }

        public USERS GetUser(string username, string password)
        {
            USERS u = QLThuVienDC.USERS.FirstOrDefault(s => s.Username == username && s.Password == password);
            return u;
        }
    }
}
