using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Pozative
{
    public class SendEMail
    {
        static GoalBase ObjGoalBase = new GoalBase();
        static string smtpAddress = "smtp.gmail.com";
        static int portNumber = 587;
        static bool enableSSL = true;
        static string emailFromAddress = "avani.panchal@adit.com";//"no-reply@adit.com"; //Sender Email Address  
        static string password = "Pranshi@7878"; //Sender Password  
        static string subject = "Server App Installation Status";
        static string body = "";
        public void SendEmail(string Organization, string Location, string Owner, string InstallationTime, string UserName)
        {
            try
            {
                //subject = subject + (CommonFunction.mailData.SystemAppConfigurationStatus ? " Success" : " Failed");
                //GetMailBodyContent(Organization, Location, Owner, InstallationTime, UserName);

                //AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, System.Net.Mime.MediaTypeNames.Text.Html);
                //LinkedResource pic1 = new LinkedResource(Application.StartupPath + "\\PozativeIOC.png", System.Net.Mime.MediaTypeNames.Image.Jpeg);
                //pic1.ContentId = "Pic1";
                //avHtml.LinkedResources.Add(pic1);

                //LinkedResource pic2 = new LinkedResource(Application.StartupPath + "\\AditText.png", System.Net.Mime.MediaTypeNames.Image.Jpeg);
                //pic2.ContentId = "Pic2";
                //avHtml.LinkedResources.Add(pic2);

                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress(emailFromAddress);
                //    mail.To.Add(CommonFunction.supportEmail);
                //    mail.Subject = subject;
                //    mail.Body = body;
                //    mail.IsBodyHtml = true;
                //    mail.AlternateViews.Add(avHtml);

                //    //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                //    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                //    {
                //        smtp.UseDefaultCredentials = false;
                //        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                //        smtp.EnableSsl = enableSSL;
                //        smtp.Send(mail);
                //    }
                //}
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("SendEmail - " + ex.Message);
            }
        }
        private void GetMailBodyContent(string Organization, string Location, string Owner, string InstallationTime, string UserName)
        {
            try
            {
                //string title2 = string.IsNullOrEmpty(Organization) && string.IsNullOrEmpty(Location) ? "" : Organization + " - " + Location;
                //string title1 = CommonFunction.mailData.SystemAppConfigurationStatus ? "Successful Server App Install for " : "Failed Server App Install for ";
                //string title3 = CommonFunction.mailData.SystemAppConfigurationStatus ? title2 + " had a successful self installation for their server app. "
                //                : (string.IsNullOrEmpty(title2) ? UserName + " user" : title2) + " recently attempted to self install their server app, however it was not successful. Please reach out to see how we can help finish their install.";

                //CommonFunction.mailData.MailSubject = subject;
                //CommonFunction.mailData.MailTitle = title1;
                //CommonFunction.mailData.Description = title3;
                //CommonFunction.mailData.OrganizationName = Organization;
                //CommonFunction.mailData.LocationName = Location;
                //CommonFunction.mailData.OwnerName = Owner;
                //CommonFunction.mailData.DateTimeDownloaded = InstallationTime;

                //body = @"<!DOCTYPE html>
                //        <html>
                //        <head>"+
                //#region Header
                //            @"<h2 style=""text-align:top-center;font-size: 28px;"">
                //                <table style='table-layout:auto;width:100%'>
                //                    <tr>
                //                        <th width=""40%""><img src=cid:Pic1  id='img' alt='' width='48px' height='48px' style=""padding-left: 85%""/></th> 
                //                        <th width=""60%"" align=""left"" style=""padding-bottom: 2%"">Server App Install</th> 
                //                    </tr>
                //                </table>
                //             <h2> 
                //             <table style='table-layout:auto;width:100%'>
                //                <tr style='height:5px'> <th bgcolor=""#092A3D""></th> <th bgcolor=""#25A8E0""></th> <th bgcolor=""#F28820""></th> </tr>
                //            </table>
                //            <style> tr.border-bottom td {border-bottom: 1pt dashed #E5E5E5;}
                //            tr.titleLine td {height:32px;text-align:center;font-size: 24px;}
                //            td.coltitle {padding-left: 20px;font-size: 16px;font-weight: 500;}
                //            td.colvalue {padding-right: 20px;font-size: 16px;font-weight: 500;text-align:right;}
                //            td.rowtitle {padding-left: 20px;font-size: 16px;font-weight: bold;width:100%;}
                //            td.successValue {padding-right: 20px;font-size: 16px;font-weight: 500;text-align:right;color:#00AF4B;}
                //            td.failedTitle {padding-left: 20px;padding-top: 15px;font-size: 16px;font-weight: 500;}
                //            td.failedvalue {padding-right: 20px;padding-top: 15px;font-size: 16px;font-weight: 500;text-align:right;color:#DA2128;}
                //            td.failedreason {padding-left: 20px;padding-bottom: 15px;font-size: 16px;font-weight: 400;text-align:top;}

                //            </style>
                //        </head>" +
                //#endregion
                //#region title
                //         @"<body>
                //        <div style=""border: thin solid #E5E5E5;border-radius:5px;"">
                //        <table style='table-layout:auto;width:100%'>
                //            <tr style='height:32px'></tr>"+
                //            @"<tr class=""titleLine"">" + title1 + "</tr>" +
                //            @"<tr class=""titleLine"">" + title2 + "</tr>" +
                //            "<tr style='height:25px'></tr>" +
                //            @"<tr style='height:20px'>
                //                <th style=""padding:20px;text-align:left;font-size: 16px;font-weight:400"">Hey Team,<br>" + title3 + "</th>" +
                //            "</tr>" +
                //#endregion

                //#region account details
                //         @"<tr style='height:60px;background-color:#F2F2F2;border-color:#CBCBCB'>
                //            <td style='padding-left: 20px;font-size: 16px;font-weight: bold;width:100%;' colspan=2>Account Details:</td>
                //            <td></td>
                //        </tr>

                //        <tr class=""border-bottom"" style='height:55px;'>
                //            <td class=""coltitle"">Org. Name</td>
                //            <td class=""colvalue"">" + Organization + "</td>" +
                //        "</tr>" +

                //         @"<tr class=""border-bottom"" style='height:55px;'>
                //            <td class=""coltitle"">Location Name</td>
                //            <td class=""colvalue"">" + Location + "</td>" +
                //        "</tr>" +

                //        @"<tr class=""border-bottom"" style='height:55px;'>
                //            <td class=""coltitle"">Owner Name</td>
                //            <td class=""colvalue"">" + Owner + "</td>" +
                //        "</tr>" +

                //        "<tr style='height:35px'></tr>" +

                //#endregion

                //#region Installation details
                //        @"<tr style='height:60px;background-color:#F2F2F2;border-color:#CBCBCB'>
                //            <td class=""rowtitle"" colspan =2>Install Details:</td>
                //            <td></td>
                //        </tr> " +

                //        //Date & Time Downloaded
                //        @"<tr class=""border-bottom"" style='height:55px;'>
                //            <td class=""coltitle"">Date & Time Downloaded</td>
                //            <td class=""successValue"">" + InstallationTime + "</td>" +
                //        "</tr>" +

                //         //Download/Install Status
                //         @"<tr class=""border-bottom"" style='height:55px;'>
                //            <td class=""coltitle"">Download/Install Status</td>
                //            <td style='padding-right: 20px;font-size: 16px;font-weight: 500;color: #00AF4B;text-align:right;'>" + (CommonFunction.mailData.DownloadInstallStatus ? "Success" : "Failed") + "</td>" +
                //        "</tr>";
                //#endregion

                //#region Adit App Login Status 
                //if (CommonFunction.mailData.AditAppLoginStatus)
                //{
                //    body += @"<tr class=""border-bottom"" style='height:55px;'>
                //           <td class=""coltitle"">Adit App Login Status </td>
                //           <td class=""successValue"">Success</td>
                //   </tr>";
                //}
                //else
                //{
                //    body += @"<tr style='height:40px;'>
                //            <td class=""failedTitle"">Adit App Login Status </td>
                //            <td class=""failedvalue"">Failed</td>
                //    </tr>
                //    <tr class=""border-bottom"" style='height:30px;'>
                //        <td class=""failedreason"" colspan:2>Reason: " + CommonFunction.mailData.AditAppLoginStatusError + " </td>" +
                //    "<td></td>" +
                //    "</tr>";
                //}
                //#endregion

                //#region EHR Connection Status 
                //if (CommonFunction.mailData.EHRConnectionStatus)
                //{
                //    body += @"<tr class=""border-bottom"" style='height:55px;'>
                //            <td class=""coltitle"">EHR Connection Status</td>
                //            <td class=""successValue"">Success</td>
                //   </tr>";
                //}
                //else
                //{
                //    body += @"<tr style='height:40px;'>
                //                <td class=""failedTitle"">EHR Connection Status</td>
                //                <td class=""failedvalue"">Failed</td>
                //    </tr>
                //    <tr class=""border-bottom"" style='height:30px;'>
                //        <td class=""failedreason"" colspan:2>Reason: " + CommonFunction.mailData.EHRConnectedError + " </td>" +
                //    "<td></td>" +
                //    "</tr>";
                //}
                //#endregion
                //#region Location Configuration Status 
                //if (CommonFunction.mailData.LocationConfigurationStatus)
                //{
                //    body += @"<tr class=""border-bottom"" style='height:55px;'>
                //                <td class=""coltitle"">Location Configuration Status </td>
                //                <td class=""successValue""Success</td>
                //   </tr>";
                //}
                //else
                //{
                //    body += @"<tr style='height:40px;'>
                //              <td class=""failedTitle"">Location Configuration Status</td>
                //              <td class=""failedvalue"">Failed</td>
                //     </tr>
                //     <tr class=""border-bottom"" style='height:30px;'>
                //                <td class=""failedreason"" colspan:2>Reason: " + CommonFunction.mailData.LocationConfigurationError + " </td>" +
                //              "<td></td>" +
                //    "</tr>";
                //}
                //#endregion
                //#region System Login Status
                //if (CommonFunction.mailData.SystemLoginStatus)
                //{
                //    body += @"<tr class=""border-bottom"" style='height:55px;'>
                //    <td class=""coltitle"">System Login Status</td
                //    <td class=""successValue"">Success</td>
                //    </tr>";
                //}
                //else
                //{
                //    body += @"<tr style='height:40px;'>
                //                    <td class=""failedTitle"">System Login Status</td>
                //                    <td class=""failedvalue"">Failed</td>
                //                    </tr>
                //                    <tr class=""border-bottom"" style='height:30px;'>
                //                    <td class=""failedreason"" colspan:2>Reason: " + CommonFunction.mailData.SystemLoginError + "</td>" +
                //                    "<td></td>" +
                //                    "</tr>";
                //}
                //#endregion
                //#region Server App Configuration Status
                //if (CommonFunction.mailData.SystemAppConfigurationStatus)
                //{
                //    body += @"<tr class=""border-bottom"" style='height:55px;'>
                //                <td class=""coltitle"">Server App Configuration Status</td>
                //                <td class=""successValue"">Success</td>
                //    </tr>";
                //}
                //else
                //{
                //    body += @"<tr style='height:40px;'>
                //                <td class=""failedTitle"">Server App Configuration Status</td>
                //                <td class=""failedvalue"">Failed</td>
                //        </tr>
                //        <tr class=""border-bottom"" style='height:30px;'>
                //            <td class=""failedreason"" colspan:2>Reason: " + CommonFunction.mailData.SystemAppConfigurationError + "</td>" +
                //            "<td></td>" +
                //        "</tr>";
                //} 
                //#endregion

                //#region footer
                //body += @"<tr style='height:100px'></tr>
                //         </table>
                //         </div>
                //         <table  style='table-layout:auto;width:100%'>
                //                <tr style='height:76px;'>
                //                    <td><img src=cid:Pic2  id='img2' alt=''/></td>
                //                    <td class=""colvalue"">Copyright © 2023 Adit. All Rights Reserved.</td>
                //                </tr>
                //         </table>
                //         </body>
                //         </html>"; 
                //#endregion

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("GetMailBodyContent - " + ex.Message);
            }
        }
    }
}
