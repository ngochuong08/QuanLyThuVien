using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace BTL
{
    class ConnectionClass
    {
        //update,delete,insert
        SqlConnection sqlConn;
        SqlDataAdapter da;
        DataSet ds;

        public ConnectionClass()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString; //using System.configuration
                sqlConn = new SqlConnection(constr);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        //phuong thuc thuc thi cau lenh sql truy van du lieu
        public DataTable Execute(string sqlStr)
        {
            da = new SqlDataAdapter(sqlStr, sqlConn);
            ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }

        //Phương thức thực thi câu lệnh Thêm, Xóa, Sửa
        public void ExecutiNonQuery(string sqlStr)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn);
                sqlConn.Open(); //Mở kết nối
                sqlCmd.ExecuteNonQuery(); //Lệnh thêm/sửa/xóa
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                sqlConn.Close();
            }
        }
    }

}
