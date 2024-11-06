using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_PatientFollowUp
    {

        public bool status { get; set; }
        public string message { get; set; }
        public object error { get; set; }
        public List<Datum> data { get; set; }
        public int totalLength { get; set; }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Followup
        {
            //[JsonPropertyAttribute("$ref")]
            public string _ref { get; set; }
            public string _type { get; set; }
        }

        public class Location
        {
           // [JsonProperty("$ref")]
            public string _ref { get; set; }
            public string _type { get; set; }
        }

        public class Organization
        {
           // [JsonProperty("$ref")]
            public string _ref { get; set; }
            public string _type { get; set; }
        }

        public class Patient
        {
           // [JsonProperty("$ref")]
            public string _ref { get; set; }
            public string _type { get; set; }
        }

        public class Datum
        {
            public string _id { get; set; }
            public string _type { get; set; }
            public string activitytype { get; set; }
            public object created_at { get; set; }
            public object deleted_at { get; set; }
            public Followup followup { get; set; }
            public bool is_active { get; set; }
            public Location location { get; set; }
            public Organization organization { get; set; }
            public Patient patient { get; set; }
            public string subtype { get; set; }
            public string title { get; set; }
            public object updated_at { get; set; }
            public string message { get; set; }
        }      

    }
}
