using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using Pozative.QRY;
using System.Data.SqlServerCe;
using Pozative.UTL;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Patterson.Eaglesoft.Library.Dtos;
using System.Net.NetworkInformation;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Win32;
using System.Management;
using Pozative.QRY.Synch;
using RestSharp;
using System.Net.Security;
using Pozative.BO;
using System.Web.Script.Serialization;
using System.Collections;
using EaglesoftDocument;
using System.Security.Cryptography;
using Ionic.Zip;
using Ionic.Zlib;
using System.Globalization;

namespace Pozative.DAL
{
    public class SynchEaglesoftDAL
    {
        private static object _lock = new object();

        /// <summary>
        /// The cryptographically-strong random number generator.
        /// </summary>
        private static RNGCryptoServiceProvider _crypto = new RNGCryptoServiceProvider();
        private static string PatientId = "";

        public static string EmergencyContactName = "";
        public static string Employer = "";
        public static string EmergencyContactNumber = "";
        public static string preferredDentist = "", preferedhygine = "";
        public static string preferredDentistDefault = "(Select  TOP 1 Provider_id From provider where Practice_id = (SELECT TOP 1 (Practice_Id) FROM Chairs ) and position_id = (Select Top 1 Position_id From Positions where description = 'Dentist') and status = 'A')";
        public static string preferedhygineDefault = "(Select  TOP 1 Provider_id From provider where Practice_id = (SELECT TOP 1 (Practice_Id) FROM Chairs ) and position_id = (Select Top 1 Position_id From Positions where description = 'Hygienist') and status = 'A')";

        public static bool GetEaglesoftConnection(string DbString)
        {

            bool IsConnected = false;
            using (OdbcConnection conn = new OdbcConnection(DbString))
            {
                try
                {
                    conn.Open();
                    IsConnected = true;
                }
                catch (Exception ex)
                {
                    Utility.WriteToErrorLogFromAll("Error:" + ex.Message + ", " + ex.StackTrace);
                    IsConnected = false;
                }
            }
            return IsConnected;
        }

