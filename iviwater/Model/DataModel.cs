using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_iPMAC.Model
{
    [BsonIgnoreExtraElements]
    public class DataModel
    {
        public System.DateTime TimeStamp { get; set; }
        public Nullable<double> Value { get; set; }
    }
}
