using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_iPMAC.Common
{
    public class Constant
    {
        //Connection String
        // public const string connectionString = "mongodb://admin:1234@localhost:27017/admin";
        public const string connectionString = "mongodb://localhost:27017";
        public const string database = "node_viwater";
        //Tables
        public const string t_Units = "t_Units";
        public const string t_SysParam = "t_SysParam";
        public const string t_Users = "t_Users";
        public const string t_Consumers = "t_Consumers";
        public const string t_Staffs = "t_Staffs";
        public const string t_Roles = "t_Roles";
        public const string t_Functions = "t_Functions";
        public const string t_Language = "t_Language";
        public const string t_LanguageTranslate = "t_LanguageTranslate";
        public const string t_DisplayGroups = "t_DisplayGroups";
        public const string t_Device_Status = "t_Device_Status";
        public const string t_Site_Availabilities = "t_Site_Availabilities";
        public const string t_Site_Status = "t_Site_Status";
        public const string t_Accreditation_Types = "t_Accreditation_Types";
        public const string t_Meters = "t_Meters";
        public const string t_Transmitters = "t_Transmitters";
        public const string t_Loggers = "t_Loggers";
        public const string t_Meter_Histories = "t_Meter_Histories";
        public const string t_Transmitter_Histories = "t_Transmitter_Histories";
        public const string t_Logger_Histories = "t_Logger_Histories";
        public const string t_Channel_Configurations = "t_Channel_Configurations";
        public const string t_Logger_Configurations = "t_Logger_Configurations";
        public const string t_Sites = "t_Sites";
        public const string t_Zone = "t_Zone";
        public const string t_DMA = "t_DMA";
        //Table prefix
        public const string t_dt_logger = "t_Data_Logger_";
        public const string t_dt_index = "t_Index_Logger_";
    }
}
