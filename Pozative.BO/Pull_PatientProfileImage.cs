using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{  
    //public class Pull_PatientProfileImage
    //{
    //    public string message { get; set; }
    //    public string code { get; set; }
    //    public string status { get; set; }
    //    public List<GetListPatientProfileImage> data { get; set; }

    //}
    //public class GetListPatientProfileImage
    //{
    //    public string patientId { get; set; }
    //}
    public class Push_PatientProfileImageRemove
    {
        public string location { get; set; }
        public string organization { get; set; }
        public string patientId { get; set; }
    }

    public class Push_CheckANDCreatePatientImageFolder
    {
        public string location { get; set; }
        public string organization { get; set; }
    }

    public class Push_PatientProfileAddImageName
    {
        public string location { get; set; }
        public string organization { get; set; }
        public string patientId { get; set; }
        public string profileimage { get; set; }
    }

    public class Pull_PatientProfileImageRemove
    {
        public string message { get; set; }
        public string code { get; set; }
        public string status { get; set; }
        public List<GetListPatientProfileImageRemove> data { get; set; }

    }
    public class GetListPatientProfileImageRemove
    {
        public string _id { get; set; }
    }


    public class Datum
    {
        public string originalname { get; set; }
    }

    public class Pull_PatientProfileImage
    {
        public string message { get; set; }
        public int code { get; set; }
        public List<Datum> data { get; set; }
        public bool status { get; set; }
    }



}
