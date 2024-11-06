using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pozative.BO
{
    public class SystemBO
    {
        public string System_Name { get; set; }
        public string System_processorID { get; set; }
        public bool IS_Install { get; set; }
    }
    public class MasterSyncBatchSize
    {
        public MasterSyncBatchSizeDetails data { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class MasterSyncBatchSizeDetails
    {
        public int AlertMaster { get; set; } = 100;
        public int Appointment { get; set; } = 200;
        public int Appointment_Multi_location { get; set; } = 200;
        public int Disease { get; set; } = 200;
        public int Form { get; set; } = 100;
        public int FormMaster { get; set; } = 100;
        public int FormQuestionMaster { get; set; } = 100;
        public int Holiday { get; set; } = 200;
        public int Medication { get; set; } = 200;
        public int OperatoryDayOff { get; set; } = 200;
        public int OperatoryEvent { get; set; } = 200;
        public int OperatoryHours { get; set; } = 200;
        public int OperatoryOfficeHours { get; set; } = 200;
        public int Patient { get; set; } = 1000;
        public int PatientBalance { get; set; } = 1000;
        public int PatientDisease { get; set; } = 200;
        public int PatientImage { get; set; } = 20;
        public int PatientMedication { get; set; } = 200;
        public int PatientStatus { get; set; } = 1000;
        public int Patient_Async { get; set; } = 1000;
        public int Patient_Multi_Location { get; set; } = 1000;
        public int Provider { get; set; } = 200;
        public int ProviderHours { get; set; } = 50;
        public int ProviderOfficeHours { get; set; } = 200;
        public int Question { get; set; } = 100;
        public int QuestionMaster { get; set; } = 100;
        public int SectionItemMaster { get; set; } = 100;
        public int SectionItemOptionMaster { get; set; } = 100;
        public int SectionMaster { get; set; } = 100;
        public int Sheet { get; set; } = 100;
        public int SheetField { get; set; } = 100;
        public string _id { get; set; }
        public string _type { get; set; }
    }
}
