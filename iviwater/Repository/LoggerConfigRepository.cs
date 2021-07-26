using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF_iPMAC.Controller;
using MongoDB.Driver;
using MongoDB.Bson;
using WF_iPMAC.Model;

namespace WF_iPMAC.Repository
{
    public class LoggerConfigRepository: RepositoryConfig
    {
        private PMAC_Controller _pmac;
        private string logger_id;
        IMongoCollection<t_Logger_Configurations> collection;
        public LoggerConfigRepository(string channel_id, PMAC_Controller pmac)
        {
            this.logger_id = channel_id.Split('_')[0];
            this._pmac = pmac;
            this.collection = database.GetCollection<t_Logger_Configurations>(Common.Constant.t_Logger_Configurations);
        }

        public bool UpdateLoggerConfig()
        {
            try
            {
                var filter = Builders<t_Logger_Configurations>.Filter.Eq("LoggerId", logger_id);
                var update = Builders<t_Logger_Configurations>.Update
                    .Set("Interval", _pmac.interval);
                collection.UpdateOne(filter, update);
                return true;
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.ToString(), "error update logger config", true);
                return false;
            }
        }
    }
}
