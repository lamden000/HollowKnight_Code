using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ServerChinh
{
    internal class Modify
    {
        public Modify() { }
        SqlCommand sqlCommad;// dùng để truy vấn cac cau lenh insert update,dele,..
        SqlDataReader reader;//dùng để đọc dữ liệu trong bảng
        public List<TaiKhoan> TaiKhoans(string query)
        {  // dung de check tai khoan
            List<TaiKhoan> taiKhoans = new List<TaiKhoan>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommad = new SqlCommand(query, sqlConnection);
                reader = sqlCommad.ExecuteReader();
                while (reader.Read())
                {
                    taiKhoans.Add(new TaiKhoan(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetDecimal(3), reader.GetString(4)));
                }
                sqlConnection.Close();
            }
            return taiKhoans;
        }

        public void Command(string query) //dung de dang ky
        {
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommad = new SqlCommand(query, sqlConnection);
                sqlCommad.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
