using Ctree.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public interface IDtxDataProvider
    {
        void OpenConnection();

        void CloseConnection();

        IDbCommand CreateCommand();

    }
    public class DataManager
    {
        private DataManager()
        {
        }

        public static IDtxDataProvider Create(string user, string password)
        {
            return (IDtxDataProvider)new DtxDataProvider(user, password);
        }
    }
    internal class DtxDataProvider : IDtxDataProvider
    {
        private Manager _manager;
        private Connection _connection;

        internal DtxDataProvider()
        {
            this._manager = new Manager();
            this.CreateConnection();
        }

        internal DtxDataProvider(string username, string password)
        {
            this._manager = new Manager(username, password);
            this.CreateConnection();
        }

        internal DtxDataProvider(string connString)
        {
            this._manager = new Manager("CONNECT", connString);
            this.CreateConnection();
        }

        private void CreateConnection()
        {
            this._connection = this._manager.CreateConnection();
        }

        public void OpenConnection()
        {
            this._connection.Open();
        }

        public void CloseConnection()
        {
            this._connection.Close();
        }

        public IDbCommand CreateCommand()
        {
            return this._connection.CreateCommand();
        }

        private bool RepairConnection(Exception e)
        {
            if (!e.Message.Contains("ctorc-treeACE does not support nested transaction") && !e.Message.Contains("There is already an open DataReader associated with this Connection which must be closed first"))
                return false;
            this._connection.Close();
            this._connection.Open();
            return true;
        }
    }
    public class Manager
    {
        private IDataProvider _provider;      
        private static string _helperLookupId;
        private static string _helperLookupId2;
        private static SecureString _HL7_test_value;
        private static string _hostName;
        private static string _dbName;

        [DllImport("Dtx32.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int DTX_GetDentrixServerMachineName(
          StringBuilder returnBuffer,
          int iPathBuffSize);

        [DllImport("Dtxdb32.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int DDB_LocateServer(StringBuilder hostName, StringBuilder ipAddress);

        [DllImport("Dtxdb32.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void DDB_GetDataBaseName(StringBuilder dbName);

        [DllImport("dtxhelp.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void DtxHelp_GetInfo(int InfoId, StringBuilder info);

        [DllImport("dtxhelp.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int DtxHelp_UsingTLS();

        [DllImport("Dentrix.API.dll")]
        public static extern void DENTRIXAPI_GetFCConnectionString(
          [MarshalAs(UnmanagedType.LPStr)] string username,
          [MarshalAs(UnmanagedType.LPStr)] string password,
          StringBuilder szConnectionsString,
          int ConnectionStringSize);

        public static Connection GetNewConnection()
        {
            return new Manager().CreateConnection();
        }

        private static string Help_Info_Id
        {
            get
            {
                int num1 = 3;
                if (Manager._helperLookupId == null)
                {
                    int num2 = num1 * 2 + num1 * 3;
                    StringBuilder info = new StringBuilder(50);
                    Manager.DtxHelp_GetInfo(num2 - 1, info);
                    Manager._helperLookupId = info.ToString();
                    info.Clear();
                }
                return Manager._helperLookupId;
            }
        }

        private static string Help_Info_Id2
        {
            get
            {
                int num1 = 3;
                if (Manager._helperLookupId2 == null)
                {
                    int num2 = num1 * 2 + num1 * 3;
                    StringBuilder info = new StringBuilder(50);
                    Manager.DtxHelp_GetInfo(num2 - 4, info);
                    Manager._helperLookupId2 = info.ToString();
                    info.Clear();
                }
                return Manager._helperLookupId2;
            }
        }
        internal IDataProvider Provider
        {
            get
            {
                return this._provider;
            }
        }

        public Manager()
        {
            this.init("dtxuser", "");
        }

        public Manager(string userName, string password)
        {
            this.init(userName, password);
        }

        private void init(string userName, string password)
        {
            this._provider = (IDataProvider)new CTreeProvider();
           // this._statementGen = (ICommandGenerator)new CTreeCommandGenerator();
            if (!(userName != "dtxuser") && !(password != ""))
                return;
            string empty = string.Empty;
            StringBuilder szConnectionsString = new StringBuilder(512);
            lock (szConnectionsString)
            {
                if (userName != "CONNECT")
                    Manager.DENTRIXAPI_GetFCConnectionString(userName, password, szConnectionsString, 512);
                else
                    szConnectionsString.Append(password);
                empty = szConnectionsString.ToString();
                szConnectionsString.Clear();
            }
            Manager._HL7_test_value = empty.EncryptString();
        }

        public Connection CreateConnection()
        {
            return new Connection(this);
        }

        private static string HN
        {
            get
            {
                try
                {
                    if (Manager._hostName == null)
                    {
                        StringBuilder stringBuilder = new StringBuilder(512);
                        if (Manager.DTX_GetDentrixServerMachineName(stringBuilder, 512) == 0)
                        {
                            StringBuilder ipAddress = new StringBuilder(512);
                            Manager.DDB_LocateServer(stringBuilder, ipAddress);
                        }
                        Manager._hostName = stringBuilder.Length <= 0 ? "localhost" : stringBuilder.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error getting host name defaulting to local :" + ex.Message);
                    Manager._hostName = "localhost";
                }
                return Manager._hostName;
            }
        }

        private static string DbN
        {
            get
            {
                try
                {
                    if (Manager._dbName == null)
                    {
                        StringBuilder dbName = new StringBuilder(512);
                        Manager.DDB_GetDataBaseName(dbName);
                        Manager._dbName = dbName.Length <= 0 ? "DentrixSQL" : dbName.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error getting database name defaulting to DentrixSQL :" + ex.Message);
                    Manager._dbName = "DentrixSQL";
                }
                return Manager._dbName;
            }
        }

     
      

        internal SecureString HL7_test_value
        {
            get
            {
                if (Manager._HL7_test_value == null)
                {
                    string format = "{0}={1};{2}={3};{4}={5};{6}={7};{8}={9};{10}";
                    if (Manager.DtxHelp_UsingTLS() == 1)
                        format = "SSL=basic;" + format;
                    Manager._HL7_test_value = string.Format(format, (object)Manager.ParseHL7String("h7odsTtX"), (object)Manager.HN, (object)Manager.ParseHL7String("uqiwde"), (object)Manager.Help_Info_Id2, (object)Manager.ParseHL7String("Pwaesrstw1o3rtd1"), (object)Manager.Help_Info_Id, (object)Manager.ParseHL7String("d5a7ttaWbTa1swe4"), (object)Manager.DbN, (object)Manager.ParseHL7String("sWe2rWv1i2cTeF"), (object)Manager.ParseHL7String("61519171"), (object)Manager.ParseHL7String("P1o2o3l4i5n6g7 8=9 0f1a2l3s4e5")).EncryptString();
                }
                return Manager._HL7_test_value;
            }
        }

        internal static string ParseHL7String(string parseMe)
        {
            int index = 0;
            string empty = string.Empty;
            do
            {
                empty += parseMe[index].ToString();
                index += 2;
            }
            while (index < parseMe.Length);
            return empty;
        }

      
    }
    internal interface IDataProvider
    {
        IDbConnection CreateConnectionObject();

        IDbCommand CreateCommandObject();

        IDbDataAdapter CreateDataAdapter();
    }
    public sealed class CTreeProvider : IDataProvider
    {
        IDbConnection IDataProvider.CreateConnectionObject()
        {
            return (IDbConnection)new CtreeSqlConnection();
        }

        IDbCommand IDataProvider.CreateCommandObject()
        {
            return (IDbCommand)new CtreeSqlCommand();
        }

        IDbDataAdapter IDataProvider.CreateDataAdapter()
        {
            return (IDbDataAdapter)new CtreeSqlDataAdapter();
        }
    }
    public class Connection : IGenerationConnection, IConnection, IDisposable
    {
        private static Connection.DynamicBuilderCache _cachedProcedures = new Connection.DynamicBuilderCache();
        private Manager _manager;
        private IDbConnection _connection;
        private IDbTransaction _transaction;



        internal Connection(Manager manager)
        {
            this._manager = manager;
            this._connection = this._manager.Provider.CreateConnectionObject();
            this._transaction = (IDbTransaction)null;
            try
            {
                PropertyInfo property = this._connection.GetType().GetProperty(Manager.ParseHL7String("CzoxncnvegcntmikolniSutyrtienqgr"), BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
                string str1 = manager.HL7_test_value.DecryptString();
                IDbConnection connection = this._connection;
                string str2 = str1;
                property.SetValue((object)connection, (object)str2, BindingFlags.Instance | BindingFlags.SetProperty, (Binder)null, (object[])null, (CultureInfo)null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Open()
        {
            this.Open(false);
        }

        public void Open(bool startTransaction)
        {
            if (this._connection.State != ConnectionState.Closed)
                return;
            this._connection.Open();
            if (!startTransaction)
                return;
            this.BeginTransaction();
        }

        public bool IsConnectionOpen()
        {
            return this._connection.State == ConnectionState.Open;
        }

        public void Close()
        {
            if (this._transaction != null)
                this.Commit();
            this._connection.Close();
        }

        public void BeginTransaction()
        {
            if (this._transaction != null)
                throw new Exception("A transaction has already been started");
            if (this._connection.State == ConnectionState.Closed)
                throw new Exception("A transaction may not be created unless the connection is opened");
            this._transaction = this._connection.BeginTransaction();
            if (this._transaction == null)
                throw new Exception("Could not create the transaction.");
        }

        public void Commit()
        {
            if (this._transaction == null)
                throw new Exception("Transaction is not viable");
            this._transaction.Commit();
            this._transaction.Dispose();
            this._transaction = (IDbTransaction)null;
        }

        public void Rollback()
        {
            if (this._transaction == null)
                throw new Exception("Transaction is not viable");
            this._transaction.Rollback();
            this._transaction.Dispose();
            this._transaction = (IDbTransaction)null;
        }

        public object GetEntity<T>(object key)
        {
            return this.GetEntity<T>(new object[1] { key });
        }


        public bool IsTransactionOpened()
        {
            return this._connection.State != ConnectionState.Closed && this._transaction != null;
        }
        public IDbCommand CreateCommand()
        {
            return this._connection.CreateCommand();
        }
        public void Dispose()
        {
            if (this._connection == null)
                return;
            this._connection.Close();
            this._connection.ConnectionString = "host=xoxoxox;uid=xoxoxox;password=xoxoxox; database=DentrixSQL;service=xoxoxox";
        }

        internal class DynamicBuilderCache
        {
            private List<Connection.DynamicBuilderCache.Tuple> _cache = new List<Connection.DynamicBuilderCache.Tuple>();

            private string GetKey(string commandText, Type t)
            {
                return string.Format("{0}:{1}", (object)commandText, (object)t.FullName);
            }

            internal bool TryGetValue(string commandText, Type t, out object es)
            {
                string key = this.GetKey(commandText, t);
                foreach (Connection.DynamicBuilderCache.Tuple tuple in this._cache)
                {
                    if (tuple.Key == key)
                    {
                        es = tuple.Data;
                        return true;
                    }
                }
                es = (object)null;
                return false;
            }

            internal bool AddItem(string commandText, Type t, object es)
            {
                this._cache.Add(new Connection.DynamicBuilderCache.Tuple(this.GetKey(commandText, t), es));
                return true;
            }

            internal struct Tuple
            {
                internal string Key;
                internal object Data;

                internal Tuple(string key, object data)
                {
                    this.Key = key;
                    this.Data = data;
                }
            }
        }
    }
    public interface IConnection : IDisposable
    {
        void Open();

        void Open(bool startTransaction);

        void Close();

        bool IsConnectionOpen();

        void BeginTransaction();

        void Commit();

        void Rollback();

        bool IsTransactionOpened();

    }
    public interface IGenerationConnection : IConnection, IDisposable
    {
        // string GenerateBaseSelectStatementStringForAdhocQuery<T>();
    }
    internal static class ExtensionMethods
    {
        internal static SecureString EncryptString(this IEnumerable<char> value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            SecureString secureString = new SecureString();
            foreach (char c in value)
                secureString.AppendChar(c);
            secureString.MakeReadOnly();
            return secureString;
        }

        internal static string DecryptString(this SecureString value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            IntPtr coTaskMemUnicode = Marshal.SecureStringToCoTaskMemUnicode(value);
            try
            {
                return Marshal.PtrToStringUni(coTaskMemUnicode);
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(coTaskMemUnicode);
            }
        }
    }
}
