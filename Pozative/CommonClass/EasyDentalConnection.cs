using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.CommonClass
{
    public class DtxApiWrapper
    {
        private const string EZD_SHELL_CLASS_NAME = "TEZDMainForm";
        private const string EZDAPI_ASSEMBLY_NAME = "EZDentalAPI.dll";

        [DllImport("EZDentalAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int EZDAPI_RegisterAccount(
          string szUserId,
          string szRegistrationKey,
          StringBuilder szPasswordBuffer,
          int nBufferSize);

        [DllImport("EZDentalAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void EZDAPI_GetConnectionString(
          StringBuilder szConnectionsString,
          int ConnectionStringSize);

        [DllImport("EZDentalAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern float DENTRIXAPI_GetDentrixVersion();


        [DllImport("EZDentalAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int EZDAPI_Initialize([MarshalAs(UnmanagedType.LPStr)] string szUserId, [MarshalAs(UnmanagedType.LPStr)] string szPassword);

        [DllImport("regfuncs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTX_GetDentrixDbPath(StringBuilder PathBuff, int iPathBuffSize);

        public static bool RegisterAccount(out string password)
        {
            StringBuilder szPasswordBuffer = new StringBuilder(20);
            bool flag = DtxApiWrapper.EZDAPI_RegisterAccount("EDEX", "EDDd2D405D`7`D5305`54442d643604586KB", szPasswordBuffer, 20) == 1;
            password = szPasswordBuffer.ToString();
            return flag;
        }

        public static string GetDtxConnectionString()
        {
           // Password = "";
           // RegisterAccount(out Password);
            //DtxApiWrapper.EZDAPI_Initialize("EDEX", Password);
           // StringBuilder szCopnnectionsString = new StringBuilder(512);
           // lock (szCopnnectionsString)
           //     DtxApiWrapper.EZDAPI_GetConnectionString(szCopnnectionsString, 512);
           // MessageBox.Show(szCopnnectionsString.ToString());
            // return szCopnnectionsString.ToString();
            return "DSN=EZD2011;DBQ=.;SERVER=NotTheServer;";

        }

        //public static string GetEDexCommonPath()
        //{
        //  string dentrixDataPath = HelperFunction.GetDentrixDataPath();
        //  if (string.IsNullOrWhiteSpace(dentrixDataPath))
        //  {
        //    int num = (int) MessageBox.Show("The Easy Dental database path could not be determined.  Please call Support.", "eDex", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        //    throw new Exception("The Easy Dental database path could not be determined. Please contact Support.");
        //  }
        //  return Path.Combine(dentrixDataPath, "eDex") + "\\";
        //}
       

        public static bool InitializeAPI()
        {
            return DtxApiWrapper.EZDAPI_Initialize("EDEX", DecryptStringAES(Password)) != 0;
        }
        private static string Password = "";
        private static string _secretKey = "WooWooWooWoo!!";
        private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
        public static string DecryptStringAES(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";
            if (string.IsNullOrEmpty(_secretKey))
            {
                Console.WriteLine("Invalid Decryption Key");
                return "";
            }
            RijndaelManaged rijndaelManaged = (RijndaelManaged)null;
            try
            {
                Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(_secretKey, _salt);
                rijndaelManaged = new RijndaelManaged();
                rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
                rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
                ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            return streamReader.ReadToEnd();
                    }
                }
            }
            finally
            {
                rijndaelManaged.Clear();
            }
        }
    }
    public class EasyDentalConnection
    {
        private static string _connectionString = (string)null;
        private static DbConnection _savedConnection;

        public static DbConnection GetEasyDentalConnection()
        {
            if (EasyDentalConnection._connectionString == null)
                // DentrixConnection._connectionString = "UID=EDEX;PWD=6nul9icsbb;DSN=DDPODBC;";  //DtxApiWrapper.GetDtxConnectionString();
                EasyDentalConnection._connectionString = DtxApiWrapper.GetDtxConnectionString();
            DbConnection Conn = (DbConnection)new OdbcConnection(EasyDentalConnection._connectionString);
            try
            {
            if(Conn.State != ConnectionState.Open) Conn.Open();
            }
            catch(Exception ex)
            {
                Conn = null;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open) Conn.Close();
                }
            }
            return Conn;
        }

        public static bool TestDentrixConnection()
        {
            bool flag = true;
            DbConnection dentrixConnection = EasyDentalConnection.GetEasyDentalConnection();
            try
            {
                dentrixConnection.Open();
            }
            catch (Exception ex)
            {
                flag = false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dentrixConnection.Dispose();
            }
            return flag;
        }
        public static bool ExecuteSingleSelect(
          string selectString,
          EasyDentalConnection.PopulateFromReader callback)
        {
            bool flag1 = false;
            try
            {
                bool flag2 = false;
                DbConnection dbConnection;
                if (EasyDentalConnection._savedConnection != null)
                {
                    dbConnection = EasyDentalConnection._savedConnection;
                    flag2 = true;
                }
                else
                {
                    dbConnection = EasyDentalConnection.GetEasyDentalConnection();
                    dbConnection.Open();
                    EasyDentalConnection._savedConnection = dbConnection;
                }
                DbCommand command = dbConnection.CreateCommand();
                command.CommandText = selectString;
                try
                {
                    DbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                        flag1 = callback(reader);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EZD Connect - Data read error: " + ex.Message);
                }
                if (!flag2)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                    EasyDentalConnection._savedConnection = (DbConnection)null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EZD Connect - Data read error: " + ex.Message);
            }
            return flag1;
        }

        public static int ExecuteScalar(string queryText)
        {
            int num = 0;
            try
            {
                bool flag = false;
                DbConnection dbConnection;
                if (EasyDentalConnection._savedConnection != null)
                {
                    dbConnection = EasyDentalConnection._savedConnection;
                    flag = true;
                }
                else
                {
                    dbConnection = EasyDentalConnection.GetEasyDentalConnection();
                    dbConnection.Open();
                    EasyDentalConnection._savedConnection = dbConnection;
                }
                DbCommand command = dbConnection.CreateCommand();
                command.CommandText = queryText;
                try
                {
                    if (command.ExecuteReader().Read())
                        ++num;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EZD Connect - Scalar Error: " + ex.Message);
                }
                if (!flag)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                    EasyDentalConnection._savedConnection = (DbConnection)null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EZD Connect - Scalar Error: " + ex.Message);
            }
            return num;
        }

        public static bool ExecuteSingleResultQuery(
          string queryText,
          ref Dictionary<string, object> resultset)
        {
            bool flag1 = false;
            try
            {
                bool flag2 = false;
                DbConnection dbConnection;
                if (EasyDentalConnection._savedConnection != null)
                {
                    dbConnection = EasyDentalConnection._savedConnection;
                    flag2 = true;
                }
                else
                {
                    dbConnection = EasyDentalConnection.GetEasyDentalConnection();
                    dbConnection.Open();
                    EasyDentalConnection._savedConnection = dbConnection;
                }
                DbCommand command = dbConnection.CreateCommand();
                command.CommandText = queryText;
                try
                {
                    DbDataReader dbDataReader = command.ExecuteReader();
                    List<string> stringList = new List<string>();
                    foreach (string key in resultset.Keys)
                        stringList.Add(key);
                    if (dbDataReader.Read())
                    {
                        foreach (string index in stringList)
                        {
                            if (dbDataReader[index] != DBNull.Value)
                                resultset[index] = dbDataReader[index];
                        }
                        flag1 = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EZD Connect - Single Result Query error: " + ex.Message);
                }
                if (!flag2)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                    EasyDentalConnection._savedConnection = (DbConnection)null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EZD Connect - Single Result Query error: " + ex.Message);
                flag1 = false;
            }
            return flag1;
        }
        public static DataTable GriddataTable = null;
        public static List<object> ExecuteMultipleResultQuery(string queryText)
        {
            List<object> objectList1 = new List<object>();
            try
            {
                bool flag = false;
                DbConnection dbConnection;

                if (EasyDentalConnection._savedConnection != null)
                {
                    dbConnection = EasyDentalConnection._savedConnection;
                    flag = true;
                }
                else
                {
                    dbConnection = EasyDentalConnection.GetEasyDentalConnection();
                    dbConnection.Open();
                    EasyDentalConnection._savedConnection = dbConnection;
                }
                DbCommand command = dbConnection.CreateCommand();
                command.CommandText = queryText;
                try
                {

                    OdbcDataAdapter odbcadpt = new OdbcDataAdapter((OdbcCommand)command);
                    GriddataTable = new DataTable();

                    odbcadpt.Fill(GriddataTable);


                    DbDataReader dbDataReader = command.ExecuteReader();

                    //var dataReader = cmd.ExecuteReader();
                    //dataTable.Load(dbDataReader);



                    while (dbDataReader.Read())
                    {
                        List<object> objectList2 = new List<object>();
                        for (int index = 0; index < dbDataReader.FieldCount; ++index)
                        {
                            if (dbDataReader[index] != DBNull.Value)
                                objectList2.Add(dbDataReader[index]);
                        }
                        objectList1.Add((object)objectList2);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EZD Connect - Multiple Result Query Error: " + ex.Message);
                }
                if (!flag)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                    EasyDentalConnection._savedConnection = (DbConnection)null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EZD Connect - Multiple Result Query Error: " + ex.Message);
            }
            return objectList1;
        }

        public static bool ExecuteNonQueryStatement(string statement, List<DbParameter> Params = null)
        {
            bool flag1 = false;
            try
            {
                bool flag2 = false;
                DbConnection dbConnection;
                if (EasyDentalConnection._savedConnection != null)
                {
                    dbConnection = EasyDentalConnection._savedConnection;
                    flag2 = true;
                }
                else
                {
                    dbConnection = EasyDentalConnection.GetEasyDentalConnection();
                    dbConnection.Open();
                    EasyDentalConnection._savedConnection = dbConnection;
                }
                DbCommand command = dbConnection.CreateCommand();
                command.CommandText = statement;
                if (Params != null)
                    command.Parameters.AddRange((Array)Params.ToArray());
                try
                {
                    command.ExecuteNonQuery();
                    flag1 = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EZD Connect - Multiple Result Query Error: " + ex.Message);
                }
                if (!flag2)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                    EasyDentalConnection._savedConnection = (DbConnection)null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EZD Connect - Multiple Result Query Error: " + ex.Message);
            }
            return flag1;
        }

        public static DbDataReader ExecStoredProc(string procName)
        {
            DbConnection conn = EasyDentalConnection.GetEasyDentalConnection();
            conn.Open();
            DbCommand command = conn.CreateCommand();
            command.CommandText = string.Format("CALL admin.{0}()", (object)procName);
            return command.ExecuteReader();
        }

        public static Dictionary<int, string> GetApptStatusCodeDescriptions()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (List<object> objectList in EasyDentalConnection.ExecuteMultipleResultQuery("select defid, description from DefRsStatus"))
            {
                int key = int.Parse(objectList[0].ToString());
                string str = objectList[1].ToString().Substring(1);
                dictionary.Add(key, str);
            }
            return dictionary;
        }

        internal static bool FillDataTableQuery(string selstring, ref DataTable dt)
        {
            bool flag1 = false;
            try
            {
                bool flag2 = false;
                DbConnection dbConnection;
                if (EasyDentalConnection._savedConnection != null)
                {
                    dbConnection = EasyDentalConnection._savedConnection;
                    flag2 = true;
                }
                else
                {
                    dbConnection = EasyDentalConnection.GetEasyDentalConnection();
                    dbConnection.Open();
                    EasyDentalConnection._savedConnection = dbConnection;
                }
                DbCommand command = dbConnection.CreateCommand();
                command.CommandText = selstring;
                DbDataAdapter dbDataAdapter = (DbDataAdapter)new OdbcDataAdapter((OdbcCommand)command);
                try
                {
                    dt.BeginInit();
                    dt.BeginLoadData();
                    dbDataAdapter.Fill(dt);
                    dt.EndLoadData();
                    dt.EndInit();
                    flag1 = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EZD Connect - Data Table Result Query Error: " + ex.Message);
                }
                if (!flag2)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                    EasyDentalConnection._savedConnection = (DbConnection)null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EZD Connect - Data Table Result Query Error: " + ex.Message);
            }
            return flag1;
        }

        public static void ForceOpenConnection()
        {
            if (EasyDentalConnection._savedConnection != null)
                return;
            EasyDentalConnection._savedConnection = EasyDentalConnection.GetEasyDentalConnection();
            EasyDentalConnection._savedConnection.Open();
        }

        public static void CloseOpenConnection()
        {
            if (EasyDentalConnection._savedConnection == null)
                return;
            EasyDentalConnection._savedConnection.Close();
            EasyDentalConnection._savedConnection.Dispose();
            EasyDentalConnection._savedConnection = (DbConnection)null;
        }

        // public delegate IRoloListDisplayable ConvertToItem(DbDataReader reader);

        public delegate bool PopulateFromReader(DbDataReader reader);
    }
}
