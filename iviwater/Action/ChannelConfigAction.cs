using MongoDB.Driver;
using SyncPrimayerToMongoDB.ConnectDB;
using SyncPrimayerToMongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncPrimayerToMongoDB.Action
{
    public class ChannelConfigAction
    {

        public void UpdateChannel(DateTime timestamp , double value, string channelid )
        {
            Connect connect = new Connect();

            var collection = connect.db.GetCollection<ChannelConfigModel>("t_Channel_Configurations");


            var filter = Builders<ChannelConfigModel>.Filter.Eq("ChannelId", channelid);

            var update = Builders<ChannelConfigModel>.Update.Set("TimeStamp", timestamp)
                .Set("LastValue", value);

            collection.UpdateOne(filter, update);

        }


        public void UpdateChannelIndex(DateTime indexTimeStamp, double indexValue, string channelid )
        {
            Connect connect = new Connect();

            var collection = connect.db.GetCollection<ChannelConfigModel>("t_Channel_Configurations");


            var filter = Builders<ChannelConfigModel>.Filter.Eq("ChannelId", channelid);

            var update = Builders<ChannelConfigModel>.Update
                .Set("IndexTimeStamp", indexTimeStamp)
                .Set("LastIndex", indexValue);

            collection.UpdateOne(filter, update);
        }
    }
}
