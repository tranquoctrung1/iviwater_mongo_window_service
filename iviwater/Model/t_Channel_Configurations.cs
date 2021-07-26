using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_iPMAC.Model
{
    [BsonIgnoreExtraElements]
    public class t_Channel_Configurations
    {
        public string ChannelId { get; set; }
        public string LoggerId { get; set; }
        public string ChannelName { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Pressure1 { get; set; }
        public Nullable<bool> Pressure2 { get; set; }
        public Nullable<bool> ForwardFlow { get; set; }
        public Nullable<bool> ReverseFlow { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public Nullable<double> LastValue { get; set; }
        public Nullable<System.DateTime> IndexTimeStamp { get; set; }
        public Nullable<double> LastIndex { get; set; }
        public Nullable<bool> DisplayOnLabel { get; set; }
        public Nullable<bool> ChannelOther { get; set; }
        public Nullable<double> basemin { get; set; }
        public Nullable<double> basemax { get; set; }
    }
}
