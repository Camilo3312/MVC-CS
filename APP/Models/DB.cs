/*using MySql.Data.MySqlClient;*/
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Models
{
    public class DB
    {
       private MySqlConnection conexion;

        public DB()
        {
            conexion = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=clients;SSLMode=None");
        }

        public bool operacionesSQL(string sql)
        {
            MySqlCommand query = new MySqlCommand(sql, conexion);
            try
            {
                conexion.Open();
                query.ExecuteNonQuery();
                conexion.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable getDatas(string sql)
        {
            DataTable dataTable = new DataTable();

            MySqlCommand query = new MySqlCommand(sql, conexion);
            try
            {
                conexion.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query);
                adapter.Fill(dataTable);
                conexion.Close();
                adapter.Dispose();
            }
            catch (Exception)
            {
                return null;
            }

            return dataTable;
        } 

  
    }
}
