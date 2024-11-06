using Pozative.UTL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using MySql.Data.MySqlClient;
using System.Data.Odbc;

namespace Pozative.DAL
{
    public class CommonDB
    {

        public static void LocalConnectionServer(ref SqlCeConnection conn)
        {
            conn = new SqlCeConnection(CommonUtility.LocalConnectionString());
        }
        public static void ClearDentSQLConnectionServer(ref SqlConnection conn)
        {
            conn = new SqlConnection(CommonUtility.ClearDentConnectionString());
        }
        public static void LocalConnectionServer_SqlServer(ref SqlConnection conn)
        {
            conn = new SqlConnection(CommonUtility.LocalConnectionString_SqlServer());
        }

        public static void MySQLAppUpdateConnectionServer(ref MySqlConnection conn)
        {
            conn = new MySqlConnection(CommonUtility.AppUpdateConnectionString());
        }

        public static void MySQLConnectionServer(ref MySqlConnection conn, string DbConnString)
        {
            conn = new MySqlConnection(CommonUtility.OpenDentalConnectionString(DbConnString));
        }

        public static void MySQLPozativeConnectionServer(ref MySqlConnection conn)
        {
            conn = new MySqlConnection(CommonUtility.LiveConnectionString());
        }

        public static void OdbcConnectionServer(ref OdbcConnection conn)
        {
            conn = new OdbcConnection(CommonUtility.DentrixConnectionString());
        }

        public static void PracticeWorkConnectionServer(ref OdbcConnection conn)
        {
            conn = new OdbcConnection(CommonUtility.DentrixConnectionString());
        }

        public static void OdbcEaglesoftConnectionServer(ref OdbcConnection conn, string DbString)
        {
            conn = new OdbcConnection(CommonUtility.EaglesoftConnectionString(DbString));
        }

        public static void SqlCeCommandServer(string sqlSelect, SqlCeConnection conn, ref SqlCeCommand cmd, string Type = "sp")
        {
            cmd = new SqlCeCommand(sqlSelect, conn);
            SqlCeCommentType(ref cmd, Type);
            cmd.Connection = ((SqlCeConnection)conn);
        }

        public static void SqlServerCommand(string sqlSelect, SqlConnection conn, ref SqlCommand cmd, string Type = "sp")
        {
            cmd = new SqlCommand(sqlSelect, conn);
            SqlServerCommentType(ref cmd, Type);
            cmd.Connection = ((SqlConnection)conn);
        }

        public static void MySqlCommandServer(string sqlSelect, MySqlConnection conn, ref MySqlCommand cmd, string Type = "sp")
        {
            cmd = new MySqlCommand(sqlSelect, conn);
            MySqlCommandType(ref cmd, Type);
            cmd.Connection = ((MySqlConnection)conn);
        }

        public static void OdbcCommandServer(string OdbcSelect, OdbcConnection conn, ref OdbcCommand cmd, string Type = "sp")
        {
            cmd = new OdbcCommand(OdbcSelect, conn);
            OdbcCommentType(ref cmd, Type);
            cmd.Connection = ((OdbcConnection)conn);
        }


        public static void SqlCeCommentType(ref SqlCeCommand cmd, string Type = "sp")
        {
            switch (Type)
            {
                case "sp":
                    cmd.CommandType = CommandType.StoredProcedure;
                    break;
                case "txt":
                    cmd.CommandType = CommandType.Text;
                    break;
                case "td":
                    cmd.CommandType = CommandType.TableDirect;
                    break;
            }
        }

        public static void SqlServerCommentType(ref SqlCommand cmd, string Type = "sp")
        {
            switch (Type)
            {
                case "sp":
                    cmd.CommandType = CommandType.StoredProcedure;
                    break;
                case "txt":
                    cmd.CommandType = CommandType.Text;
                    break;
                case "td":
                    cmd.CommandType = CommandType.TableDirect;
                    break;
            }
        }

        public static void MySqlCommandType(ref MySqlCommand cmd, string Type = "sp")
        {
            switch (Type)
            {
                case "sp":
                    cmd.CommandType = CommandType.StoredProcedure;
                    break;
                case "txt":
                    cmd.CommandType = CommandType.Text;
                    break;
                case "td":
                    cmd.CommandType = CommandType.TableDirect;
                    break;
            }
        }

        public static void OdbcCommentType(ref OdbcCommand cmd, string Type = "sp")
        {
            switch (Type)
            {
                case "sp":
                    cmd.CommandType = CommandType.StoredProcedure;
                    break;
                case "txt":
                    cmd.CommandType = CommandType.Text;
                    break;
                case "td":
                    cmd.CommandType = CommandType.TableDirect;
                    break;
            }
        }


        public static void SqlCeCommandServer(ref SqlCeCommand cmd)
        {
            cmd = new SqlCeCommand();
        }

        public static void SqlServerCommand(ref SqlCeCommand cmd)
        {
            cmd = new SqlCeCommand();
        }

        public static void MySqlCommandServer(ref MySqlCommand cmd)
        {
            cmd = new MySqlCommand();
        }

        public static void OdbcCommandServer(ref OdbcCommand cmd)
        {
            cmd = new OdbcCommand();
        }


        //public static void SqlCeTransServer(SqlCeConnection conn, ref SqlCeTransaction trans)
        //{
        //   trans = ((SqlCeConnection)conn).BeginTransaction();
        //}

        //public static void MySqlTransServer(MySqlConnection conn, ref MySqlTransaction trans)
        //{
        //    trans = ((MySqlConnection)conn).BeginTransaction();
        //}

        //public static void OdbcTransServer(OdbcConnection conn, ref OdbcTransaction trans)
        //{
        //    trans = ((OdbcConnection)conn).BeginTransaction();
        //}


        public static void SqlCeDatatAdapterServer(SqlCeCommand cmd, ref SqlCeDataAdapter dataadaper)
        {
            dataadaper = new SqlCeDataAdapter(cmd);
        }

        public static void SqlServerDataAdapter(SqlCommand cmd, ref SqlDataAdapter dataadaper)
        {
            dataadaper = new SqlDataAdapter(cmd);
        }

        public static void MySqlDatatAdapterServer(MySqlCommand cmd, ref MySqlDataAdapter dataadaper)
        {
            dataadaper = new MySqlDataAdapter(cmd);
        }

        public static void OdbcDatatAdapterServer(OdbcCommand cmd, ref OdbcDataAdapter dataadaper)
        {
            dataadaper = new OdbcDataAdapter(cmd);
        }

        public static void TrackerSQLConnectionServer(ref SqlConnection conn)
        {
            conn = new SqlConnection(CommonUtility.TrackerConnectionString());
        }
        public static void AbelDentSQLConnectionServer(ref SqlConnection conn)
        {
            conn = new SqlConnection(CommonUtility.AbelDentConnectionString());
        }
    }
}
