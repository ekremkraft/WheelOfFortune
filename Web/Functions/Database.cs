using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EarnApi.Functions
{
    public class Database
    {
        public static DataTable ExecSelect(string Command)
        {
            SqlConnection Connect = new SqlConnection("connectionString");

            DataTable tablo = new DataTable();
            try
            {
                SqlDataAdapter cmd = new SqlDataAdapter(Command, Connect);
                Connect.Open();
                cmd.Fill(tablo);
                Connect.Close();
                return tablo;
            }
            catch
            {
                Connect.Close();
                return null;
            }
        }

        public static int ExecQuery(string Command)
        {
            SqlConnection Connect = new SqlConnection("connectionString");
            int result;
            try
            {
                SqlCommand cmd = new SqlCommand(Command, Connect);
                Connect.Open();
                result = cmd.ExecuteNonQuery();
                Connect.Close();
                return result;
            }
            catch
            {
                Connect.Close();
                return -1;
            }
        }

        public static string Clean(string Text)
        {
            string strReturn = "";
            if (Text != null)
            {
                strReturn = Text.Trim();
                strReturn = strReturn.Replace("&gt;", "");
                strReturn = strReturn.Replace("&lt;", "");
                strReturn = strReturn.Replace("--", "");
                strReturn = strReturn.Replace("'", "''");
                strReturn = strReturn.Replace("char ", "");
                strReturn = strReturn.Replace("delete ", "");
                strReturn = strReturn.Replace("insert ", "");
                strReturn = strReturn.Replace("update ", "");
                strReturn = strReturn.Replace("select ", "");
                strReturn = strReturn.Replace("truncate ", "");
                strReturn = strReturn.Replace("union ", "");
                strReturn = strReturn.Replace("script ", "");
            }
            return strReturn;
        }
    }
}