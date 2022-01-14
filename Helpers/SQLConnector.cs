using MySql.Data.MySqlClient;
using RD.MySQLConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ToDoStuff
{
    public class SQLConnector
    {
        public DataBaseManager dbMan = null;

        public SQLConnector()
        {
            dbMan = new DataBaseManager();
        }

        public SQLConnector(string server, string username,
            string password, string database) : this()
        {
            dbMan.connection = new MySqlConnection("port=3306;server=" + server + ";user id=" + username + ";password=" + password + ";database=" + database);
            dbMan.connection.Open();
        }

        public DataSet GetTables()
        {
            dbMan.cmd.Connection = dbMan.connection;
            dbMan.cmd.CommandText = "SHOW TABLES;";

            return dbMan.ExecuteDataSet(dbMan.cmd);
        }

        public DataSet ColumnList(string tablename, string schema)
        {
            dbMan.cmd.Connection = dbMan.connection;
            dbMan.cmd.CommandText = "select * from information_schema.columns where table_name='" + tablename + "' and table_schema='" + schema + "';";

            return dbMan.ExecuteDataSet(dbMan.cmd);
        }
    }
}
