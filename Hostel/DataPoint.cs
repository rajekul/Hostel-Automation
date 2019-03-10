using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Hostel
{
    
    [DataContract]
    public class DataPoint
    {
        public DataPoint(double y, string label)
        {
            this.Y = y;
            this.Label = label;
        }


        [DataMember(Name = "label")]
        public string Label = null;


        [DataMember(Name = "y")]
        public Nullable<double> Y = null;


    }
}