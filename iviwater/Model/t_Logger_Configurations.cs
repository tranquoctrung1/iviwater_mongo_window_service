using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_iPMAC.Model
{
    [BsonIgnoreExtraElements]
    public class t_Logger_Configurations
    {
        public string LoggerId { get; set; }
        public string SiteId { get; set; }
        public Nullable<byte> StartHour { get; set; }
        public string TelephoneNumber { get; set; }
        public Nullable<byte> Pressure1 { get; set; }
        public Nullable<byte> Pressure2 { get; set; }
        public Nullable<byte> ForwardFlow { get; set; }
        public Nullable<byte> ReverseFlow { get; set; }
        public Nullable<bool> Alarm { get; set; }
        public Nullable<int> TimeDelayAlarm { get; set; }
        public Nullable<byte> Interval { get; set; }
        public Nullable<bool> Status1 { get; set; }
        public Nullable<bool> Status2 { get; set; }
        public Nullable<int> TimeOffset { get; set; }
        public Nullable<byte> ChannelOther { get; set; }
    }
}
