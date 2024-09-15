using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChinh
{
    internal class Connection
    {
        private static string stringConnection = @"Data Source=DESKTOP-34T5M4R\MSSQLSERVER01;Initial Catalog=Database1;Integrated Security=True;";
        public static SqlConnection GetSqlConnection()
        {

            return new SqlConnection(stringConnection);
        }
    }
}
