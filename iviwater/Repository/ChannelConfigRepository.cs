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
    public class ChannelConfigRepository: RepositoryConfig
    {
        private PMAC_Controller _pmac;
        private string channel_id;
        IMongoCollection<t_Channel_Configurations> collection;
        public ChannelConfigRepository(string channel_id, PMAC_Controller pmac)
        {
            this.channel_id = channel_id;
            this._pmac = pmac;
            this.collection = database.GetCollection<t_Channel_Configurations>(Common.Constant.t_Channel_Configurations);
        }

        public ChannelConfigRepository()
        {
            this.collection = database.GetCollection<t_Channel_Configurations>(Common.Constant.t_Channel_Configurations);
        }

        public bool UpdateChannelConfig()
        {
            try
            {
                var filter = Builders<t_Channel_Configurations>.Filter.Eq("ChannelId", channel_id);
                var update = Builders<t_Channel_Configurations>.Update
                    .Set("TimeStamp", _pmac.last_time)
                    .Set("LastValue", _pmac.last_value)
                    .Set("IndexTimeStamp", _pmac.last_time_index)
                    .Set("LastIndex", _pmac.last_value_index);
                collection.UpdateOne(filter, update);
                return true;
            }
            catch(Exception ex)
            {
                _log.WriteLog(ex.ToString(), "error update channel config", true);
                return false;
            }
        }

        public List<string> GetChannelIds()
        {
            return collection.AsQueryable().Where(x => x.ChannelId[0] != '7').Select(x => x.ChannelId).ToList();
        }
    }
}
