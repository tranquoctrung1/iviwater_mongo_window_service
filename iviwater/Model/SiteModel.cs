using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncPrimayerToMongoDB.Models
{
    [BsonIgnoreExtraElements]
    public class SiteModel
    {
        public ObjectId Id { get; set; }
        public string SiteId { get; set; }
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string DisplayGroup { get; set; }
        public string LoggerId { get; set; }
        public Nullable<double> StartDay { get; set; }
        public Nullable<double> StartHour { get; set; }
        public string PipeSize { get; set; }
        public Nullable<double> InterVal { get; set; }
        public string Available { get; set; }
        public Nullable<double> TimeDelay { get; set; }
        public string Note { get; set; }

        public string ConsumerId { get; set; }

        public string Status { get; set; }

        public Nullable<bool> IsPrimayer { get; set; }
    }
}
