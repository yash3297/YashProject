using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class PozativeConfigErrorLog
    {
        public string datetime { get; set; }
        public string dentrix_pdf_connection_string { get; set; }
        public bool ehr_connected { get; set; }
        public string email { get; set; }
        public bool error_log_file_generated { get; set; }
        public string error_message { get; set; }
        public string image_folder_path { get; set; }
        public string locationId { get; set; }
        public List<string> multiclinic_selected { get; set; }
        public bool multidatabase_configure { get; set; }
        public string organizationId { get; set; }
        public string password { get; set; }
        public string selected_ehr { get; set; }
        public bool sucessfully_configured { get; set; }
        public SystemConfiguration system_configuration { get; set; }
        public string total_installed_ehr { get; set; }
    }
    public class SystemConfiguration
    {
        public string cpu { get; set; }
        public string framework { get; set; }
        public string mac_address { get; set; }
        public string os { get; set; }
        public string ram { get; set; }
    }
    public class PozativeAudioText
    {
        public PozativeAudioTextList[] data { get; set; }
    }
    public class PozativeAudioTextList
    {
        public string key { get; set; }
        public string val { get; set; }
        
    }
    public class class1
    {
        public class2 msg { get; set; }

    }
    public class class2
    {
        public List<object> msg { get; set; }

    }
    public class MailData
    {
        public string to { get; set; }
        public string MailSubject { get; set; }
        public string MailTitle { get; set; }
        public string MailTitle2 { get; set; }
        public string Description { get; set; }
        public string heading1 { get; set; }
        public string OrganizationName { get; set; }
        public string LocationName { get; set; }
        public string OwnerName { get; set; }
        public string DateTimeDownloaded { get; set; }
        public bool DownloadInstallStatus { get; set; } = false;
        public bool AditAppLoginStatus { get; set; } = false;
        public string AditAppLoginStatusError { get; set; }
        public bool EHRConnectionStatus { get; set; } = false;
        public string EHRConnectedError { get; set; }
        public bool LocationConfigurationStatus { get; set; } = false;
        public string LocationConfigurationError { get; set; }
        public bool SystemLoginStatus { get; set; } = false;
        public string SystemLoginError { get; set; }
        public bool SystemAppConfigurationStatus { get; set; } = false;
        public string SystemAppConfigurationError { get; set; }
    }
    public class GeneralInfoDetails
    {
        public string ehrdefaultconfigurationmissing { get; set; }
        public string ehrnotexists { get; set; }
        public string errormessage { get; set; }
        public string multipleclinicsexists { get; set; }
        public string multipleehrexists { get; set; }
        public string success { get; set; }
        public string welcomemessage { get; set; }
        public string helppage { get; set; }
        public string support_email { get; set; }
    }
    public class GeneralInfo
    {
        public string message { get; set; }
        public int code { get; set; }
        public GeneralInfoDetails data { get; set; }
        public bool status { get; set; }
    }
}
