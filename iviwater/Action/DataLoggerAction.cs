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
    public class DataLoggerAction
    {
        public int InsertDataLogger(string channelid, List<DataLoggerModel> datas)
        {
            int nRows = 0;

            Connect connect = new Connect();

            var collection = connect.db.GetCollection<DataLoggerModel>("t_Data_Logger_" + channelid);

            collection.InsertMany(datas);

            return nRows;
        }

        public int InsertIndexLogger(string channelid, List<DataLoggerModel> datas)
        {
            int nRows = 0;

            Connect connect = new Connect();

            var collection = connect.db.GetCollection<DataLoggerModel>("t_Index_Logger_" + channelid);

            collection.InsertMany(datas);

            return nRows;
        }

        public Nullable<DateTime> getCurrentTimeStamp(string channelid)
        {
            Nullable<DateTime> time = null;

            Connect connect = new Connect();

            var collection = connect.db.GetCollection<DataLoggerModel>("t_Data_Logger_" + channelid);

            var result = collection.Find(_ => true).ToList().OrderByDescending(d => d.TimeStamp).ToList();

            if(result.Count > 0)
            {
                time = result[0].TimeStamp;
            }
            else
            {
                time = null;
            }

            return time;
        }

        public Nullable<DateTime> getCurrentTimeStampIndex(string channelid)
        {
            Nullable<DateTime> time = null;

            Connect connect = new Connect();

            var collection = connect.db.GetCollection<DataLoggerModel>("t_Index_Logger_" + channelid);

            var result = collection.Find(_ => true).ToList().OrderByDescending(d => d.TimeStamp).ToList();

            if (result.Count > 0)
            {
                time = result[0].TimeStamp;
            }
            else
            {
                time = null;
            }

            return time;
        }

        public bool checkHasData(string channelid)
        {

            Connect connect = new Connect();

            var collection = connect.db.GetCollection<DataLoggerModel>("t_Data_Logger_" + channelid);

            var data = collection.Find(_ => true).ToList();

            return data.Count > 0 ? true : false;
        }

    }

}
