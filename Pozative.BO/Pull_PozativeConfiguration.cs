using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Data
    {
        public string _id { get; set; }
        public string _type { get; set; }
        public long created_at { get; set; }
        public object deleted_at { get; set; }
        public bool is_active { get; set; }
        public List<Push_LocationBO> location { get; set; }
        public List<Push_OrganizationBO> organization { get; set; }
        public List<Push_ServiceInstallationBO> service_installation { get; set; }
        public long updated_at { get; set; }
    }

    public class PozativeConfigurationRoot
    {
        public string message { get; set; }
        public Data data { get; set; }
        public bool status { get; set; }
    }

    public class Pull_Configuration
    {
        //Pull_Configuration(string lid,string oid,string sid)
        //{
        //    locationId = lid;
        //    organizationId = oid;
        //    installationId = sid;
        //}
        public string locationId { get; set; }
        public string organizationId { get; set; }
        public string installationId { get; set; }
    }

    public class Pull_LocationBO
    {
        public string AditLocationSyncEnable { get; set; }
        public string AditSync { get; set; }
        public string ApptAutoBook { get; set; }
        public string Clinic_Number { get; set; }
        public string Loc_ID { get; set; }
        public string Location_ID { get; set; }
        public string Organization_ID { get; set; }
        public string Service_Install_Id { get; set; }
        public string User_ID { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string google_address { get; set; }
        public string language { get; set; }
        public string location_numbers { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public string phone { get; set; }
        public string website_url { get; set; }
    }

    public class Pull_OrganizationBO
    {
        public string Adit_User_Email_ID { get; set; }
        public string Adit_User_Email_Password { get; set; }
        public string Name { get; set; }
        public string Organization_ID { get; set; }
        public string address { get; set; }
        public string currency { get; set; }
        public string email { get; set; }
        public string info { get; set; }
        public string is_active { get; set; }
        public string owner { get; set; }
        public string phone { get; set; }
    }

    public class Pull_ServiceInstallationBO
    {
        public string AditSync { get; set; }
        public string AppIdleStartTime { get; set; }
        public string AppIdleStopTime { get; set; }
        public string ApplicationIdleTimeOff { get; set; }
        public string ApplicationInstalledTime { get; set; }
        public string Application_Name { get; set; }
        public string Application_Version { get; set; }
        public string ApptAutoBook { get; set; }
        public string DBConnString { get; set; }
        public string Database { get; set; }
        public string DentrixPDFConstring { get; set; }
        public string DentrixPDFPassword { get; set; }
        public string Document_Path { get; set; }
        public string DontAskPasswordOnSaveSetting { get; set; }
        public string EHR_Sub_Version { get; set; }
        public string EHR_VersionNumber { get; set; }
        public string Hostname { get; set; }
        public string IS_Install { get; set; }
        public string Installation_Date { get; set; }
        public string Installation_ID { get; set; }
        public string Installation_Modify_Date { get; set; }
        public string IntegrationKey { get; set; }
        public string Location_ID { get; set; }
        public string NotAllowToChangeSystemDateFormat { get; set; }
        public string Organization_ID { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string PozativeEmail { get; set; }
        public string PozativeLocationID { get; set; }
        public string PozativeLocationName { get; set; }
        public string PozativeSync { get; set; }
        public string System_Name { get; set; }
        public string System_processorID { get; set; }
        public string UserId { get; set; }
        public string WebAdminUserToken { get; set; }
        public string Windows_Service_Version { get; set; }
        public string timezone { get; set; }
    }
}
