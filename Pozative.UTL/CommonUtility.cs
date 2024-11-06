using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;
using System.Management;
using Microsoft.Win32;
namespace Pozative.UTL
{

    public class CommonUtility
    {

        public static string LocalConnectionString()
        {                  
            string ConnectionString = ConfigurationManager.ConnectionStrings["LocalDBConnectionString"].ConnectionString;
            if (Utility.UseMaxBufferSize)
            {
                if (ConnectionString.ToLower().IndexOf("max buffer size=") < 0)
                {
                    ConnectionString += ";SSCE:Max Buffer Size=1024;";
                }
            }
            return ConnectionString;                                      
        }

        public static string LocalConnectionString_SqlServer()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["LocalDBConnectionStringSqlServer"].ConnectionString;
            return ConnectionString;
        }

        public static void AddHolidaySub(string dateColumnsName,DataTable dtLocalDB, DateTime  date,string holidayName, string commentColName,ref DataTable dtHolidays)
        {
            try
            {
                DataRow[] row = dtHolidays.Select("" + dateColumnsName + " = '" + Convert.ToDateTime(date) + "'");
                //DataRow[] rowLocal = dtLocalDB.Select("SchedDate = '" + Convert.ToDateTime(date) + "'");
                if (row.Length == 0 )
                {
                    DataRow ApptDtldr = dtHolidays.NewRow();
                    ApptDtldr[dateColumnsName] = date;
                    ApptDtldr[commentColName] = holidayName;
                    dtHolidays.Rows.Add(ApptDtldr);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DateTime GetWeekdayOfMonth(DateTime inputDate, DayOfWeek weekday, int weekNumber)
        {
            DateTime startDate;
            DateTime currentDate;
            int weekdayCount = 0;

            startDate = inputDate;
            currentDate = new DateTime(startDate.Year, startDate.Month, 1);

            while (weekdayCount < weekNumber)
            {
                if (currentDate.DayOfWeek == weekday)
                {
                    weekdayCount++;
                    if (weekdayCount == weekNumber)
                    {
                        break;
                    }
                }
                currentDate = currentDate.AddDays(1);
            }

            return currentDate;
        }

        public static DateTime LastMonday(int year, int month)
        {
            DateTime dt;
            if (month < 12)
                dt = new DateTime(year, month + 1, 1);
            else
                dt = new DateTime(year + 1, 1, 1);
            dt = dt.AddDays(-1);
            while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);
            return dt;
        }

        public static DataTable AddHolidays(DataTable dtHolidays,DataTable dtLocalDB, string dateColumnsName, string commentColName,string EHRUniqueId)
        {
            try
            {
                dtHolidays = AddHolidaysYearWise(dtHolidays, dtLocalDB, DateTime.Now.Year, dateColumnsName, commentColName);
                dtHolidays = AddHolidaysYearWise(dtHolidays, dtLocalDB, DateTime.Now.Year + 1, dateColumnsName, commentColName);

                if (!dtHolidays.Columns.Contains(EHRUniqueId))
                {
                    dtHolidays.Columns.Add(EHRUniqueId,typeof(string));
                }
                dtHolidays.AsEnumerable().Where(o => o.Field<object>(EHRUniqueId) == null)
                       .All(o =>
                       {
                           if (dtHolidays.Columns.Contains(dateColumnsName))
                           {
                               o[EHRUniqueId] = Convert.ToDateTime(o[dateColumnsName]).Year.ToString() + "" + Convert.ToDateTime(o[dateColumnsName]).Month.ToString() + "" + Convert.ToDateTime(o[dateColumnsName]).Day.ToString() + "_0";
                           }
                           return true;
                       });

                return dtHolidays;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable AddHolidaysYearWise(DataTable dtHolidays,DataTable dtLocalDB, int CurrentYear,string dateColumnsName,string commentColName)
        {
            try
            {               
                AddHolidaySub(dateColumnsName, dtLocalDB, Convert.ToDateTime("01/01/" + CurrentYear), "New Year Day",  commentColName, ref dtHolidays);                
                AddHolidaySub(dateColumnsName, dtLocalDB, Convert.ToDateTime("07/04/" + CurrentYear), "Independence Day",  commentColName, ref dtHolidays);
                AddHolidaySub(dateColumnsName, dtLocalDB, Convert.ToDateTime("12/25/" + CurrentYear), "Christmas Day",  commentColName, ref dtHolidays);
                string First_Mon_Sep_month = GetWeekdayOfMonth(Convert.ToDateTime("09/01/" + CurrentYear), DayOfWeek.Monday, 1).ToShortDateString();
                AddHolidaySub(dateColumnsName, dtLocalDB, Convert.ToDateTime(First_Mon_Sep_month), "Labor Day",  commentColName, ref dtHolidays);
                string Forth_Thu_Nov_month = GetWeekdayOfMonth(Convert.ToDateTime("11/01/" + CurrentYear), DayOfWeek.Thursday, 4).ToShortDateString();
                AddHolidaySub(dateColumnsName, dtLocalDB, Convert.ToDateTime(Forth_Thu_Nov_month), "Thanksgiving Day",  commentColName, ref dtHolidays);
                string Last_Mon_May_month = LastMonday(Convert.ToInt32(CurrentYear), 05).ToString();
                AddHolidaySub(dateColumnsName, dtLocalDB, Convert.ToDateTime(Last_Mon_May_month), "Memorial Day",  commentColName, ref dtHolidays);
                
                return dtHolidays;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string AppUpdateConnectionString()
        {
            string ConnectionString = string.Empty;
            ConnectionString = "server=50.116.20.152;port=3306;database=pozative_Server_App;uid=pozative_dentrix;pwd=mvc)s6o*zFN9;default command timeout=120;";
            return ConnectionString;
        }


        public static string ClearDentConnectionString()
        {
            string ConnectionString = Utility.DBConnString;
            return ConnectionString;
        }

        public static string TrackerConnectionString()
        {
            return Utility.DBConnString;
        }

        public static string AbelDentConnectionString()
        {
            return Utility.DBConnString;
        }

        public static string OpenDentalConnectionString(string DbConnString)
        {
            //string ConnectionString = string.Empty;
            //ConnectionString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";
            //return ConnectionString;

            string ConnectionString = DbConnString == "" ? Utility.DBConnString : DbConnString;//Utility.GetDataBaseConnectionByServicesInstallId(ServiceInstallId); //Utility.DBConnString;
            return ConnectionString;
        }

        public static string LiveConnectionString()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["LivePozativeDBConnectionString"].ConnectionString;
            return ConnectionString;
        }

        public static string GetAditTreatmentDocTempPath()
        {
            var temppath = Utility.AditDocTempPath;
            string tempPatientDoc = Path.Combine(temppath, "tempPatientTreatmentDoc");

            if (!(System.IO.Directory.Exists(tempPatientDoc)))
                System.IO.Directory.CreateDirectory(tempPatientDoc);

            return tempPatientDoc;
        }

        //rooja 
        public static string GetAditInsuranceCarrierDocTempPath()
        {
            var temppath = Utility.AditDocTempPath;
            string tempPatientDoc = Path.Combine(temppath, "tempPatientInsuranceCarrierDoc");

            if (!(System.IO.Directory.Exists(tempPatientDoc)))
                System.IO.Directory.CreateDirectory(tempPatientDoc);

            return tempPatientDoc;
        }


        public static string GetAditDocTempPath()
        {
            var temppath = Utility.AditDocTempPath;
            string tempPatientDoc = Path.Combine(temppath, "tempPatientDoc");

            if (!(System.IO.Directory.Exists(tempPatientDoc)))
                System.IO.Directory.CreateDirectory(tempPatientDoc);

            return tempPatientDoc;
        }
        public static string GetAditPatientProfileImagePath()
        {
            var temppath = Utility.AditPatientProfileImagePath;
            string tempPatientDoc = Path.Combine(temppath, "PatientProfileImage");

            if (!(System.IO.Directory.Exists(tempPatientDoc)))
                System.IO.Directory.CreateDirectory(tempPatientDoc);

            return tempPatientDoc;
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.Clamp);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                }
            }

            return destImage;
        }

        public static void SaveImageEHRToLocal(DataRow dr, string sourceLocation, string destPatientProfileImageFilePath)
        {
            Image NewImage = null;
            Int16 RHeight = 200;
            try
            {
                if (!File.Exists(sourceLocation) && sourceLocation.Contains("_100."))//Given condition if code is checking for _100 with image name and image exist without this suffix
                {
                    if (File.Exists(sourceLocation.Replace("_100.",".")))
                    {
                        sourceLocation = sourceLocation.Replace("_100.", ".");
                    }
                }
                
                if (File.Exists(sourceLocation))
                {
                    Image img = Image.FromFile(sourceLocation);
                    string OHeight = img.Size.Height.ToString();
                    string OWidth = img.Size.Width.ToString();

                    NewImage = CommonUtility.ResizeImage(img, (Convert.ToInt16(RHeight) * Convert.ToInt16(OWidth) / Convert.ToInt16(OHeight)), Convert.ToInt16(RHeight));
                    NewImage.Save(destPatientProfileImageFilePath, ImageFormat.Jpeg);
                }
                else
                {
                    if (Utility.Application_ID == 5)
                    {
                        string imgPath = Path.GetTempPath();
                        imgPath = imgPath.Replace("\\Temp\\", "\\VirtualStore\\");
                        imgPath = imgPath + Utility.EHRProfileImagePath.Substring(Utility.EHRProfileImagePath.IndexOf('\\') + 1);
                        imgPath = imgPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\-1\\" + dr["Patient_Images_FilePath"].ToString();
                        if (File.Exists(imgPath))
                        {
                            Image img = Image.FromFile(imgPath);
                            string OHeight = img.Size.Height.ToString();
                            string OWidth = img.Size.Width.ToString();

                            NewImage = CommonUtility.ResizeImage(img, (Convert.ToInt16(RHeight) * Convert.ToInt16(OWidth) / Convert.ToInt16(OHeight)), Convert.ToInt16(RHeight));
                            NewImage.Save(destPatientProfileImageFilePath, ImageFormat.Jpeg);
                        }
                        else
                        {
                            dr["Is_deleted"] = 1;                            
                        }
                    }
                    else
                    {
                        dr["Is_deleted"] = 1;
                        //Image img = Image.FromFile(Utility.AditPatientProfileDefaultImagePath);
                        //string OHeight = img.Size.Height.ToString();
                        //string OWidth = img.Size.Width.ToString();
                        //NewImage = CommonUtility.ResizeImage(img, (Convert.ToInt16(RHeight) * Convert.ToInt16(OWidth) / Convert.ToInt16(OHeight)), Convert.ToInt16(RHeight));
                    }
                }
            }
            catch
            {
                dr["Is_deleted"] = 1;
                //Image img = Image.FromFile(Utility.AditPatientProfileDefaultImagePath);
                //string OHeight = img.Size.Height.ToString();
                //string OWidth = img.Size.Width.ToString();
                //NewImage = CommonUtility.ResizeImage(img, (Convert.ToInt16(RHeight) * Convert.ToInt16(OWidth) / Convert.ToInt16(OHeight)), Convert.ToInt16(RHeight));
                //NewImage.Save(destPatientProfileImageFilePath, ImageFormat.Jpeg);
            }
        }

        public static string PracticeWorkConnectionString()
        {
            string ConnectionString = Utility.DBConnString.Trim();
            return ConnectionString;
        }

        public static string DentrixConnectionString()
        {
            //string ConnectionString = ConfigurationManager.ConnectionStrings["DentrixDbConnectionString"].ConnectionString;

            //string ConnectionString = string.Empty;
            //if ((Utility.Application_Version.ToLower() == "DTX G5".ToLower())
            // || (Utility.Application_Version.ToLower() == "DTX G5.1".ToLower())
            // || (Utility.Application_Version.ToLower() == "DTX G5.2".ToLower())
            // || (Utility.Application_Version.ToLower() == "DTX G6".ToLower())
            // || (Utility.Application_Version.ToLower() == "DTX G6.1".ToLower()))
            //{
            //    ConnectionString = "host=" + Utility.EHRHostname + ";UID=" + Utility.EHRUserId + ";PWD=" + Utility.EHRPassword + ";Database=DentrixSQL;DSN=c-treeACE ODBC Driver;port=" + Utility.EHRPort + string.Empty;
            //}
            //else
            //{
            //    ConnectionString = Utility.EHR_DentrixG62_ConnString;
            //}

            string ConnectionString = Utility.DBConnString.Trim();

            return ConnectionString;
        }

        public static string EaglesoftConnectionString(string DbConnString)
        {


            //Type esSettingsType = Type.GetTypeFromProgID("EaglesoftSettings.EaglesoftSettings");
            //dynamic settings = Activator.CreateInstance(esSettingsType);

            //bool tokenIsValid = settings.SetToken("Adit", "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4");
            if (Utility.Application_Version.ToLower() == "21.20".ToLower())
            {
                string conn1 = DbConnString == "" ? Utility.DecryptString(Utility.DBConnString) : Utility.DecryptString(DbConnString);
                return conn1;
            }
            else
            {
                string conn1 = DbConnString == "" ? Utility.DBConnString :DbConnString;
                return conn1;
            }
        }

        public static string GetValueFromAppConfig(string Name)
        {
            string value = string.Empty;
            try
            {
                value = ConfigurationManager.AppSettings[Name].ToString();
            }
            catch (Exception) { }
            return value;
        }

        public static DataTable CreateHolidayEHRId(DataTable dtHolidayForLocal)
        {
            try
            {
                if (!dtHolidayForLocal.Columns.Contains("H_EHR_ID"))
                {
                    DataColumn dcCol = new DataColumn();
                    dcCol.ColumnName = "H_EHR_ID";
                    dcCol.DefaultValue = "";
                    dtHolidayForLocal.Columns.Add(dcCol);
                }
                //EagleSoft
                if (Utility.Application_ID == 1)
                {
                    dtHolidayForLocal.AsEnumerable()
                        .All(o =>
                        {
                            if (dtHolidayForLocal.Columns.Contains("SchedDate"))
                            {
                                o["H_EHR_ID"] = Convert.ToDateTime(o["SchedDate"]).Year.ToString() + "" + Convert.ToDateTime(o["SchedDate"]).Month.ToString() + "" + Convert.ToDateTime(o["SchedDate"]).Day.ToString() + "_0";
                            }
                            return true;
                        });

                }
                //Dentrix
                else if (Utility.Application_ID == 3)
                {
                    dtHolidayForLocal.AsEnumerable()
                       .All(o =>
                       {
                           if (dtHolidayForLocal.Columns.Contains("sched_exception_date"))
                           {
                               if (dtHolidayForLocal.Columns.Contains("op_id"))
                               {
                                   o["H_EHR_ID"] = Convert.ToDateTime(o["sched_exception_date"]).Year.ToString() + "" + Convert.ToDateTime(o["sched_exception_date"]).Month.ToString() + "" + Convert.ToDateTime(o["sched_exception_date"]).Day.ToString() + "_" + o["op_id"].ToString();
                               }
                               else
                               {
                                   o["H_EHR_ID"] = Convert.ToDateTime(o["sched_exception_date"]).Year.ToString() + "" + Convert.ToDateTime(o["sched_exception_date"]).Month.ToString() + "" + Convert.ToDateTime(o["sched_exception_date"]).Day.ToString() + "_0";
                               }
                           }
                           return true;
                       });
                }
                //Tracker
               else if (Utility.Application_ID == 6)
                {
                    dtHolidayForLocal.AsEnumerable()
                        .All(o =>
                        {
                            if (dtHolidayForLocal.Columns.Contains("SchedDate"))
                            {
                                o["H_EHR_ID"] = Convert.ToDateTime(o["SchedDate"]).Year.ToString() + "" + Convert.ToDateTime(o["SchedDate"]).Month.ToString() + "" + Convert.ToDateTime(o["SchedDate"]).Day.ToString() + "_0";
                            }
                            return true;
                        });

                }
                return dtHolidayForLocal;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("CreateHolidayEHRId - "+ ex.Message.ToString());
                return dtHolidayForLocal;
            }
        }

        public static void GetDentrixDLLForDocumentUpload(string exePath)
        {
            try
            {
                DataTable dtDllName = CollectDentrixDLL();
                string filePath = "";
                string fileSourcePath = exePath.Substring(0, exePath.LastIndexOf("\\"));
                string FileTargetPath = Application.StartupPath.ToString() + "\\DocumentDLL";
                string sourceFile = "", destFile = "";
                if (!Directory.Exists(FileTargetPath))
                {
                    Directory.CreateDirectory(FileTargetPath);
                }
                foreach (DataRow drRow in dtDllName.Rows)
                {
                    filePath = FileTargetPath + "\\" + drRow["DLLName"].ToString();
                    if (!File.Exists(filePath))
                    {
                        sourceFile = Path.Combine(fileSourcePath, drRow["DLLName"].ToString());
                        destFile = Path.Combine(FileTargetPath, drRow["DLLName"].ToString());
                        if (File.Exists(sourceFile))
                        {
                            File.Copy(sourceFile, destFile);
                        }
                    }
                }
                //sourceFile = Path.Combine(Application.StartupPath, "AcroPDF.dll");
                //destFile = Path.Combine(FileTargetPath, "AcroPDF.dll");
                //filePath = FileTargetPath + "\\AcroPDF.dll";
                //if (!File.Exists(filePath))
                //{
                //    File.Copy(sourceFile, destFile);
                //}

                sourceFile = Path.Combine(Application.StartupPath, "Dtx.POAssist.dll");
                destFile = Path.Combine(FileTargetPath, "Dtx.POAssist.dll");
                filePath = FileTargetPath + "\\Dtx.POAssist.dll";
                if (!File.Exists(filePath))
                {
                    File.Copy(sourceFile, destFile);
                }
                sourceFile = Path.Combine(Application.StartupPath, "Document.CenterDoc.exe");
                destFile = Path.Combine(FileTargetPath, "Document.CenterDoc.exe");
                filePath = FileTargetPath + "\\Document.CenterDoc.exe";
                if (!File.Exists(filePath))
                {
                    File.Copy(sourceFile, destFile);
                }
                sourceFile = Path.Combine(Application.StartupPath, "Document.CommonUtils.dll");
                destFile = Path.Combine(FileTargetPath, "Document.CommonUtils.dll");
                filePath = FileTargetPath + "\\Document.CommonUtils.dll";
                if (!File.Exists(filePath))
                {
                    File.Copy(sourceFile, destFile);
                }
                sourceFile = Path.Combine(Application.StartupPath, "Splash.dll");
                destFile = Path.Combine(FileTargetPath, "Splash.dll");
                filePath = FileTargetPath + "\\Splash.dll";
                if (!File.Exists(filePath))
                {
                    File.Copy(sourceFile, destFile);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static DataTable CollectDentrixDLL()
        {
            try
            {
                DataTable dtDentrixDLL = new DataTable();
                dtDentrixDLL.Columns.Add("Id");
                dtDentrixDLL.Columns.Add("DLLName");
                dtDentrixDLL.Columns["Id"].AutoIncrement = true;
                dtDentrixDLL.Columns["Id"].AutoIncrementSeed = 1;
                dtDentrixDLL.Columns["Id"].AutoIncrementStep = 1;

                //Need to put this file into the DentrixDLL Folder


                DataRow drRow1 = dtDentrixDLL.NewRow();
                drRow1["DLLName"] = "AxInterop.AcroPDFLib.dll";
                dtDentrixDLL.Rows.Add(drRow1);

                DataRow drRow2 = dtDentrixDLL.NewRow();
                drRow2["DLLName"] = "C4dll.dll";
                dtDentrixDLL.Rows.Add(drRow2);

                DataRow drRow3 = dtDentrixDLL.NewRow();
                drRow3["DLLName"] = "CDAStore.dll";
                dtDentrixDLL.Rows.Add(drRow3);

                DataRow drRow4 = dtDentrixDLL.NewRow();
                drRow4["DLLName"] = "ChilkatDotNet4.dll";
                dtDentrixDLL.Rows.Add(drRow4);

                DataRow drRow5 = dtDentrixDLL.NewRow();
                drRow5["DLLName"] = "ChilkatDotNet46.dll";
                dtDentrixDLL.Rows.Add(drRow5);

                DataRow drRow6 = dtDentrixDLL.NewRow();
                drRow6["DLLName"] = "Controls.Core.dll";
                dtDentrixDLL.Rows.Add(drRow6);

                DataRow drRow7 = dtDentrixDLL.NewRow();
                drRow7["DLLName"] = "Controls.Dave.dll";
                dtDentrixDLL.Rows.Add(drRow7);

                DataRow drRow8 = dtDentrixDLL.NewRow();
                drRow8["DLLName"] = "Ctree.Data.SqlClient.dll";
                dtDentrixDLL.Rows.Add(drRow8);

                DataRow drRow9 = dtDentrixDLL.NewRow();
                drRow9["DLLName"] = "ctsqlapi.dll";
                dtDentrixDLL.Rows.Add(drRow9);

                DataRow drRow10 = dtDentrixDLL.NewRow();
                drRow10["DLLName"] = "Dentrix.Common.Entities.dll";
                dtDentrixDLL.Rows.Add(drRow10);

                DataRow drRow11 = dtDentrixDLL.NewRow();
                drRow11["DLLName"] = "Document.Viewer.dll";
                dtDentrixDLL.Rows.Add(drRow11);

                DataRow drRow12 = dtDentrixDLL.NewRow();
                drRow12["DLLName"] = "Dtx.Activation.dll";
                dtDentrixDLL.Rows.Add(drRow12);

                DataRow drRow13 = dtDentrixDLL.NewRow();
                drRow13["DLLName"] = "Dtx.Common.dll";
                dtDentrixDLL.Rows.Add(drRow13);

                DataRow drRow14 = dtDentrixDLL.NewRow();
                drRow14["DLLName"] = "Dtx.Controls.dll";
                dtDentrixDLL.Rows.Add(drRow14);

                DataRow drRow15 = dtDentrixDLL.NewRow();
                drRow15["DLLName"] = "Dtx.Imaging.dll";
                dtDentrixDLL.Rows.Add(drRow15);

                DataRow drRow16 = dtDentrixDLL.NewRow();
                drRow16["DLLName"] = "Dtx.Shared.dll";
                dtDentrixDLL.Rows.Add(drRow16);

                DataRow drRow17 = dtDentrixDLL.NewRow();
                drRow17["DLLName"] = "Dtx.Shared.Forms.dll";
                dtDentrixDLL.Rows.Add(drRow17);

                DataRow drRow18 = dtDentrixDLL.NewRow();
                drRow18["DLLName"] = "Dtx.Shared.Legacy.dll";
                dtDentrixDLL.Rows.Add(drRow18);

                DataRow drRow19 = dtDentrixDLL.NewRow();
                drRow19["DLLName"] = "Dtx.Shared.Nondb.dll";
                dtDentrixDLL.Rows.Add(drRow19);

                DataRow drRow20 = dtDentrixDLL.NewRow();
                drRow20["DLLName"] = "Dtx.XMLEngine.dll";
                dtDentrixDLL.Rows.Add(drRow20);

                DataRow drRow21 = dtDentrixDLL.NewRow();
                drRow21["DLLName"] = "Dtx32.dll";
                dtDentrixDLL.Rows.Add(drRow21);

                DataRow drRow22 = dtDentrixDLL.NewRow();
                drRow22["DLLName"] = "dtxdb32.dll";
                dtDentrixDLL.Rows.Add(drRow22);

                DataRow drRow23 = dtDentrixDLL.NewRow();
                drRow23["DLLName"] = "DtxHelp.dll";
                dtDentrixDLL.Rows.Add(drRow23);

                DataRow drRow24 = dtDentrixDLL.NewRow();
                drRow24["DLLName"] = "dtxsdir.dll";
                dtDentrixDLL.Rows.Add(drRow24);

                DataRow drRow25 = dtDentrixDLL.NewRow();
                drRow25["DLLName"] = "DtxTracer.dll";
                dtDentrixDLL.Rows.Add(drRow25);

                DataRow drRow26 = dtDentrixDLL.NewRow();
                drRow26["DLLName"] = "DtxTracker.dll";
                dtDentrixDLL.Rows.Add(drRow26);

                DataRow drRow27 = dtDentrixDLL.NewRow();
                drRow27["DLLName"] = "DtxUtils_Dll.dll";
                dtDentrixDLL.Rows.Add(drRow27);

                DataRow drRow28 = dtDentrixDLL.NewRow();
                drRow28["DLLName"] = "Dxprnt32.dll";
                dtDentrixDLL.Rows.Add(drRow28);

                DataRow drRow29 = dtDentrixDLL.NewRow();
                drRow29["DLLName"] = "Infragistics2.Shared.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow29);

                DataRow drRow30 = dtDentrixDLL.NewRow();
                drRow30["DLLName"] = "Infragistics2.Win.Misc.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow30);

                DataRow drRow31 = dtDentrixDLL.NewRow();
                drRow31["DLLName"] = "Infragistics2.Win.UltraWinDock.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow31);

                DataRow drRow32 = dtDentrixDLL.NewRow();
                drRow32["DLLName"] = "Infragistics2.Win.UltraWinEditors.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow32);

                DataRow drRow33 = dtDentrixDLL.NewRow();
                drRow33["DLLName"] = "Infragistics2.Win.UltraWinGrid.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow33);

                DataRow drRow34 = dtDentrixDLL.NewRow();
                drRow34["DLLName"] = "Infragistics2.Win.UltraWinMaskedEdit.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow34);

                DataRow drRow35 = dtDentrixDLL.NewRow();
                drRow35["DLLName"] = "Infragistics2.Win.UltraWinSchedule.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow35);

                DataRow drRow36 = dtDentrixDLL.NewRow();
                drRow36["DLLName"] = "Infragistics2.Win.UltraWinStatusBar.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow36);

                DataRow drRow37 = dtDentrixDLL.NewRow();
                drRow37["DLLName"] = "Infragistics2.Win.UltraWinTabControl.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow37);

                DataRow drRow38 = dtDentrixDLL.NewRow();
                drRow38["DLLName"] = "Infragistics2.Win.UltraWinToolbars.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow38);

                DataRow drRow39 = dtDentrixDLL.NewRow();
                drRow39["DLLName"] = "Infragistics2.Win.UltraWinTree.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow39);

                DataRow drRow40 = dtDentrixDLL.NewRow();
                drRow40["DLLName"] = "Infragistics2.Win.v8.1.dll";
                dtDentrixDLL.Rows.Add(drRow40);
                /////////
                DataRow drRow41 = dtDentrixDLL.NewRow();
                drRow41["DLLName"] = "InsEmp.dll";
                dtDentrixDLL.Rows.Add(drRow41);

                DataRow drRow42 = dtDentrixDLL.NewRow();
                drRow42["DLLName"] = "InsEstimator.dll";
                dtDentrixDLL.Rows.Add(drRow42);

                DataRow drRow43 = dtDentrixDLL.NewRow();
                drRow43["DLLName"] = "Interop.AcroPDFLib.dll";
                dtDentrixDLL.Rows.Add(drRow43);

                DataRow drRow44 = dtDentrixDLL.NewRow();
                drRow44["DLLName"] = "mtclient.dll";
                dtDentrixDLL.Rows.Add(drRow44);

                DataRow drRow45 = dtDentrixDLL.NewRow();
                drRow45["DLLName"] = "Namtuk.MyScreenCaptureNET.dll";
                dtDentrixDLL.Rows.Add(drRow45);

                DataRow drRow46 = dtDentrixDLL.NewRow();
                drRow46["DLLName"] = "NETWrapper.dll";
                dtDentrixDLL.Rows.Add(drRow46);

                DataRow drRow47 = dtDentrixDLL.NewRow();
                drRow47["DLLName"] = "PatAlert.dll";
                dtDentrixDLL.Rows.Add(drRow47);

                DataRow drRow48 = dtDentrixDLL.NewRow();
                drRow48["DLLName"] = "Persist.dll";
                dtDentrixDLL.Rows.Add(drRow48);

                DataRow drRow49 = dtDentrixDLL.NewRow();
                drRow49["DLLName"] = "PORep.dll";
                dtDentrixDLL.Rows.Add(drRow49);

                DataRow drRow50 = dtDentrixDLL.NewRow();
                drRow50["DLLName"] = "Referral.dll";
                dtDentrixDLL.Rows.Add(drRow50);

                DataRow drRow51 = dtDentrixDLL.NewRow();
                drRow51["DLLName"] = "ReportEngine.dll";
                dtDentrixDLL.Rows.Add(drRow51);

                DataRow drRow52 = dtDentrixDLL.NewRow();
                drRow52["DLLName"] = "RptArchitect.dll";
                dtDentrixDLL.Rows.Add(drRow52);

                DataRow drRow53 = dtDentrixDLL.NewRow();
                drRow53["DLLName"] = "Rx.dll";
                dtDentrixDLL.Rows.Add(drRow53);

                DataRow drRow54 = dtDentrixDLL.NewRow();
                drRow54["DLLName"] = "SettingsEngine.dll";
                dtDentrixDLL.Rows.Add(drRow54);

                DataRow drRow56 = dtDentrixDLL.NewRow();
                drRow56["DLLName"] = "SrvrFinder.dll";
                dtDentrixDLL.Rows.Add(drRow56);

                DataRow drRow57 = dtDentrixDLL.NewRow();
                drRow57["DLLName"] = "ssce5532.dll";
                dtDentrixDLL.Rows.Add(drRow57);

                DataRow drRow58 = dtDentrixDLL.NewRow();
                drRow58["DLLName"] = "Touchless.Vision.dll";
                dtDentrixDLL.Rows.Add(drRow58);

                DataRow drRow59 = dtDentrixDLL.NewRow();
                drRow59["DLLName"] = "Trackerbird.Tracker.dll";
                dtDentrixDLL.Rows.Add(drRow59);

                DataRow drRow60 = dtDentrixDLL.NewRow();
                drRow60["DLLName"] = "Updater.dll";
                dtDentrixDLL.Rows.Add(drRow60);

                DataRow drRow61 = dtDentrixDLL.NewRow();
                drRow61["DLLName"] = "VintaSoft.Twain.dll";
                dtDentrixDLL.Rows.Add(drRow61);

                DataRow drRow62 = dtDentrixDLL.NewRow();
                drRow62["DLLName"] = "WebCamLib.dll";
                dtDentrixDLL.Rows.Add(drRow62);

                return dtDentrixDLL;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public static string THardDisk = "";
        public static string AHardDisk = "";
        public static string FrameWork = "";
        public static string CPUUsage = "";
        public static string ServicePack = "";
        public static string ARAM = "";
        public static string TRAM = "";
        public static string OperatingSystem = "";
        public static string SystemType = "";
        public static string ProcessorName = "";
        public static string SystemName = "";
        public static void GetSystemDetails()
        {
            try
            {
                SystemName = Environment.MachineName;
                GetOperatingSystemInfo();
                GetProcessorInfo();
                GetServicePack(); ;
                GetAvailableRAM();
                GetDotNetFramwork();
                GetHardDiskSpace();
                TRAM = GetTotalRAMsize();
               // GetCPUUsage();
            }
            catch
            {
                Utility.WriteToSyncLogFile_All("Error during Get System Detail");
            }
        }

        public static void GetHardDiskSpace()
        {
            double totalhddsize = 0;
            double Ahddsize = 0;
            foreach (DriveInfo info in DriveInfo.GetDrives())
            {
                if (info.IsReady && info.DriveType == DriveType.Fixed)
                {
                    totalhddsize += info.TotalSize;
                    Ahddsize += info.AvailableFreeSpace;
                }
            }
            double TotalSpaceinGB = totalhddsize / (1024 * 1024 * 1024);
            double AfreeSpaceinGB = Ahddsize / (1024 * 1024 * 1024);
            THardDisk = Convert.ToInt16(TotalSpaceinGB).ToString() + " GB";
            AHardDisk = Convert.ToInt16(AfreeSpaceinGB).ToString() + " GB";
        }

        public static void GetDotNetFramwork()
        {
            string path = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo dpath = new DirectoryInfo(path);
            var myFile = dpath.GetDirectories()
              .OrderByDescending(f => f.Name)
              .FirstOrDefault();
            FrameWork = myFile.Name.Replace("v", "");
        }

        public static void GetCPUUsage()
        {
            var cpuCounter = new PerformanceCounter(
           "Processor Information",
           "% Processor Utility",
           "_Total",
           true
       );

            var processUsage = cpuCounter.NextValue();


            CPUUsage = processUsage.ToString();
        }

        public static void GetServicePack()
        {
            OperatingSystem os = Environment.OSVersion;
            String sp = os.ServicePack;
            ServicePack = sp.ToString();
        }

        private static String GetTotalRAMsize()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject item in moc)
            {
                return Convert.ToString(Math.Round(Convert.ToDouble(item.Properties["TotalPhysicalMemory"].Value) / (1024 * 1024 * 1024), 0)) + " GB";//1048576
            }

            return "RAMsize";
        }

        public static void GetAvailableRAM()
        {
            ComputerInfo CI = new ComputerInfo();
            ulong mem = ulong.Parse(CI.AvailablePhysicalMemory.ToString());
            ARAM = (mem / 1048576 + " MB").ToString();
        }

        public static void GetOperatingSystemInfo()
        {
            Console.WriteLine("Displaying operating system info....\n");
            //Create an object of ManagementObjectSearcher class and pass query as parameter.
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject managementObject in mos.Get())
            {
                if (managementObject["Caption"] != null)
                {
                    OperatingSystem = managementObject["Caption"].ToString();   //Display operating system caption
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    SystemType = managementObject["OSArchitecture"].ToString();   //Display operating system architecture.
                }
                //if (managementObject["CSDVersion"] != null)
                //{
                //    txt_ServicePack.Text = managementObject["CSDVersion"].ToString();     //Display operating system version.
                //}
            }
        }

        public static void GetProcessorInfo()
        {
            Console.WriteLine("\n\nDisplaying Processor Name....");
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    ProcessorName = processor_name.GetValue("ProcessorNameString").ToString();   //Display processor ingo.
                }
            }
        }

        public static string LocalBackupDBConnectionString()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["LocalBackupDBConnectionString"].ConnectionString;
            if (Utility.UseMaxBufferSize)
            {
                if (ConnectionString.ToLower().IndexOf("max buffer size=") < 0)
                {
                    ConnectionString += ";SSCE:Max Buffer Size=1024;";
                }
            }
            return ConnectionString;
        }
        public static string PatientDBConnectionString()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["PatientDBConnectionString"].ConnectionString;
            if (Utility.UseMaxBufferSize)
            {
                if (ConnectionString.ToLower().IndexOf("max buffer size=") < 0)
                {
                    ConnectionString += ";SSCE:Max Buffer Size=1024;";
                }
            }
            return ConnectionString;
        }
    }

}
