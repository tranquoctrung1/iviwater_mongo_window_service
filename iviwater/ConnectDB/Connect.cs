using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncPrimayerToMongoDB.ConnectDB
{
    public class Connect
    {
        public MongoClient client;

        public IMongoDatabase db;

        public Connect()
        {
           client  = new MongoClient(ConfigurationManager.ConnectionStrings["mongo"].ConnectionString);

            db = client.GetDatabase(ConfigurationManager.ConnectionStrings["db"].ConnectionString);
        }


    }
}