        public static async Task<HttpResponseMessage> Authenticate(string Hostname, string IntegrationKey, string UserId, string Password)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var authDto = new AuthenticateUserDto()
                    {
                        IntegrationKey = IntegrationKey,
                        UserId = UserId,
                        Password = Password
                    };
                    var postBody = JsonConvert.SerializeObject(authDto);
                    var content = new StringContent(postBody, Encoding.UTF8, "application/json");
                    return await client.PostAsync("https://" + Hostname + ":9888/Authenticate", content).ConfigureAwait(false);
                }
            }

            catch (Exception)
            {

                return null;
            }
        }


        #region Treatment Document

        public static bool Save_Treatment_Document_in_EagleSoft(string DbString, string Service_Install_Id, string DocPath, string strTreatmentPlanID = "")
        {

            try
            {
                DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLocaleTreatmentDocData(strTreatmentPlanID);
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
                    }
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string FileName = dr["SubmittedDate"].ToString().Trim() + "-" + dr["TreatmentPlanName"].ToString().Trim() + "-" + dr["PatientName"].ToString().Trim() + ".pdf";
                        FileName = FileName.Replace(":", "");
                        FileName = FileName.Replace("/", "-");

                        string sourceLocation = CommonUtility.GetAditTreatmentDocTempPath() + "\\" + FileName;
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            PullLiveDatabaseDAL.Update_TreatrmentDocNotFound_Live_To_Local(dr["TreatmentPlanId"].ToString());
                            continue;
                        }

                        string tmpFileOrgName = Path.GetFileName(sourceLocation);
                        string SourcePath = Path.GetDirectoryName(sourceLocation);

                        string DestPath = "";
                        if (DocPath == string.Empty || DocPath == "")
                        {
                            DestPath = DocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            DestPath = @"C:\EagleSoft\Data\Documents\patient\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        else
                        {
                            DestPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        // string DestPath = @"C:\EagleSoft\Data\Documents\patient\" + dr["Patient_EHR_ID"].ToString().Trim();

                        string document_type = "";
                        int OpenWith = 0;

                        switch (Path.GetExtension(sourceLocation).ToLower())
                        {
                            case ".doc":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".docx":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".xls":
                                document_type = "Microsoft Excel Worksheet";
                                OpenWith = 0;
                                break;
                            case ".xlsx":
                                document_type = "Microsoft Excel Worksheet";
                                OpenWith = 0;
                                break;
                            case ".ppt":
                                document_type = "Microsoft PowerPoint Presentation";
                                OpenWith = 0;
                                break;
                            case ".pptx":
                                document_type = "Microsoft PowerPoint Presentation";
                                OpenWith = 0;
                                break;
                            case ".pdf":
                                document_type = "Adobe Acrobat Document";
                                OpenWith = 3;
                                break;
                            case ".html":
                                document_type = "HTML document";
                                OpenWith = 1;
                                break;
                            case ".htm":
                                document_type = "HTML document";
                                OpenWith = 1;
                                break;
                            case ".txt":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".rtf":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".jpg":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jpeg":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jpe":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jfif":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".png":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".BMP":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".GIF":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            default:
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                        }
                        if (!System.IO.Directory.Exists(DestPath))
                        {
                            System.IO.Directory.CreateDirectory(DestPath);
                        }
                        Int64 SavePatientDocId = 0;
                        string showingName = dr["TreatmentPlanName"].ToString().Trim().Replace(":", "").Replace("/", "-") + "-" + dr["PatientName"].ToString().Trim();

                        Int64 FolderEHRId = 0;
                        try
                        {
                            string strSelect = "Select group_id from Document_Group Where description = 'Treatment Plans'";
                            using (OdbcConnection conn = new OdbcConnection(DbString))
                            {
                                using (OdbcCommand OdbcCommand = new OdbcCommand(strSelect, conn))
                                {
                                    OdbcCommand.CommandTimeout = 200;
                                    OdbcCommand.CommandType = CommandType.Text;
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    FolderEHRId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                }
                            }
                        }
                        catch(Exception exFol)
                        {
                            FolderEHRId = 0;
                            Utility.WriteToErrorLogFromAll("Error in Folder_EHR_Id : Save_Treatment_Document_in_EagleSoft : Message: " + exFol.Message);
                        }


                        string OdbcSelect = SynchEagleSoftQRY.InsertPatientTreatmentDocData;
                        OdbcSelect = OdbcSelect.Replace("@document_name", showingName);
                        OdbcSelect = OdbcSelect.Replace("@document_type", "'" + document_type.ToString() + "'");
                        OdbcSelect = OdbcSelect.Replace("@document_creation_date", "getdate()");
                        OdbcSelect = OdbcSelect.Replace("@document_modified_date", "getdate()");
                        OdbcSelect = OdbcSelect.Replace("@open_with", OpenWith.ToString());
                        OdbcSelect = OdbcSelect.Replace("@ref_id", "'" + dr["Patient_EHR_ID"].ToString().Trim() + "'");
                        OdbcSelect = OdbcSelect.Replace("@ref_table", "'patient'");
                        OdbcSelect = OdbcSelect.Replace("@EHR_User_ID", "'" + Utility.EHR_UserLogin_ID + "'");
                        // OdbcSelect = OdbcSelect.Replace("@original_user_id", "'GGY'");
                        OdbcSelect = OdbcSelect.Replace("@private", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@display_in_docmgr", "'Y'");
                        OdbcSelect = OdbcSelect.Replace("@signed", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@needs_converted", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@notice_of_privacy", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@privacy_authorization", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@consent", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@practice_id", "(SELECT TOP 1 (Practice_Id) FROM Chairs )");
                        OdbcSelect = OdbcSelect.Replace("@custom_document_id", "-1");
                        OdbcSelect = OdbcSelect.Replace("@headerfooter_added", "0");
                        OdbcSelect = OdbcSelect.Replace("@document_group_id", (FolderEHRId > 0 ? FolderEHRId.ToString() : "23"));
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                try
                                {
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                catch (Exception exAuth)
                                {
                                    //ERROR [23000] [SAP][ODBC Driver][SQL Anywhere]No primary key value for foreign key 'fk_group' in table 'document'
                                    if (!ExecuteNonQuery(DbString, OdbcCommand))
                                    {
                                        Utility.WriteToErrorLogFromAll("Save_Treatment_Document_in_EagleSoft Error " + exAuth.Message.ToString());
                                    }
                                }
                            }
                            OdbcSelect = "Select CONVERT(NUMERIC(10,0),( MAX(ISNULL(document_Id,0)) ) ) From Document";
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                SavePatientDocId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                            }
                        }

                        //OdbcSelect = "delete From Document where document_id = 189";
                        //CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        //OdbcCommand.ExecuteNonQuery();    
                        string outputPath = DestPath;
                        string str = Path.Combine(SourcePath, tmpFileOrgName);
                        File.Copy(str, str + ".ORG", true);
                        string newZipFileName = Path.Combine(outputPath, string.Format("{0}~{1}.esd", (object)dr["Patient_EHR_ID"].ToString().Trim(), (object)SavePatientDocId.ToString()));
                        using (ZipFile zipFile = new ZipFile())
                        {
                            zipFile.Password = "HIMOM";
                            zipFile.UseZip64WhenSaving = Zip64Option.AsNecessary;
                            zipFile.CompressionLevel = CompressionLevel.BestCompression;
                            zipFile.Encryption = EncryptionAlgorithm.PkzipWeak;
                            zipFile.ZipErrorAction = ZipErrorAction.Throw;
                            zipFile.ParallelDeflateThreshold = -1L;
                            zipFile.AddFile(str, "");
                            // zipFile.AddFile(str + ".ORG", "");
                            zipFile.Save(newZipFileName);
                        }
                        //Document.CreateSmartDoc(dr["Patient_EHR_ID"].ToString().Trim(), // Patient Id
                        //                          SavePatientDocId.ToString(), // Document Id
                        //                          tmpFileOrgName, // Source File Name
                        //                          SourcePath, // Source File Path
                        //                          "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4",
                        //                          true, // Keep Source Copy is true
                        //                          DestPath
                        //                         );
                        PullLiveDatabaseDAL.Update_TreatmentFormDoc_Local_To_EHR(dr["TreatmentDoc_Web_ID"].ToString(), SavePatientDocId.ToString());
                        File.Delete(sourceLocation);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;

            }
        }

        #endregion


        #region Insurance Carrier

        public static bool Save_InsuranceCarrier_Document_in_EagleSoft(string DbString, string Service_Install_Id, string DocPath, string strInsuranceCarrierID = "")
        {

            try
            {
                DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLocaleInsuranceCarrierDocData(strInsuranceCarrierID);
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
                    }
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string FileName = dr["InsuranceCarrier_Doc_Name"].ToString().Trim() ;
                        FileName = FileName.Replace(":", "");
                        FileName = FileName.Replace("/", "-");

                        string sourceLocation = CommonUtility.GetAditInsuranceCarrierDocTempPath() + "\\" + FileName;
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            PullLiveDatabaseDAL.Update_InsuranceCarrierDocNotFound_Live_To_Local(dr["InsuranceCarrier_Doc_Web_ID"].ToString());
                            continue;
                        }

                        string tmpFileOrgName = Path.GetFileName(sourceLocation);
                        string SourcePath = Path.GetDirectoryName(sourceLocation);

                        string DestPath = "";
                        if (DocPath == string.Empty || DocPath == "")
                        {
                            DestPath = DocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            DestPath = @"C:\EagleSoft\Data\Documents\patient\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        else
                        {
                            DestPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        // string DestPath = @"C:\EagleSoft\Data\Documents\patient\" + dr["Patient_EHR_ID"].ToString().Trim();

                        string document_type = "";
                        int OpenWith = 0;

                        switch (Path.GetExtension(sourceLocation).ToLower())
                        {
                            case ".doc":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".docx":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".xls":
                                document_type = "Microsoft Excel Worksheet";
                                OpenWith = 0;
                                break;
                            case ".xlsx":
                                document_type = "Microsoft Excel Worksheet";
                                OpenWith = 0;
                                break;
                            case ".ppt":
                                document_type = "Microsoft PowerPoint Presentation";
                                OpenWith = 0;
                                break;
                            case ".pptx":
                                document_type = "Microsoft PowerPoint Presentation";
                                OpenWith = 0;
                                break;
                            case ".pdf":
                                document_type = "Adobe Acrobat Document";
                                OpenWith = 3;
                                break;
                            case ".html":
                                document_type = "HTML document";
                                OpenWith = 1;
                                break;
                            case ".htm":
                                document_type = "HTML document";
                                OpenWith = 1;
                                break;
                            case ".txt":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".rtf":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".jpg":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jpeg":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jpe":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jfif":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".png":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".BMP":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".GIF":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            default:
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                        }
                        if (!System.IO.Directory.Exists(DestPath))
                        {
                            System.IO.Directory.CreateDirectory(DestPath);
                        }
                        Int64 SavePatientDocId = 0;
                        string showingName = dr["InsuranceCarrier_Doc_Name"].ToString().Trim().Replace(":", "").Replace("/", "-");

                        //Int64 FolderEHRId = 0;
                        //try
                        //{
                        //    string strSelect = "Select group_id from Document_Group Where description = 'Treatment Plans'";
                        //    using (OdbcConnection conn = new OdbcConnection(DbString))
                        //    {
                        //        using (OdbcCommand OdbcCommand = new OdbcCommand(strSelect, conn))
                        //        {
                        //            OdbcCommand.CommandTimeout = 200;
                        //            OdbcCommand.CommandType = CommandType.Text;
                        //            if (conn.State == ConnectionState.Closed) conn.Open();
                        //            FolderEHRId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                        //        }
                        //    }
                        //}
                        //catch (Exception exFol)
                        //{
                        //    FolderEHRId = 0;
                        //    Utility.WriteToErrorLogFromAll("Error in Folder_EHR_Id : Save_Treatment_Document_in_EagleSoft : Message: " + exFol.Message);
                        //}


                        string OdbcSelect = SynchEagleSoftQRY.InsertPatientInsuranceCarrierDocData;
                        OdbcSelect = OdbcSelect.Replace("@document_name", showingName);
                        OdbcSelect = OdbcSelect.Replace("@document_type", "'" + document_type.ToString() + "'");
                        OdbcSelect = OdbcSelect.Replace("@document_creation_date", "getdate()");
                        OdbcSelect = OdbcSelect.Replace("@document_modified_date", "getdate()");
                        OdbcSelect = OdbcSelect.Replace("@open_with", OpenWith.ToString());
                        OdbcSelect = OdbcSelect.Replace("@ref_id", "'" + dr["Patient_EHR_ID"].ToString().Trim() + "'");
                        OdbcSelect = OdbcSelect.Replace("@ref_table", "'patient'");
                        OdbcSelect = OdbcSelect.Replace("@EHR_User_ID", "'" + Utility.EHR_UserLogin_ID + "'");
                        // OdbcSelect = OdbcSelect.Replace("@original_user_id", "'GGY'");
                        OdbcSelect = OdbcSelect.Replace("@private", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@display_in_docmgr", "'Y'");
                        OdbcSelect = OdbcSelect.Replace("@signed", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@needs_converted", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@notice_of_privacy", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@privacy_authorization", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@consent", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@practice_id", "(SELECT TOP 1 (Practice_Id) FROM Chairs )");
                        OdbcSelect = OdbcSelect.Replace("@custom_document_id", "-1");
                        OdbcSelect = OdbcSelect.Replace("@headerfooter_added", "0");
                        OdbcSelect = OdbcSelect.Replace("@document_group_id", dr["InsuranceCarrier_FolderName"].ToString().Trim());
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                try
                                {
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                catch (Exception exAuth)
                                {
                                    //ERROR [23000] [SAP][ODBC Driver][SQL Anywhere]No primary key value for foreign key 'fk_group' in table 'document'
                                    if (!ExecuteNonQuery(DbString, OdbcCommand))
                                    {
                                        Utility.WriteToErrorLogFromAll("Save_InsuranceCarrier_Document_in_EagleSoft Error " + exAuth.Message.ToString());
                                    }
                                }
                            }
                            OdbcSelect = "Select CONVERT(NUMERIC(10,0),( MAX(ISNULL(document_Id,0)) ) ) From Document";
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                SavePatientDocId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                            }
                        }

                        //OdbcSelect = "delete From Document where document_id = 189";
                        //CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        //OdbcCommand.ExecuteNonQuery();    
                        string outputPath = DestPath;
                        string str = Path.Combine(SourcePath, tmpFileOrgName);
                        File.Copy(str, str + ".ORG", true);
                        string newZipFileName = Path.Combine(outputPath, string.Format("{0}~{1}.esd", (object)dr["Patient_EHR_ID"].ToString().Trim(), (object)SavePatientDocId.ToString()));
                        using (ZipFile zipFile = new ZipFile())
                        {
                            zipFile.Password = "HIMOM";
                            zipFile.UseZip64WhenSaving = Zip64Option.AsNecessary;
                            zipFile.CompressionLevel = CompressionLevel.BestCompression;
                            zipFile.Encryption = EncryptionAlgorithm.PkzipWeak;
                            zipFile.ZipErrorAction = ZipErrorAction.Throw;
                            zipFile.ParallelDeflateThreshold = -1L;
                            zipFile.AddFile(str, "");
                            // zipFile.AddFile(str + ".ORG", "");
                            zipFile.Save(newZipFileName);
                        }
                        //Document.CreateSmartDoc(dr["Patient_EHR_ID"].ToString().Trim(), // Patient Id
                        //                          SavePatientDocId.ToString(), // Document Id
                        //                          tmpFileOrgName, // Source File Name
                        //                          SourcePath, // Source File Path
                        //                          "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4",
                        //                          true, // Keep Source Copy is true
                        //                          DestPath
                        //                         );
                        PullLiveDatabaseDAL.Update_InsuranceCarrierDoc_Local_To_EHR(dr["InsuranceCarrier_Doc_Web_ID"].ToString(), SavePatientDocId.ToString());
                        File.Delete(sourceLocation);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;

            }
        }

        #endregion

        #region ActualVersionNumber
        public static string GetEaglesoftEHR_VersionNumber(string DbString)
        {
            string version = "";
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEaglesoftActualVersion;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        //OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                        if (OdbcDt.Rows.Count > 0)
                            if (OdbcDt.Rows[0]["version"].ToString() != "" || OdbcDt.Rows[0]["version"].ToString() != string.Empty)
                            {

                                version = OdbcDt.Rows[0]["version"].ToString();

                            }
                            else
                            {
                                version = "";
                            }
                    }
                }
                return version;
            }
            catch (Exception ex)
            {
                version = "";
                throw ex;
                //version = "";

            }
        }




        #endregion

        #region  Appointment

        public static DataTable GetEaglesoftAppointmentData(string DbString, string strApptID = "")
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftAppointmentData;
                    if (!string.IsNullOrEmpty(strApptID))
                    {
                        OdbcSelect = OdbcSelect + " And appointment_id = '" + strApptID + "'";
                        if (ToDate == default(DateTime))
                        {
                            ToDate = Utility.Datetimesetting().AddDays(-7);
                        }
                    }
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEagleSoftCustPrompts(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    //" INSERT INTO patient_answers (Patient_Prompt_Id,Patient_id,answer) values (@EmergencyType,@Patient_EHR_Id,@EmergencyValue)";
                    string OdbcSelect = "Select * From patient_prompts";
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEaglesoftAppointmentIds(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftAppointmentEhrIds;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEaglesoftAppointment_Procedures_Data(string DbString, string strApptID = "")
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEaglesoftAppointment_Procedures_Data;
                    if (!string.IsNullOrEmpty(strApptID))
                    {
                        OdbcSelect = OdbcSelect + " And CONVERT(Varchar(50),Appt.appointment_id) = '" + strApptID + "'";
                        if (ToDate == default(DateTime))
                        {
                            ToDate = Utility.Datetimesetting().AddDays(-7);
                        }
                    }
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool Save_Appointment_Eaglesoft_To_Local(DataTable dtEaglesoftAppointment, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;
                    int tmpappointment_status_ehr_key = 0;
                    string AppointmentStatus = string.Empty;
                    bool is_deleted = false;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        bool is_ehr_updated = false;

                        foreach (DataRow dr in dtEaglesoftAppointment.Rows)
                        {
                            is_ehr_updated = false;
                            tmpappointment_status_ehr_key = 0;
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;
                                        is_ehr_updated = true;
                                        break;
                                    case 5:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Contact;
                                        is_ehr_updated = true;
                                        break;
                                    case 4:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID;
                                        is_ehr_updated = true;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                        break;
                                }
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    tmpappointment_status_ehr_key = Convert.ToInt32(dr["appointment_status_ehr_key"].ToString());
                                    switch (tmpappointment_status_ehr_key)
                                    {
                                        case 0:
                                            AppointmentStatus = "Unconfirmed";
                                            break;
                                        case 1:
                                            AppointmentStatus = "Confirmed";
                                            break;
                                        case 2:
                                            AppointmentStatus = "Sent Email";
                                            break;
                                        case 3:
                                            AppointmentStatus = "Left Message";
                                            break;
                                        case 4:
                                            AppointmentStatus = "No Answer";
                                            break;
                                        case 5:
                                            AppointmentStatus = "Phone Busy";
                                            break;
                                        case 6:
                                            AppointmentStatus = "Waiting For Callback";
                                            break;
                                        case 7:
                                            AppointmentStatus = "Other";
                                            break;
                                        case 8:
                                            AppointmentStatus = "completed";
                                            break;
                                    }



                                    int commentlen = 1999;
                                    if (dr["comment"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["comment"].ToString().Trim().Length;
                                    }
                                    //rooja 2-5-23 - added classification=4 cond for unscheduled appt
                                    if (dr["Operatory_EHR_ID"].ToString().Trim() == "-1" || dr["Operatory_EHR_ID"].ToString().Trim() == "" || dr["classification"].ToString().Trim() == "16" || dr["classification"].ToString().Trim() == "32" || dr["classification"].ToString().Trim() == "64" || dr["classification"].ToString().Trim() == "4")
                                    {
                                        is_deleted = true;
                                    }
                                    else
                                    {
                                        is_deleted = false;
                                    }

                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Patient_EHR_id", dr["Patient_EHR_id"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("MI", dr["MI"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Home_Contact", Utility.ConvertContactNumber(dr["Home_Contact"].ToString().Trim()));
                                    SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", Utility.ConvertContactNumber(dr["Mobile_Contact"].ToString().Trim()));
                                    SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Address", dr["Address"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ST", dr["ST"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Zip", dr["Zip"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_Name", dr["ProviderName"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ApptType", dr["ApptType"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("birth_date", dr["Birth_date"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", dr["Appt_DateTime"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", dr["Appt_EndDateTime"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Status", "1");
                                    SqlCeCommand.Parameters.AddWithValue("Patient_Status", "new");
                                    SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", tmpappointment_status_ehr_key);
                                    SqlCeCommand.Parameters.AddWithValue("Appointment_Status", AppointmentStatus);
                                    SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", "");
                                    SqlCeCommand.Parameters.AddWithValue("confirmed_status", "");
                                    SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
                                    SqlCeCommand.Parameters.AddWithValue("unschedule_status", "");
                                    SqlCeCommand.Parameters.AddWithValue("Is_Appt", "EHR");
                                    SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", is_ehr_updated);
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    //SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());                                  
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Convert.ToDateTime(dr["EHR_Entry_Date"]));
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", is_deleted);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_asap", Convert.ToBoolean(dr["is_asap"]));
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.Parameters.AddWithValue("appt_treatmentcode", "");
                                    SqlCeCommand.Parameters.AddWithValue("ProcedureDesc", dr["ProcedureDesc"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ProcedureCode", dr["ProcedureCode"].ToString().Trim());
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    //  SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static DataTable GetEaglesoftAppointmentProviderData(string Appointment_EHR_ID, string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftAppointmentProviderData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("@patient_id", Appointment_EHR_ID);
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEaglesoftAppointmentProviderData_New(string DbString)
        {
            try
            {
                DateTime ToDate = Utility.LastSyncDateAditServer;
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftAppointmentProviderDataNew;
                    OdbcSelect = OdbcSelect.Replace("@ToDate", ToDate.ToString("yyyy/MM/dd"));
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("GetEaglesoftAppointmentProviderData_New -> Error : " + ex.Message);
                throw ex;
            }

        }

        public static bool UpdateEaglesoftConfirmAppointmentData(string Appointment_EHR_ID, string DbString)
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            try
            {
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.UpdateEagleSoftConfirmAppointmentData;
                    OdbcSelect = OdbcSelect.Replace("@date_Confirmed", "'" + dtCurrentDtTime.ToString("yyyy/MM/dd") + "'");
                    OdbcSelect = OdbcSelect.Replace("@AppointmentId", "'" + Appointment_EHR_ID.ToString() + "'");
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;

            }
        }

        public static bool UpdateAppointmentConfirmationSetConfirmed(string Appt_EHR_ID, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.UpdateAppointmentConfirmationSetConfirmed;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", Appt_EHR_ID);
                        SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", "1");
                        SqlCeCommand.Parameters.AddWithValue("Appointment_Status", "Confirmed");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                    // SqlCetx.Commit();

                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static void UpdateAppoitnmentStatusToWeb(string appointmentEHRId, string LocationId,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            try
            {
                var JsonStatusAppointmentlist = new System.Text.StringBuilder();

                Push_ChangeStatusAppointmentListBO StatusAppointmentlist = new Push_ChangeStatusAppointmentListBO
                {
                    Location_ID = LocationId,
                    Organization_ID = Utility.Organization_ID,
                    appt_ehr_id = appointmentEHRId,
                    created_by = Utility.User_ID
                };
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JsonStatusAppointmentlist.Append(javaScriptSerializer.Serialize(StatusAppointmentlist) + ",");

                if (Utility.AppointmentEHRIds == "")
                {
                    Utility.AppointmentEHRIds = appointmentEHRId;
                }
                else
                {
                    Utility.AppointmentEHRIds = Utility.AppointmentEHRIds + "," + appointmentEHRId;
                }
                // }

                string jsonString = "[" + JsonStatusAppointmentlist.ToString().Remove(JsonStatusAppointmentlist.Length - 1) + "]";
                string strStatusAppointmentlist = PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_StatusAppointmentlist(jsonString, "StatusAppointmentlist", LocationId);
                Utility.WriteToSyncLogFile_All("UpdateAppoitnmentStatusToWeb " + strStatusAppointmentlist.ToString());
                if(_filename_EHR_appointment!= "" && _EHRLogdirectory_EHR_appointment!="")
                {
                    Utility.WriteSyncPullLog(_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment, " Update Appoitnment Status To Web for AppointmentId=" + appointmentEHRId);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Update_Status_EHR_Appointment_Live_To_Eaglesoft(DataTable dtLiveAppointment, string DbString, string LocationId,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
            }
            OdbcConnection conn = new OdbcConnection(DbString);
            OdbcCommand OdbcCommand = new OdbcCommand();
            //CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string confirmed_status_key = string.Empty;
                string OdbcSelect = "";
                foreach (DataRow dr in dtLiveAppointment.Rows)
                {
                    Utility.WriteToSyncLogFile_All("SynchDataLiveDB_Pull_EHR_appointment Send to Save " + dr["confirmed_status_ehr_key"].ToString());

                    confirmed_status_key = "";
                    confirmed_status_key = dr["confirmed_status_ehr_key"].ToString().Trim();
                    if (confirmed_status_key == "")
                    {
                        confirmed_status_key = "1";
                    }
                    if (confirmed_status_key == "8")
                    {
                        OdbcSelect = SynchEagleSoftQRY.UpdateEagleSoft_Status_MarkAsWalkedOut;
                    }
                    else
                    {
                        OdbcSelect = SynchEagleSoftQRY.UpdateEagleSoftConfirmAppointmentData;
                    }
                    // Utility.WriteToSyncLogFile_All("SynchDataLiveDB_Pull_EHR_appointment Change Status Query " + OdbcSelect.ToString());
                    try
                    {
                        OdbcSelect = OdbcSelect.Replace("@confirmation_status", confirmed_status_key);
                        OdbcSelect = OdbcSelect.Replace("@date_Confirmed", "'" + dtCurrentDtTime.ToString("yyyy/MM/dd") + "'");
                        OdbcSelect = OdbcSelect.Replace("@AppointmentId", dr["Appt_EHR_ID"].ToString().Trim());
                        OdbcSelect = OdbcSelect.Replace("@EHR_User_ID", "'" + Utility.EHR_UserLogin_ID + "'");
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        //Utility.WriteToSyncLogFile_All("SynchDataLiveDB_Pull_EHR_appointment Execute Query " + OdbcSelect.ToString());
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                            if (confirmed_status_key == "8")
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, " Update EagleSoft Status Mark As WalkedOut into local for AppointmentId=" + dr["Appt_EHR_ID"] + ",EHR_User_ID=" + Utility.EHR_UserLogin_ID.ToString () + ",confirmation_status="+ confirmed_status_key);
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, " Update EagleSoft Confirm AppointmentData into local for AppointmentId=" + dr["Appt_EHR_ID"] + ",EHR_User_ID=" + Utility.EHR_UserLogin_ID.ToString() + ",confirmation_status=" + confirmed_status_key);
                            }
                        }
                        catch (Exception exAuth)
                        {
                            ExecuteNonQuery(DbString, OdbcCommand);
                        }
                        //Utility.WriteToSyncLogFile_All("SynchDataLiveDB_Pull_EHR_appointment Successfully Executed Query " + OdbcSelect.ToString());
                        UpdateAppoitnmentStatusToWeb(dr["Appt_EHR_ID"].ToString().Trim(), LocationId,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
                        Utility.WriteToSyncLogFile_All("SynchDataLiveDB_Pull_EHR_appointment Successfully Status Updated to Live ");
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToSyncLogFile_All("Error while update Status " + ex.Message.ToString());
                        //System.Windows.Forms.MessageBox("Error while update Status " + ex.Message.ToString());
                        //Pozative.UTL.Utility.WriteToSyncLogFile("Error to update Status in Eaglesoft " + ex.Message.ToString());
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;

            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

        }

        #endregion

        #region  OperatoryEvent

        public static DataTable GetEagleSoftOperatoryEventData(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftOperatoryEventData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Save_OperatoryEvent_Eaglesoft_To_Local(DataTable dtEaglesoftOperatoryEvent, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtEaglesoftOperatoryEvent.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryEventData;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
                                        break;
                                }
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    int commentlen = 1999;
                                    if (dr["comment"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["comment"].ToString().Trim().Length;
                                    }
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime", Convert.ToDateTime(dr["StartTime"].ToString()).ToShortDateString() + " " + (Convert.ToDateTime(dr["StartTime"].ToString()).ToShortTimeString() == "00:00" ? Convert.ToDateTime(dr["StartTime"].ToString()).AddMinutes(1).ToShortTimeString() : Convert.ToDateTime(dr["StartTime"].ToString()).ToShortTimeString()));
                                    SqlCeCommand.Parameters.AddWithValue("EndTime", (Convert.ToDateTime(dr["EndTime"].ToString()).ToShortTimeString() == "00:00" ? Convert.ToDateTime(dr["EndTime"].ToString()).AddDays(-1).ToShortDateString() : Convert.ToDateTime(dr["EndTime"].ToString()).ToShortDateString()) + " " + (Convert.ToDateTime(dr["EndTime"].ToString()).ToShortTimeString() == "00:00" ? Convert.ToDateTime(dr["EndTime"].ToString()).AddMinutes(-1).ToShortTimeString() : Convert.ToDateTime(dr["EndTime"].ToString()).ToShortTimeString()));
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.Parameters.AddWithValue("Allow_Book_Appt", Convert.ToBoolean(false));
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    //  SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    //   SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region  Provider


        public static DataTable GetEagleSoftIdelProvider(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftIdelProvider;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEagleSoftProviderData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftProviderData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Save_Provider_Eaglesoft_To_Local(DataTable dtEaglesoftProvider, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //      SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        foreach (DataRow dr in dtEaglesoftProvider.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Provider;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Provider;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Provider_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("MI", "");  // dr["MI"].ToString());
                                if (dr["gender"].ToString().ToLower().Trim() == "M".ToString().ToLower())
                                {
                                    SqlCeCommand.Parameters.AddWithValue("gender", "male");
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("gender", "female");
                                }
                                SqlCeCommand.Parameters.AddWithValue("provider_speciality", dr["provider_speciality"].ToString().Trim());   // dr["specialty_id"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("is_active", Convert.ToInt16(dr["is_active"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region  Speciality

        public static DataTable GetEagleSoftSpecialityData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftSpecialty;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool Save_Speciality_Eaglesoft_To_Local(DataTable dtEaglesoftSpeciality, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //      SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtEaglesoftSpeciality.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"]) == 1)
                            {
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Speciality;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Speciality_Name", dr["Speciality_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //  SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //  SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region FolderList

        public static DataTable GetEagleSoftFolderListData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftFolderListData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool Save_FolderList_Eaglesoft_To_Local(DataTable dtEaglesoftFolderList, string Service_Install_Id, string clinicNumber)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        foreach (DataRow dr in dtEaglesoftFolderList.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }

                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_FolderList;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_FolderList;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_FolderList;
                                        break;
                                    case 4:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_False_FolderList;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("FolderList_EHR_ID", dr["FolderList_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Folder_Name", dr["Folder_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.Parameters.AddWithValue("FolderOrder", 0);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();

                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //  SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static DataTable GetEaglesoftDeletedFolderListData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEaglesoftDeletedOperatoryData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion 

        #region  Operatory

        public static DataTable GetEagleSoftOperatoryData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftOperatoryData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEaglesoftDeletedOperatoryData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEaglesoftDeletedOperatoryData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool Save_Operatory_Eaglesoft_To_Local(DataTable dtEaglesoftOperatory, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        foreach (DataRow dr in dtEaglesoftOperatory.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }

                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Operatory;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Operatory;
                                        break;
                                    case 4:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_False_Operatory;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("OperatoryOrder", dr["OperatoryOrder"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();

                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //  SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region  Appointment Type

        public static DataTable GetEagleSoftApptTypeData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftApptTypeData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool Save_ApptType_Eaglesoft_To_Local(DataTable dtEaglesoftApptType, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtEaglesoftApptType.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptType;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_ApptType;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ApptType_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //   SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region  Patient

        public static DataTable GetPatientListFromEagleSoft(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetPatientListFromEagleSoft;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEagleSoftPatientIds(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftPatientIds;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEaglesoftInsertPatientData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftInsertPatientData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEaglesoftPatientData(string DbString, string strPatIDs = "")
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftPatientData;
                    if (!string.IsNullOrEmpty(strPatIDs))
                    {
                        StringBuilder newQuery = new StringBuilder();
                        newQuery.Append(OdbcSelect);
                        newQuery.Append(" Where p.patient_id in (" + strPatIDs + ") ");
                        OdbcSelect = newQuery.ToString();
                    }
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static DataTable GetEaglesoftAppointmentsPatientData(string DbString, string strPatID = "")
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftAppointmentsPatientData;
                    if (!string.IsNullOrEmpty(strPatID))
                    {
                        OdbcSelect = OdbcSelect + " And p.patient_id = '" + strPatID + "'";
                        if (ToDate == default(DateTime))
                        {
                            ToDate = Utility.Datetimesetting().AddDays(-7);
                        }
                    }
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static DataTable GetEaglesoftPatientPayment(string DbString, string patientEHRId, string Service_Install_Id)
        {

            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftPatientDataFromPayment;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        //OdbcCommand.Parameters.Add("@PatientEHRId", OdbcType.Date).Value = patientEHRId.ToString();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }

                if (OdbcDt.Rows.Count > 0)
                {
                    using (SqlCeConnection connce = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                    {
                        string SqlCeSelect = SynchLocalQRY.Update_Patient;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, connce))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                            OdbcDt.Rows[0]["InsUptDlt"] = "2";

                            ExecuteQuery("Patient", OdbcDt.Rows[0], SqlCeCommand, Service_Install_Id);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static DataTable GetAllEaglesoftPatientRecallTpyeDueDate(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetAllEagleSoftPatientRecallTpyeDueDateData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        //OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEaglesoftPatientRecallTpyeDueDate(string Patient_EHR_ID, string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftPatientRecallTpyeDueDateData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("@patient_id", Patient_EHR_ID);
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public static bool Save_Patient_Eaglesoft_To_Local(DataTable dtEaglesoftPatient)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    //      SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    //      SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        //if (conn.State == ConnectionState.Closed) conn.Open(); 
        //        string sqlSelect = string.Empty;
        //        string Status = string.Empty;

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtEaglesoftPatient.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                try
        //                {
        //                    Status = dr["Status"].ToString().Trim();
        //                }
        //                catch (Exception)
        //                { Status = ""; }

        //                if (Status == "A")
        //                { Status = "A"; }
        //                else
        //                { Status = "I"; }

        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["Patient_EHR_ID"].ToString().Trim());
        //                //SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", i.ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Status", Status);
        //                SqlCeCommand.Parameters.AddWithValue("Sex", dr["tmpSex"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("MaritalStatus", dr["tmpMaritalStatus"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Birth_Date", dr["tmpBirth_Date"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Address1", dr["Address1"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Zipcode"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Status", dr["ResponsibleParty_Status"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("CurrentBal", dr["CurrentBal"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["tmpFirstVisit_Date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["tmpLastVisit_Date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["PrimaryInsuranceCompanyId"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["PrimaryInsuranceCompanyName"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["SecondaryInsuranceCompanyId"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["SecondaryInsuranceCompanyName"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", dr["tmpReceiveSMS"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", dr["tmpReceiveEmail"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["tmpnextvisit_date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", dr["remaining_benefit"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("used_benefit", dr["used_benefit"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("collect_payment", dr["collect_payment"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", "");
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }
        //        //SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        // SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        public static bool Save_Patient_Eaglesoft_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id, bool isApptPatient)
        {
            bool _successfullstataus = true;
            try
            {


                SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtOpenDentalDataToSave, "0", Service_Install_Id);
            }
            catch (Exception)
            {
            }
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    using (SqlCeCommand SqlCeComBulk = new SqlCeCommand("", conn))
                    {
                        SqlCeComBulk.CommandType = CommandType.TableDirect;
                        SqlCeComBulk.CommandText = "Patient";
                        SqlCeComBulk.Connection = conn;
                        SqlCeComBulk.IndexName = "unique_Patient_EHR_ID";

                        SqlCeResultSet rs = SqlCeComBulk.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                        foreach (DataRow dr in dtOpenDentalDataToSave.Rows)
                        {
                            string tmpBirthDate = string.Empty;
                            string Status = string.Empty;
                            try
                            {
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                                {
                                    try
                                    {
                                        Status = dr["Status"].ToString().Trim();
                                    }
                                    catch (Exception)
                                    { Status = ""; }

                                    if (Status == "A")
                                    { Status = "A"; }
                                    else
                                    { Status = "I"; }

                                    tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

                                    if (tmpBirthDate != "")
                                    {
                                        tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
                                    }

                                    try
                                    {
                                        Status = dr["Status"].ToString().Trim();
                                    }
                                    catch (Exception)
                                    { Status = ""; }

                                    bool found = false;
                                    try
                                    {
                                        found = rs.Seek(DbSeekOptions.FirstEqual, new { PatID = dr["Patient_EHR_ID"].ToString().Trim(), CliNum = Clinic_Number, ServiceInstalledID = Service_Install_Id });
                                    }
                                    catch (Exception exFound)
                                    {
                                        if (exFound.Message.ToUpper().Contains("OBJECT MUST IMPLEMENT ICONVERTIBLE"))
                                        {
                                            found = rs.Seek(DbSeekOptions.FirstEqual, new object[] { dr["Patient_EHR_ID"].ToString().Trim(), Clinic_Number, Service_Install_Id });
                                        }
                                        else
                                        {
                                            throw exFound;
                                        }
                                    }


                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                                    {
                                        if (found == false)
                                        {
                                            SqlCeUpdatableRecord rec = rs.CreateRecord();
                                            rec.SetValue(rs.GetOrdinal("patient_ehr_id"), dr["Patient_EHR_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("patient_Web_ID"), "");
                                            rec.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Salutation"), dr["Salutation"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("preferred_name"), dr["preferred_name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Status"), Status);
                                            rec.SetValue(rs.GetOrdinal("Sex"), dr["Sex"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("MaritalStatus"), dr["MaritalStatus"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Birth_Date"), tmpBirthDate);
                                            rec.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Work_Phone"), Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Address1"), dr["Address1"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Address2"), dr["Address2"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("City"), dr["City"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("State"), dr["State"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Zipcode"), dr["Zipcode"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ResponsibleParty_Status"), dr["ResponsibleParty_Status"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("CurrentBal"), dr["CurrentBal"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ThirtyDay"), dr["ThirtyDay"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("SixtyDay"), dr["SixtyDay"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("NinetyDay"), dr["NinetyDay"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Over90"), dr["Over90"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Primary_Insurance"), dr["Primary_Insurance"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Primary_Insurance_CompanyName"), dr["Primary_Insurance_CompanyName"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Secondary_Insurance"), dr["Secondary_Insurance"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Secondary_Insurance_CompanyName"), dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Guar_ID"), dr["Guar_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Pri_Provider_ID"), dr["Pri_Provider_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Sec_Provider_ID"), dr["Sec_Provider_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ReceiveSMS"), dr["ReceiveSMS"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ReceiveEmail"), dr["ReceiveEmail"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("due_date"), dr["due_date"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("remaining_benefit"), dr["remaining_benefit"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("used_benefit"), dr["used_benefit"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("collect_payment"), dr["collect_payment"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.GetCurrentDatetimestring());
                                            rec.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rec.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rec.SetValue(rs.GetOrdinal("Secondary_Ins_Subscriber_ID"), dr["Secondary_Ins_Subscriber_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Primary_Ins_Subscriber_ID"), dr["Primary_Ins_Subscriber_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Clinic_Number"), "0");
                                            bool isDeleted = false;
                                            try
                                            {
                                                if (dr.Table.Columns.Contains("Is_Deleted"))
                                                {
                                                    if (dr["Is_Deleted"] == DBNull.Value)
                                                    {
                                                        isDeleted = false;
                                                    }
                                                    else
                                                    {
                                                        isDeleted = Convert.ToBoolean(dr["Is_Deleted"]);
                                                    }
                                                }
                                            }
                                            catch
                                            { }
                                            rec.SetValue(rs.GetOrdinal("Is_Deleted"), isDeleted);
                                            rec.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
                                            rec.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("PreferredLanguage"), dr["PreferredLanguage"].ToString().Trim());
                                            if (!isApptPatient)
                                            {
                                                rec.SetValue(rs.GetOrdinal("ssn"), dr["ssn"].ToString().Trim());
                                                rec.SetValue(rs.GetOrdinal("encrypted_social_security"), dr["encrypted_social_security"].ToString().Trim());
                                            }
                                            rec.SetValue(rs.GetOrdinal("driverlicense"), dr["driverlicense"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("groupid"), dr["groupid"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("emergencycontactId"), dr["emergencycontactId"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("EmergencyContact_First_Name"), dr["EmergencyContact_First_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("EmergencyContact_Last_Name"), dr["EmergencyContact_Last_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("emergencycontactnumber"), dr["emergencycontactnumber"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("school"), dr["School"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("employer"), dr["Employer"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("spouseId"), dr["SpouseId"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Spouse_First_Name"), dr["Spouse_First_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Spouse_Last_Name"), dr["Spouse_Last_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("responsiblepartyId"), dr["responsiblepartyId"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ResponsibleParty_First_Name"), dr["ResponsibleParty_First_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ResponsibleParty_Last_Name"), dr["ResponsibleParty_Last_Name"].ToString().Trim());
                                            if (!isApptPatient)
                                            { 
                                                rec.SetValue(rs.GetOrdinal("responsiblepartyssn"), dr["responsiblepartyssn"].ToString().Trim());
                                                rec.SetValue(rs.GetOrdinal("RespEncrypted_social_security"), dr["RespEncrypted_social_security"].ToString().Trim());
                                            }
                                            rec.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), dr["responsiblepartybirthdate"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Prim_Ins_Company_Phonenumber"), dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Sec_Ins_Company_Phonenumber"), dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Patient_Note"), dr["Patient_Note"].ToString().Trim().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).Trim() : dr["Patient_Note"].ToString().Trim());
                                            try
                                            {
                                                rs.Insert(rec);
                                            }
                                            catch (Exception exduplicate)
                                            {
                                                if (exduplicate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                                {
                                                    Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient: " + dr["Patient_EHR_ID"].ToString().Trim() + ", Clinic_Number:" + Clinic_Number + ", Service_Installed_ID:" + Service_Install_Id);
                                                    continue;
                                                }
                                                else
                                                {
                                                    throw exduplicate;
                                                }
                                            }
                                        }
                                    }
                                    else if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 2 || Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                    {
                                        if (found == true)
                                        {
                                            rs.Read();
                                            rs.SetValue(rs.GetOrdinal("patient_ehr_id"), dr["Patient_EHR_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("patient_Web_ID"), "");
                                            rs.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Salutation"), dr["Salutation"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("preferred_name"), dr["preferred_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Status"), Status);
                                            rs.SetValue(rs.GetOrdinal("Sex"), dr["Sex"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("MaritalStatus"), dr["MaritalStatus"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Birth_Date"), tmpBirthDate);
                                            rs.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Work_Phone"), Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Address1"), dr["Address1"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Address2"), dr["Address2"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("City"), dr["City"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("State"), dr["State"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Zipcode"), dr["Zipcode"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_Status"), dr["ResponsibleParty_Status"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("CurrentBal"), dr["CurrentBal"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ThirtyDay"), dr["ThirtyDay"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("SixtyDay"), dr["SixtyDay"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("NinetyDay"), dr["NinetyDay"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Over90"), dr["Over90"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Primary_Insurance"), dr["Primary_Insurance"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Primary_Insurance_CompanyName"), dr["Primary_Insurance_CompanyName"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Insurance"), dr["Secondary_Insurance"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Insurance_CompanyName"), dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Guar_ID"), dr["Guar_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Pri_Provider_ID"), dr["Pri_Provider_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sec_Provider_ID"), dr["Sec_Provider_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveSMS"), dr["ReceiveSMS"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveEmail"), dr["ReceiveEmail"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("due_date"), dr["due_date"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("remaining_benefit"), dr["remaining_benefit"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("used_benefit"), dr["used_benefit"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("collect_payment"), dr["collect_payment"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.GetCurrentDatetimestring());
                                            rs.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rs.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rs.SetValue(rs.GetOrdinal("Secondary_Ins_Subscriber_ID"), dr["Secondary_Ins_Subscriber_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Primary_Ins_Subscriber_ID"), dr["Primary_Ins_Subscriber_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Clinic_Number"), "0");
                                            bool isDeleted = false;
                                            try
                                            {
                                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                                {
                                                    isDeleted = true;
                                                }
                                                else
                                                {
                                                    if (dr.Table.Columns.Contains("Is_Deleted"))
                                                    {
                                                        if (dr["Is_Deleted"] == DBNull.Value)
                                                        {
                                                            isDeleted = false;
                                                        }
                                                        else
                                                        {
                                                            isDeleted = Convert.ToBoolean(dr["Is_Deleted"]);
                                                        }
                                                    }
                                                }
                                            }
                                            catch
                                            { }
                                            rs.SetValue(rs.GetOrdinal("Is_Deleted"), Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3 ? true : isDeleted);
                                            rs.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
                                            rs.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("PreferredLanguage"), dr["PreferredLanguage"].ToString().Trim());
                                            if (!isApptPatient)
                                            {
                                                rs.SetValue(rs.GetOrdinal("ssn"), dr["ssn"].ToString().Trim());
                                                rs.SetValue(rs.GetOrdinal("encrypted_social_security"), dr["encrypted_social_security"].ToString().Trim());
                                            }
                                            rs.SetValue(rs.GetOrdinal("driverlicense"), dr["driverlicense"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("groupid"), dr["groupid"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("emergencycontactId"), dr["emergencycontactId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EmergencyContact_First_Name"), dr["EmergencyContact_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EmergencyContact_Last_Name"), dr["EmergencyContact_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("emergencycontactnumber"), dr["emergencycontactnumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("school"), dr["School"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("employer"), dr["Employer"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("spouseId"), dr["SpouseId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Spouse_First_Name"), dr["Spouse_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Spouse_Last_Name"), dr["Spouse_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartyId"), dr["responsiblepartyId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_First_Name"), dr["ResponsibleParty_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_Last_Name"), dr["ResponsibleParty_Last_Name"].ToString().Trim());
                                            if (!isApptPatient)
                                            {
                                                rs.SetValue(rs.GetOrdinal("responsiblepartyssn"), dr["responsiblepartyssn"].ToString().Trim());
                                                rs.SetValue(rs.GetOrdinal("RespEncrypted_social_security"), dr["RespEncrypted_social_security"].ToString().Trim());
                                            }
                                            rs.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), dr["responsiblepartybirthdate"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Prim_Ins_Company_Phonenumber"), dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sec_Ins_Company_Phonenumber"), dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Patient_Note"), dr["Patient_Note"].ToString().Trim().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).Trim() : dr["Patient_Note"].ToString().Trim());
                                            try
                                            {
                                                rs.Update();
                                            }
                                            catch (Exception exduplicateupdate)
                                            {
                                                if (exduplicateupdate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                                {
                                                    Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient: " + dr["Patient_EHR_ID"].ToString().Trim() + ", Clinic_Number:" + Clinic_Number + ", Service_Installed_ID:" + Service_Install_Id);
                                                    continue;
                                                }
                                                else
                                                {
                                                    throw exduplicateupdate;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                Utility.WriteToErrorLogFromAll("[Save_Patient_Eaglesoft_To_Local_New] -> [Error Insert/Update Patient] Patient ID : " + dr["patient_ehr_id"].ToString() + ", Message: " + ex1.Message.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool Save_Patient_Eaglesoft_To_Local(DataTable dtEaglesoftPatient, string InsertTableName, DataTable dtLocalPatient, string Service_Install_Id, bool bSetDeleted = false)
        {
            bool _successfullstataus = true;
            SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtEaglesoftPatient, "0", Service_Install_Id);
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                string sqlSelect = string.Empty;
                if (conn.State == ConnectionState.Closed) conn.Open();
                //      SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        //Utility.WriteToSyncLogFile_All("Send to save patient DAL 1" + DateTime.Now.ToString());
                        if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                        {
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.CommandText = "Delete from PatientCompare where Service_Install_Id = @Service_Install_Id ";
                            SqlCeCommand.ExecuteNonQuery();

                            //if (conn.State == ConnectionState.Closed) conn.Open();
                            //SqlCeCommand.CommandText = "ALTER TABLE [PatientCompare] ALTER COLUMN [Patient_LocalDB_ID] IDENTITY (1, 1)";
                            //SqlCeCommand.ExecuteNonQuery();
                        }

                        //foreach (DataRow dr in dtEaglesoftPatient.Rows)
                        //{

                        //''###
                        string patSEX = string.Empty;
                        string ReceiveSMS = string.Empty;
                        string ReceiveEmail = string.Empty;
                        string Status = string.Empty;
                        string MaritalStatus = string.Empty;

                        string tmpRecallType = string.Empty;
                        string tmpRecallTypeId = string.Empty;
                        string tmpDueDate = string.Empty;
                        double tmpremaining_benefit = 0;
                        double tmplocremaining_benefit = 0;
                        double tmpused_benefit = 0;
                        double tmplocused_benefit = 0;
                        string RecallType_DueDate = string.Empty;

                        foreach (DataRow dtDtxRow in dtEaglesoftPatient.Rows)
                        {
                            ExecuteQuery(InsertTableName, dtDtxRow, SqlCeCommand, Service_Install_Id);
                        }
                        //rooja
                        Utility.WriteToSyncLogFile_All("Save_Patient_Eaglesoft_To_Local - insert data into " + InsertTableName + "table ends here " + dtEaglesoftPatient.Rows.Count.ToString() + "  (" + Utility.Application_Name + ") ends.");


                        if (bSetDeleted)
                        {
                            IEnumerable<string> PatientEHRIDs = dtEaglesoftPatient.AsEnumerable().Where(sid => sid["Service_Install_Id"].ToString() == Service_Install_Id).Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                            if (PatientEHRIDs != null && PatientEHRIDs.Any())
                            {

                                IEnumerable<string> LocalEHRIDs = dtLocalPatient.AsEnumerable()
                                    .Where(sid => sid["Service_Install_Id"].ToString() == Service_Install_Id)
                                    .Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                                if (LocalEHRIDs != null && LocalEHRIDs.Any())
                                {

                                    string DeletedEHRIDs = string.Join("','", LocalEHRIDs.Except(PatientEHRIDs).ToList());
                                    if (DeletedEHRIDs != string.Empty)
                                    {
                                        DeletedEHRIDs = "'" + DeletedEHRIDs + "'";
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        string SqlCeSelect = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                                        SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", DeletedEHRIDs);
                                        SqlCeCommand.CommandText = SqlCeSelect;
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }

                    #region Get Records from PatientCompareTAble
                    // Utility.WriteToSyncLogFile_All("Send to save patient Start Compare " + DateTime.Now.ToString());
                    if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                    {
                        DataTable dtPatientCompareRec = new DataTable();

                        SqlCeDataAdapter SqlCeDa = null;
                        // CommonDB.LocalConnectionServer(ref conn);

                        if (conn.State == ConnectionState.Closed) conn.Open();

                        string SqlCeSelect = SynchLocalQRY.PatientCompareQuery;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                            CommonDB.SqlCeDatatAdapterServer(SqlCeCommand, ref SqlCeDa);
                            DataTable SqlCeDt = new DataTable();
                            SqlCeDa.Fill(dtPatientCompareRec);

                            foreach (DataRow drRow in dtPatientCompareRec.Rows)
                            {
                                ExecuteQuery("Patient", drRow, SqlCeCommand, Service_Install_Id);
                            }
                            //rooja
                            Utility.WriteToSyncLogFile_All("Save_Patient_Eaglesoft_To_Local - PatientCompareQuery count " + dtPatientCompareRec.Rows.Count + "  (" + Utility.Application_Name + ") ends.");

                            //if (conn.State == ConnectionState.Closed) conn.Open();
                            //CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.CommandText = "Delete from PatientCompare where Service_Install_Id = @Service_Install_Id ";
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    //Utility.WriteToSyncLogFile_All("Send to save patient Saved Compare " + DateTime.Now.ToString());
                    #endregion



                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static void ExecuteQuery(string InsertTableName, DataRow dr, SqlCeCommand SqlCeCommand, string Service_Install_Id)
        {
            try
            {
                string tmpBirthDate = string.Empty;
                string Status = string.Empty;
                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                {
                    try
                    {
                        Status = dr["Status"].ToString().Trim();
                    }
                    catch (Exception)
                    { Status = ""; }

                    if (Status == "A")
                    { Status = "A"; }
                    else
                    { Status = "I"; }

                    switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                    {
                        case 1:
                            SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
                            if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                            {
                                SqlCeCommand.CommandText = SqlCeCommand.CommandText.Replace("INSERT INTO Patient", "INSERT INTO PatientCompare");
                            }
                            break;
                        case 2:
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient;
                            break;
                        case 3:
                            SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
                            break;
                    }


                    tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

                    if (tmpBirthDate != "")
                    {
                        tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
                    }

                    try
                    {
                        Status = dr["Status"].ToString().Trim();
                    }
                    catch (Exception)
                    { Status = ""; }


                    SqlCeCommand.Parameters.Clear();
                    SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["Patient_EHR_ID"].ToString().Trim());
                    //SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", i.ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
                    SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Status", Status);
                    SqlCeCommand.Parameters.AddWithValue("Sex", dr["Sex"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("MaritalStatus", dr["MaritalStatus"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Birth_Date", tmpBirthDate);
                    SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Address1", dr["Address1"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Zipcode"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Status", dr["ResponsibleParty_Status"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("CurrentBal", dr["CurrentBal"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["Primary_Insurance"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["Primary_Insurance_CompanyName"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["Secondary_Insurance"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", dr["ReceiveSMS"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", dr["ReceiveEmail"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("remaining_benefit", dr["remaining_benefit"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("used_benefit", dr["used_benefit"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("collect_payment", dr["collect_payment"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                    SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", dr["Secondary_Ins_Subscriber_ID"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", dr["Primary_Ins_Subscriber_ID"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                    SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    SqlCeCommand.Parameters.AddWithValue("EHR_Status", dr["EHR_Status"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ReceiveVoiceCall", dr["ReceiveVoiceCall"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("PreferredLanguage", dr["PreferredLanguage"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ssn", dr["ssn"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("driverlicense", dr["driverlicense"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("groupid", dr["groupid"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("emergencycontactId", dr["emergencycontactId"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("EmergencyContact_First_Name", dr["EmergencyContact_First_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("EmergencyContact_Last_Name", dr["EmergencyContact_Last_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("emergencycontactnumber", dr["emergencycontactnumber"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("school", dr["School"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("employer", dr["Employer"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("spouseId", dr["SpouseId"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Spouse_First_Name", dr["Spouse_First_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Spouse_Last_Name", dr["Spouse_Last_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("responsiblepartyId", dr["responsiblepartyId"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_First_Name", dr["ResponsibleParty_First_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Last_Name", dr["ResponsibleParty_Last_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("responsiblepartyssn", dr["responsiblepartyssn"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("responsiblepartybirthdate", dr["responsiblepartybirthdate"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Prim_Ins_Company_Phonenumber", dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Sec_Ins_Company_Phonenumber", dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Patient_Note", dr["Patient_Note"].ToString().Trim().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).Trim() : dr["Patient_Note"].ToString().Trim());
                    SqlCeCommand.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                if (ex.Message.Contains("unique_Patient_EHR_ID"))
                {
                }
                else
                {
                    throw;
                }
            }
        }

        public static bool Update_Receive_SMS_Patient_EHR_Live_To_Eaglesoft(DataTable dtLiveAppointment, string DbString,string Locationid, string Loc_ID,string _filename_EHR_patientoptout = "",string _EHRLogdirectory_EHR_patientoptout = "")
        {

            OdbcConnection conn = new OdbcConnection(DbString);
            OdbcCommand OdbcCommand = new OdbcCommand();
            //  CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                Patient_OptOutBO_StatusUpdate updatedPatientid = new Patient_OptOutBO_StatusUpdate();
                List<Patientids_OptOutBO_StatusUpdate> Patient_StatusUpdate = new List<Patientids_OptOutBO_StatusUpdate>();
                foreach (DataRow dr in dtLiveAppointment.Rows)
                {
                    try
                    {
                        OdbcSelect = SynchEagleSoftQRY.UpdateEagleSoftPatientReceive_sms;
                        OdbcSelect = OdbcSelect.Replace("@patient_id", "'" + dr["patient_ehr_id"].ToString() + "'");
                        OdbcSelect = OdbcSelect.Replace("@receives_sms", "'" + (dr["receive_sms"].ToString() == "Y" ? "Y" : "N") + "'");
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        OdbcCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_patientoptout, _EHRLogdirectory_EHR_patientoptout, " Update Receive SMS Patient EHR Live To Eaglesoft for patient_id=" + dr["patient_ehr_id"].ToString());
                        Patientids_OptOutBO_StatusUpdate Patientids = new Patientids_OptOutBO_StatusUpdate();
                        Patientids.esId = dr["esid"].ToString();
                        Patientids.patientId = dr["patientid"].ToString();
                        Patient_StatusUpdate.Add(Patientids);

                        //Utility.WriteToSyncLogFile_All("SynchDataLiveDB_Pull_EHR_appointment Successfully Status Updated to Live ");
                    }
                    catch (Exception ex)
                    { }

                }
                if (Patient_StatusUpdate.Count > 0)
                {
                    updatedPatientid.locationId = Loc_ID;
                    updatedPatientid.organizationId = Utility.Organization_ID;
                    updatedPatientid.patientIds = Patient_StatusUpdate;
                    PushLiveDatabaseDAL.UpdatePatientReceive_SMStStatusToWeb(updatedPatientid, Locationid, Loc_ID);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;

            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

        }

        #endregion

        #region  RecallType

        public static DataTable GetEagleSoftRecallTypeData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftPatient_RecallType;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        //OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Save_RecallType_Eaglesoft_To_Local(DataTable dtEaglesoftRecallType, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        foreach (DataRow dr in dtEaglesoftRecallType.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_RecallType;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_RecallType;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_RecallType;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("RecallType_EHR_ID", dr["RecallType_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("RecallType_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("RecallType_Name", dr["RecallType_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("RecallType_Descript", "");
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region USer

        public static string GetEaglesoftUserLogin_ID(string DbString)
        {
            try
            {
                string UserId = "";
                bool isexception = false;
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    try
                    {
                        string OdbcSelect = "select isnull(provider_id,'0') as UserId from provider where position_id =5 AND first_name= 'Adit'";
                        using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                        {
                            OdbcCommand.CommandTimeout = 200;
                            OdbcCommand.CommandType = CommandType.Text;
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            object result = OdbcCommand.ExecuteScalar();
                            if (result != null)
                            {
                                UserId = result.ToString();
                            }
                            else
                            {
                                UserId = "0";
                            }
                        }
                        if (UserId == "0")
                        {
                            try
                            {
                                if (Utility.Application_Version != "23.00" && Utility.Application_Version != "22.00")
                                {
                                    string OdbcSelect1 = SynchEagleSoftQRY.InsertEaglesoftUserLogin_Data;
                                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect1, conn))
                                    {
                                        OdbcCommand.CommandTimeout = 200;
                                        OdbcCommand.CommandType = CommandType.Text;
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        try
                                        {
                                            OdbcCommand.ExecuteNonQuery();
                                        }
                                        catch (Exception)
                                        {
                                            ExecuteNonQuery(DbString, OdbcCommand);
                                        }

                                    }
                                }
                            }
                            catch (Exception)
                            {
                                string strqry1;
                                strqry1 = "select Top 1 provider_id from provider where position_id = 5 AND status = 'A' Order by first_name ";
                                using (OdbcCommand OdbcCommand = new OdbcCommand(strqry1, conn))
                                {
                                    OdbcCommand.CommandTimeout = 200;
                                    OdbcCommand.CommandType = CommandType.Text;
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    UserId = OdbcCommand.ExecuteScalar().ToString();
                                }
                                isexception = true;
                            }
                            //UserId = OdbcCommand.ExecuteScalar().ToString();
                            if (!isexception)
                            {
                                string strqry;
                                strqry = "select provider_id from provider where position_id =5 AND first_name= 'Adit'";
                                using (OdbcCommand OdbcCommand = new OdbcCommand(strqry, conn))
                                {
                                    OdbcCommand.CommandTimeout = 200;
                                    OdbcCommand.CommandType = CommandType.Text;
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    UserId = OdbcCommand.ExecuteScalar().ToString();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToErrorLogFromAll("Error in user createion" + ex.Message);
                        UserId = "";
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
                return UserId;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error in user createion" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEagleSoftUserData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftUserData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool Save_User_Eaglesoft_To_Local(DataTable dtEaglesoftUser, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        foreach (DataRow dr in dtEaglesoftUser.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_User;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_User;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_User;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.AddWithValue("User_EHR_ID", dr["User_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("User_EHR_ID", dr["User_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("User_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Updated_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Active", dr["Is_Active"]);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Deleted", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                }
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region  Appointment Status

        public static bool Save_AppointmentStatus_Eaglesoft_To_Local(DataTable dtEaglesoftAppointmentStatus, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtEaglesoftAppointmentStatus.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_AppointmentStatus;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_AppointmentStatus;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_AppointmentStatus;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_EHR_ID", dr["ApptStatus_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Name", dr["ApptStatus_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Type", "normal");
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region  Holidays

        public static DataTable GetEagleSoftHolidayConsiderOnNextDay(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftHolidayConsiderOnNextDay;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        //OdbcCommand.Parameters.Clear();
                        //OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEaglesoftHolidaysData(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftHolidayData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@FromDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                        // OdbcDt = SetHolidayDateForLaborDay(OdbcDt, DbString);
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable SetHolidayDateForLaborDay(DataTable dtResult, string DbString)
        {
            try
            {
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    if (dtResult.AsEnumerable().Where(o => o.Field<object>("comment").ToString().ToUpper() == "LABOR DAY").Count() > 0)
                    {
                        DateTime laborDay = Convert.ToDateTime((System.DateTime.Now.Year + 1) + "/09/01");
                        while (laborDay.DayOfWeek.ToString().ToUpper() != "MONDAY")
                        {
                            laborDay = laborDay.AddDays(1);
                        }
                        dtResult.AsEnumerable().Where(o => o.Field<object>("comment").ToString().ToUpper() == "LABOR DAY")
                            .All(o => { o["SchedDate"] = laborDay; return true; });

                    }
                    if (dtResult.AsEnumerable().Where(o => o.Field<object>("comment").ToString().ToUpper() == "THANKS GIVING DAY").Count() > 0)
                    {
                        DateTime thanksgivingday = Convert.ToDateTime((System.DateTime.Now.Year + 1) + "/11/01");
                        int cnt = 0;
                        while (cnt != 4)
                        {
                            while (thanksgivingday.DayOfWeek.ToString().ToUpper() == "THURSDAY")
                            {
                                cnt = cnt + 1;
                                if (cnt != 4)
                                {
                                    thanksgivingday = thanksgivingday.AddDays(1);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (thanksgivingday.DayOfWeek.ToString().ToUpper() != "THURSDAY")
                            {
                                thanksgivingday = thanksgivingday.AddDays(1);
                            }
                        }
                        dtResult.AsEnumerable().Where(o => o.Field<object>("comment").ToString().ToUpper() == "THANKS GIVING DAY")
                            .All(o => { o["SchedDate"] = thanksgivingday; return true; });
                    }


                    if (dtResult.AsEnumerable().Where(o => Convert.ToDateTime(o.Field<object>("SchedDate").ToString()).Day.ToString().ToUpper() == "SATURDAY" || Convert.ToDateTime(o.Field<object>("SchedDate").ToString()).Day.ToString().ToUpper() == "SUNDAY").Count() > 0)
                    {
                        DataTable considerHolidayOption = GetEagleSoftHolidayConsiderOnNextDay(DbString);
                        if (considerHolidayOption != null && Convert.ToString(considerHolidayOption.Rows[0]["AllowNextDayHoliday"]) == "Y")
                        {
                            dtResult.AsEnumerable().Where(o => Convert.ToDateTime(o.Field<object>("SchedDate").ToString()).Day.ToString().ToUpper() == "SATURDAY" || Convert.ToDateTime(o.Field<object>("SchedDate").ToString()).Day.ToString().ToUpper() == "SUNDAY")
                                .All(o =>
                                {
                                    if (Convert.ToDateTime(o.Field<object>("SchedDate").ToString()).Day.ToString().ToUpper() == "SATURDAY")
                                    {
                                        o["SchedDate"] = Convert.ToDateTime(o["SchedDate"]).AddDays(-1);
                                    }
                                    if (Convert.ToDateTime(o.Field<object>("SchedDate").ToString()).Day.ToString().ToUpper() == "SUNDAY")
                                    {
                                        o["SchedDate"] = Convert.ToDateTime(o["SchedDate"]).AddDays(1);
                                    }
                                    return true;
                                });
                        }
                    }


                }
                return dtResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Save_Holidays_Eaglesoft_To_Local(DataTable dtEaglesoftHoliday, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;


                        string AppointmentStatus = string.Empty;
                        foreach (DataRow dr in dtEaglesoftHoliday.Rows)
                        {

                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_HolidayData;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_HolidayData;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Eaglesoft_HolidayData;
                                        break;
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("SchedDate", dr["SchedDate"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {

                                    int commentlen = 1999;
                                    if (dr["comment"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["comment"].ToString().Trim().Length;
                                    }
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("H_EHR_ID", dr["H_EHR_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("H_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("SchedDate", Utility.CheckValidDatetime(dr["SchedDate"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_1", "");
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_1", "");
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_2", "");
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_2", "");
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_3", "");
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_3", "");
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region Create Patient & Appointment IN EHR
        public static void OdbcConnectionServer(ref OdbcConnection conn)
        {

            Type esSettingsType = Type.GetTypeFromProgID("EaglesoftSettings.EaglesoftSettings");
            dynamic settings = Activator.CreateInstance(esSettingsType);

            bool tokenIsValid = settings.SetToken("Adit", "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4");

            string conn1 = settings.GetLegacyConnectionString(true);
            conn1 = " SET TEMPORARY OPTION CONNECTION_AUTHENTICATION= " + conn1;
            conn1 = conn1.Replace("DENTAL", "PozativeDSN");
            conn = new OdbcConnection(conn1);
        }

        public static string Save_Patient_Local_To_EagleSoft(string LastName, string FirstName, string MiddleName, string MobileNo, string Email, string ApptProv, string AppointmentDateTime, string Patient_Gur_id, int OperatoryId, string Birth_Date, string DbString)
        {
            try
            {
                string PatientId = "0";
                string connctionstring = DbString;
                string conreg = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION= Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407;";
                DbString = conreg + connctionstring;

                if (LastName.Length == 0)
                {
                    LastName += "NA";
                }

                string patBirthDate = "";
                try
                {
                    patBirthDate = Convert.ToDateTime(Birth_Date.ToString()).ToString("yyyy-MM-dd");
                }
                catch (Exception)
                {
                    patBirthDate = "";
                }
                try
                {
                    object Provid = "";
                    object Practiceid;
                    string OdbcSelect = "";
                    using (OdbcConnection conn = new OdbcConnection(DbString))
                    {
                        using (OdbcCommand OdbcCmd = new OdbcCommand("", conn))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            OdbcCmd.CommandTimeout = 200;
                            OdbcCmd.CommandType = CommandType.Text;
                            OdbcSelect = "SELECT (Practice_Id) FROM Chairs WHERE Chair_num = @OperatoryId";
                            OdbcCmd.CommandText = OdbcSelect.Replace("@OperatoryId", OperatoryId.ToString());
                            OdbcCmd.CommandType = CommandType.Text;
                            Practiceid = OdbcCmd.ExecuteScalar();
                        }
                        if (Practiceid == null || Practiceid.ToString() == "")
                        {
                            using (OdbcCommand OdbcCmd = new OdbcCommand("", conn))
                            {
                                OdbcSelect = "Select top 1 Practice_Id FROM Chairs ";
                                OdbcCmd.CommandText = OdbcSelect;
                                OdbcCmd.CommandType = CommandType.Text;
                                Practiceid = OdbcCmd.ExecuteScalar();
                            }
                        }
                        else
                        {
                            Practiceid = Convert.ToInt16(Practiceid);
                        }
                    }
                    if (ApptProv == "" || ApptProv == "0")
                    {
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCmd = new OdbcCommand("", conn))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                OdbcCmd.CommandTimeout = 200;
                                OdbcCmd.CommandType = CommandType.Text;
                                OdbcSelect = "select Top 1 provider_id from provider where position_id = 1 and Status = 'A' Order by  first_name";
                                OdbcCmd.CommandText = OdbcSelect;
                                OdbcCmd.CommandType = CommandType.Text;
                                Provid = OdbcCmd.ExecuteScalar();

                                if (Provid == null || Provid.ToString() == "")
                                {
                                    Provid = 1;
                                }
                                else
                                {
                                    //Provid = Convert.ToInt16(Provid);
                                    Provid = (Provid.ToString().Trim());
                                }
                            }
                        }
                    }
                    if (patBirthDate == "")
                    {
                        OdbcSelect = SynchEagleSoftQRY.InsertPatientDetails;
                    }
                    else
                    {
                        OdbcSelect = SynchEagleSoftQRY.InsertPatientDetails_With_BirthDate;
                    }

                    if (!Utility.GenerateRandomPatientId)
                    {
                        string strQauery = "";
                        string patient_EHR_Id = "0";
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCmd = new OdbcCommand("", conn))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                OdbcCmd.CommandTimeout = 200;
                                OdbcCmd.CommandType = CommandType.Text;
                                strQauery = " SELECT  convert(numeric(10,0), min(patient_id + 1)) as Patient_EHR_Id FROM patient WHERE patient_id liKE '[0-9]%' and reverse(patient_id) like  '[0-9]%'  and ((patient_id + 1) not in (SELECT DISTINCT patient_id FROM patient WHERE patient_id liKE '[0-9]%' and reverse(patient_id) like  '[0-9]%' )) ";
                                OdbcCmd.CommandText = strQauery;
                                patient_EHR_Id = OdbcCmd.ExecuteScalar().ToString();
                            }
                        }

                        OdbcSelect = OdbcSelect.Replace("@Patient_Id", "'" + patient_EHR_Id + "'");
                        OdbcSelect = OdbcSelect.Replace("@FindMaxPatient", "'" + patient_EHR_Id + "'");
                    }
                    else
                    {
                        ReGenerate:
                        string PatId = GetRandomUppercaseAlphaNumericValue(5);
                        DataTable dtPatIds = GetEagleSoftPatientIds(DbString);
                        if (dtPatIds != null && dtPatIds.Rows.Count > 0)
                        {
                            if (dtPatIds.AsEnumerable().Where(o => o.Field<string>("PATIENT_ID").ToUpper() == PatId.ToString().ToUpper()).Count() > 0)
                            {
                                goto ReGenerate;
                            }
                        }
                        OdbcSelect = OdbcSelect.Replace("@Patient_Id", "'" + PatId + "'");
                        OdbcSelect = OdbcSelect.Replace("@FindMaxPatient", "'" + PatId + "'");
                    }
                    OdbcSelect = OdbcSelect.Replace("@Last_Name", "'" + LastName + "'");
                    OdbcSelect = OdbcSelect.Replace("@First_Name", "'" + FirstName + "'");
                    OdbcSelect = OdbcSelect.Replace("@Middle_Initial", "'" + MiddleName + "'");
                    OdbcSelect = OdbcSelect.Replace("@Cell_Phone", "'" + MobileNo + "'");
                    OdbcSelect = OdbcSelect.Replace("@Email_Address", "'" + Email + "'");
                    if (ApptProv == "" || ApptProv == "0")
                    {
                        OdbcSelect = OdbcSelect.Replace("@preferred_dentist", "'" + Provid.ToString() + "'");
                    }
                    else
                    {
                        OdbcSelect = OdbcSelect.Replace("@preferred_dentist", "'" + ApptProv + "'");
                    }
                    OdbcSelect = OdbcSelect.Replace("@First_Visit_Date", "NULL");
                    OdbcSelect = OdbcSelect.Replace("@Status", "'A'");
                    OdbcSelect = OdbcSelect.Replace("@OperatoryId", Practiceid.ToString());
                    OdbcSelect = OdbcSelect.Replace("@Patient_Gur_id", "'" + Patient_Gur_id.ToString() + "'");
                    if (patBirthDate != "")
                    {
                        OdbcSelect = OdbcSelect.Replace("@Birth_Date", "'" + Convert.ToDateTime(patBirthDate).ToString("yyyy/MM/dd") + "'");
                    }
                    Utility.WriteToSyncLogFile_All("save Patient " + OdbcSelect.Replace("@Birth_Date", "'" + Convert.ToDateTime(patBirthDate).ToString("yyyy/MM/dd") + "'")
                        .Replace("@Patient_Gur_id", "'" + Patient_Gur_id.ToString() + "'")
                        .Replace("@OperatoryId", Practiceid.ToString())
                        .Replace("@Status", "'A'")
                        .Replace("@First_Visit_Date", "NULL")
                        .Replace("@Email_Address", "'" + Email + "'")
                        .Replace("@Cell_Phone", "'" + MobileNo + "'")
                        .Replace("@Middle_Initial", "'" + MiddleName + "'")
                        .Replace("@First_Name", "'" + FirstName + "'")
                        .Replace("@Last_Name", "'" + LastName + "'")
                     );
                    //CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    DataTable dt = new DataTable();
                    //Thread.Sleep(30000);
                    using (OdbcConnection conn = new OdbcConnection(DbString))
                    {
                        using (OdbcCommand OdbcCmd = new OdbcCommand(OdbcSelect, conn))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            OdbcCmd.CommandTimeout = 200;
                            OdbcCmd.CommandType = CommandType.Text;
                            try
                            {
                                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCmd))
                                {
                                    OdbcDa.Fill(dt);
                                    PatientId = dt.Rows[0][0].ToString();
                                }
                            }
                            catch (Exception exAuth)
                            {
                                object PID = null;
                                if (ExecuteScalar(DbString, OdbcCmd, ref PID))
                                {
                                    PatientId = PID.ToString();
                                }
                                else
                                {
                                    Utility.WriteToErrorLogFromAll("Error 1 in Save_Patient_Local_To_EagleSoft to Eaglesoft For Patient : " + FirstName + " " + LastName + ", System Error " + exAuth.Message.ToString());
                                }
                            }

                        }
                    }
                    try
                    {
                        if (Patient_Gur_id == "0")
                        {
                            OdbcSelect = SynchEagleSoftQRY.UpdatePatientDetails;
                            OdbcSelect = OdbcSelect.Replace("@Patient_Id", "'" + PatientId.ToString() + "'");
                            using (OdbcConnection conn = new OdbcConnection(DbString))
                            {
                                using (OdbcCommand OdbcCmd = new OdbcCommand(OdbcSelect, conn))
                                {
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    OdbcCmd.CommandTimeout = 200;
                                    OdbcCmd.CommandType = CommandType.Text;
                                    try
                                    {
                                        OdbcCmd.ExecuteNonQuery();
                                    }
                                    catch (Exception exAuth)
                                    {
                                        if (!ExecuteNonQuery(DbString, OdbcCmd))
                                        {
                                            Utility.WriteToErrorLogFromAll("Save_Patient_Local_To_EagleSoft Error 2 " + exAuth.Message.ToString());
                                            throw exAuth;
                                        }
                                    }
                                }
                            }
                        }

                        OdbcSelect = SynchEagleSoftQRY.InsertPatientUsed;
                        OdbcSelect = OdbcSelect.Replace("@Patient_Id", "'" + PatientId.ToString() + "'");
                        OdbcSelect = OdbcSelect.Replace("@Time_used", "'" + Convert.ToDateTime(AppointmentDateTime).ToString("yyyy/MM/dd HH:mm") + "'");
                        OdbcSelect = OdbcSelect.Replace("@preferredDentist", "(Select  TOP 1 Provider_id From provider where Practice_id = (SELECT TOP 1 (Practice_Id) FROM Chairs ) and position_id = (Select Top 1 Position_id From Positions where description = 'Dentist') and status = 'A')");
                        OdbcSelect = OdbcSelect.Replace("@preferredHygenist", "(Select  TOP 1 Provider_id From provider where Practice_id = (SELECT TOP 1 (Practice_Id) FROM Chairs ) and position_id = (Select Top 1 Position_id From Positions where description = 'Hygienist') and status = 'A')");
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCmd = new OdbcCommand(OdbcSelect, conn))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                OdbcCmd.CommandTimeout = 200;
                                OdbcCmd.CommandType = CommandType.Text;
                                try
                                {
                                    OdbcCmd.ExecuteNonQuery();
                                }
                                catch (Exception exAuth)
                                {
                                    if (!ExecuteNonQuery(DbString, OdbcCmd))
                                    {
                                        Utility.WriteToErrorLogFromAll("Save_Patient_Local_To_EagleSoft Error 3 " + exAuth.Message.ToString());
                                        throw exAuth;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToSyncLogFile_All("Error in save Patient " + ex.Message);

                    }
                }
                catch (Exception ex)
                {
                    Utility.WriteToSyncLogFile_All("Provider id is 0 so select provider or default provider ");
                    PatientId = "0";
                    throw ex;
                }
                return PatientId;
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime dateTime, string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetBookOperatoryAppointmenetWiseDateTime;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("Start_Time", dateTime.ToString("yyyy/MM/dd 00:00"));
                        OdbcCommand.Parameters.AddWithValue("End_Time", dateTime.AddDays(1).ToString("yyyy/MM/dd 00:00"));
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Int64 Save_Appointment_Local_To_EagleSoft(string FirstNameLastName, DateTime AppointmentStartTime, DateTime AppointmentEndTime, string PatNum, string OperatoryId,
            string classification, string ApptTypeId, DateTime AppointedDateTime, string ProvNum, string AppointmentConfirmationStatus, string apptcomment,
            bool allday_event, bool sooner_if_possible, bool privateAppointment, bool auto_confirm_sent, string TreatmentCodes, string DbString)
        {
            try
            {
                string id = GetEaglesoftUserLogin_ID(DbString);
                if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                {
                    Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
                }
                Int64 AppointmentId = 0;
                OdbcConnection conn = new OdbcConnection(DbString);
                OdbcCommand OdbcCommand = new OdbcCommand();
                //CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);

                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    OdbcCommand.CommandTimeout = 200;

                    string OdbcSelect = SynchEagleSoftQRY.InsertAppointment;
                    OdbcSelect = OdbcSelect.Replace("@description", "'" + FirstNameLastName + "'");
                    OdbcSelect = OdbcSelect.Replace("@start_time", "'" + AppointmentStartTime.ToString("yyyy/MM/dd HH:mm") + "'");
                    OdbcSelect = OdbcSelect.Replace("@end_time", "'" + AppointmentEndTime.ToString("yyyy/MM/dd HH:mm") + "'");
                    OdbcSelect = OdbcSelect.Replace("@patient_id", "'" + PatNum + "'");
                    OdbcSelect = OdbcSelect.Replace("@location_id", "'" + OperatoryId + "'");
                    OdbcSelect = OdbcSelect.Replace("@classification", "'" + classification + "'");
                    OdbcSelect = OdbcSelect.Replace("@appointment_type_id", "'" + ApptTypeId + "'");
                    OdbcSelect = OdbcSelect.Replace("@date_appointed", "'" + AppointedDateTime.ToString("yyyy/MM/dd") + "'");
                    OdbcSelect = OdbcSelect.Replace("@scheduled_by", "'" + Utility.EHR_UserLogin_ID + "'");
                    OdbcSelect = OdbcSelect.Replace("@modified_by", "'" + Utility.EHR_UserLogin_ID + "'");
                    OdbcSelect = OdbcSelect.Replace("@confirmation_status", "'" + AppointmentConfirmationStatus + "'");
                    OdbcSelect = OdbcSelect.Replace("@allday_event", allday_event == false ? "0" : "1");
                    OdbcSelect = OdbcSelect.Replace("@sooner_if_possible", sooner_if_possible == false ? "0" : "1");
                    OdbcSelect = OdbcSelect.Replace("@private", privateAppointment == false ? "0" : "1");
                    OdbcSelect = OdbcSelect.Replace("@auto_confirm_sent", auto_confirm_sent == false ? "0" : "1");
                    OdbcSelect = OdbcSelect.Replace("@appointment_notes", "'" + apptcomment.Replace("'", "''") + "'");
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    try
                    {
                        AppointmentId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                    }
                    catch (Exception)
                    {
                        object ApptID = null;
                        ExecuteScalar(DbString, OdbcCommand, ref ApptID);
                        AppointmentId = Convert.ToInt64(ApptID);
                    }

                    OdbcSelect = SynchEagleSoftQRY.InsertAppointmentLog;
                    OdbcSelect = OdbcSelect.Replace("@appt_id", AppointmentId.ToString());
                    OdbcSelect = OdbcSelect.Replace("@EHR_User_ID", "'" + Utility.EHR_UserLogin_ID + "'");
                    OdbcSelect = OdbcSelect.Replace("@new_date", "'" + AppointedDateTime.ToString("yyyy/MM/dd") + "'");
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    try
                    {
                        OdbcCommand.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        ExecuteNonQuery(DbString, OdbcCommand);
                    }

                    OdbcSelect = SynchEagleSoftQRY.InsertAppointmentProvider;
                    OdbcSelect = OdbcSelect.Replace("@appointment_id", AppointmentId.ToString());
                    OdbcSelect = OdbcSelect.Replace("@provider_id", "'" + ProvNum + "'");
                    OdbcSelect = OdbcSelect.Replace("@start_time", "'" + AppointmentStartTime.ToString("yyyy/MM/dd HH:mm") + "'");
                    OdbcSelect = OdbcSelect.Replace("@end_time", "'" + AppointmentEndTime.ToString("yyyy/MM/dd HH:mm") + "'");
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    try
                    {
                        OdbcCommand.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        ExecuteNonQuery(DbString, OdbcCommand);
                    }

                    string pid; int pkey;

                    if (TreatmentCodes != null && TreatmentCodes.Length > 0)
                    {
                        int i = 1;
                        foreach (var treatmentCode in TreatmentCodes.Split(','))
                        {
                            //D1351_200_1,00802_0

                            string[] plans = treatmentCode.Split('_');

                            if (plans.Count() == 3)
                            {
                                // Update AppointmentId in Planned Services
                                try
                                {
                                    OdbcSelect = SynchEagleSoftQRY.UpdatePlannedServices;
                                    OdbcSelect = OdbcSelect.Replace("@Patient_id", "'" + PatNum + "'");
                                    OdbcSelect = OdbcSelect.Replace("@line_number", "'" + plans[2] + "'");
                                    OdbcSelect = OdbcSelect.Replace("@appt_id", "'" + AppointmentId + "'");
                                    OdbcSelect = OdbcSelect.Replace("@service_code", "'" + plans[0] + "'");
                                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                                    try
                                    {
                                        OdbcCommand.ExecuteReader();
                                    }
                                    catch (Exception)
                                    {
                                        ExecuteNonQuery(DbString, OdbcCommand);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Utility.WriteToSyncLogFile_All("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                                }
                                // 
                            }
                            else
                            {

                                // Insert records in Planned Services with AppointmentId
                                try
                                {
                                    OdbcSelect = SynchEagleSoftQRY.InsertPlannedServices;
                                    OdbcSelect = OdbcSelect.Replace("@patient_id", "'" + PatNum + "'");
                                    OdbcSelect = OdbcSelect.Replace("@LNumber", " ( select ISNULL( MAX( Line_number),0) from planned_services  where patient_id = '" + PatNum + "')");
                                    OdbcSelect = OdbcSelect.Replace("@line_number", " ( select ISNULL( MAX( Line_number),0) + 1 from planned_services  where patient_id = '" + PatNum + "')");

                                    OdbcSelect = OdbcSelect.Replace("@provider_id", "'" + ProvNum + "'");
                                    // OdbcSelect = OdbcSelect.Replace("@date_planned", "'" + DateTime.Now.ToString("yyyy/MM/dd") + "'");
                                    OdbcSelect = OdbcSelect.Replace("@date_planned", "'" + DateTime.Now.ToString("yyyy/MM/dd") + "'");
                                    OdbcSelect = OdbcSelect.Replace("@status_date", "'" + DateTime.Now.ToString("yyyy/MM/dd") + "'");
                                    OdbcSelect = OdbcSelect.Replace("@appt_id", "'" + AppointmentId + "'");
                                    OdbcSelect = OdbcSelect.Replace("@service_code", "'" + plans[0] + "'");
                                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                                    try
                                    {
                                        OdbcCommand.ExecuteReader();
                                    }
                                    catch (Exception)
                                    {
                                        ExecuteNonQuery(DbString, OdbcCommand);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }


                                i++;
                            }


                        }
                    }

                }
                catch (Exception ex)
                {
                    AppointmentId = 0;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
                return AppointmentId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Patient Forms

        /// <summary>
        /// Return a string of the provided length comprised of only uppercase alpha-numeric characters each of which are
        /// selected randomly.
        /// </summary>
        /// <param name="ofLength">The length of the string which will be returned.</param>
        /// <returns>Return a string of the provided length comprised of only uppercase alpha-numeric characters each of which are
        /// selected randomly.</returns>
        public static string GetRandomUppercaseAlphaNumericValue(int ofLength)
        {
            lock (_lock)
            {
                var builder = new StringBuilder();

                for (int i = 1; i <= ofLength; i++)
                {
                    builder.Append(GetRandomUppercaseAphanumericCharacter());
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Return a randomly-generated uppercase alpha-numeric character (A-Z or 0-9).
        /// </summary>
        /// <returns>Return a randomly-generated uppercase alpha-numeric character (A-Z or 0-9).</returns>
        private static char GetRandomUppercaseAphanumericCharacter()
        {
            var possibleAlphaNumericValues =
                new char[]{'A','B','C','D','E','F','G','H','I','J','K','L',
                'M','N','O','P','Q','R','S','T','U','V','W','X','Y',
                'Z','0','1','2','3','4','5','6','7','8','9'};

            return possibleAlphaNumericValues[GetRandomInteger(0, possibleAlphaNumericValues.Length - 1)];
        }

        /// <summary>
        /// Return a random integer between a lower bound and an upper bound.
        /// </summary>
        /// <param name="lowerBound">The lower-bound of the random integer that will be returned.</param>
        /// <param name="upperBound">The upper-bound of the random integer that will be returned.</param>
        /// <returns> Return a random integer between a lower bound and an upper bound.</returns>
        private static int GetRandomInteger(int lowerBound, int upperBound)
        {
            uint scale = uint.MaxValue;

            // we never want the value to exceed the maximum for a uint, 
            // so loop this until something less than max is found.
            while (scale == uint.MaxValue)
            {
                byte[] fourBytes = new byte[4];
                _crypto.GetBytes(fourBytes); // Get four random bytes.
                scale = BitConverter.ToUInt32(fourBytes, 0); // Convert that into an uint.
            }

            var scaledPercentageOfMax = (scale / (double)uint.MaxValue); // get a value which is the percentage value where scale lies between a uint's min (0) and max value.
            var range = upperBound - lowerBound;
            var scaledRange = range * scaledPercentageOfMax; // scale the range based on the percentage value
            return (int)(lowerBound + scaledRange);
        }

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string AppointmentEHRId, string AppointmentWebId, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType_EHR_ID;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", AppointmentEHRId);
                        SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", AppointmentWebId);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static DataTable GetEaglesoftOperatoryChairData(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftOperatoryChairData;
                    OdbcSelect = OdbcSelect.Replace("@occur_on_date", "'" + ToDate.ToString("yyyy/MM/dd") + "'");
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        //OdbcCommand.Parameters.AddWithValue("Start_Time", dateTime.ToString("yyyy/MM/dd 00:00"));
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEaglesoftOperatoryHours(string DbString, string Service_Install_Id)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftOperatoryHours;
                    OdbcSelect = OdbcSelect.Replace("@ToDate", "'" + ToDate.ToString("yyyy/MM/dd") + "'");
                    OdbcSelect = OdbcSelect.Replace("@Service_Install_Id", Service_Install_Id);
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Save_OperatoryDayOff_Eaglesoft_To_Local(DataTable dtEaglesoftOperatoryEvent, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        bool is_ehr_updated = false;

                        foreach (DataRow dr in dtEaglesoftOperatoryEvent.Rows)
                        {
                            is_ehr_updated = false;

                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryDayOffData;
                                        is_ehr_updated = true;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryDayOffData;
                                        is_ehr_updated = true;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryDayOffData;
                                        break;
                                }
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    int commentlen = 1999;
                                    if (dr["comment"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["comment"].ToString().Trim().Length;
                                    }
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime", dr["StartTime"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("EndTime", dr["EndTime"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdatePatientTableRecords(string tablename, DataRow dr, string DbString)
        {
            try
            {
                //OdbcConnection conn = null;
                //OdbcCommand OdbcCommand = new OdbcCommand();
                //CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);
                string strQauery = string.Empty;
                string Update_PatientForm_Record_ID = "";
                string phoneNo = "";
                //string ColumnList = "";
                //string ValueList = "";
                string patient_EHR_Id = "";

                if (dr["Patient_EHR_ID"].ToString() != string.Empty && dr["ehrfield"].ToString().Trim().ToLower() != "prim_member_id" && dr["ehrfield"].ToString().Trim().ToLower() != "sec_member_id")
                {
                    patient_EHR_Id = dr["Patient_EHR_ID"].ToString();
                    strQauery = SynchEagleSoftQRY.Update_Patinet_Record_By_Patient_Form;
                    strQauery = strQauery.Replace("Patient", tablename);
                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
                    strQauery = strQauery.Replace("@Patient_EHR_ID", "'" + dr["Patient_EHR_ID"].ToString().Trim() + "'");
                    if (dr["ehrfield"].ToString().Trim().ToUpper() == "HOME_PHONE" || dr["ehrfield"].ToString().Trim().ToUpper() == "WORK_PHONE" || dr["ehrfield"].ToString().Trim().ToUpper() == "CELL_PHONE")
                    {
                        phoneNo = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "");
                        strQauery = strQauery.Replace("@ehrfield_value", "'" + phoneNo + "'");
                    }
                    else if (dr["ehrfield"].ToString().Trim().ToUpper() == "RECEIVE_EMAIL" ||
                        dr["ehrfield"].ToString().Trim().ToUpper() == "RECEIVES_SMS" ||
                        dr["ehrfield"].ToString().Trim().ToUpper() == "MARITAL_STATUS" ||
                        dr["ehrfield"].ToString().Trim().ToUpper() == "SEX")
                    {
                        strQauery = strQauery.Replace("@ehrfield_value", "'" + AssigneValueCompitibleTOEHR(dr["ehrfield_value"].ToString(), dr["ehrfield"].ToString().Trim()) + "'");
                    }
                    else if ((dr["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNAME" || dr["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNUMBER") && tablename.ToUpper() == "PATIENT_ANSWERS")
                    {
                        strQauery = " IF EXISTS ( SELECT 1 FROM " + tablename + " WHERE Patient_id = '" + dr["Patient_EHR_ID"].ToString().Trim() + "' AND Patient_Prompt_id = EmergencyType ) BEGIN "
                                     + " UPDATE " + tablename + " SET ANSWER = '" + dr["ehrfield_value"].ToString().Trim() + "' WHERE  Patient_id = '" + dr["Patient_EHR_ID"].ToString().Trim() + "' AND Patient_Prompt_id = EmergencyType  END "
                                     + " ELSE " +
                                     " IF Exists (Select 1 From patient where patient_id ='" + dr["Patient_EHR_ID"].ToString().Trim() + "') " +
                                     " BEGIN INSERT INTO " + tablename + " (Patient_Prompt_id,Patient_id,ANSWER) VALUES (EmergencyType,'" + dr["Patient_EHR_ID"].ToString().Trim() + "','" + dr["ehrfield_value"].ToString().Trim() + "') END ";
                        if (dr["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNAME")
                        {
                            strQauery = strQauery.Replace("EmergencyType", Get_Patient_Prompt_To_EagleSoft(DbString, true).ToString());
                        }
                        else if (dr["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNUMBER")
                        {
                            strQauery = strQauery.Replace("EmergencyType", Get_Patient_Prompt_To_EagleSoft(DbString, false).ToString());
                        }
                    }
                    else
                    {
                        strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim() + "'");
                    }
                    using (OdbcConnection conn = new OdbcConnection(DbString))
                    {
                        using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                        {
                            OdbcCommand.CommandTimeout = 200;
                            OdbcCommand.CommandType = CommandType.Text;
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            try
                            {
                                OdbcCommand.ExecuteNonQuery();
                            }
                            catch (Exception exAuth)
                            {
                                if (!ExecuteNonQuery(DbString, OdbcCommand))
                                {
                                    Utility.WriteToErrorLogFromAll("UpdatePatientTableRecords Error " + exAuth.Message.ToString());
                                }
                            }
                        }
                    }

                    if (dr["ehrfield"].ToString().Trim().ToUpper() == "PREFERRED_DENTIST" && dr["ehrfield_value"].ToString().Trim() != string.Empty)
                    {
                        strQauery = " IF EXISTS ( SELECT 1 FROM patient_site_providers where patient_id = '" + dr["Patient_EHR_ID"].ToString().Trim() + "' ) "
                                    + "   BEGIN  update patient set preferred_dentist = '" + dr["ehrfield_value"].ToString().Trim() + "' WHERE patient_id = '" + dr["Patient_EHR_ID"].ToString().Trim() + "' Update patient_site_providers Set preferred_dentist = '" + dr["ehrfield_value"].ToString().Trim() + "' WHERE patient_id = '" + dr["Patient_EHR_ID"].ToString().Trim() + "' END "
                                    + "   ELSE BEGIN INSERT INTO patient_site_providers ( patient_id ,practice_id,preferred_dentist,preferred_hygienist) VALUES ( '" + dr["Patient_EHR_ID"].ToString().Trim() + "',(SELECT TOP 1 (Practice_Id) FROM Chairs ), '" + dr["ehrfield_value"].ToString().Trim() + "'," + preferedhygineDefault + ")  END ";
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                try
                                {
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                catch (Exception exAuth)
                                {
                                    if (!ExecuteNonQuery(DbString, OdbcCommand))
                                    {
                                        Utility.WriteToErrorLogFromAll("UpdatePatientTableRecords Error(2) " + exAuth.Message.ToString());
                                    }
                                }
                            }
                        }
                    }
                    if (dr["ehrfield"].ToString().Trim().ToUpper() == "PREFERRED_HYGIENIST" && dr["ehrfield_value"].ToString().Trim() != string.Empty)
                    {
                        strQauery = " IF EXISTS ( SELECT 1 FROM patient_site_providers where patient_id = '" + dr["Patient_EHR_ID"].ToString().Trim() + "' ) "
                                    + "   BEGIN  update patient set preferred_hygienist = '" + dr["ehrfield_value"].ToString().Trim() + "' WHERE patient_id = '" + dr["Patient_EHR_ID"].ToString().Trim() + "' Update patient_site_providers Set preferred_hygienist = '" + dr["ehrfield_value"].ToString().Trim() + "' WHERE patient_id = '" + dr["Patient_EHR_ID"].ToString().Trim() + "' END "
                                    + "   ELSE BEGIN INSERT INTO patient_site_providers ( patient_id ,practice_id,preferred_dentist,preferred_hygienist) VALUES ( '" + dr["Patient_EHR_ID"].ToString().Trim() + "',(SELECT TOP 1 (Practice_Id) FROM Chairs ), " + preferredDentistDefault + ",'" + dr["ehrfield_value"].ToString().Trim() + "')  END ";
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                try
                                {
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                catch (Exception exAuth)
                                {
                                    if (!ExecuteNonQuery(DbString, OdbcCommand))
                                    {
                                        Utility.WriteToErrorLogFromAll("UpdatePatientTableRecords Error(3) " + exAuth.Message.ToString());
                                    }
                                }
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Int16 Get_Patient_Prompt_To_EagleSoft(string DbString, bool isname)
        {
            try
            {
                Int16 Emergency_Contact_id = 0;

                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;
                    object id = null;
                    if (isname)
                    {
                        Emergency_Contact_id = 0;
                        #region Add Patient Form details 'Emergency_Cont' To Patient_Prompts
                        using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandTimeout = 200;
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = "IF NOT EXISTS ( select 1 from patient_prompts where prompt = 'Emergency Contact') BEGIN " +
                                "Insert Into patient_prompts (patient_prompt_id,prompt) VALUES ((select max(Patient_prompt_id) + 1 from patient_prompts),'Emergency Contact') " +
                                "select max(Patient_prompt_id)  from patient_prompts where prompt = 'Emergency Contact' END else begin select top 1 patient_prompt_id from patient_prompts where prompt = 'Emergency Contact' end";
                            id = SqlCeCommand.ExecuteScalar();
                            if (id == null || id.ToString() == "")
                            {
                                if (Utility.Application_Version == "20.0" || Utility.Application_Version == "21.20")
                                {
                                    Emergency_Contact_id = 4;
                                }
                                else
                                {
                                    Emergency_Contact_id = 3;
                                }
                            }
                            else
                            {
                                Emergency_Contact_id = Convert.ToInt16(id);
                            }
                        }
                    }
                    #endregion
                    else
                    {
                        Emergency_Contact_id = 0;
                        #region Add Patient Form details 'Emergency_Name' To Patient_Prompts
                        using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandTimeout = 200;
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = "IF NOT EXISTS ( select 1 from patient_prompts where prompt = 'Emergency Contact #') BEGIN " +
                                "Insert Into patient_prompts (patient_prompt_id,prompt) VALUES ((select max(Patient_prompt_id) + 1 from patient_prompts),'Emergency Contact #') " +
                                "select max(Patient_prompt_id)  from patient_prompts where prompt = 'Emergency Contact #' END else begin select top 1 patient_prompt_id from patient_prompts where prompt = 'Emergency Contact #' end";
                            id = SqlCeCommand.ExecuteScalar();
                            if (id == null || id.ToString() == "")
                            {
                                if (Utility.Application_Version == "20.0" || Utility.Application_Version == "21.20")
                                {
                                    Emergency_Contact_id = 5;
                                }
                                else
                                {
                                    Emergency_Contact_id = 4;
                                }
                            }
                            else
                            {
                                Emergency_Contact_id = Convert.ToInt16(id);
                            }
                        }
                    }

                    #endregion
                }
                return Emergency_Contact_id;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
        }
        public static void Insert_Patient_Prompt_To_EagleSoft(string DbString)
        {
            try
            {
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;

                    #region Add Patient Form details 'Emergency_Cont' To Patient_Prompts
                    using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandTimeout = 200;
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchEagleSoftQRY.CheckModeExistAsEmergency_Cont;
                        SqlCeCommand.ExecuteNonQuery();
                    }
                    #endregion

                    #region Add Patient Form details 'Emergency_Name' To Patient_Prompts
                    using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandTimeout = 200;
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchEagleSoftQRY.CheckModeExistAsEmergency_Name;
                        SqlCeCommand.ExecuteNonQuery();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Save_Patient_Form_Local_To_EagleSoft(DataTable dtWebPatient_Form, string DbString, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;
            bool is_Record_Update = false;
            //Utility.CheckEntryUserLoginIdExist();
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
            }
            //OdbcConnection conn = null;
            //OdbcCommand OdbcCommand = new OdbcCommand();
            //CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);

            //if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                //OdbcCommand.CommandTimeout = 200;
                string strQauery = string.Empty;
                string Update_PatientForm_Record_ID = "";
                string phoneNo = "";
                //string ColumnList = "";
                //string ValueList = "";
                string patient_EHR_Id = "";

                if (dtWebPatient_Form.AsEnumerable()
                    .Where(o => o.Field<object>("Patient_EHR_ID") != null &&
                           o.Field<object>("Patient_EHR_ID").ToString() != string.Empty &&
                           o.Field<object>("PatientForm_Web_ID") != null &&
                           o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty).Count() > 0)
                {

                    dtWebPatient_Form.AsEnumerable()
                    .Where(o => o.Field<object>("Patient_EHR_ID") != null &&
                           o.Field<object>("Patient_EHR_ID").ToString() != string.Empty &&
                           o.Field<object>("PatientForm_Web_ID") != null &&
                           o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {
                             foreach (DataRow dr in dtWebPatient_Form.Select(" PatientForm_Web_ID = '" + o.ToString() + "' "))
                             {
                                 patient_EHR_Id = dr["Patient_EHR_ID"].ToString();
                                 if (dr["ehrfield"].ToString().Trim() != string.Empty)
                                 {
                                     UpdatePatientTableRecords(dr["TableName"].ToString(), dr, DbString);
                                 }
                             }
                             UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim(), Service_Install_Id);
                             Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";
                             return true;
                         });
                }

                #region Insert Patient Into EagleSoft

                patient_EHR_Id = "";

                DataTable dtEagleSoftPatentColumns = GetEagleSoftTableColumnName("Patient", DbString);

                //OdbcConnection conn = null;
                //OdbcCommand OdbcCommand = new OdbcCommand();
                //CommonDB.OdbcEaglesoftConnectionServer(ref conn);
                if (
                dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty).Count() > 0)
                {
                    dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {
                             preferredDentist = "";
                             preferedhygine = "";
                             PatientId = "";

                             if (!Utility.GenerateRandomPatientId)
                             {
                                 strQauery = " SELECT  convert(numeric(10,0), min(patient_id + 1)) as Patient_EHR_Id   FROM patient WHERE patient_id liKE '[0-9]%' and reverse(patient_id) like  '[0-9]%'  and ((patient_id + 1) not in (SELECT DISTINCT patient_id FROM patient WHERE patient_id liKE '[0-9]%' and reverse(patient_id) like  '[0-9]%' )) ";
                                 using (OdbcConnection conn = new OdbcConnection(DbString))
                                 {
                                     using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                     {
                                         OdbcCommand.CommandTimeout = 200;
                                         OdbcCommand.CommandType = CommandType.Text;
                                         if (conn.State == ConnectionState.Closed) conn.Open();
                                         patient_EHR_Id = OdbcCommand.ExecuteScalar().ToString();
                                     }
                                 }
                             }
                             strQauery = CreatePatientInsertQuery(dtWebPatient_Form, dtEagleSoftPatentColumns, o.ToString(), "Patient", DbString);

                             if (Utility.GenerateRandomPatientId)
                             {
                                 strQauery = strQauery + " SELECT '" + PatientId + "'";
                             }
                             else
                             {
                                 strQauery = strQauery + " SELECT '" + patient_EHR_Id + "'";
                             }

                             using (OdbcConnection conn = new OdbcConnection(DbString))
                             {
                                 using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                 {
                                     OdbcCommand.CommandTimeout = 200;
                                     OdbcCommand.CommandType = CommandType.Text;
                                     //Utility.WriteToSyncLogFile_All(strQauery.ToString());
                                     if (conn.State == ConnectionState.Closed) conn.Open();
                                     try
                                     {
                                         patient_EHR_Id = OdbcCommand.ExecuteScalar().ToString();
                                     }
                                     catch (Exception exAuth)
                                     {
                                         object PID = null;
                                         if (ExecuteScalar(DbString, OdbcCommand, ref PID))
                                         {
                                             patient_EHR_Id = PID.ToString();
                                         }
                                         else
                                         {
                                             Utility.WriteToErrorLogFromAll("Err_Save Patient Form to Eaglesoft For WebId : " + o.ToString() + " System Error " + exAuth.Message.ToString());
                                         }
                                     }
                                 }
                             }
                             if (patient_EHR_Id != "")
                             {
                                 try
                                 {
                                     strQauery = SynchEagleSoftQRY.UpdatePatientDetails;
                                     strQauery = strQauery.Replace("@Patient_Id", "'" + patient_EHR_Id.ToString() + "'");
                                     using (OdbcConnection conn = new OdbcConnection(DbString))
                                     {
                                         using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                         {
                                             OdbcCommand.CommandTimeout = 200;
                                             OdbcCommand.CommandType = CommandType.Text;
                                             //Utility.WriteToSyncLogFile_All(strQauery.ToString());
                                             if (conn.State == ConnectionState.Closed) conn.Open();
                                             try
                                             {
                                                 OdbcCommand.ExecuteNonQuery();
                                             }
                                             catch (Exception exAuth)
                                             {
                                                 if (!ExecuteNonQuery(DbString, OdbcCommand))
                                                 {
                                                     Utility.WriteToErrorLogFromAll("Err_Save Patient Form to Eaglesoft For WebId : " + o.ToString() + " System Error " + exAuth.Message.ToString());
                                                 }
                                             }
                                         }
                                     }
                                     strQauery = SynchEagleSoftQRY.InsertPatientUsed;
                                     strQauery = strQauery.Replace("@Patient_Id", "'" + patient_EHR_Id.ToString() + "'");
                                     strQauery = strQauery.Replace("@Time_used", "'" + Convert.ToDateTime(System.DateTime.Now).ToString("yyyy/MM/dd HH:mm") + "'");
                                     if (preferredDentist.Length > 200)
                                     {
                                         strQauery = strQauery.Replace("@preferredDentist", preferredDentist);
                                     }
                                     else
                                     {
                                         strQauery = strQauery.Replace("@preferredDentist", "'" + preferredDentist + "'");
                                     }
                                     if (preferedhygine.Length > 200)
                                     {
                                         strQauery = strQauery.Replace("@preferredHygenist", preferedhygine);
                                     }
                                     else
                                     {

                                         strQauery = strQauery.Replace("@preferredHygenist", "'" + preferedhygine + "'");
                                     }
                                     using (OdbcConnection conn = new OdbcConnection(DbString))
                                     {
                                         using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                         {
                                             OdbcCommand.CommandTimeout = 200;
                                             OdbcCommand.CommandType = CommandType.Text;
                                             Utility.WriteToSyncLogFile_All(strQauery.ToString());
                                             if (conn.State == ConnectionState.Closed) conn.Open();
                                             try
                                             {
                                                 OdbcCommand.ExecuteNonQuery();
                                             }
                                             catch (Exception exAuth)
                                             {
                                                 if (!ExecuteNonQuery(DbString, OdbcCommand))
                                                 {
                                                     Utility.WriteToErrorLogFromAll("Err_Save Patient Form to Eaglesoft For WebId : " + o.ToString() + " System Error " + exAuth.Message.ToString());
                                                 }
                                             }
                                         }
                                     }

                                     #region Emergency Contact
                                     try
                                     {
                                         if (dtWebPatient_Form.AsEnumerable().Where(b => b.Field<object>("patientform_web_id").ToString() == o.ToString() && b.Field<object>("ehrfield").ToString().ToLower() == "emergencycontactname") != null &&
                                             dtWebPatient_Form.AsEnumerable().Where(b => b.Field<object>("patientform_web_id").ToString() == o.ToString() && b.Field<object>("ehrfield").ToString().ToLower() == "emergencycontactname").Count() > 0)
                                         {
                                             strQauery = SynchEagleSoftQRY.InsertEmergencyContact;
                                             strQauery = strQauery.Replace("@Patient_EHR_Id", "'" + patient_EHR_Id.ToString() + "'");
                                             strQauery = strQauery.Replace("@EmergencyType", Get_Patient_Prompt_To_EagleSoft(DbString, true).ToString());

                                             strQauery = strQauery.Replace("@EmergencyValue", "'" + dtWebPatient_Form.AsEnumerable().Where(b => b.Field<object>("patientform_web_id").ToString() == o.ToString() && b.Field<object>("ehrfield").ToString().ToLower() == "emergencycontactname").Select(c => c.Field<object>("ehrfield_value").ToString()).First().ToString() + "'");
                                             using (OdbcConnection conn = new OdbcConnection(DbString))
                                             {
                                                 using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                                 {
                                                     OdbcCommand.CommandTimeout = 200;
                                                     OdbcCommand.CommandType = CommandType.Text;
                                                     Utility.WriteToSyncLogFile_All(strQauery.ToString());
                                                     if (conn.State == ConnectionState.Closed) conn.Open();
                                                     try
                                                     {
                                                         OdbcCommand.ExecuteNonQuery();
                                                     }
                                                     catch (Exception exAuth)
                                                     {
                                                         if (!ExecuteNonQuery(DbString, OdbcCommand))
                                                         {
                                                             Utility.WriteToErrorLogFromAll("Err_Save Patient Form to Eaglesoft For WebId : " + o.ToString() + " System Error " + exAuth.Message.ToString());
                                                         }
                                                     }
                                                 }
                                             }
                                         }

                                         if (dtWebPatient_Form.AsEnumerable().Where(b => b.Field<object>("patientform_web_id").ToString() == o.ToString() && b.Field<object>("ehrfield").ToString().ToLower() == "emergencycontactnumber") != null &&
                                             dtWebPatient_Form.AsEnumerable().Where(b => b.Field<object>("patientform_web_id").ToString() == o.ToString() && b.Field<object>("ehrfield").ToString().ToLower() == "emergencycontactnumber").Count() > 0)
                                         {
                                             strQauery = SynchEagleSoftQRY.InsertEmergencyContact;
                                             strQauery = strQauery.Replace("@Patient_EHR_Id", "'" + patient_EHR_Id.ToString() + "'");
                                             strQauery = strQauery.Replace("@EmergencyType", Get_Patient_Prompt_To_EagleSoft(DbString, false).ToString());


                                             strQauery = strQauery.Replace("@EmergencyValue", "'" + dtWebPatient_Form.AsEnumerable().Where(b => b.Field<object>("patientform_web_id").ToString() == o.ToString() && b.Field<object>("ehrfield").ToString().ToLower() == "emergencycontactnumber").Select(c => c.Field<object>("ehrfield_value").ToString()).First().ToString() + "'");
                                             using (OdbcConnection conn = new OdbcConnection(DbString))
                                             {
                                                 using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                                 {
                                                     OdbcCommand.CommandTimeout = 200;
                                                     OdbcCommand.CommandType = CommandType.Text;
                                                     Utility.WriteToSyncLogFile_All(strQauery.ToString());
                                                     if (conn.State == ConnectionState.Closed) conn.Open();
                                                     try
                                                     {
                                                         OdbcCommand.ExecuteNonQuery();
                                                     }
                                                     catch (Exception exAuth)
                                                     {
                                                         if (!ExecuteNonQuery(DbString, OdbcCommand))
                                                         {
                                                             Utility.WriteToErrorLogFromAll("Err_Save Patient Form to Eaglesoft For WebId : " + o.ToString() + " System Error " + exAuth.Message.ToString());
                                                         }
                                                     }
                                                 }
                                             }
                                         }

                                     }
                                     catch (Exception ex2)
                                     {
                                         throw ex2;
                                     }
                                     //EmergencyContactName
                                     #endregion


                                 }
                                 catch (Exception ex1)
                                 {

                                     using (OdbcConnection conn = new OdbcConnection(DbString))
                                     {
                                         strQauery = " Delete from patient_site_providers Where Patient_id = '" + patient_EHR_Id.ToString() + "'";
                                         using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                         {
                                             OdbcCommand.CommandTimeout = 200;
                                             OdbcCommand.CommandType = CommandType.Text;
                                             Utility.WriteToSyncLogFile_All(strQauery.ToString());
                                             if (conn.State == ConnectionState.Closed) conn.Open();
                                             try
                                             {
                                                 OdbcCommand.ExecuteNonQuery();
                                             }
                                             catch (Exception exAuth)
                                             {
                                                 if (!ExecuteNonQuery(DbString, OdbcCommand))
                                                 {
                                                     Utility.WriteToErrorLogFromAll("Err_Save Patient Form to Eaglesoft For WebId : " + o.ToString() + " System Error " + exAuth.Message.ToString());
                                                 }
                                             }
                                         }
                                         strQauery = " Delete from used_patients Where Patient_id = '" + patient_EHR_Id.ToString() + "'";
                                         using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                         {
                                             OdbcCommand.CommandTimeout = 200;
                                             OdbcCommand.CommandType = CommandType.Text;
                                             Utility.WriteToSyncLogFile_All(strQauery.ToString());
                                             if (conn.State == ConnectionState.Closed) conn.Open();
                                             try
                                             {
                                                 OdbcCommand.ExecuteNonQuery();
                                             }
                                             catch (Exception exAuth)
                                             {
                                                 if (!ExecuteNonQuery(DbString, OdbcCommand))
                                                 {
                                                     Utility.WriteToErrorLogFromAll("Err_Save Patient Form to Eaglesoft For WebId : " + o.ToString() + " System Error " + exAuth.Message.ToString());
                                                 }
                                             }
                                         }
                                         strQauery = " Delete from Patient Where Patient_id = '" + patient_EHR_Id.ToString() + "'";
                                         using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                         {
                                             OdbcCommand.CommandTimeout = 200;
                                             OdbcCommand.CommandType = CommandType.Text;
                                             Utility.WriteToSyncLogFile_All(strQauery.ToString());
                                             if (conn.State == ConnectionState.Closed) conn.Open();
                                             try
                                             {
                                                 OdbcCommand.ExecuteNonQuery();
                                             }
                                             catch (Exception exAuth)
                                             {
                                                 if (!ExecuteNonQuery(DbString, OdbcCommand))
                                                 {
                                                     Utility.WriteToErrorLogFromAll("Err_Save Patient Form to Eaglesoft For WebId : " + o.ToString() + " System Error " + exAuth.Message.ToString());
                                                 }
                                             }
                                         }
                                     }
                                     throw ex1;
                                 }
                             }
                             UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim(), Service_Install_Id);

                             Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";

                             return true;
                         });
                }
                #endregion

                DataView view = new DataView(dtWebPatient_Form);
                DataTable distinctValues = view.ToTable(true, "PatientForm_Web_ID", "Service_Install_Id");

                SynchLocalDAL.UpdatePatientFormEHR_Updateflg(distinctValues);

                is_Record_Update = true;
            }
            catch (Exception ex)
            {
                is_Record_Update = false;
                throw ex;
            }

            return is_Record_Update;
        }

        public static DataTable GetInsuratnce_CompanyName(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = " select ic.name,er.employer_id, maximum_coverage AS benefits_remaining , yearly_deductible AS remaining_deductible "
                          + " from employer er join insurance_company ic on ic.insurance_company_id = er.insurance_company_id ";
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void UpdateInsurance_CompanyName(string Ins_Type, string InsCompanyName, string patient_EHR_Id, string DbString)
        {
            //OdbcConnection conn = null;
            //OdbcCommand OdbcCommand = new OdbcCommand();
            //OdbcDataAdapter OdbcDa = null;
            //CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);

            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = " select er.employer_id, maximum_coverage AS benefits_remaining , yearly_deductible AS remaining_deductible "
                                + " from employer er join insurance_company ic on ic.insurance_company_id = er.insurance_company_id where ic.name = '" + InsCompanyName + "' ";
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }

                if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                {

                    string strQauery = string.Empty;
                    if (Ins_Type.ToLower() == "primary")
                    {
                        strQauery = SynchEagleSoftQRY.InsertPatientPryInsuranceDetail;
                    }
                    else
                    {
                        strQauery = SynchEagleSoftQRY.InsertPatientSecInsuranceDetail;
                    }
                    strQauery = strQauery.Replace("@patient_id", patient_EHR_Id.ToString());
                    strQauery = strQauery.Replace("@employer_id", OdbcDt.Rows[0]["employer_id"].ToString());
                    strQauery = strQauery.Replace("@benefits_remaining", OdbcDt.Rows[0]["benefits_remaining"].ToString());
                    strQauery = strQauery.Replace("@remaining_deductible", OdbcDt.Rows[0]["remaining_deductible"].ToString());
                    using (OdbcConnection conn = new OdbcConnection(DbString))
                    {
                        using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                        {
                            OdbcCommand.CommandTimeout = 200;
                            OdbcCommand.CommandType = CommandType.Text;
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            OdbcCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static string AssigneValueCompitibleTOEHR(string fieldValue, string EHRColumnsName)
        {
            try
            {
                string returnvalue = "";

                switch (fieldValue.ToString().Trim().ToUpper())
                {
                    case "MALE":
                        returnvalue = "M";
                        break;
                    case "FEMALE":
                        returnvalue = "F";
                        break;
                    case "YES":
                        returnvalue = "Y";
                        break;
                    case "NO":
                        returnvalue = "N";
                        break;
                    case "TRUE":
                        returnvalue = "Y";
                        break;
                    case "FALSE":
                        returnvalue = "N";
                        break;
                    case "1":
                        returnvalue = "Y";
                        break;
                    case "0":
                        returnvalue = "N";
                        break;
                    case "SINGLE":
                        returnvalue = "S";
                        break;
                    case "MARRIED":
                        returnvalue = "M";
                        break;
                    case "UNMARRIED":
                        returnvalue = "U";
                        break;
                    case "DIVORCED":
                        returnvalue = "D";
                        break;
                    case "WIDOWED":
                        returnvalue = "W";
                        break;
                    case "SEPARATED":
                        returnvalue = "X";
                        break;
                    default:
                        if (EHRColumnsName == "RECEIVE_EMAIL" || EHRColumnsName == "RECEIVES_SMS")
                        {
                            returnvalue = "Y";
                        }
                        else if (EHRColumnsName == "MARITAL_STATUS")
                        {
                            returnvalue = "U";
                        }
                        else if (EHRColumnsName == "SEX")
                        {
                            returnvalue = "M";
                        }
                        break;
                }
                return returnvalue;
            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }

        private static string CreatePatientInsertQuery(DataTable dtWebPatient_Form, DataTable dtEagleSoftPatentColumns, string patientFormWebId, string tableName, string DbString)
        {
            try
            {
                string strQauery = "";
                string ColumnList = "";
                string ValueList = "";

                dtEagleSoftPatentColumns.AsEnumerable().Where(z => z.Field<string>("EHRColumnName") != "")
                    .All(e =>
                    {

                        var tableExists = dtWebPatient_Form.AsEnumerable()
                           .Where(a => a.Field<object>("TableName").ToString().ToUpper() == tableName.ToUpper());

                        if (tableExists != null && tableExists.Count() > 0)
                        {
                            var dtColumnsExists = dtWebPatient_Form.AsEnumerable()
                                .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == patientFormWebId.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == e.Field<object>("EHRColumnName").ToString().ToUpper());

                            ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";

                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0)
                            {

                                if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "HOME_PHONE" || e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "WORK_PHONE" || e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "CELL_PHONE")
                                {
                                    ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "") + "'" + ",";
                                }
                                else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "BIRTH_DATE")
                                {
                                    ValueList = ValueList + "'" + Convert.ToDateTime(dtColumnsExists.First().Field<object>("ehrfield_value")).ToString("yyyy/MM/dd HH:mm").ToString() + "'" + ",";
                                }
                                else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "RECEIVE_EMAIL"
                                    || e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "RECEIVES_SMS"
                                    || e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "MARITAL_STATUS"
                                    || e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "SEX")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null && dtColumnsExists.First().Field<object>("ehrfield_value").ToString() != string.Empty)
                                    {
                                        ValueList = ValueList + "'" + AssigneValueCompitibleTOEHR(dtColumnsExists.First().Field<object>("ehrfield_value").ToString(), e.Field<object>("EHRColumnName").ToString().ToUpper()) + "'" + ",";
                                    }
                                }
                                else
                                {
                                    ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                }

                                if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PREFERRED_DENTIST")
                                {
                                    preferredDentist = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                                    if (preferredDentist == "")
                                    {
                                        preferredDentist = "(Select  TOP 1 Provider_id From provider where Practice_id = (SELECT TOP 1 (Practice_Id) FROM Chairs ) and position_id = (Select Top 1 Position_id From Positions where description = 'Dentist') and status = 'A')";
                                    }
                                }
                                if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PREFERRED_HYGIENIST")
                                {
                                    preferedhygine = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                                    if (preferedhygine == "")
                                    {
                                        preferedhygine = "(Select  TOP 1 Provider_id From provider where Practice_id = (SELECT TOP 1 (Practice_Id) FROM Chairs ) and position_id = (Select Top 1 Position_id From Positions where description = 'Hygienist') and status = 'A')";
                                    }
                                }

                            }
                            else
                            {

                                if (e.Field<object>("EHRDataType") != null && e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PATIENT_ID")
                                {

                                    if (Utility.GenerateRandomPatientId)
                                    {
                                        ReGenerate:
                                        string PatId = GetRandomUppercaseAlphaNumericValue(5);
                                        DataTable dtPatIds = GetEagleSoftPatientIds(DbString);
                                        if (dtPatIds != null && dtPatIds.Rows.Count > 0)
                                        {
                                            if (dtPatIds.AsEnumerable().Where(o => o.Field<string>("PATIENT_ID").ToUpper() == PatId.ToString().ToUpper()).Count() > 0)
                                            {
                                                goto ReGenerate;
                                            }
                                        }
                                        PatientId = PatId;
                                        ValueList = ValueList + "'" + PatId + "'" + ",";
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "(SELECT  convert(numeric(10,0), min(patient_id + 1)) as Patient_EHR_Id   FROM patient WHERE patient_id liKE '[0-9]%' and reverse(patient_id) like  '[0-9]%'  and ((patient_id + 1) not in (SELECT DISTINCT patient_id FROM patient WHERE patient_id liKE '[0-9]%' and reverse(patient_id) like  '[0-9]%' )))" + ",";
                                    }
                                }
                                else if (e.Field<object>("EHRDataType") != null && (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "DATE" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "DATETIME"))
                                {
                                    ValueList = ValueList + "NULL" + ",";
                                }
                                else if (e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                                {
                                    if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "NUMERIC")
                                    {
                                        ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "'" + e.Field<object>("DefaultValue").ToString() + "'" + ",";
                                    }
                                }
                                else if (e.Field<object>("AllowNull").ToString().Trim().ToUpper() == "YES")
                                {
                                    ValueList = ValueList + "NULL" + ",";
                                }
                                else
                                {
                                    ValueList = ValueList + "''" + ",";
                                }

                                if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PREFERRED_DENTIST")
                                {
                                    preferredDentist = "(Select  TOP 1 Provider_id From provider where Practice_id = (SELECT TOP 1 (Practice_Id) FROM Chairs ) and position_id = (Select Top 1 Position_id From Positions where description = 'Dentist') and status = 'A')";
                                }
                                if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PREFERRED_HYGIENIST")
                                {
                                    preferedhygine = "(Select  TOP 1 Provider_id From provider where Practice_id = (SELECT TOP 1 (Practice_Id) FROM Chairs ) and position_id = (Select Top 1 Position_id From Positions where description = 'Hygienist') and status = 'A')";
                                }
                            }
                        }
                        else if (tableName.ToUpper() == "PATIENT_ANSWERS")
                        {
                            var dtColumnsExists = dtWebPatient_Form.AsEnumerable()
                               .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == patientFormWebId.ToString()
                                   && a.Field<object>("TableName").ToString().ToUpper() == "PATIENT_ANSWERS");

                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0)
                            {
                                if (e.Field<object>("ehrfield").ToString().Trim().ToUpper() == "EMERGENCYCONTACTNAME")
                                {
                                    EmergencyContactName = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                                }
                                else if (e.Field<object>("ehrfield").ToString().Trim().ToUpper() == "EMERGENCYCONTACTNUMBER")
                                {
                                    EmergencyContactNumber = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                                }
                            }
                        }
                        else if (tableName.ToUpper() == "EMPLOYER")
                        {
                            var dtColumnsExists = dtWebPatient_Form.AsEnumerable()
                               .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == patientFormWebId.ToString()
                                   && a.Field<object>("TableName").ToString().ToUpper() == "EMPLOYER");

                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0)
                            {
                                if (e.Field<object>("ehrfield").ToString().Trim().ToUpper() == "NAME")
                                {
                                    Employer = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                                }
                            }
                        }
                        return true;
                    });

                ColumnList = ColumnList.Substring(0, ColumnList.Length - 1);
                ValueList = ValueList.Substring(0, ValueList.Length - 1);
                strQauery = " Insert into " + tableName + " ( " + ColumnList + " ) VALUES ( " + ValueList + " )";
                return strQauery;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool UpdatePatientEHRIdINPatientForm(string PatientEHRId, string PatientFormWebId, string Service_Install_Id)
        {
            try
            {
                bool _successfullstataus = true;
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string sqlSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientForm_PatientEHRId;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_Id", PatientEHRId.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", PatientFormWebId.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.ExecuteNonQuery();
                            _successfullstataus = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        _successfullstataus = false;
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
                return _successfullstataus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static DataTable GetEagleSoftTableColumnName(string tableName, string DbString)
        {
            try
            {

                DataTable userTables = null;
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string[] restrictions = new string[3];
                    restrictions[1] = "PPM";
                    userTables = conn.GetSchema("Columns", restrictions);
                    userTables.Columns.RemoveAt(0);
                    userTables.Columns.RemoveAt(0);
                    userTables.Columns.RemoveAt(2);
                }


                userTables.DefaultView.RowFilter = "TABLE_NAME = '" + tableName + "'";
                DataTable dtResult = userTables.DefaultView.ToTable();
                DataTable OdbcDt = new DataTable();
                string SqlCeSelect = " SELECT COLUMN_NAME,'' AS EHRColumnName,'' AS EHRDataType,'' AS AllowNull,'' AS DefaultValue  FROM information_Schema.columns where table_name = '" + tableName + "'"; ;
                using (SqlCeConnection conn1 = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn1))
                    {
                        if (conn1.State == ConnectionState.Closed) conn1.Open();
                        SqlCeCommand.CommandType = CommandType.Text;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            OdbcDt = new DataTable();
                            SqlCeDa.Fill(OdbcDt);
                        }
                    }
                }
                OdbcDt.AsEnumerable()
                    .All(a =>
                    {
                        if (a["COLUMN_NAME"].ToString().ToUpper() == "PATIENT_EHR_ID")
                        {
                            a["EHRColumnName"] = "patient_Id";

                        }
                        if (a["COLUMN_NAME"].ToString() == "First_name")
                        {
                            a["EHRColumnName"] = "first_name";
                            a["DefaultValue"] = "NA";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "LAST_NAME")
                        {
                            a["EHRColumnName"] = "last_name";
                            a["DefaultValue"] = "NA";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MIDDLE_NAME")
                        {
                            a["EHRColumnName"] = "middle_initial";
                            // a["DefaultValue"] = "NA"; 
                            a["DefaultValue"] = "";   //https://app.asana.com/0/830628876728852/1198957614913934
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MOBILE")
                        {
                            a["EHRColumnName"] = "cell_phone";
                            a["DefaultValue"] = "";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "STATUS")
                        {
                            a["EHRColumnName"] = "Status";
                            a["DefaultValue"] = "A";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "ADDRESS1")
                        {
                            a["COLUMN_NAME"] = "ADDRESS_ONE";
                            a["EHRColumnName"] = "Address_1";
                            a["DefaultValue"] = "";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "ADDRESS2")
                        {
                            a["COLUMN_NAME"] = "ADDRESS_TWO";
                            a["EHRColumnName"] = "Address_2";
                            a["DefaultValue"] = "";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "BIRTH_DATE")
                        {
                            a["EHRColumnName"] = "Birth_Date";
                            a["DefaultValue"] = null;
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MARITALSTATUS")
                        {
                            a["EHRColumnName"] = "Marital_Status";
                            a["DefaultValue"] = "U";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "STATE")
                        {
                            a["EHRColumnName"] = "State";
                            //a["DefaultValue"] = null;
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "CITY")
                        {
                            a["EHRColumnName"] = "City";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "CURRENTBAL")
                        {
                            a["EHRColumnName"] = "current_bal";
                            a["DefaultValue"] = "0";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "THIRTYDAY")
                        {
                            a["EHRColumnName"] = "thirty_day";
                            a["DefaultValue"] = "0";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SIXTYDAY")
                        {
                            a["EHRColumnName"] = "sixty_day";
                            a["DefaultValue"] = "0";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "NINETYDAY")
                        {
                            a["EHRColumnName"] = "ninety_day";
                            a["DefaultValue"] = "0";
                        }

                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "EMAIL")
                        {
                            a["EHRColumnName"] = "email_address";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "HOME_PHONE")
                        {
                            a["EHRColumnName"] = "Home_Phone";
                            a["DefaultValue"] = "";
                        }

                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PREFERRED_NAME")
                        {
                            a["EHRColumnName"] = "preferred_name";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRI_PROVIDER_ID")
                        {
                            a["EHRColumnName"] = "preferred_dentist";
                            a["DefaultValue"] = "";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE")
                        {
                            a["EHRColumnName"] = "";
                            a["DefaultValue"] = "";
                        }
                        else if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INS_SUBSCRIBER_ID")
                        {
                            a["EHRColumnName"] = "prim_member_id";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME")
                        {
                            a["EHRColumnName"] = "";
                            a["DefaultValue"] = "";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "RECEIVEEMAIL")
                        {
                            a["COLUMN_NAME"] = "RECEIVE_EMAIL";
                            a["EHRColumnName"] = "receive_email";
                            a["DefaultValue"] = "Y";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "RECEIVESMS")
                        {
                            a["COLUMN_NAME"] = "RECEIVE_SMS";
                            a["EHRColumnName"] = "receives_sms";
                            a["DefaultValue"] = "Y";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SALUTATION")
                        {
                            a["EHRColumnName"] = "Salutation";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SEC_PROVIDER_ID")
                        {
                            a["EHRColumnName"] = "preferred_hygienist";
                            a["DefaultValue"] = "";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE")
                        {
                            a["EHRColumnName"] = "";
                        }
                        else if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INS_SUBSCRIBER_ID")
                        {
                            a["EHRColumnName"] = "sec_member_id";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME")
                        {
                            a["EHRColumnName"] = "";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SEX")
                        {
                            a["EHRColumnName"] = "sex";
                            a["DefaultValue"] = "";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "WORK_PHONE")
                        {
                            a["EHRColumnName"] = "work_phone";
                            a["DefaultValue"] = "0000000000";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "ZIPCODE")
                        {
                            a["EHRColumnName"] = "zipcode";
                            a["DefaultValue"] = "";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "FIRSTVISIT_DATE")
                        {
                            a["EHRColumnName"] = "first_visit_date";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "LASTVISIT_DATE")
                        {
                            a["EHRColumnName"] = "last_date_seen";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "NEXTVISIT_DATE")
                        {
                            a["EHRColumnName"] = "next_regular_appointment";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SSN")
                        {
                            a["EHRColumnName"] = "social_security";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "DRIVERLICENSE")
                        {
                            a["EHRColumnName"] = "drivers_license";
                        }
                        if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SCHOOL")
                        {
                            a["EHRColumnName"] = "school";
                        }

                        if (a["EHRColumnName"].ToString() != "")
                        {
                            if (dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMN_NAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Count() > 0)
                            {
                                a["EHRDataType"] = dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMN_NAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<string>("Type_Name")).First().ToString();
                                a["AllowNull"] = dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMN_NAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<string>("IS_NULLABLE")).First().ToString();
                            }
                        }

                        return true;
                    });


                DataRow drNewRow = OdbcDt.NewRow();
                drNewRow["COLUMN_NAME"] = "Practice_Id";
                drNewRow["EHRColumnName"] = "Practice_Id";
                drNewRow["EHRDataType"] = "Numeric";
                drNewRow["AllowNull"] = "No";
                drNewRow["DefaultValue"] = "(SELECT TOP 1 (Practice_Id) FROM Chairs )";
                OdbcDt.Rows.Add(drNewRow);

                AddRowsINDatatable("prim_relationship", "char", "", "prim_relationship", "Yes", ref OdbcDt);
                AddRowsINDatatable("prim_employer_id", "integer", "", "prim_employer_id", "Yes", ref OdbcDt);
                AddRowsINDatatable("prim_outstanding_balance", "numeric", "0", "prim_outstanding_balance", "Yes", ref OdbcDt);
                AddRowsINDatatable("prim_benefits_remaining", "numeric", "0", "prim_benefits_remaining", "Yes", ref OdbcDt);
                AddRowsINDatatable("prim_remaining_deductible", "numeric", "0", "prim_remaining_deductible", "Yes", ref OdbcDt);
                AddRowsINDatatable("patient_status", "char", "Y", "patient_status", "Yes", ref OdbcDt);
                AddRowsINDatatable("policy_holder_status", "char", "Y", "policy_holder_status", "Yes", ref OdbcDt);
                AddRowsINDatatable("sec_relationship", "char", "", "sec_relationship", "Yes", ref OdbcDt);
                AddRowsINDatatable("sec_employer_id", "integer", "", "sec_employer_id", "Yes", ref OdbcDt);
                AddRowsINDatatable("sec_outstanding_balance", "numeric", "0", "sec_outstanding_balance", "Yes", ref OdbcDt);
                AddRowsINDatatable("sec_benefits_remaining", "numeric", "0", "sec_benefits_remaining", "Yes", ref OdbcDt);
                AddRowsINDatatable("sec_remaining_deductible", "numeric", "0", "sec_remaining_deductible", "Yes", ref OdbcDt);

                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AddRowsINDatatable(string columnName, string datatype, string defaultvalue, string ehrColumnsName, string allownull, ref DataTable dtTable)
        {
            try
            {
                DataRow drNewRow = dtTable.NewRow();
                drNewRow["COLUMN_NAME"] = columnName;
                drNewRow["EHRColumnName"] = ehrColumnsName;
                drNewRow["EHRDataType"] = datatype;
                drNewRow["AllowNull"] = allownull;
                drNewRow["DefaultValue"] = defaultvalue;
                dtTable.Rows.Add(drNewRow);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Save_Document_in_EagleSoft(string DbString, string Service_Install_Id, string DocPath, string strPatientFormID = "")
        {

            try
            {
                DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocData(Service_Install_Id, strPatientFormID);
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    //Utility.CheckEntryUserLoginIdExist();
                    //rooja added below function to fetch ES folderlist details

                    //rooja - by shruti
                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
                    }
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            if (SynchLocalDAL.CheckLivePatientFormDocDataSynced(Service_Install_Id, dr["PatientDoc_Web_ID"].ToString()))
                            {
                                PullLiveDatabaseDAL.Update_PatientDocNotFound_Live_To_Local(dr["PatientDoc_Web_ID"].ToString(), Service_Install_Id);
                            }
                            continue;
                        }

                        string tmpFileOrgName = Path.GetFileName(sourceLocation);
                        string SourcePath = Path.GetDirectoryName(sourceLocation);

                        string DestPath = "";
                        if (DocPath == string.Empty || DocPath == "")
                        {
                            DestPath = DocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            DestPath = @"C:\EagleSoft\Data\Documents\patient\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        else
                        {
                            DestPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        // string DestPath = @"C:\EagleSoft\Data\Documents\patient\" + dr["Patient_EHR_ID"].ToString().Trim();

                        string document_type = "";
                        int OpenWith = 0;

                        switch (Path.GetExtension(sourceLocation).ToLower())
                        {
                            case ".doc":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".docx":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".xls":
                                document_type = "Microsoft Excel Worksheet";
                                OpenWith = 0;
                                break;
                            case ".xlsx":
                                document_type = "Microsoft Excel Worksheet";
                                OpenWith = 0;
                                break;
                            case ".ppt":
                                document_type = "Microsoft PowerPoint Presentation";
                                OpenWith = 0;
                                break;
                            case ".pptx":
                                document_type = "Microsoft PowerPoint Presentation";
                                OpenWith = 0;
                                break;
                            case ".pdf":
                                document_type = "Adobe Acrobat Document";
                                OpenWith = 3;
                                break;
                            case ".html":
                                document_type = "HTML document";
                                OpenWith = 1;
                                break;
                            case ".htm":
                                document_type = "HTML document";
                                OpenWith = 1;
                                break;
                            case ".txt":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".rtf":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".jpg":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jpeg":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jpe":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jfif":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".png":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".BMP":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".GIF":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            default:
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                        }
                        if (!System.IO.Directory.Exists(DestPath))
                        {
                            System.IO.Directory.CreateDirectory(DestPath);
                        }
                        Int64 SavePatientDocId = 0;


                        SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM-dd-yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM-dd-yy");
                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString().Replace("'", "''") : "";
                        PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString().Replace("'", "''") : "";

                        switch (dr["DocNameFormat"].ToString().Trim())
                        {
                            case "DS-FN-PN":
                                //SubmitedDate +"-"+
                                ShowingName = SubmitedDate + "-" + FormName + "-" + PatientName;
                                break;
                            case "DS-PN-FN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + PatientName + "-" + FormName;
                                break;
                            case "DS-PN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + PatientName;
                                break;
                            case "DS-FN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + FormName;
                                break;
                            //case "DS":
                            //    ShowingName = SubmitedDate;
                            //    break;
                            default:
                                ShowingName = SubmitedDate;
                                break;
                        }
                        //DateTime.Now.ToString("HH:mm:ss")
                        //Utility.WriteToErrorLogFromAll("Save_Document_in_EagleSoft() -ShowingName : " + ShowingName.ToString());
                        string OdbcSelect = "";
                        if (dr["folder_ehr_id"].ToString() == "" || dr["folder_ehr_id"].ToString().Trim() == "0")
                        {
                            OdbcSelect = SynchEagleSoftQRY.InsertPatientDocData;
                        }
                        else
                        {
                            OdbcSelect = SynchEagleSoftQRY.InsertPatientDocData_version32;
                        }

                        //string OdbcSelect = SynchEagleSoftQRY.InsertPatientDocData;
                        //rooja 11-4-23
                        //OdbcSelect = OdbcSelect.Replace("@document_name", "'Web:" + DateTime.Now.ToString() + "'");
                        OdbcSelect = OdbcSelect.Replace("@document_name", "'" + ShowingName.Replace(" ", "") + "'");
                        OdbcSelect = OdbcSelect.Replace("@document_type", "'" + document_type.ToString() + "'");
                        OdbcSelect = OdbcSelect.Replace("@document_creation_date", "getdate()");
                        OdbcSelect = OdbcSelect.Replace("@document_modified_date", "getdate()");
                        OdbcSelect = OdbcSelect.Replace("@open_with", OpenWith.ToString());
                        OdbcSelect = OdbcSelect.Replace("@ref_id", "'" + dr["Patient_EHR_ID"].ToString().Trim() + "'");

                        if (dr["folder_ehr_id"].ToString() != "" && dr["folder_ehr_id"].ToString().Trim() != "0")
                        {
                            OdbcSelect = OdbcSelect.Replace("@document_group_id", dr["folder_ehr_id"].ToString());
                        }
                        OdbcSelect = OdbcSelect.Replace("@ref_table", "'patient'");
                        OdbcSelect = OdbcSelect.Replace("@EHR_User_ID", "'" + Utility.EHR_UserLogin_ID + "'");
                        // OdbcSelect = OdbcSelect.Replace("@original_user_id", "'GGY'");
                        OdbcSelect = OdbcSelect.Replace("@private", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@display_in_docmgr", "'Y'");
                        OdbcSelect = OdbcSelect.Replace("@signed", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@needs_converted", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@notice_of_privacy", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@privacy_authorization", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@consent", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@practice_id", "(SELECT TOP 1 (Practice_Id) FROM Chairs )");
                        OdbcSelect = OdbcSelect.Replace("@custom_document_id", "-1");
                        OdbcSelect = OdbcSelect.Replace("@headerfooter_added", "0");
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                try
                                {
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                catch (Exception exAuth)
                                {
                                    if (!ExecuteNonQuery(DbString, OdbcCommand))
                                    {
                                        Utility.WriteToErrorLogFromAll("Err_Save Patient Document to Eaglesoft For PatientID : " + dr["Patient_EHR_ID"].ToString().Trim() + " System Error " + exAuth.Message.ToString());
                                    }
                                }
                            }
                            OdbcSelect = "Select CONVERT(NUMERIC(10,0),( MAX(ISNULL(document_Id,0)) ) ) From Document";
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                SavePatientDocId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                            }
                        }

                        //OdbcSelect = "delete From Document where document_id = 189";
                        //CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        //OdbcCommand.ExecuteNonQuery();    
                        string outputPath = DestPath;
                        string str = Path.Combine(SourcePath, tmpFileOrgName);
                        File.Copy(str, str + ".ORG", true);
                        string newZipFileName = Path.Combine(outputPath, string.Format("{0}~{1}.esd", (object)dr["Patient_EHR_ID"].ToString().Trim(), (object)SavePatientDocId.ToString()));
                        using (ZipFile zipFile = new ZipFile())
                        {
                            zipFile.Password = "HIMOM";
                            zipFile.UseZip64WhenSaving = Zip64Option.AsNecessary;
                            zipFile.CompressionLevel = CompressionLevel.BestCompression;
                            zipFile.Encryption = EncryptionAlgorithm.PkzipWeak;
                            zipFile.ZipErrorAction = ZipErrorAction.Throw;
                            zipFile.ParallelDeflateThreshold = -1L;
                            zipFile.AddFile(str, "");
                            // zipFile.AddFile(str + ".ORG", "");
                            zipFile.Save(newZipFileName);
                        }
                        //Document.CreateSmartDoc(dr["Patient_EHR_ID"].ToString().Trim(), // Patient Id
                        //                          SavePatientDocId.ToString(), // Document Id
                        //                          tmpFileOrgName, // Source File Name
                        //                          SourcePath, // Source File Path
                        //                          "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4",
                        //                          true, // Keep Source Copy is true
                        //                          DestPath
                        //                         );
                        PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), SavePatientDocId.ToString(), Service_Install_Id);
                        File.Delete(sourceLocation);
                        Save_DocumentAttachment_in_EagleSoft(DbString, Service_Install_Id, DocPath, dr["PatientDoc_Web_ID"].ToString());
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;

            }
        }

        public static bool Save_DocumentAttachment_in_EagleSoft(string DbString, string Service_Install_Id, string DocPath,string PatientForm_web_Id)
        {

            try
            {
                DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocAttachmentData(Service_Install_Id, PatientForm_web_Id);
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    //Utility.CheckEntryUserLoginIdExist();
                    //rooja added below function to fetch ES folderlist details

                    //rooja - by shruti
                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
                    }
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            if (SynchLocalDAL.CheckLivePatientFormDocAttachmentDataSynced(Service_Install_Id, dr["PatientDoc_Web_ID"].ToString()))
                            {
                                PullLiveDatabaseDAL.Update_PatientDocAttachmentNotFound_Live_To_Local(dr["PatientForm_web_Id"].ToString(), Service_Install_Id);
                            }
                            continue;
                        }
                        else
                        {
                            long length = new System.IO.FileInfo(sourceLocation).Length;
                            if (length <= 0)
                            {
                                PullLiveDatabaseDAL.Update_PatientDocAttachmentNotFound_Live_To_Local(dr["PatientForm_web_Id"].ToString(), "1");
                                continue;
                            }
                        }

                        string tmpFileOrgName = Path.GetFileName(sourceLocation);
                        string SourcePath = Path.GetDirectoryName(sourceLocation);

                        string DestPath = "";
                        if (DocPath == string.Empty || DocPath == "")
                        {
                            DestPath = DocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            DestPath = @"C:\EagleSoft\Data\Documents\patient\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        else
                        {
                            DestPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim();
                        }
                        // string DestPath = @"C:\EagleSoft\Data\Documents\patient\" + dr["Patient_EHR_ID"].ToString().Trim();

                        string document_type = "";
                        int OpenWith = 0;

                        switch (Path.GetExtension(sourceLocation).ToLower())
                        {
                            case ".doc":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".docx":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".xls":
                                document_type = "Microsoft Excel Worksheet";
                                OpenWith = 0;
                                break;
                            case ".xlsx":
                                document_type = "Microsoft Excel Worksheet";
                                OpenWith = 0;
                                break;
                            case ".ppt":
                                document_type = "Microsoft PowerPoint Presentation";
                                OpenWith = 0;
                                break;
                            case ".pptx":
                                document_type = "Microsoft PowerPoint Presentation";
                                OpenWith = 0;
                                break;
                            case ".pdf":
                                document_type = "Adobe Acrobat Document";
                                OpenWith = 3;
                                break;
                            case ".html":
                                document_type = "HTML document";
                                OpenWith = 1;
                                break;
                            case ".htm":
                                document_type = "HTML document";
                                OpenWith = 1;
                                break;
                            case ".txt":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".rtf":
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                            case ".jpg":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jpeg":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jpe":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".jfif":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".png":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".BMP":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            case ".GIF":
                                document_type = "Image";
                                OpenWith = 4;
                                break;
                            default:
                                document_type = "Microsoft Word Document";
                                OpenWith = 0;
                                break;
                        }
                        if (!System.IO.Directory.Exists(DestPath))
                        {
                            System.IO.Directory.CreateDirectory(DestPath);
                        }
                        Int64 SavePatientDocId = 0;


                        SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM-dd-yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM-dd-yy");
                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString().Replace("'", "''") : "";
                        PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString().Replace("'", "''") : "";

                        switch (dr["DocNameFormat"].ToString().Trim())
                        {
                            case "FN-PN-DS":
                                //SubmitedDate +"-"+
                                ShowingName = FormName + "-" + PatientName + "-" + SubmitedDate;
                                break;
                            case "DS-FN-PN":
                                //SubmitedDate +"-"+
                                ShowingName = SubmitedDate + "-" + FormName + "-" + PatientName;
                                break;
                            case "DS-PN-FN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + PatientName + "-" + FormName;
                                break;
                            case "DS-PN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + PatientName;
                                break;
                            case "DS-FN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + FormName;
                                break;
                            //case "DS":
                            //    ShowingName = SubmitedDate;
                            //    break;
                            default:
                                ShowingName = SubmitedDate;
                                break;
                        }
                        //DateTime.Now.ToString("HH:mm:ss")
                        //Utility.WriteToErrorLogFromAll("Save_Document_in_EagleSoft() -ShowingName : " + ShowingName.ToString());
                        string OdbcSelect = "";
                        if (dr["folder_ehr_id"].ToString() == "" || dr["folder_ehr_id"].ToString().Trim() == "0")
                        {
                            OdbcSelect = SynchEagleSoftQRY.InsertPatientDocData;
                        }
                        else
                        {
                            OdbcSelect = SynchEagleSoftQRY.InsertPatientDocData_version32;
                        }
                        if (ShowingName.Length > 40)
                        {
                            ShowingName = ShowingName.Substring(0, 39);
                        }

                        //string OdbcSelect = SynchEagleSoftQRY.InsertPatientDocData;
                        //rooja 11-4-23
                        //OdbcSelect = OdbcSelect.Replace("@document_name", "'Web:" + DateTime.Now.ToString() + "'");
                        OdbcSelect = OdbcSelect.Replace("@document_name", "'" + ShowingName.Replace(" ", "") + "'");
                        OdbcSelect = OdbcSelect.Replace("@document_type", "'" + document_type.ToString() + "'");
                        OdbcSelect = OdbcSelect.Replace("@document_creation_date", "getdate()");
                        OdbcSelect = OdbcSelect.Replace("@document_modified_date", "getdate()");
                        OdbcSelect = OdbcSelect.Replace("@open_with", OpenWith.ToString());
                        OdbcSelect = OdbcSelect.Replace("@ref_id", "'" + dr["Patient_EHR_ID"].ToString().Trim() + "'");

                        if (dr["folder_ehr_id"].ToString() != "" && dr["folder_ehr_id"].ToString().Trim() != "0")
                        {
                            OdbcSelect = OdbcSelect.Replace("@document_group_id", dr["folder_ehr_id"].ToString());
                        }
                        OdbcSelect = OdbcSelect.Replace("@ref_table", "'patient'");
                        OdbcSelect = OdbcSelect.Replace("@EHR_User_ID", "'" + Utility.EHR_UserLogin_ID + "'");
                        // OdbcSelect = OdbcSelect.Replace("@original_user_id", "'GGY'");
                        OdbcSelect = OdbcSelect.Replace("@private", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@display_in_docmgr", "'Y'");
                        OdbcSelect = OdbcSelect.Replace("@signed", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@needs_converted", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@notice_of_privacy", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@privacy_authorization", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@consent", "'N'");
                        OdbcSelect = OdbcSelect.Replace("@practice_id", "(SELECT TOP 1 (Practice_Id) FROM Chairs )");
                        OdbcSelect = OdbcSelect.Replace("@custom_document_id", "-1");
                        OdbcSelect = OdbcSelect.Replace("@headerfooter_added", "0");
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                try
                                {
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                catch (Exception exAuth)
                                {
                                    if (!ExecuteNonQuery(DbString, OdbcCommand))
                                    {
                                        Utility.WriteToErrorLogFromAll("Err_Save Patient Document to Eaglesoft For PatientID : " + dr["Patient_EHR_ID"].ToString().Trim() + " System Error " + exAuth.Message.ToString());
                                    }
                                }
                            }
                            OdbcSelect = "Select CONVERT(NUMERIC(10,0),( MAX(ISNULL(document_Id,0)) ) ) From Document";
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                SavePatientDocId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                            }
                        }

                        //OdbcSelect = "delete From Document where document_id = 189";
                        //CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        //OdbcCommand.ExecuteNonQuery();    
                        string outputPath = DestPath;
                        string str = Path.Combine(SourcePath, tmpFileOrgName);
                        File.Copy(str, str + ".ORG", true);
                        string newZipFileName = Path.Combine(outputPath, string.Format("{0}~{1}.esd", (object)dr["Patient_EHR_ID"].ToString().Trim(), (object)SavePatientDocId.ToString()));
                        using (ZipFile zipFile = new ZipFile())
                        {
                            zipFile.Password = "HIMOM";
                            zipFile.UseZip64WhenSaving = Zip64Option.AsNecessary;
                            zipFile.CompressionLevel = CompressionLevel.BestCompression;
                            zipFile.Encryption = EncryptionAlgorithm.PkzipWeak;
                            zipFile.ZipErrorAction = ZipErrorAction.Throw;
                            zipFile.ParallelDeflateThreshold = -1L;
                            zipFile.AddFile(str, "");
                            // zipFile.AddFile(str + ".ORG", "");
                            zipFile.Save(newZipFileName);
                        }
                        //Document.CreateSmartDoc(dr["Patient_EHR_ID"].ToString().Trim(), // Patient Id
                        //                          SavePatientDocId.ToString(), // Document Id
                        //                          tmpFileOrgName, // Source File Name
                        //                          SourcePath, // Source File Path
                        //                          "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4",
                        //                          true, // Keep Source Copy is true
                        //                          DestPath
                        //                         );
                        PullLiveDatabaseDAL.Update_PatientFormDocAttachment_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), SavePatientDocId.ToString(), Service_Install_Id);
                        File.Delete(sourceLocation);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;

            }
        }
        public static string GetEagleSoftDocPath(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftDocPath;
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                if (OdbcDt.Rows.Count > 0)
                {
                    if (OdbcDt.Rows[0]["server_computer"].ToString() == "" || OdbcDt.Rows[0]["server_computer"].ToString() == string.Empty)
                    {
                        if (OdbcDt.Rows[0]["server_share"].ToString().Trim() == "" || OdbcDt.Rows[0]["server_share"].ToString().Trim() == string.Empty)
                        {
                            return @"C:\EagleSoft\Data\Documents\patient";
                        }
                        else
                        {
                            return OdbcDt.Rows[0]["server_share"].ToString().TrimEnd('\\') + "\\Documents\\patient";
                        }
                    }
                    else
                    {
                        if (OdbcDt.Rows[0]["server_share"].ToString().Trim() == "" || OdbcDt.Rows[0]["server_share"].ToString().Trim() == string.Empty)
                        {
                            return @"C:\EagleSoft\Data\Documents\patient";
                        }
                        else
                        {
                            return "\\\\" + OdbcDt.Rows[0]["server_computer"].ToString() + "\\" + OdbcDt.Rows[0]["server_share"].ToString().TrimEnd('\\') + "\\Documents\\patient";
                        }

                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Int64 SavePatientPaymentTOEHR(string DbString, DataTable dtTable, string ServiceInstallationId,string _filename_EHR_Payment="", string EHRLogdirectory_EHR_Payment="")
        {
            // OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            Int64 TransactionHeaderId = 0;
            Int64 DiscountHeaderId = 0;
            string EHRLogId = "0";

            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
            }

            try
            {
                Int64 PayTypeId = 0;           // For payment
                Int64 Adjustment_Type_Id = 0;  //For refund
                Int64 Discount_Type_Id = 0;    //For Discount 
                Int64 CareCreditId = 0; // For Care Credit
                Int64 CareCreditAdjustment_Type_Id = 0;  //For CareCredit refund
                Int64 CareCreditDiscount_Type_Id = 0;    //For CareCredit Discount 
                string OdbcSelect = "";
                string note = "";
                TransactionHeaderId = 0;   //FinancialtransactionId to be inserted into transactionDetail

                #region Add PaymentMode 'Adit Pay' Categeries

                string sqlSelect = string.Empty;
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    #region Check For The Adit Pay Mode
                    using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //if (conn.State == ConnectionState.Closed) conn.Open();
                        SqlCeCommand.CommandText = SynchEagleSoftQRY.CheckPaymentModeExistAsAditPay;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            PayTypeId = Convert.ToInt64(SqlCeCommand.ExecuteScalar());
                            if (PayTypeId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Check and Add Payment Mode Exist As AditPay with PayTypeId=" + PayTypeId.ToString());
                            }
                        }
                        catch (Exception exAuth)
                        {
                            if (!exAuth.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw exAuth;
                            using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn))
                            {
                                odc.CommandTimeout = 200;
                                odc.CommandType = CommandType.Text;
                                odc.Parameters.Clear();
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                                {
                                    OdbcDa.UpdateCommand = odc;
                                    OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                    OdbcDa.UpdateCommand.Connection = conn;
                                    OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                    OdbcDa.UpdateCommand.ExecuteNonQuery();
                                }
                            }
                            PayTypeId = Convert.ToInt64(SqlCeCommand.ExecuteScalar());
                            if (PayTypeId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Check and Add Payment Mode Exist As AditPay with PayTypeId=" + PayTypeId.ToString());
                            }
                        }

                    }
                    #endregion

                    #region Add PaymentMode 'Adit Pay' To Adjustment_Type
                    var result = dtTable.AsEnumerable().Where(o => o.Field<object>("PaymentMode").ToString().ToUpper() == "REFUNDED" || o.Field<object>("PaymentMode").ToString().ToUpper() == "PARTIAL-REFUNDED").FirstOrDefault();
                    if (result != null)
                    {
                        using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchEagleSoftQRY.CheckModeExistAsAditPayInAdjustmentType;
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            Adjustment_Type_Id = Convert.ToInt64(SqlCeCommand.ExecuteScalar());
                            if (Adjustment_Type_Id > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, " Check and Add Mode Exist As AditPay In AdjustmentType with Adjustment_Type_Id" + Adjustment_Type_Id.ToString());
                            }
                        }
                    }
                    #endregion

                    #region check for Adit Pay Discount
                    result = dtTable.AsEnumerable().Where(o => o.Field<decimal>("Discount") > 0).FirstOrDefault();
                    if (result != null)
                    {
                        #region Add PaymentMode 'Adit Pay' To Discount_Type
                        using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchEagleSoftQRY.CheckModeExistAsDiscountType;
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            Discount_Type_Id = Convert.ToInt64(SqlCeCommand.ExecuteScalar());
                            if (Discount_Type_Id > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Check and Add PaymentMode 'Adit Pay' To Discount_Type with Discount_Type_Id=" + Discount_Type_Id.ToString());
                            }
                        }
                    }
                    #endregion

                    #endregion
                }
                #endregion

                #region Add PaymentMode 'CareCredit' Categeries

                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    #region ckeck for CareCredit mode
                    using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //if (conn.State == ConnectionState.Closed) conn.Open();
                        SqlCeCommand.CommandText = SynchEagleSoftQRY.CheckPaymentModeExistAsCareCredit;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CareCreditId = Convert.ToInt64(SqlCeCommand.ExecuteScalar());
                        if (CareCreditId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Check and Add Payment Mode Exist As AditPay with CareCreditId=" + CareCreditId.ToString());
                        }
                    }
                    #endregion

                    #region check for Carecredit Refund
                    var result = dtTable.AsEnumerable().Where(o => o.Field<object>("PaymentMode").ToString().ToUpper() == "REFUNDED" || o.Field<object>("PaymentMode").ToString().ToUpper() == "PARTIAL-REFUNDED").FirstOrDefault();
                    if (result != null)
                    {
                        using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchEagleSoftQRY.CheckModeExistAsCareCreditInAdjustmentType;
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            CareCreditAdjustment_Type_Id = Convert.ToInt64(SqlCeCommand.ExecuteScalar());
                            if (CareCreditAdjustment_Type_Id > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, " Check and Add Mode Exist As AditPay In AdjustmentType with CareCreditAdjustment_Type_Id" + CareCreditAdjustment_Type_Id.ToString());
                            }
                        }
                    }
                    #endregion

                    #region Check for CareCredit Discount
                    result = dtTable.AsEnumerable().Where(o => o.Field<decimal>("Discount") > 0).FirstOrDefault();
                    if (result != null)
                    {
                        #region Add PaymentMode 'Adit Pay' To Discount_Type
                        using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchEagleSoftQRY.CheckModeExistAsCareCreditDiscountType;
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            CareCreditDiscount_Type_Id = Convert.ToInt64(SqlCeCommand.ExecuteScalar());
                            if (CareCreditDiscount_Type_Id > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Check and Add PaymentMode 'Adit Pay' To Discount_Type with CareCreditDiscount_Type_Id=" + CareCreditDiscount_Type_Id.ToString());
                            }
                        }
                    }
                    #endregion


                    #endregion
                }

                #endregion  

                decimal amount = 0;
                string ProviderId = "";
                foreach (DataRow drRow in dtTable.Rows)
                {
                    amount = 0;
                    EHRLogId = "0";
                    TransactionHeaderId = 0;
                    DiscountHeaderId = 0;
                    ProviderId = "";
                    try
                    {
                        string paymentnote = "[UQID:" + drRow["PatientPaymentWebId"].ToString() + "]";

                        drRow["template"] = paymentnote;

                        if (drRow["template"].ToString().Length > 50)
                        {
                            note = drRow["template"].ToString().Substring(0, 50);
                        }
                        else
                        {
                            note = drRow["template"].ToString();
                        }
                        //Get Provider
                        #region Get Provider
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCmd = new OdbcCommand(sqlSelect, conn))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                OdbcSelect = "Select Preferred_dentist from patient where Patient_id = ? ";
                                OdbcCmd.CommandText = OdbcSelect;
                                OdbcCmd.CommandType = CommandType.Text;
                                OdbcCmd.Parameters.Clear();
                                OdbcCmd.Parameters.AddWithValue("@PatientEHRID", drRow["PatientEHRId"].ToString());
                                object Provid = OdbcCmd.ExecuteScalar();
                                if (Provid != null)
                                {
                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Get Provide for Patient_id=" + drRow["PatientEHRId"].ToString());
                                }
                                if (Provid == null || Provid.ToString() == "")
                                {
                                    OdbcSelect = "Select top 1 Preferred_dentist from patient ";
                                    OdbcCmd.CommandText = OdbcSelect;
                                    OdbcCmd.CommandType = CommandType.Text;
                                    ProviderId = OdbcCmd.ExecuteScalar().ToString();
                                    if (ProviderId != null)
                                    {
                                        Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Get Preferred_dentist for Provid = " + ProviderId.ToString());
                                    }
                                }
                                else
                                {
                                    ProviderId = Provid.ToString();
                                }
                            }
                        }
                        #endregion

                        amount = Convert.ToDecimal(drRow["Amount"]) - Convert.ToDecimal(drRow["Discount"]);

                        if (drRow["PaymentMethod"].ToString().ToLower() == "carecredit")
                        {
                            SaveCareCreditPaymentToEHR(DbString, ServiceInstallationId, drRow, amount, CareCreditId,ProviderId, TransactionHeaderId, DiscountHeaderId, note,CareCreditDiscount_Type_Id,CareCreditAdjustment_Type_Id, _filename_EHR_Payment, EHRLogdirectory_EHR_Payment, EHRLogId);
                        }
                        else
                        {
                            SaveAditPayPaymentToEHR(DbString, ServiceInstallationId, drRow, amount, PayTypeId,ProviderId, TransactionHeaderId, DiscountHeaderId, note, Discount_Type_Id, Adjustment_Type_Id, _filename_EHR_Payment, EHRLogdirectory_EHR_Payment, EHRLogId);
                        }
                    }
                    catch (Exception ex1)
                    {
                        //NoteId = "";
                        bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow);
                    }
                    // SynchLocalDAL.UpdatePatientPaymentEHRId_In_Local(TransactionHeaderId.ToString(), drRow["PatientPaymentWebId"].ToString().Trim(), "1");
                }
                return TransactionHeaderId;
            }
            catch (Exception ex)
            {
                return 0;
                throw;
            }

        }

        public static void SaveCareCreditPaymentToEHR(string DbString, string ServiceInstallationId, DataRow drRow, decimal amount, Int64 CareCreditId,string ProviderId,Int64 TransactionHeaderId, Int64 DiscountHeaderId, string note,Int64 CareCreditDiscount_Type_Id,Int64 CareCreditAdjustment_Type_Id, string _filename_EHR_Payment, string _EHRLogdirectory_EHR_Payment, string EHRLogId)
        {
            string OdbcSelect = "";
            try
            {
                #region Create Payment Entry in EHR if Payment Mode is Paid or Partial Paid and Log seeting is Ledger or Both
                if (Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 2 || Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 3)
                {
                    OdbcSelect = "";
                    if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        #region Save Payment Entries to Payment Table's
                        if (amount != 0)
                        {
                            TransactionHeaderId = SavePayment(drRow["PaymentMode"].ToString(), drRow["PatientEHRId"].ToString(), ProviderId, CareCreditId.ToString(), Convert.ToDateTime(drRow["PaymentDate"]), note, -amount, 'A', false, DbString, drRow["PatientPaymentWebId"].ToString(), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        }
                        #endregion  

                        #region If Discount is greater than Zero then Save Discount entrys to Tables
                        if (Convert.ToDecimal(drRow["Discount"]) > 0)
                        {
                            DiscountHeaderId = SavePayment(drRow["PaymentMode"].ToString(), drRow["PatientEHRId"].ToString(), ProviderId, CareCreditDiscount_Type_Id.ToString(), Convert.ToDateTime(drRow["PaymentDate"]), note, (-Convert.ToDecimal(drRow["Discount"])), 'C', true, DbString, drRow["PatientPaymentWebId"].ToString(), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        }
                        #endregion  
                    }
                    else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        #region Save Refund Entries To tables
                        TransactionHeaderId = SavePayment(drRow["PaymentMode"].ToString(), drRow["PatientEHRId"].ToString(), ProviderId, CareCreditAdjustment_Type_Id.ToString(), Convert.ToDateTime(drRow["PaymentDate"]), note, amount, 'D', true, DbString, drRow["PatientPaymentWebId"].ToString(), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        #endregion
                    }
                    drRow["PaymentEHRId"] = TransactionHeaderId.ToString();
                    drRow["PaymentUpdatedEHR"] = true;
                    drRow["PaymentUpdatedEHRDateTime"] = DateTime.Now.ToString();
                    Utility.WriteToErrorLogFromAll("Insert Records in Transaction Hearder " + OdbcSelect.ToString());
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Records in Transaction Hearder PaymentEHRId : " + TransactionHeaderId.ToString());

                }
                #endregion

                #region Create Payment Log entry bcoz Financial setting is only create log or Both
                if (Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 1 || Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 3)
                {
                    EHRLogId = Save_PatientPaymentLog_LocalToEagelsoft(drRow, DbString, ServiceInstallationId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                    if (EHRLogId != null)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment Log Local To Eagelsoft with PaymentEHRId : " + EHRLogId.ToString());
                    }
                }
                #endregion  

                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 0)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "Sync Log and Payment is disabled from Adit App", TransactionHeaderId.ToString(), EHRLogId.ToString(), CareCreditId.ToString(), "Sync Log and Payment is disabled from Adit App", Convert.ToInt32(drRow["TryInsert"]));
                }
                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", TransactionHeaderId.ToString(), EHRLogId.ToString(), CareCreditId.ToString(), "", Convert.ToInt32(drRow["TryInsert"]));

            }
            catch (Exception ex1)
            {
                Utility.WriteToErrorLogFromAll("Error in save payment "+ex1.Message);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", ex1.Message.ToString(), "", "", "", ex1.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]));
            }
        }

        public static void SaveAditPayPaymentToEHR(string DbString, string ServiceInstallationId, DataRow drRow, decimal amount, Int64 PayTypeId,string ProviderId,Int64 TransactionHeaderId, Int64 DiscountHeaderId, string note, Int64 Discount_Type_Id,Int64 Adjustment_Type_Id, string _filename_EHR_Payment, string _EHRLogdirectory_EHR_Payment, string EHRLogId)
        {
            string OdbcSelect = "";
            try
            {
                #region Create Payment Entry in EHR if Payment Mode is Paid or Partial Paid and Log seeting is Ledger or Both
                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 2 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                {
                    OdbcSelect = "";
                    if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        #region Save Payment Entries to Payment Table's
                        if (amount != 0)
                        {
                            TransactionHeaderId = SavePayment(drRow["PaymentMode"].ToString(), drRow["PatientEHRId"].ToString(), ProviderId, PayTypeId.ToString(), Convert.ToDateTime(drRow["PaymentDate"]), note, -amount, 'A', false, DbString, drRow["PatientPaymentWebId"].ToString(), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        }
                        #endregion

                        #region If Discount is greater than Zero then Save Discount entrys to Tables
                        if (Convert.ToDecimal(drRow["Discount"]) > 0)
                        {
                            DiscountHeaderId = SavePayment(drRow["PaymentMode"].ToString(), drRow["PatientEHRId"].ToString(), ProviderId, Discount_Type_Id.ToString(), Convert.ToDateTime(drRow["PaymentDate"]), note, (-Convert.ToDecimal(drRow["Discount"])), 'C', true, DbString, drRow["PatientPaymentWebId"].ToString(), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        }
                        #endregion

                    }
                    else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        #region Save Refund Entries To tables
                        TransactionHeaderId = SavePayment(drRow["PaymentMode"].ToString(), drRow["PatientEHRId"].ToString(), ProviderId, Adjustment_Type_Id.ToString(), Convert.ToDateTime(drRow["PaymentDate"]), note, amount, 'D', true, DbString, drRow["PatientPaymentWebId"].ToString(), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        #endregion
                    }
                    drRow["PaymentEHRId"] = TransactionHeaderId.ToString();
                    drRow["PaymentUpdatedEHR"] = true;
                    drRow["PaymentUpdatedEHRDateTime"] = DateTime.Now.ToString();
                    Utility.WriteToErrorLogFromAll("Insert Records in Transaction Hearder " + OdbcSelect.ToString());
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Records in Transaction Hearder PaymentEHRId : " + TransactionHeaderId.ToString());

                }
                #endregion

                #region Create Payment Log entry bcoz Financial setting is only create log or Both
                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 1 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                {
                    EHRLogId = Save_PatientPaymentLog_LocalToEagelsoft(drRow, DbString, ServiceInstallationId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                    if (EHRLogId != null)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment Log Local To Eagelsoft with PaymentEHRId : " + EHRLogId.ToString());
                    }
                }
                #endregion  

                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 0)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "Sync Log and Payment is disabled from Adit App", TransactionHeaderId.ToString(), EHRLogId.ToString(), DiscountHeaderId.ToString(), "Sync Log and Payment is disabled from Adit App", Convert.ToInt32(drRow["TryInsert"]));
                }
                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", TransactionHeaderId.ToString(), EHRLogId.ToString(), DiscountHeaderId.ToString(), "", Convert.ToInt32(drRow["TryInsert"]));

            }
            catch (Exception ex1)
            {

                Utility.WriteToErrorLogFromAll("Error in save payment " + ex1.Message);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", ex1.Message.ToString(), "", "", "", ex1.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]));

            }
        }

        public static bool UpdateIncludeAditDepositsFor_Reports()
        {
            bool status = false;
            try
            {
                for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                {
                    string sqlSelect = string.Empty;
                    string DbString = Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString();
                    using (OdbcConnection conn = new OdbcConnection(DbString))
                    {
                        using (OdbcCommand SqlCeCommand = new OdbcCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            //if (conn.State == ConnectionState.Closed) conn.Open();
                            SqlCeCommand.CommandText = SynchEagleSoftQRY.UpdatePaytypeFieldForDepositReport;
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            status = Convert.ToBoolean(SqlCeCommand.ExecuteScalar());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error In UpdatePaytypeFieldForDepositReport Query "+ex.Message.ToString());

            }
            return status;
        }
        public static Int64 SavePayment(string PaymentMode, string Patient_EHR_ID, string ProviderId, string PayTypeId, DateTime PaymentDate, string note, decimal amount, char PaymentType, bool IsAdj, string DbString, string paymentwebid,string _filename_EHR_Payment="",string EHRLogdirectory_EHR_Payment="")
        {
            string OdbcSelect = "";
            Int64 TransactionHeaderId = 0;

            #region Check entry in Transactions_header table
            using (OdbcCommand OdbcCmd = new OdbcCommand("", null))
            {
                OdbcCmd.CommandType = CommandType.Text;
                if (IsAdj)
                {
                    // OdbcSelect = " select tran_num from Transactions_header where Resp_party_id = ? and User_Id= ? and Tran_date=? and description=? and type =? and amount=? and adjustment_type = ?";
                    OdbcSelect = " select tran_num from Transactions_header where Resp_party_id = ? and Tran_date=?  and description=?   and type =? and amount=? and adjustment_type = ?";
                }
                else
                {
                    //OdbcSelect = " select tran_num from Transactions_header where Resp_party_id = ? and User_Id= ? and Tran_date=? and description=? and type =? and amount=? and paytype_id = ?";
                    OdbcSelect = " select tran_num from Transactions_header where Resp_party_id = ? and Tran_date=?  and description=?   and type =? and amount=? and paytype_id = ?";
                }
                OdbcCmd.CommandText = OdbcSelect;
                OdbcCmd.Parameters.Clear();
                OdbcCmd.Parameters.AddWithValue("@PatientEHRID", Patient_EHR_ID);
                //OdbcCmd.Parameters.AddWithValue("@ProviderId", Utility.EHR_UserLogin_ID);
                OdbcCmd.Parameters.Add("@PaymentDate", OdbcType.Date).Value = Convert.ToDateTime(PaymentDate).ToString("yyyy/MM/dd");
                OdbcCmd.Parameters.AddWithValue("@Note", note);
                OdbcCmd.Parameters.AddWithValue("@type", PaymentType);
                OdbcCmd.Parameters.AddWithValue("@Amount", amount);
                OdbcCmd.Parameters.AddWithValue("@adjustment_type", PayTypeId);
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    OdbcCmd.Connection = conn;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    object id = OdbcCmd.ExecuteScalar();
                    TransactionHeaderId = id != null ? Convert.ToInt64(id) : 0;
                    if (TransactionHeaderId > 0)
                    {
                        if (IsAdj)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Get Transactions header for PatientEHRID = " + Patient_EHR_ID + " and type=" + PaymentType + " and adjustment_type" + PayTypeId);
                        }
                        else
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Get Transactions header for  PatientEHRID = " + Patient_EHR_ID + " and type=" + PaymentType + " and paytype_id");
                        }
                    }

                }
            }
            #endregion

            if (TransactionHeaderId == 0)
            {
                #region If payment entry not exists already then insert the payment to TransactionHeader table 
                try
                {
                    TransactionHeaderId = SaveInTransactionHeader(Patient_EHR_ID, amount, PaymentDate, IsAdj, PayTypeId, PaymentType, note, DbString, _filename_EHR_Payment, EHRLogdirectory_EHR_Payment);
                    if (TransactionHeaderId > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Save Transaction Header with  PatientEHRID = " + Patient_EHR_ID + " and type=" + PaymentType + " and PayTypeId" + PayTypeId.ToString());
                    }
                }
                catch (Exception ex1)
                {
                    Utility.WriteToErrorLogFromAll("Error in TransactionHeaderId query (log from savepayment mtd) " + ex1.Message.ToString());
                }
                #endregion

            }
            //else
            //{
            //    #region check same amount payments exist ? if not then allow entry to ehr 

            //    string paynote = string.Empty;
            //    paynote = GetPaymentNote(TransactionHeaderId,DbString);
            //    if (!paynote.Contains(paymentwebid) && paynote != string.Empty)
            //    {
            //        TransactionHeaderId = SaveInTransactionHeader(Patient_EHR_ID, amount, PaymentDate, IsAdj, PayTypeId, PaymentType, note, DbString);
            //    }
            //    #endregion
            //}

            Int64 TransactionDetailId = 0;

            #region Check entry in transactions_detail Table
            using (OdbcCommand OdbcCmd = new OdbcCommand("", null))
            {
                OdbcCmd.CommandType = CommandType.Text;
                OdbcSelect = " select detail_id from transactions_detail where tran_num = ? and date_entered=? and provider_id=? and collections_go_to=? and patient_id=? and amount =?";
                OdbcCmd.CommandText = OdbcSelect;
                OdbcCmd.Parameters.Clear();
                OdbcCmd.Parameters.AddWithValue("@TransactionHeaderId", TransactionHeaderId.ToString());
                // OdbcCmd.Parameters.AddWithValue("@ProviderId", ProviderId);
                OdbcCmd.Parameters.Add("@PaymentDate", OdbcType.Date).Value = Convert.ToDateTime(PaymentDate).ToString("yyyy/MM/dd");
                OdbcCmd.Parameters.AddWithValue("@ProviderId2", ProviderId);
                OdbcCmd.Parameters.AddWithValue("@ProviderId3", ProviderId);
                OdbcCmd.Parameters.AddWithValue("@PatientEHRID", Patient_EHR_ID);
                OdbcCmd.Parameters.AddWithValue("@Amount", amount);
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    OdbcCmd.Connection = conn;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    object id = OdbcCmd.ExecuteScalar();
                    TransactionDetailId = id != null ? Convert.ToInt64(id) : 0;
                    if (TransactionDetailId > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Get transactions detail for PatientEHRID = " + Patient_EHR_ID + " and TransactionHeaderId=" + TransactionHeaderId.ToString());
                    }
                }
            }
            #endregion

            if (TransactionDetailId == 0)
            {
                #region If payment entry not exists already then insert the payment to transactions_detail table 
                using (OdbcCommand OdbcCmd = new OdbcCommand("", null))
                {
                    OdbcCmd.CommandType = CommandType.Text;
                    OdbcSelect = "Insert into transactions_detail(detail_id, tran_num, user_id, date_entered, provider_id, collections_go_to, patient_id, amount, provider_practice_id, patient_practice_id, applied_to, status, status_modifier, posneg) "
                                    + "values((select max(detail_id) + 1 from transactions_detail ),?,?,?,?,?,?,?,1,1,NULL,1,NULL,0 ) ";
                    OdbcCmd.CommandText = OdbcSelect;
                    OdbcCmd.Parameters.Clear();
                    OdbcCmd.Parameters.AddWithValue("@TransactionHeaderId", TransactionHeaderId.ToString());
                    OdbcCmd.Parameters.AddWithValue("@ProviderId", Utility.EHR_UserLogin_ID);
                    OdbcCmd.Parameters.Add("@PaymentDate", OdbcType.DateTime).Value = Convert.ToDateTime(PaymentDate);
                    OdbcCmd.Parameters.AddWithValue("@ProviderId2", ProviderId);
                    OdbcCmd.Parameters.AddWithValue("@ProviderId3", ProviderId);
                    OdbcCmd.Parameters.AddWithValue("@PatientEHRID", Patient_EHR_ID);
                    OdbcCmd.Parameters.AddWithValue("@Amount", amount);
                    using (OdbcConnection conn = new OdbcConnection(DbString))
                    {
                        OdbcCmd.Connection = conn;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCmd.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Save transactions detail with PatientEHRID = " + Patient_EHR_ID + " and TransactionHeaderId=" + TransactionHeaderId.ToString());
                        }
                        catch (Exception exAuth)
                        {
                            if (!exAuth.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw exAuth;
                            using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn))
                            {
                                odc.CommandTimeout = 200;
                                odc.CommandType = CommandType.Text;
                                odc.Parameters.Clear();
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                                {
                                    OdbcDa.UpdateCommand = odc;
                                    OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                    OdbcDa.UpdateCommand.Connection = conn;
                                    OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                    OdbcDa.UpdateCommand.ExecuteNonQuery();
                                }
                            }
                            OdbcCmd.ExecuteNonQuery();

                            //if (!ExecuteNonQuery(DbString, OdbcCmd))
                            //{
                            //    Utility.WriteToErrorLogFromAll("Err_Save Patient Payment to Eaglesoft For PatientID : " + Patient_EHR_ID.ToString() + " System Error " + exAuth.Message.ToString());
                            //}
                        }
                    }
                }
                #endregion  
            }
            if (TransactionHeaderId > 0)
            {
                #region If TransactionHeaderId is greater than zero the check entry and save the entry to Transactions_extra table
                Int64 Transactions_extraid = 0;
                using (OdbcCommand OdbcCmd = new OdbcCommand("", null))
                {
                    OdbcCmd.CommandType = CommandType.Text;
                    OdbcSelect = " select 1 from Transactions_extra where tran_num = ? ";
                    OdbcCmd.CommandText = OdbcSelect;
                    OdbcCmd.Parameters.Clear();
                    OdbcCmd.Parameters.AddWithValue("@TransactionHeaderId", TransactionHeaderId.ToString());
                    using (OdbcConnection conn = new OdbcConnection(DbString))
                    {
                        OdbcCmd.Connection = conn;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        object id = OdbcCmd.ExecuteScalar();
                        Transactions_extraid = id != null ? Convert.ToInt64(id) : 0;
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Get Transactions extra for TransactionHeaderId=" + TransactionHeaderId.ToString());
                    }
                }
                if (Transactions_extraid == 0)
                {
                    using (OdbcCommand OdbcCmd = new OdbcCommand("", null))
                    {
                        OdbcCmd.CommandType = CommandType.Text;
                        OdbcSelect = SynchEagleSoftQRY.SavePatientPayment_TransactionExtra;
                        OdbcSelect = OdbcSelect.Replace("@TransactionHeaderId", TransactionHeaderId.ToString());
                        OdbcCmd.CommandText = OdbcSelect;
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            OdbcCmd.Connection = conn;
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            try
                            {
                                OdbcCmd.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Save Patient Payment TransactionExtra with TransactionHeaderId=" + TransactionHeaderId.ToString());
                            }
                            catch (Exception exAuth)
                            {
                                if (!exAuth.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw exAuth;
                                using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn))
                                {
                                    odc.CommandTimeout = 200;
                                    odc.CommandType = CommandType.Text;
                                    odc.Parameters.Clear();
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                                    {
                                        OdbcDa.UpdateCommand = odc;
                                        OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                        OdbcDa.UpdateCommand.Connection = conn;
                                        OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                        OdbcDa.UpdateCommand.ExecuteNonQuery();
                                    }
                                }
                                OdbcCmd.ExecuteNonQuery();

                                //if (!ExecuteNonQuery(DbString, OdbcCmd))
                                //{
                                //    Utility.WriteToErrorLogFromAll("Err_Save Patient Payment to Eaglesoft For PatientID : " + Patient_EHR_ID.ToString() + " System Error " + exAuth.Message.ToString());
                                //}
                            }
                        }
                    }
                }
                #endregion

            }

            string sqlSelect = string.Empty;
            using (OdbcCommand OdbcCmd = new OdbcCommand(sqlSelect, null))
            {
                OdbcSelect = "";
                OdbcCmd.CommandType = CommandType.Text;
                OdbcSelect = SynchEagleSoftQRY.UpdatePatientBalance;
                OdbcCmd.Parameters.Clear();
                // OdbcCmd.Parameters.AddWithValue("@PaymentAmount", amount );
                OdbcCmd.Parameters.AddWithValue("@PatientEHRID1", Patient_EHR_ID);
                OdbcCmd.Parameters.AddWithValue("@PatientEHRID", Patient_EHR_ID);
                OdbcCmd.CommandText = OdbcSelect;
                Utility.WriteToSyncLogFile_All("Update Patient Balance " + OdbcSelect.ToString());
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    OdbcCmd.Connection = conn;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        OdbcCmd.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Update Patient Balance  PatientEHRID = " + Patient_EHR_ID );
                    }
                    catch (Exception exAuth)
                    {
                        if (!exAuth.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw exAuth;
                        using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn))
                        {
                            odc.CommandTimeout = 200;
                            odc.CommandType = CommandType.Text;
                            odc.Parameters.Clear();
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                            {
                                OdbcDa.UpdateCommand = odc;
                                OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                OdbcDa.UpdateCommand.Connection = conn;
                                OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                OdbcDa.UpdateCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Update Patient Balance error : " + exAuth.Message.ToString ());
                            }
                        }
                        OdbcCmd.ExecuteNonQuery();

                        //if (!ExecuteNonQuery(DbString, OdbcCmd))
                        //{
                        //    Utility.WriteToErrorLogFromAll("Err_Save Patient Payment to Eaglesoft For PatientID : " + Patient_EHR_ID.ToString() + " System Error " + exAuth.Message.ToString());
                        //}
                    }
                }
            }

            return TransactionHeaderId;
        }

        public static Int64 SaveInTransactionHeader(string Patient_EHR_ID, decimal amount, DateTime PaymentDate, bool IsAdj, string PayTypeId, char PaymentType, string note, string DbString,string _filename_EHR_Payment="",string EHRLogdirectory_EHR_Payment="")
        {
            Int64 TransactionHeaderId = 0;
            string OdbcSelect = "";
            using (OdbcCommand OdbcCmd = new OdbcCommand("", null))
            {
                OdbcSelect = "Insert Into Transactions_header (Tran_num,User_Id,Resp_party_id,Tran_date,Type,amount,service_code,sequence,statement_num," +
                "surface,fee,discount_surcharge,tax,description,defective,impacts,status,claim_id,est_primary,est_secondary,paid_primary," +
                "paid_secondary,bulk_payment_num,aging_date,tooth,lab_fee,lab_fee2,lab_code,lab_code2,pre_fee,standard_fee_id,practice_id,procedure_type_codes,balance,paytype_id,adjustment_type,)" +
                " values ((SELECT MAX(Tran_num)+1 FROM Transactions ),?,?,?,?,?,NULL,'','','',0,0,0,?,'N','C','A','',0,0,0,0,NULL,?,'',0,0,'','',0,'',1,'',0,?,?) " +
                " SElect @@Identity";

                OdbcCmd.CommandText = OdbcSelect;
                OdbcCmd.Parameters.Clear();
                OdbcCmd.Parameters.AddWithValue("@ProviderId", Utility.EHR_UserLogin_ID);
                OdbcCmd.Parameters.AddWithValue("@PatientEHRID3", Patient_EHR_ID);
                OdbcCmd.Parameters.Add("@PaymentDate2", OdbcType.DateTime).Value = Convert.ToDateTime(PaymentDate);
                OdbcCmd.Parameters.AddWithValue("@type", PaymentType);
                OdbcCmd.Parameters.AddWithValue("@Amount", amount);
                // OdbcCmd.Parameters.AddWithValue("@PayTypeId", PayTypeId.ToString());
                OdbcCmd.Parameters.AddWithValue("@Note", note);
                OdbcCmd.Parameters.Add("@PaymentDate3", OdbcType.DateTime).Value = Convert.ToDateTime(PaymentDate);
                if (IsAdj)
                {
                    OdbcCmd.Parameters.AddWithValue("@PayTypeId", DBNull.Value);
                    OdbcCmd.Parameters.AddWithValue("@adjustment_type", PayTypeId);
                }
                else
                {
                    OdbcCmd.Parameters.AddWithValue("@PayTypeId", PayTypeId);
                    OdbcCmd.Parameters.AddWithValue("@adjustment_type", DBNull.Value);
                }
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    OdbcCmd.Connection = conn;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        TransactionHeaderId = Convert.ToInt64(OdbcCmd.ExecuteScalar());
                        if (TransactionHeaderId > 0)
                        {
                            if (IsAdj)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Save In TransactionHeader with PatientEHRID = " + Patient_EHR_ID + " and Adjustment type=" + PayTypeId);
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Save In TransactionHeader with PatientEHRID = " + Patient_EHR_ID + " and PayTypeId=" + PayTypeId);
                            }
                        }
                    }
                    catch (Exception exAuth)
                    {
                        if (!exAuth.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw exAuth;
                        using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn))
                        {
                            odc.CommandTimeout = 200;
                            odc.CommandType = CommandType.Text;
                            odc.Parameters.Clear();
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                            {
                                OdbcDa.UpdateCommand = odc;
                                OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                OdbcDa.UpdateCommand.Connection = conn;
                                OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                OdbcDa.UpdateCommand.ExecuteNonQuery();
                            }
                        }
                        TransactionHeaderId = Convert.ToInt64(OdbcCmd.ExecuteScalar());
                        //object TID = null;
                        //if (ExecuteScalar(DbString, OdbcCmd, ref TID))
                        //{
                        //    TransactionHeaderId = Convert.ToInt64(TID);
                        //}
                        //else
                        //{
                        //    Utility.WriteToErrorLogFromAll("Err_Save Patient Payment to Eaglesoft For PatientID : " + Patient_EHR_ID.ToString() + " System Error " + exAuth.Message.ToString());
                        //}
                    }
                }
            }
            return TransactionHeaderId;
        }

        public static string GetPaymentNote(Int64 TransactionHeaderId,string DbString)
        {
            string note = string.Empty;
            try
            {

                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = "select description from transactions_header where tran_num=?";
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("TransactionHeaderId", TransactionHeaderId);
                        note=Convert.ToString(OdbcCommand.ExecuteScalar());
                    }
                }

                return note;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error in GetPaymentNote "+ex.Message.ToString());
                return "";
            }
        }

        #region Provider Office Hours

        private static string GetWeekDays(int weekDayIndex)
        {
            string returnweekdaysName = "";
            try
            {
                switch (weekDayIndex)
                {
                    case 1:
                        returnweekdaysName = "Monday";
                        break;
                    case 2:
                        returnweekdaysName = "Tuesday";
                        break;
                    case 3:
                        returnweekdaysName = "Wednesday";
                        break;
                    case 4:
                        returnweekdaysName = "Thursday";
                        break;
                    case 5:
                        returnweekdaysName = "Friday";
                        break;
                    case 6:
                        returnweekdaysName = "Saturday";
                        break;
                    case 7:
                        returnweekdaysName = "Sunday";
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnweekdaysName;
        }

        private static DataTable CreateTableOfProviderOfficeHours(DataTable dtProviderOfficeHours, string DbString, string Service_Install_Id)
        {
            DataTable dtResultProviderOfficeHours = new DataTable();
            try
            {
                #region GetDistinct Provider & Get ProviderHour Structure
                DataTable dtProvider = GetEagleSoftProviderData(DbString);
                dtResultProviderOfficeHours = SynchLocalDAL.GetLocalProviderOfficeHoursBlankStructure();
                #endregion

                #region Create PRovider & Daywise Datatable
                dtProvider.AsEnumerable()
                    .All(o =>
                    {
                        for (int i = 1; i <= 7; i++)
                        {
                            DataRow drNewRow = dtResultProviderOfficeHours.NewRow();
                            drNewRow["POH_EHR_ID"] = o["Provider_EHR_ID"].ToString() + "_" + i.ToString();
                            drNewRow["Provider_EHR_ID"] = o["Provider_EHR_ID"].ToString();
                            drNewRow["WeekDay"] = GetWeekDays(i).ToString();
                            drNewRow["Clinic_Number"] = "0";
                            drNewRow["Service_Install_Id"] = Service_Install_Id;
                            dtResultProviderOfficeHours.Rows.Add(drNewRow);
                        }
                        return true;
                    });
                #endregion

                #region Update Start & EndDatetime in Provider Daywise DataTable
                string dayBeg = "", dayEnd = "", dayLunchBeg = "", dayLunchEnd = "";

                dtResultProviderOfficeHours.AsEnumerable()
                    .All(o =>
                    {
                        dayBeg = o["WeekDay"].ToString().Substring(0, 3) + "_beg";
                        dayEnd = o["WeekDay"].ToString().Substring(0, 3) + "_end";
                        dayLunchBeg = o["WeekDay"].ToString().Substring(0, 3) + "_l_beg";
                        dayLunchEnd = o["WeekDay"].ToString().Substring(0, 3) + "_l_end";

                        var resultProvider = dtProviderOfficeHours.AsEnumerable().Where(a => a.Field<object>("Provider_Id").ToString().ToUpper() == o["Provider_EHR_ID"].ToString().ToUpper());

                        if (resultProvider.Count() > 0)
                        {
                            if (resultProvider.Select(b => b.Field<object>(dayBeg)).First() != null && resultProvider.Select(b => b.Field<object>(dayBeg)).First().ToString() != "")
                            {
                                o["StartTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(dayBeg)).First().ToString());
                            }
                            else
                            {
                                o["StartTime1"] = "01/01/2020 00:00:00";
                            }
                            if (resultProvider.Select(b => b.Field<object>(dayLunchBeg)).First() != null && resultProvider.Select(b => b.Field<object>(dayLunchBeg)).First().ToString() != "")
                            {
                                o["EndTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(dayLunchBeg)).First().ToString());
                            }
                            else if (resultProvider.Select(b => b.Field<object>(dayEnd)).First() != null && resultProvider.Select(b => b.Field<object>(dayEnd)).First().ToString() != "")
                            {
                                o["EndTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(dayEnd)).First().ToString());
                            }
                            else
                            {
                                o["EndTime1"] = "01/01/2020 00:00:00";
                            }
                            if (resultProvider.Select(b => b.Field<object>(dayLunchEnd)).First() != null && resultProvider.Select(b => b.Field<object>(dayLunchEnd)).First().ToString() != "")
                            {
                                o["StartTime2"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(dayLunchEnd)).First().ToString());
                            }
                            else
                            {
                                o["StartTime2"] = "01/01/2020 00:00:00";
                            }
                            if (resultProvider.Select(b => b.Field<object>(dayEnd)).First() != null && resultProvider.Select(b => b.Field<object>(dayEnd)).First().ToString() != ""
                                && resultProvider.Select(b => b.Field<object>(dayLunchBeg)).First() != null && resultProvider.Select(b => b.Field<object>(dayLunchBeg)).First().ToString() != "")
                            {
                                o["EndTime2"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(dayEnd)).First().ToString());
                            }
                            else
                            {
                                o["EndTime2"] = "01/01/2020 00:00:00";
                            }
                            o["StartTime3"] = "01/01/2020 00:00:00";
                            o["EndTime3"] = "01/01/2020 00:00:00";
                        }
                        else
                        {
                            o["StartTime1"] = "01/01/2020 00:00:00";
                            o["EndTime1"] = "01/01/2020 00:00:00";
                            o["StartTime2"] = "01/01/2020 00:00:00";
                            o["EndTime2"] = "01/01/2020 00:00:00";
                            o["StartTime3"] = "01/01/2020 00:00:00";
                            o["EndTime3"] = "01/01/2020 00:00:00";
                        }

                        return true;
                    });
                #endregion

            }
            catch (Exception)
            {
                throw;
            }
            return dtResultProviderOfficeHours;
        }

        private static DataTable CreateTableOfOperatoryOfficeHours(DataTable dtOperatoryOfficeHours, string DbString, string Service_Install_Id)
        {
            DataTable dtResultOperatoryOfficeHours = new DataTable();
            try
            {
                #region GetDistinct Operatory & Get OperatoryHour Structure
                DataTable dtOperatory = GetEagleSoftOperatoryData(DbString);
                dtResultOperatoryOfficeHours = SynchLocalDAL.GetLocalOperatoryOfficeHoursBlankStructure();
                #endregion

                #region Create Operatory & Daywise Datatable
                dtOperatory.AsEnumerable()
                    .All(o =>
                    {
                        for (int i = 1; i <= 7; i++)
                        {
                            DataRow drNewRow = dtResultOperatoryOfficeHours.NewRow();
                            drNewRow["OOH_EHR_ID"] = o["Operatory_EHR_ID"].ToString() + "_" + i.ToString();
                            drNewRow["Operatory_EHR_ID"] = o["Operatory_EHR_ID"].ToString();
                            drNewRow["WeekDay"] = GetWeekDays(i).ToString();
                            drNewRow["Clinic_Number"] = "0";
                            drNewRow["Service_Install_Id"] = Service_Install_Id;
                            dtResultOperatoryOfficeHours.Rows.Add(drNewRow);
                        }
                        return true;
                    });
                #endregion

                #region Update Start & EndDatetime in Operatory Daywise DataTable
                string dayBeg = "", dayEnd = "", dayLunchBeg = "", dayLunchEnd = "";

                dtResultOperatoryOfficeHours.AsEnumerable()
                    .All(o =>
                    {
                        dayBeg = o["WeekDay"].ToString().Substring(0, 3) + "_beg";
                        dayEnd = o["WeekDay"].ToString().Substring(0, 3) + "_end";
                        dayLunchBeg = o["WeekDay"].ToString().Substring(0, 3) + "_l_beg";
                        dayLunchEnd = o["WeekDay"].ToString().Substring(0, 3) + "_l_end";

                        var resultOperatory = dtOperatoryOfficeHours.AsEnumerable().Where(a => a.Field<object>("Operatory_Id").ToString().ToUpper() == o["Operatory_EHR_ID"].ToString().ToUpper());

                        if (resultOperatory.Count() > 0)
                        {
                            if (resultOperatory.Select(b => b.Field<object>(dayBeg)).First() != null && resultOperatory.Select(b => b.Field<object>(dayBeg)).First().ToString() != "")
                            {
                                o["StartTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(dayBeg)).First().ToString());
                            }
                            else
                            {
                                o["StartTime1"] = "01/01/2020 00:00:00";
                            }
                            if (resultOperatory.Select(b => b.Field<object>(dayLunchBeg)).First() != null && resultOperatory.Select(b => b.Field<object>(dayLunchBeg)).First().ToString() != "")
                            {
                                o["EndTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(dayLunchBeg)).First().ToString());
                            }
                            else if (resultOperatory.Select(b => b.Field<object>(dayEnd)).First() != null && resultOperatory.Select(b => b.Field<object>(dayEnd)).First().ToString() != "")
                            {
                                o["EndTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(dayEnd)).First().ToString());
                            }
                            else
                            {
                                o["EndTime1"] = "01/01/2020 00:00:00";
                            }
                            if (resultOperatory.Select(b => b.Field<object>(dayLunchEnd)).First() != null && resultOperatory.Select(b => b.Field<object>(dayLunchEnd)).First().ToString() != "")
                            {
                                o["StartTime2"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(dayLunchEnd)).First().ToString());
                            }
                            else
                            {
                                o["StartTime2"] = "01/01/2020 00:00:00";
                            }
                            if (resultOperatory.Select(b => b.Field<object>(dayEnd)).First() != null && resultOperatory.Select(b => b.Field<object>(dayEnd)).First().ToString() != ""
                                && resultOperatory.Select(b => b.Field<object>(dayLunchBeg)).First() != null && resultOperatory.Select(b => b.Field<object>(dayLunchBeg)).First().ToString() != "")
                            {
                                o["EndTime2"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(dayEnd)).First().ToString());
                            }
                            else
                            {
                                o["EndTime2"] = "01/01/2020 00:00:00";
                            }
                            o["StartTime3"] = "01/01/2020 00:00:00";
                            o["EndTime3"] = "01/01/2020 00:00:00";
                        }
                        else
                        {
                            o["StartTime1"] = "01/01/2020 00:00:00";
                            o["EndTime1"] = "01/01/2020 00:00:00";
                            o["StartTime2"] = "01/01/2020 00:00:00";
                            o["EndTime2"] = "01/01/2020 00:00:00";
                            o["StartTime3"] = "01/01/2020 00:00:00";
                            o["EndTime3"] = "01/01/2020 00:00:00";
                        }

                        return true;
                    });
                #endregion

            }
            catch (Exception)
            {
                throw;
            }
            return dtResultOperatoryOfficeHours;
        }

        public static DataTable GetEagleSoftOperatoryOfficeHours(string DbString, string Service_Install_Id)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftOperatoryOfficeHours;
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return CreateTableOfOperatoryOfficeHours(OdbcDt, DbString, Service_Install_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEagleSoftProviderOfficeHours(string DbString, string Service_Install_Id)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftProviderOfficeHours;
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        //OdbcCommand.Parameters.AddWithValue("@providerIds", joinedstring);
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return CreateTableOfProviderOfficeHours(OdbcDt, DbString, Service_Install_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Save_ProviderOfficeHours_Eaglesoft_To_Local(DataTable dtEagleSoftProviderOfficeHours, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        string AppointmentStatus = string.Empty;
                        foreach (DataRow dr in dtEagleSoftProviderOfficeHours.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.ProviderOfficeHours_Insert;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.ProviderOfficeHours_Update;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.ProviderOfficeHours_Delete;
                                        break;
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("POH_EHR_ID", dr["POH_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {

                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("POH_EHR_ID", dr["POH_EHR_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("POH_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("WeekDay", dr["WeekDay"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("StartTime1", Utility.CheckValidDatetime(dr["StartTime1"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("EndTime1", Utility.CheckValidDatetime(dr["EndTime1"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime2", Utility.CheckValidDatetime(dr["StartTime2"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("EndTime2", Utility.CheckValidDatetime(dr["EndTime2"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime3", Utility.CheckValidDatetime(dr["StartTime3"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("EndTime3", Utility.CheckValidDatetime(dr["EndTime3"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static DataTable GetEagleSoftProviderHours(string DbString, string Service_Install_Id)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftProviderHours;
                OdbcSelect = OdbcSelect.Replace("@ToDate", "'" + ToDate.ToString("yyyy/MM/dd") + "'");
                OdbcSelect = OdbcSelect.Replace("@Service_Install_Id", Service_Install_Id);
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        //  MySqlCommand.CommandTimeout = 120;
                        OdbcCommand.CommandTimeout = 200;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable GetEagleSoftPatientDiseaseData(string DbString)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftPatientAllergies;
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEagleSoftPatientMedicationData(string DbString, string Patient_EHR_IDS)
        {
            try
            {
                string OdbcSelect = "";
                if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                {
                    OdbcSelect = SynchEagleSoftQRY.GetEagleSoftPatientMedication;
                }
                else
                {
                    OdbcSelect = SynchEagleSoftQRY.GetEagleSoftPatientMedication + " and mp.patient_id in (" + Patient_EHR_IDS + ")";
                }
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEagleSoftDiseaseData(string DbString)
        {
            try
            {
                string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftAllergies;
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEagleSoftMedicationData(string DbString)
        {
            try
            {
                string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftMedication;
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Save_Disease_EagleSoft_To_Local(DataTable dtEagleSoftDisease, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtEagleSoftDisease.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Disease;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Disease;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Disease;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Disease_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Disease_Name", dr["Disease_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("is_deleted", Convert.ToInt32(dr["is_deleted"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool Save_PatientDisease_EagleSoft_To_Local(DataTable dtEagleSoftpatientDisease, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtEagleSoftpatientDisease.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_PatientDisease;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientDisease;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_PatientDisease;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Disease_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Disease_Name", dr["Disease_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("is_deleted", string.IsNullOrEmpty(dr["is_deleted"].ToString()) ? false : Convert.ToBoolean(dr["is_deleted"]));
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }
        #endregion

        #region Get Medical History Fields

        public static DataTable GetPatientWiseMedicalForm(string DbString, string strPatientID = "")
        {
            try
            {
                string OdbcSelect = SynchEagleSoftQRY.GetPatientWiseFormEntry;
                if (!string.IsNullOrEmpty(strPatientID))
                {
                    OdbcSelect = SynchEagleSoftQRY.GetPatientWiseFormEntryByPatID;
                    OdbcSelect = OdbcSelect.Replace("@Patient_EHR_ID", strPatientID);
                }
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetEaglesoftMedicalHistoryData(string DbString, string Service_Install_Id)
        {
            try
            {
                DataSet dsMedicalHistory = new DataSet();
                DataTable dtFormMaster = new DataTable();
                DataTable dtSectionMaster = new DataTable();
                DataTable dtAlertMaster = new DataTable();
                DataTable dtSectionItemQuestion = new DataTable();
                DataTable dtSectionItemOption = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = "";
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcSelect = SynchEagleSoftQRY.GetEagleSoftSectionMaster;
                        OdbcSelect = OdbcSelect.Replace("@Service_Install_Id", Service_Install_Id);
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.CommandText = OdbcSelect;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(dtSectionMaster);
                        }
                        OdbcSelect = SynchEagleSoftQRY.GetEagleSoftFormMaster;
                        OdbcSelect = OdbcSelect.Replace("@Service_Install_Id", Service_Install_Id);
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.CommandText = OdbcSelect;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(dtFormMaster);
                        }
                        OdbcSelect = SynchEagleSoftQRY.GetEagleSoftAlertMaster;
                        OdbcSelect = OdbcSelect.Replace("@Service_Install_Id", Service_Install_Id);
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.CommandText = OdbcSelect;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(dtAlertMaster);
                        }
                        OdbcSelect = SynchEagleSoftQRY.GetEaglesoftSectionItemMaster;
                        OdbcSelect = OdbcSelect.Replace("@Service_Install_Id", Service_Install_Id);
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.CommandText = OdbcSelect;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(dtSectionItemQuestion);
                        }
                        OdbcSelect = SynchEagleSoftQRY.GetEagleSoftSectionItemOptions;
                        OdbcSelect = OdbcSelect.Replace("@Service_Install_Id", Service_Install_Id);
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.CommandText = OdbcSelect;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(dtSectionItemOption);
                        }
                    }
                }
                dtFormMaster.TableName = "EagleSoftFormMaster";
                dtSectionMaster.TableName = "EagleSoftSectionMaster";
                dtAlertMaster.TableName = "EagleSoftAlertMaster";
                dtSectionItemQuestion.TableName = "EagleSoftSectionItemMaster";
                dtSectionItemOption.TableName = "EagleSoftSectionItemOptionMaster";

                dsMedicalHistory.Tables.Add(dtFormMaster);
                dsMedicalHistory.Tables.Add(dtSectionMaster);
                dsMedicalHistory.Tables.Add(dtAlertMaster);
                dsMedicalHistory.Tables.Add(dtSectionItemQuestion);
                dsMedicalHistory.Tables.Add(dtSectionItemOption);

                return dsMedicalHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateAnswerInEagleSoftListAnswer(string tableName, Int64 formInstanceId, int answerChoice, string listItemId, string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                string strQauery = SynchEagleSoftQRY.UpdateEaglesoftListItemAnswer.Replace("@answer_choice", answerChoice.ToString()).Replace("@instance_id", formInstanceId.ToString()).Replace("@list_item_id", listItemId);
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("UpdateAnswerInEagleSoftListAnswer Error " + exAuth.Message.ToString());
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateAnswerInEagleSoftSignleQuestionAnswer(string tableName, Int64 formInstanceId, int answerChoice, string section_item_id, string comment, string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                string strQauery = SynchEagleSoftQRY.UpdateEaglesoftSingleQuestionAnswer.Replace("@answer_choice", answerChoice.ToString()).Replace("@instance_id", formInstanceId.ToString()).Replace("@section_item_id", section_item_id).Replace("@comment", "'" + comment.Replace("'", "''") + "'");
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("UpdateAnswerInEagleSoftSignleQuestionAnswer Error " + exAuth.Message.ToString());
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateAnswerInEagleSoftSignleQuestionAnswerFeelFree(string tableName, Int64 formInstanceId, int answerChoice, string section_item_id, string comment, string AnsValue, string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                string strQauery = SynchEagleSoftQRY.UpdateEaglesoftSingleQuestionAnswerFeelFree.Replace("@answer_choice", answerChoice.ToString()).Replace("@instance_id", formInstanceId.ToString()).Replace("@section_item_id", section_item_id).Replace("@AnswerText", "'" + AnsValue + "'").Replace("@comment", "'" + comment + "'");
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("UpdateAnswerInEagleSoftSignleQuestionAnswerFeelFree Error " + exAuth.Message.ToString());
                            }
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool UpdateAnswerInEagleSoftCommentAnswer(string tableName, Int64 formInstanceId, string commentAns, string sectionItemId, string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                string strQauery = SynchEagleSoftQRY.UpdateEaglesoftCommentAnswer.Replace("@commentAns", "'" + commentAns.ToString().Replace("'", "''") + "'").Replace("@instance_id", formInstanceId.ToString()).Replace("@sectionitemId", sectionItemId);
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("UpdateAnswerInEagleSoftCommentAnswer Error " + exAuth.Message.ToString());
                            }
                        }
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertAnswerInEagleSoftListAnswer(string tableName, Int64 formInstanceId, int answerChoice, string listItemId, string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                string strQauery = SynchEagleSoftQRY.InsertEaglesoftListItemAnswer.Replace("@answer_choice", answerChoice.ToString()).Replace("@instance_id", formInstanceId.ToString()).Replace("@List_item_id", listItemId);
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("InsertAnswerInEagleSoftListAnswer Error " + exAuth.Message.ToString());
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool InsertAnswerInEagleSoftSingleQuestionAnswer(string tableName, Int64 formInstanceId, int answerChoice, string section_item_id, string comment, string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {

                string strQauery = SynchEagleSoftQRY.InsertEaglesoftSingleQuestionAnswer.Replace("@answer_choice", answerChoice.ToString()).Replace("@instance_id", formInstanceId.ToString()).Replace("@section_item_id", section_item_id).Replace("@comment", "'" + comment + "'");
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("InsertAnswerInEagleSoftSingleQuestionAnswer Error " + exAuth.Message.ToString());
                            }
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool InsertAnswerInEagleSoftSingleQuestionAnswerFeelFree(string tableName, Int64 formInstanceId, int answerChoice, string section_item_id, string comment, string AnsValue, string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                string strQauery = SynchEagleSoftQRY.InsertEaglesoftSingleQuestionAnswerFeelFree.Replace("@answer_choice", answerChoice.ToString()).Replace("@instance_id", formInstanceId.ToString()).Replace("@section_item_id", section_item_id).Replace("@AnswerText", "'" + AnsValue + "'").Replace("@comment", "'" + comment + "'");
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("InsertAnswerInEagleSoftSingleQuestionAnswerFeelFree Error " + exAuth.Message.ToString());
                            }
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool InsertAnswerInEagleSoftCommentAnswer(string tableName, Int64 formInstanceId, string commentAns, string sectionItemId, string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {

                string strQauery = SynchEagleSoftQRY.InsertEaglesoftCommentAnswer.Replace("@commentAns", "'" + commentAns.ToString().Replace("'", "''") + "'").Replace("@instance_id", formInstanceId.ToString()).Replace("@Section_item_id", sectionItemId);
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("InsertAnswerInEagleSoftCommentAnswer Error " + exAuth.Message.ToString());
                            }
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateFormInstance(Int64 formInstanceId, string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                string strQauery = SynchEagleSoftQRY.UpdateEaglesoftFormInstance.Replace("@instance_id", formInstanceId.ToString());
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("UpdateFormInstance Error " + exAuth.Message.ToString());
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Int64 InsertFormInstance(string formId, string patientId, string DbString)
        {
            Int64 formInstanceId = 0;
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                string strQauery = SynchEagleSoftQRY.InsertFormInstance.Replace("@formid", formId).Replace("@patientId", "'" + patientId.ToString() + "'");
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        try
                        {
                            formInstanceId = Convert.ToInt64(OdbcCommand.ExecuteScalar().ToString());
                        }
                        catch (Exception exAuth)
                        {
                            object frmInsId = null;
                            ExecuteScalar(DbString, OdbcCommand, ref frmInsId);
                            if (frmInsId != null)
                            {
                                formInstanceId = Convert.ToInt64(frmInsId);
                            }
                        }

                    }
                }
                return formInstanceId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public static bool SaveAllergiesToEaglesoft(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            OdbcConnection conn = new OdbcConnection(DbString);
            OdbcCommand OdbcCommand = new OdbcCommand();
            try
            {
                // CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);
                DataTable dtPatientDisease = SynchLocalDAL.GetLocalPatientFormDiseaseResponseToSaveINEHR(Service_Install_Id, strPatientFormID);
                if (dtPatientDisease.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPatientDisease.Rows)
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        string OdbcSelect = SynchEagleSoftQRY.InsertPatientAllergies;
                        OdbcSelect = OdbcSelect.Replace("@Patient_id", "'" + dr["PatientEHRId"] + "'");
                        OdbcSelect = OdbcSelect.Replace("@Alert_id", dr["Disease_EHR_Id"].ToString());
                        Utility.WriteToErrorLogFromAll(OdbcSelect);

                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("SaveAllergiesToEaglesoft Error " + exAuth.Message.ToString());
                            }
                        }

                        Utility.WriteToErrorLogFromAll("Call Function to update disease response");
                        SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), (dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString()), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), Service_Install_Id);

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static string GetEaglesoftPatient_AllergyID(string DbString,string SectionEHR_ID)
        {
            string alert_id = "";
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEagleSoftMedicalHistoryAllergies;
                    OdbcSelect = OdbcSelect.Replace("@SectionEHR_ID", "'" + SectionEHR_ID + "'");
                    
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                        if (OdbcDt.Rows.Count > 0)
                            if (OdbcDt.Rows[0]["alert_id"].ToString() != "" || OdbcDt.Rows[0]["alert_id"].ToString() != string.Empty)
                            {

                                alert_id = OdbcDt.Rows[0]["alert_id"].ToString();

                            }
                            else
                            {
                                alert_id = "";
                            }
                    }
                }
                return alert_id;
            }
            catch (Exception ex)
            {
                alert_id = "";
                throw ex;
                //version = "";

            }
        }


        public static bool SaveAllergiesFromMedicalHistoryFormToEaglesoft(string DbString, string Service_Install_Id, string SectionEHR_ID,string patient_ehr_id, string strPatientFormID = "")
        {
            OdbcConnection conn = new OdbcConnection(DbString);
            OdbcCommand OdbcCommand = new OdbcCommand();
           
            try
            {
                string AlertID = GetEaglesoftPatient_AllergyID(DbString, SectionEHR_ID) ;
                
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        string OdbcSelect = SynchEagleSoftQRY.InsertPatientAllergies;
                        OdbcSelect = OdbcSelect.Replace("@Patient_id", "'" + patient_ehr_id + "'");
                        OdbcSelect = OdbcSelect.Replace("@Alert_id", "'" + AlertID.ToString() + "'");
                        
                        //Utility.WriteToErrorLogFromAll(OdbcSelect);

                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("SaveAllergiesToEaglesoft Error " + exAuth.Message.ToString());
                            }
                        }
                        //Utility.WriteToErrorLogFromAll("Call Function to update disease response");
                       // SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), (dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString()), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), Service_Install_Id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool DeleteAllergiesToEaglesoft(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            OdbcConnection conn = new OdbcConnection(DbString);
            OdbcCommand OdbcCommand = new OdbcCommand();
            try
            {
                //CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);
                DataTable dtPatientDeleteDisease = SynchLocalDAL.GetLocalPatientFormDiseaseDeleteResponseToSaveINEHR(Service_Install_Id, strPatientFormID);
                if (dtPatientDeleteDisease.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPatientDeleteDisease.Rows)
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        string OdbcSelect = SynchEagleSoftQRY.DeletePatientAllergies;
                        OdbcSelect = OdbcSelect.Replace("@Patient_id", "'" + dr["Patient_EHR_Id"] + "'");
                        OdbcSelect = OdbcSelect.Replace("@Alert_id", dr["Disease_EHR_Id"].ToString());
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        try
                        {
                            OdbcCommand.ExecuteNonQuery();
                        }
                        catch (Exception exAuth)
                        {
                            if (!ExecuteNonQuery(DbString, OdbcCommand))
                            {
                                Utility.WriteToErrorLogFromAll("DeleteAllergiesToEaglesoft Error " + exAuth.Message.ToString());
                            }
                        }
                        SynchLocalDAL.UpdateDeleteDiseaseEHR_Updateflg(dr["Disease_EHR_Id"].ToString(), dr["Patient_EHR_Id"].ToString(), Service_Install_Id);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetPatientWiseRecall(string DbString, string Service_Install_Id)
        {
            OdbcConnection conn = new OdbcConnection(DbString);
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            //  CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchEagleSoftQRY.GetPatientWiseRecallData;
                OdbcSelect = OdbcSelect.Replace("@Service_Install_Id", Service_Install_Id);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static bool SavePatientWiseRecallType_Eaglesoft_To_Local(DataTable dtEaglesoftRecallType, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtEaglesoftRecallType.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_PatientWiseRecallType;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientWiseRecallType;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_PatientWiseRecallType;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Patient_Recall_Id", dr["Patient_Recall_Id"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Patient_EHR_id", dr["Patient_EHR_id"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Recall_Date", dr["Recall_Date"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Recall_Date", dr["Last_Recall_Date"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("RecallType_EHR_ID", dr["RecallType_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("RecallType_Name", dr["RecallType_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("RecallType_Descript", dr["RecallType_Descript"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Default_Recall", dr["Default_Recall"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", DateTime.Now.ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", DateTime.Now.ToString());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", 0);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", 0);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString().Trim());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static string Save_PatientPaymentLog_LocalToEagelsoft(DataRow drRow, string DbString, string ServiceInstallationId,string _filename_EHR_Payment,string  EHRLogdirectory_EHR_Payment )
        {
            try
            {
                if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                {
                    Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
                }

                string NoteId = "";
                OdbcConnection conn = new OdbcConnection(DbString);
                OdbcCommand OdbcCommand = new OdbcCommand();
                // CommonDB.OdbcEaglesoftConnectionServer(ref conn, DbString);

                //if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 1 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                //{
                if (conn.State == ConnectionState.Closed) conn.Open();
                {
                    try
                    {
                        OdbcCommand.CommandTimeout = 200;
                        string OdbcSelect = SynchEagleSoftQRY.InsertPatientPaymentLog;
                        OdbcSelect = OdbcSelect.Replace("@PatientEHRId", drRow["PatientEHRId"].ToString());
                        OdbcSelect = OdbcSelect.Replace("@note_type", "'A'");
                        OdbcSelect = OdbcSelect.Replace("@notetypeid", "4");
                        OdbcSelect = OdbcSelect.Replace("@NoteDescription", "''");
                        OdbcSelect = OdbcSelect.Replace("@EHR_User_ID", "'" + Utility.EHR_UserLogin_ID + "'");
                        OdbcSelect = OdbcSelect.Replace("@Note", "'" + drRow["template"].ToString().Replace("'", "''") + "'");
                        OdbcSelect = OdbcSelect.Replace("@date_entered", "'" + Convert.ToDateTime(drRow["PaymentDate"]).ToString("yyyy/MM/dd HH:mm").ToString() + "'");
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        try
                        {
                            NoteId = OdbcCommand.ExecuteScalar().ToString();
                            if (NoteId != null)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Save Patient PaymentLog with PatientEHRId" +  drRow["PatientEHRId"].ToString() + ",EHR_User_ID=" + Utility.EHR_UserLogin_ID);
                            }
                        }
                        catch (Exception exAuth)
                        {
                            if (!exAuth.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw exAuth;
                            using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn))
                            {
                                odc.CommandTimeout = 200;
                                odc.CommandType = CommandType.Text;
                                odc.Parameters.Clear();
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                                {
                                    OdbcDa.UpdateCommand = odc;
                                    OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                    OdbcDa.UpdateCommand.Connection = conn;
                                    OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                    OdbcDa.UpdateCommand.ExecuteNonQuery();
                                }
                            }
                            NoteId = OdbcCommand.ExecuteScalar().ToString();
                            //Object NID = null;
                            //if (ExecuteScalar(DbString, OdbcCommand, ref NID))
                            //{
                            //    NoteId = NID.ToString();
                            //}
                            //else
                            //{
                            //    Utility.WriteToErrorLogFromAll("Err_Save Patient Payment Log to Eaglesoft For PatientID : " + drRow["PatientEHRId"].ToString() + " System Error " + exAuth.Message.ToString());
                            //}
                        }

                        //SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", ServiceInstallationId.Trim(), drRow["Clinic_Number"].ToString().Trim(), "", NoteId,"","","");

                    }
                    catch (Exception ex)
                    {
                        NoteId = "";
                        // SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", ServiceInstallationId.Trim(), drRow["Clinic_Number"].ToString().Trim(), "", NoteId,"","",ex.Message.ToString());
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
                // }

                return NoteId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string Save_PatientSMSCallLog_LocalToEagelsoft(DataTable dtWebPatientPayment, string DbString, string ServiceInstallationId)
        {
            try
            {
                string NoteId = "";

                if (!dtWebPatientPayment.Columns.Contains("Log_Status"))
                {
                    dtWebPatientPayment.Columns.Add("Log_Status", typeof(string));
                }
                DataTable dtResultCopy = new DataTable();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    NoteId = "";
                    dtResultCopy = dtWebPatientPayment.Select("Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' and Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();

                    if (dtResultCopy != null)
                    {
                        if (dtResultCopy.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                            {
                                Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
                            }
                        }
                    }
                    Utility.WriteToErrorLogFromAll("Local Patient SMSCall 1.2 dtResultCopy Data:" + dtResultCopy.Rows.Count);
                    foreach (DataRow drRow in dtResultCopy.Rows)
                    {
                        if (drRow["PatientEHRId"] != null && drRow["PatientEHRId"].ToString() != string.Empty && drRow["PatientEHRId"].ToString() != "")
                        {
                            try
                            {
                                using (OdbcCommand OdbcCommand = new OdbcCommand("", null))
                                {
                                    OdbcCommand.CommandTimeout = 200;
                                    OdbcCommand.CommandType = CommandType.Text;
                                    string OdbcSelect = SynchEagleSoftQRY.InsertPatientPaymentLog;

                                    OdbcSelect = OdbcSelect.Replace("@PatientEHRId", "'" + drRow["PatientEHRId"].ToString() + "'");
                                    OdbcSelect = OdbcSelect.Replace("@note_type", "'G'");
                                    OdbcSelect = OdbcSelect.Replace("@notetypeid", "7");
                                    OdbcSelect = OdbcSelect.Replace("@EHR_User_ID", "'" + Utility.EHR_UserLogin_ID + "'");
                                    OdbcSelect = OdbcSelect.Replace("@NoteDescription", "'" + drRow["app_alias"].ToString() + "'");
                                    OdbcSelect = OdbcSelect.Replace("@date_entered", "'" + Convert.ToDateTime(drRow["PatientSMSCallLogDate"]).ToString("yyyy/MM/dd HH:mm").ToString() + "'");
                                    OdbcSelect = OdbcSelect.Replace("@Note", "'" + drRow["text"].ToString().Replace("'", "''") + "'");
                                    OdbcCommand.CommandText = OdbcSelect;

                                    using (OdbcConnection conn = new OdbcConnection(DbString))
                                    {
                                        OdbcCommand.Connection = conn;
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        try
                                        {
                                            NoteId = OdbcCommand.ExecuteScalar().ToString();
                                        }
                                        catch (Exception exAuth)
                                        {
                                            object NID = null;
                                            if (ExecuteScalar(DbString, OdbcCommand, ref NID))
                                            {
                                                NoteId = NID.ToString();
                                            }
                                            else
                                            {
                                                Utility.WriteToErrorLogFromAll("Err_Save PatientSMSLog to EagleSoft. Patient ID: " + drRow["PatientEHRId"].ToString() + ". System.Erorr : " + exAuth.Message);
                                            }
                                        }
                                        drRow["LogEHRId"] = NoteId.ToString();
                                        drRow["Log_Status"] = "completed";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToErrorLogFromAll("Save_PatientSMSCallLog_LocalToEagelsoft Exception : " + ex.Message);
                            }
                        }
                    }

                    if (dtResultCopy.Rows.Count > 0)
                    {
                        if (dtResultCopy.Select("LogType = 0").Count() > 0)
                        {
                            SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtResultCopy.Select("LogType = 0").CopyToDataTable(), "completed", ServiceInstallationId.Trim(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }
                        if (dtResultCopy.Select("LogType = 1").Count() > 0)
                        {
                            SynchLocalDAL.CallPatientFollowUpStatusCompleted(dtResultCopy.Select("LogType = 1").CopyToDataTable(), "completed", ServiceInstallationId.Trim(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }
                    }
                }
                return NoteId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable GetEaglesoftPatientStatusData(string clinicNumber, string DbString, string strPatID = "")
        {
            try
            {
                string OdbcSelect = SynchEagleSoftQRY.GetPatientStatus;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = SynchEagleSoftQRY.GetPatientStatusByPatID;
                    OdbcSelect = OdbcSelect.Replace("@Patient_EHR_ID", strPatID);
                }
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEaglesoftPatientImagesData(string clinicNumber, string DbString)
        {
            try
            {
                string OdbcSelect = SynchEagleSoftQRY.GetEaglesoftPatientImagesData;
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetDuplicateRecords(string DbString, string ServiceInstalltionId)
        {
            try
            {
                string OdbcSelect = SynchEagleSoftQRY.GetDuplicatePatientLog;
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteDuplicatePatientLog(string DbString, string ServiceInstalltionId)
        {
            //  bool is_Record_Update = false;
            try
            {
                string NoteId = "";

                string sqlSelect = string.Empty;
                DataTable dtDuplicateRecords = GetDuplicateRecords(DbString, ServiceInstalltionId);
                string OdbcSelect = "";
                Utility.WriteToSyncLogFile_All("Total records to be deleted");
                foreach (DataRow drRow in dtDuplicateRecords.Rows)
                {
                    OdbcSelect = "";
                    try
                    {
                        Utility.WriteToSyncLogFile_All("Start " + drRow["text"].ToString().Replace("'", "''"));

                        if (drRow["Mobile"].ToString() == "")
                        {
                            OdbcSelect = SynchEagleSoftQRY.DeletePatientLogBlankMobile;
                        }
                        else
                        {
                            OdbcSelect = SynchEagleSoftQRY.DeletePatientLogMobileExists;
                            OdbcSelect = OdbcSelect.Replace("@NoteId", drRow["NoteId"].ToString());
                        }
                        //Change by Dipika - 21-Jun-2023 (Start)
                        //OdbcSelect = OdbcSelect.Replace("@PatientEHRId", drRow["PatientEHRId"].ToString());
                        OdbcSelect = OdbcSelect.Replace("@PatientEHRId", "'" + drRow["PatientEHRId"].ToString() + "'");
                        //Change by Dipika - 21-Jun-2023 (End)

                        OdbcSelect = OdbcSelect.Replace("@note_type", "'G'");
                        OdbcSelect = OdbcSelect.Replace("@notetypeid", "7");
                        OdbcSelect = OdbcSelect.Replace("@description", "'" + drRow["app_alias"].ToString() + "'");
                        //OdbcSelect = OdbcSelect.Replace("@date_entered","'"+   Convert.ToDateTime(  drRow["PatientSMSCallLogDate"].ToString()).ToString("yyyyMMdd HH:MM") + "'");
                        OdbcSelect = OdbcSelect.Replace("@date_entered", "'" + Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString()).ToString("yyyy/MM/dd HH:mm:ss") + "'");

                        OdbcSelect = OdbcSelect.Replace("@Note", "'" + drRow["text"].ToString().Replace("'", "''") + "'");
                        using (OdbcConnection conn = new OdbcConnection(DbString))
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandTimeout = 200;
                                OdbcCommand.CommandType = CommandType.Text;
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                OdbcCommand.ExecuteNonQuery();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Length >= 100)
                        {
                            Utility.WriteToSyncLogFile_All("Err_ " + ex.Message.ToString() + "_ " + drRow["text"].ToString().Replace("'", "''"));
                        }
                        else
                        {
                            Utility.WriteToSyncLogFile_All("Err_ " + ex.Message.ToString() + "_ " + drRow["text"].ToString().Replace("'", "''"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Database Functions
        public static bool ExecuteNonQuery(string pstrConnString, OdbcCommand OdbcCommand)
        {
            try
            {
                using (OdbcConnection conn = new OdbcConnection(pstrConnString))
                {
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Connection = conn;
                    if (conn.State != ConnectionState.Open) conn.Open();
                    OdbcCommand.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (!ex.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw ex;
                    using (OdbcConnection conn2 = new OdbcConnection(pstrConnString))
                    {
                        using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn2))
                        {
                            odc.CommandTimeout = 200;
                            odc.CommandType = CommandType.Text;
                            odc.Parameters.Clear();
                            if (conn2.State == ConnectionState.Closed) conn2.Open();
                            using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                            {
                                OdbcDa.UpdateCommand = odc;
                                OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                OdbcDa.UpdateCommand.Connection = conn2;
                                OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                OdbcDa.UpdateCommand.ExecuteNonQuery();
                            }
                        }
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Connection = conn2;
                        if (conn2.State != ConnectionState.Open) conn2.Open();
                        OdbcCommand.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception exAuth)
                {
                    throw exAuth;
                }
            }
        }

        public static bool ExecuteScalar(string pstrConnString, OdbcCommand odbcCommand, ref object retValue)
        {
            try
            {
                using (OdbcConnection conn = new OdbcConnection(pstrConnString))
                {
                    odbcCommand.CommandType = CommandType.Text;
                    odbcCommand.Connection = conn;
                    if (conn.State != ConnectionState.Open) conn.Open();
                    retValue = (object)odbcCommand.ExecuteScalar();
                }
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (!ex.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw ex;
                    using (OdbcConnection conn2 = new OdbcConnection(pstrConnString))
                    {
                        using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn2))
                        {
                            odc.CommandTimeout = 200;
                            odc.CommandType = CommandType.Text;
                            odc.Parameters.Clear();
                            if (conn2.State == ConnectionState.Closed) conn2.Open();
                            using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                            {
                                OdbcDa.UpdateCommand = odc;
                                OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                OdbcDa.UpdateCommand.Connection = conn2;
                                OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                OdbcDa.UpdateCommand.ExecuteNonQuery();
                            }
                        }
                        odbcCommand.CommandType = CommandType.Text;
                        odbcCommand.Connection = conn2;
                        if (conn2.State != ConnectionState.Open) conn2.Open();
                        retValue = (object)odbcCommand.ExecuteScalar();
                    }
                    return true;
                }
                catch (Exception exAuth)
                {
                    retValue = null;
                    throw exAuth;
                }
            }
        }

        public static bool ExecuteReader(string pstrConnString, OdbcCommand odbcCommand)
        {
            try
            {
                using (OdbcConnection conn = new OdbcConnection(pstrConnString))
                {
                    odbcCommand.CommandType = CommandType.Text;
                    odbcCommand.Connection = conn;
                    if (conn.State != ConnectionState.Open) conn.Open();
                    odbcCommand.ExecuteReader();
                }
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (!ex.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw ex;
                    using (OdbcConnection conn2 = new OdbcConnection(pstrConnString))
                    {
                        using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn2))
                        {
                            odc.CommandTimeout = 200;
                            odc.CommandType = CommandType.Text;
                            odc.Parameters.Clear();
                            if (conn2.State == ConnectionState.Closed) conn2.Open();
                            using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                            {
                                OdbcDa.UpdateCommand = odc;
                                OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                OdbcDa.UpdateCommand.Connection = conn2;
                                OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                OdbcDa.UpdateCommand.ExecuteNonQuery();
                            }
                        }
                        odbcCommand.CommandType = CommandType.Text;
                        odbcCommand.Connection = conn2;
                        if (conn2.State != ConnectionState.Open) conn2.Open();
                        odbcCommand.ExecuteReader();
                    }
                    return true;
                }
                catch (Exception exAuth)
                {
                    throw exAuth;
                }
            }
        }
        public static bool ExecuteSql(string pstrConnString, OdbcCommand odbcCommand, ref DataTable dt)
        {
            try
            {
                using (OdbcConnection conn = new OdbcConnection(pstrConnString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    odbcCommand.Connection = conn;
                    odbcCommand.CommandTimeout = 200;
                    odbcCommand.CommandType = CommandType.Text;
                    using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(odbcCommand))
                    {
                        OdbcDa.Fill(dt);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (!ex.Message.ToString().ToUpper().Contains(("Authentication Violation").ToUpper())) throw ex;
                    using (OdbcConnection conn2 = new OdbcConnection(pstrConnString))
                    {
                        using (OdbcCommand odc = new OdbcCommand("SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'", conn2))
                        {
                            odc.CommandTimeout = 200;
                            odc.CommandType = CommandType.Text;
                            odc.Parameters.Clear();
                            if (conn2.State == ConnectionState.Closed) conn2.Open();
                            using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter())
                            {
                                OdbcDa.UpdateCommand = odc;
                                OdbcDa.UpdateCommand.CommandText = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION='Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407'";
                                OdbcDa.UpdateCommand.Connection = conn2;
                                OdbcDa.UpdateCommand.CommandTimeout = 2147483647;
                                OdbcDa.UpdateCommand.ExecuteNonQuery();
                            }
                        }
                        if (conn2.State == ConnectionState.Closed) conn2.Open();
                        odbcCommand.Connection = conn2;
                        odbcCommand.CommandTimeout = 200;
                        odbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(odbcCommand))
                        {
                            OdbcDa.Fill(dt);
                        }
                    }
                    return true;
                }
                catch (Exception exAuth)
                {
                    throw exAuth;
                }
            }
        }


        public static bool SaveMedicationToEaglesoft(string DbString, string Service_Install_Id, ref bool isRecordSaved, ref string Save_Patient_EHR_ids, string strPatientFormID = "")
        {
            OdbcConnection conn = new OdbcConnection(DbString);
            OdbcCommand OdbcCommand = new OdbcCommand();
            Int64 MedicationPatientId = 0;
            Int64 MedicationNum = 0;
            string odbcSelect = "";
            Save_Patient_EHR_ids = "";
            DataTable dtMedication;
            DataTable dtPatientMedication;
            try
            {


                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationResponseToSaveINEHR(Service_Install_Id, strPatientFormID);
                if (dtPatientMedicationResponse != null)
                {
                    if (dtPatientMedicationResponse.Rows.Count > 0)
                    {
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = GetEaglesoftUserLogin_ID(DbString);
                        }
                        dtMedication = GetEagleSoftMedicationData(DbString);
                        dtPatientMedication = GetEagleSoftPatientMedicationData(DbString, "");

                        foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                        {
                            MedicationNum = 0;
                            MedicationPatientId = 0;
                            if (dr["Medication_EHR_Id"] == DBNull.Value) dr["Medication_EHR_Id"] = "";
                            if (dr["Medication_EHR_Id"].ToString().Trim() == "" || dr["Medication_EHR_Id"].ToString().Trim() == "0")
                            {
                                object objMedNum = null;
                                //odbcSelect = SynchEagleSoftQRY.GetMedication;
                                //odbcSelect = odbcSelect.Replace("@MedicationName", dr["Medication_Name"].ToString().Trim());
                                //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                //ExecuteScalar(DbString, OdbcCommand, ref objMedNum);
                                //if (objMedNum != null)
                                //{
                                //    MedicationNum = Convert.ToInt64(objMedNum);
                                //}

                                DataRow[] drMedRows = dtMedication.Copy().Select("Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "'");

                                if (drMedRows.Length > 0)
                                {
                                    MedicationNum = Convert.ToInt64(drMedRows[0]["Medication_Parent_EHR_ID"].ToString().Trim());
                                }

                                if (MedicationNum <= 0)
                                {
                                    Utility.WriteToErrorLogFromAll("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());
                                    continue;

                                    //throw new Exception("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());

                                    //odbcSelect = SynchEagleSoftQRY.InsertMedication;
                                    //odbcSelect = odbcSelect.Replace("@MedicationName", dr["Medication_Name"].ToString().Trim());
                                    //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    //ExecuteScalar(DbString, OdbcCommand, ref objMedNum);
                                    //MedicationNum = Convert.ToInt64(objMedNum);

                                    //DataRow newMedRow = dtMedication.NewRow();
                                    //newMedRow["Medication_EHR_ID"] = MedicationNum.ToString();
                                    //newMedRow["Medication_Name"] = dr["Medication_Name"].ToString().Trim();
                                    //newMedRow["Medication_Description"] = "";
                                    //newMedRow["Medication_Notes"] = dr["Medication_Note"].ToString().Trim();
                                    //newMedRow["Medication_Sig"] = "";
                                    //newMedRow["Medication_Parent_EHR_ID"] = "";
                                    //newMedRow["Medication_Type"] = "";
                                    //newMedRow["Allow_Generic_Sub"] = "";
                                    //newMedRow["Drug_Quantity"] = "";
                                    //newMedRow["Refills"] = "";
                                    //newMedRow["Is_Active"] = "";
                                    //newMedRow["EHR_Entry_DateTime"] = "";
                                    //newMedRow["Medication_Provider_ID"] = "";
                                    //newMedRow["is_deleted"] = "";
                                    //newMedRow["Is_Adit_Updated"] = "";
                                    //newMedRow["Clinic_Number"] = "";
                                    //dtMedication.Rows.Add(dtMedication);
                                }
                                dr["Medication_EHR_Id"] = MedicationNum.ToString();
                            }
                            else
                            {
                                MedicationNum = Convert.ToInt64(dr["Medication_EHR_Id"].ToString().Trim());
                            }

                            string EHRUserLoginID = Utility.EHR_UserLogin_ID;
                            if (dr.Table.Columns.Contains("Clinic_Number"))
                            {
                                if (dr["Clinic_Number"].ToString().Trim() != "" && dr["Clinic_Number"].ToString().Trim() != "0")
                                {
                                    DataRow[] drUserRows = Utility.dtLocationWiseUser.Copy().Select("ClinicNumber = " + dr["Clinic_Number"].ToString().Trim());
                                    if (drUserRows != null)
                                    {
                                        if (drUserRows.Length > 0)
                                        {
                                            if (drUserRows[0]["EHR_UserLogin_ID"] != DBNull.Value)
                                            {
                                                if (drUserRows[0]["EHR_UserLogin_ID"].ToString().Trim() != "")
                                                {
                                                    EHRUserLoginID = drUserRows[0]["EHR_UserLogin_ID"].ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            object objPatMedNum = null;

                            if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "0";
                            if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" && dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                            {
                                MedicationPatientId = Convert.ToInt64(dr["PatientMedication_EHR_Id"]);
                            }
                            if (MedicationPatientId <= 0)
                            {
                                string strSelect = "Patient_EHR_ID = '" + dr["PatientEHRID"].ToString().Trim() +
                                            "' And Medication_EHR_ID = '" + MedicationNum + "' And is_active='True' ";

                                DataRow[] drPatMedRow = dtPatientMedication.Copy().Select(strSelect);
                                if (drPatMedRow.Length > 0)
                                {
                                    MedicationPatientId = Convert.ToInt64(drPatMedRow[0]["PatientMedication_EHR_ID"].ToString().Trim());
                                }
                            }



                            if (MedicationPatientId <= 0)
                            {
                                odbcSelect = SynchEagleSoftQRY.InsertPatientMedication;
                                odbcSelect = odbcSelect.Replace("@MedicationNum", MedicationNum.ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@Patient_EHR_ID", dr["PatientEHRId"].ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@Medication_Note", dr["Medication_Note"].ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@Provider_EHR_ID", "(Select preferred_dentist from patient where patient_id = " + dr["PatientEHRId"].ToString().Trim() + ")");
                                odbcSelect = odbcSelect.Replace("@Entered_By", EHRUserLoginID);
                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                ExecuteScalar(DbString, OdbcCommand, ref objPatMedNum);
                                MedicationPatientId = Convert.ToInt64(objPatMedNum);
                                dr["PatientMedication_EHR_ID"] = MedicationPatientId.ToString().Trim();

                                DataRow newPatMedRow = dtPatientMedication.NewRow();
                                newPatMedRow["Patient_EHR_ID"] = dr["PatientEHRID"].ToString();
                                newPatMedRow["Medication_EHR_ID"] = MedicationNum.ToString();
                                newPatMedRow["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                                newPatMedRow["Drug_Quantity"] = "";
                                newPatMedRow["Medication_Name"] = dr["Medication_Name"].ToString();
                                newPatMedRow["Medication_Type"] = "";
                                newPatMedRow["Medication_Note"] = dr["Medication_Note"].ToString();
                                newPatMedRow["Provider_EHR_ID"] = "";
                                newPatMedRow["Start_Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                newPatMedRow["End_Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                newPatMedRow["EHR_Entry_DateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                newPatMedRow["is_deleted"] = "0";
                                newPatMedRow["Clinic_Number"] = "0";
                                newPatMedRow["is_active"] = "True";

                                dtPatientMedication.Rows.Add(newPatMedRow);
                                dtPatientMedication.AcceptChanges();
                            }
                            else
                            {
                                odbcSelect = SynchEagleSoftQRY.UpdatePatientMedicationNotes;
                                odbcSelect = odbcSelect.Replace("@Medication_Note", dr["Medication_Note"].ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", MedicationPatientId.ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@MedicationNum", MedicationNum.ToString().Trim());
                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                ExecuteNonQuery(DbString, OdbcCommand);
                            }
                            if (!Save_Patient_EHR_ids.Contains("'" + dr["PatientEHRID"].ToString() + "'"))
                            {
                                Save_Patient_EHR_ids = Save_Patient_EHR_ids + "'" + dr["PatientEHRID"].ToString() + "',";
                            }
                            SynchLocalDAL.UpdateMedicationEHR_Updateflg(dr["MedicationResponse_Local_ID"].ToString(), MedicationPatientId.ToString(), dr["PatientEHRID"].ToString(), dr["Medication_EHR_Id"].ToString(), Service_Install_Id);
                        }
                        isRecordSaved = true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool DeleteMedicationToEaglesoft(string DbString, string Service_Install_Id, ref bool isRecordsDeleted, ref string Delete_Patient_EHR_ids, string strPatientFormID = "")
        {
            OdbcConnection conn = new OdbcConnection(DbString);
            OdbcCommand OdbcCommand = new OdbcCommand();
            string odbcSelect = "";
            Delete_Patient_EHR_ids = "";
            try
            {
                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationRemovedResponseToSaveINEHR("1", strPatientFormID);
                if (dtPatientMedicationResponse != null)
                {
                    foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                    {
                        if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "";

                        if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" || dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                        {
                            odbcSelect = SynchEagleSoftQRY.DeletePatientMedication;
                            odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", dr["PatientMedication_EHR_Id"].ToString().Trim());
                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            ExecuteNonQuery(DbString, OdbcCommand);
                            if (!Delete_Patient_EHR_ids.Contains("'" + dr["PatientEHRID"].ToString() + "'"))
                            {
                                Delete_Patient_EHR_ids = Delete_Patient_EHR_ids + "'" + dr["PatientEHRID"].ToString() + "',";
                            }
                            SynchLocalDAL.UpdateRemovedMedicationEHR_Updateflg(dr["MedicationRemovedResponse_Local_ID"].ToString(), dr["PatientMedication_EHR_Id"].ToString(), dr["PatientEHRID"].ToString(), Service_Install_Id);
                        }
                    }
                    isRecordsDeleted = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        #endregion

        #endregion

        #region Encryption
        public static bool DecryptSSN(ref System.Data.DataTable dtData)
        {
            try
            {
                var DLL = Assembly.LoadFile(System.IO.Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ES_Helper", "EaglesoftSettings.dll"));
                foreach (Type type in DLL.GetExportedTypes())
                {
                    if (type.Name == "EaglesoftSettings")
                    {
                        dynamic settings = Activator.CreateInstance(type);
                        settings.DecryptSSN(ref dtData);
                        break;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Error in DecryptSSN: " + ex.Message);
                return false;
                //throw ex;
            }
        }

        private static string EncryptSSN(string plainText)
        {
            string encrypt = "";
            try
            {
                var DLL = Assembly.LoadFile(System.IO.Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ES_Helper", "EaglesoftSettings.dll"));
                foreach (Type type in DLL.GetExportedTypes())
                {
                    if (type.Name == "EaglesoftSettings")
                    {
                        dynamic settings = Activator.CreateInstance(type);
                        encrypt = settings.Encrypt(plainText);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return encrypt;
        }
        #endregion

        //rooja add Insurance Master
        #region Insurance
        public static DataTable GetEaglesoftInsuranceData(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(DbString))
                {
                    string OdbcSelect = SynchEagleSoftQRY.GetEaglesoftInsuranceData;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                        // OdbcDt = SetHolidayDateForLaborDay(OdbcDt, DbString);
                    }
                }

                //rooja 20-8-24
               
                DataTable Dttmp = OdbcDt.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == "1")
                    {
                        for (int j = 0; j < OdbcDt.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = OdbcDt.Rows[j].ItemArray;
                            Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                            Dttmp.Rows.Add(Dr);
                        }
                    }
                }
                return Dttmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool Save_Insurance_Eaglesoft_To_Local(DataTable dtEaglesoftInsurance, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtEaglesoftInsurance.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Insurance;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Insurance;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Insurance;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Insurance_EHR_ID", dr["insurance_company_id"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Name", dr["name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address", dr["address_1"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address2", dr["address_2"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("City", dr["city"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("State", dr["state"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["zipcode"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Phone", dr["phone1"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ElectId", dr["neic_payer_id"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EmployerName", "");
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr["status"].ToString().Trim() == "A" ? false : true);
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }
        #endregion
    }
}


