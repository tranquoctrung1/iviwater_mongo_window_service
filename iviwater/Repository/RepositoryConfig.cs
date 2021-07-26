using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using WF_iPMAC.Controller;

namespace WF_iPMAC.Repository
{
    public class RepositoryConfig
    {
        protected MongoClient dbClient = new MongoClient(Common.Constant.connectionString);
        public IMongoDatabase database;
        public Log_Controller _log;
        public RepositoryConfig()
        {
            database = dbClient.GetDatabase(Common.Constant.database);
            _log = new Log_Controller();
        }
    }
}
