using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pozative.DentrixDoc
{
    #region Enum
    public enum EDocumentFormat
    {
        Undefined = 0,
        Image = 1,
        PDF = 2,
        Word = 3,
        Excel = 4,
        PowerPoint = 5,
        Text = 6,
        RTF = 7,
        NeverUsed = 8,
        CrystalReports = 9,
        HTML = 10,
        XML = 11,
        EConsent = 12
    }
    public enum ECapturedObjectDataFormat
    {
        Undefined,
        DataFormatIsActualObject,
        DocumentFormatIsAFilePointer,
    }
    #endregion

    #region Class
    public class CapturedDocument
    {
        public CapturedDocument(
          object data,
          EDocumentFormat eDocFormat,
          ECapturedObjectDataFormat eDataFormat)
        {
            this.Data = data;
            this.EDocumentFormat = eDocFormat;
            this.ECapturedDataFormat = eDataFormat;
        }

        public object Data { get; set; }

        public ECapturedObjectDataFormat ECapturedDataFormat { get; set; }

        public EDocumentFormat EDocumentFormat { get; set; }
    }

    public class DentrixDocumentAttachement
    {       

        public bool CheckFileFormats(string[] fileNames)
        {
            bool flag = true;
            foreach (string fileName in fileNames)
            {
                if (GetDocumentFormat(fileName) == null)
                    flag = false;
            }
            return flag;
        }

        public  EDocumentFormat GetFormatFromExtension( string extension)
        {
            if (extension == null)
                return EDocumentFormat.Undefined;
            switch (extension.ToUpper())
            {
                case ".BMP":
                case ".DIB":
                case ".GIF":
                case ".JFIF":
                case ".JPE":
                case ".JPEG":
                case ".JPG":
                case ".PNG":
                case ".TIF":
                case ".TIFF":
                    return EDocumentFormat.Image;
                case ".DOC":
                case ".DOCX":
                    return EDocumentFormat.Word;
                case ".HTM":
                case ".HTML":
                    return EDocumentFormat.HTML;
                case ".PDF":
                    return EDocumentFormat.PDF;
                case ".PPT":
                case ".PPTX":
                    return EDocumentFormat.PowerPoint;
                case ".RTF":
                    return EDocumentFormat.RTF;
                case ".TXT":
                    return EDocumentFormat.Text;
                case ".XLS":
                case ".XLSX":
                    return EDocumentFormat.Excel;
                case ".XML":
                    return EDocumentFormat.XML;
                default:
                    return EDocumentFormat.Undefined;
            }
        }

        public  EDocumentFormat GetDocumentFormat(string fileName)
        {
            try
            {
                return GetFormatFromExtension(new FileInfo(fileName).Extension);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting file type from extension:" + ex.Message);
            }
            return (EDocumentFormat)0;
        }

        public List<CapturedDocument> GetFiles()
        {
            List<CapturedDocument> files = new List<CapturedDocument>();
            //if (this.CheckRightRestriction != EPermission.NoPasswordRequired || Password.CheckRights(this.CheckRightRestriction))
            //{
                Microsoft.Win32.RegistryKey keyDocFileName = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DentrixAttachmentDocumentPath");
                string documentPath = "";
                if (keyDocFileName != null)
                {
                    documentPath = keyDocFileName.GetValue("DentrixDocPath").ToString();
                }

                string[] fileNames = { "" };
                fileNames[0] = documentPath;//Glabal.Global.documentPath;
                //string[] fileNames = !this.RestrictAcqusitionToImageType ? DocumentImportDialog.GetFileList(EImportableFileTypes.ImageAndOleTypes, this.AllowMultipleSelection) : DocumentImportDialog.GetFileList(EImportableFileTypes.ImageFormats, this.AllowMultipleSelection);
                if (fileNames.Length != 0)
                {
                    if (this.CheckFileFormats(fileNames))
                    {
                        foreach (string str in fileNames)
                            files.Add(new CapturedDocument((object)str, GetDocumentFormat(str), ECapturedObjectDataFormat.DocumentFormatIsAFilePointer));
                    }
                    else if (fileNames.Length == 1)
                    {
                        //int num1 = (int)MessageBox.Show("The file \"" + fileNames[0] + "\" is in an unknown or unsupported file format\nThe document cannot be added.");
                    }
                    else
                    {
                       // int num2 = (int)MessageBox.Show("One or more selected files is in an unknown or unsupported file format\nThe documents cannot be added.");
                    }
                }
           // }
            return files;
        }

        private void OnDocumentsAcquired(string deviceName, List<CapturedDocument> acquiredDocuments)
        {
            //IDocumentAttachmentEntity attachToEnity = this.documentTreeControl1.ActiveEntity;
            //if (attachToEnity == null && this._filter.Mode == EFilterMode.BySingleEntity)
              //  attachToEnity = this._filter.DefaultEntity;
            //if (attachToEnity == null || !attachToEnity.IsValid)
            //{
            //    int newDocuments = (int)this._acquisitionManager.CreateNewDocuments(deviceName, acquiredDocuments, (IDocumentAttachmentEntity)null, this._filter.Manager);
            //}
            //else
            //{
            //    switch (this._acquisitionManager.CreateNewDocuments(deviceName, acquiredDocuments, attachToEnity, this._filter.Manager))
            //    {
            //        case EDocumentAcquisitionResult.Success:
            //            this.documentTreeControl1.SelectAttachment(this._acquisitionManager.MostRecentAttachment);
            //            LegacyFuncs.RefreshLegacyToolbar((EDentrixModule)3);
            //            break;
            //        case EDocumentAcquisitionResult.UserCanceled:
            //            break;
            //        default:
            //            int num = (int)MessageBox.Show("Error creating attachment", "Dentrix Dental Systems", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //            break;
            //    }
            //}
        }
    }
    #endregion
}
