using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace My.World.Api.DataAccess
{
    public class DBContext
    {
        public string ConnectionString { get; set; }
        public MySqlCommand cmd = new MySqlCommand();
        public DBContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        private bool IsDataExist(DataSet dataSet)
        {
            int tableCount = 0;
            bool returnValue = true;
            foreach (DataTable dt in dataSet.Tables)
            {
                if (dt.Rows.Count == 0)
                    tableCount = tableCount + 1;
            }
            if (tableCount == dataSet.Tables.Count)
            {
                returnValue = false;
            }
            return returnValue;
        }

        public void AddInParameter(MySqlCommand command, string name, object value, bool isDefaultValueIsNotNull = true)
        {
            if (command != null)
            {
                if (value != null && isDefaultValueIsNotNull)
                {
                    if ((value.GetType() == typeof(DateTime) && (DateTime)value == DateTime.MinValue)
                        || (value.GetType() == typeof(Int64) && (Int64)value == 0)
                        || (value.GetType() == typeof(Int32) && (Int32)value == 0)
                        || (value.GetType() == typeof(double) && (double)value == 0)
                        || (value.GetType() == typeof(string) && value.ToString().Trim() == string.Empty)
                       )
                    {
                        value = DBNull.Value;
                    }
                }
                command.Parameters.Add(new MySqlParameter(name, value));
                cmd.Parameters[name].Direction = ParameterDirection.Input;
            }
        }

        public void AddOutParameter(MySqlCommand command, string name, MySqlDbType dbType, object value)
        {
            if (command != null)
                command.Parameters.Add(new MySqlParameter(name, dbType, int.MaxValue, ParameterDirection.InputOutput, true, 0, 0, String.Empty, DataRowVersion.Default, value));
        }

        public DataSet ExecuteDataSet(MySqlCommand command)
        {
            DataSet ds = null;
            try
            {
                if (command != null)
                {
                    MySqlDataReader rdr = command.ExecuteReader();
                    ds = new DataSet();
                    ds.Tables.Add("Table");
                    ds.Tables[0].Load(rdr);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return ds;
        }
    }
}
