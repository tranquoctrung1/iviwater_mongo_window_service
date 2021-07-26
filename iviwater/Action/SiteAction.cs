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
    public class SiteAction
    {
        public List<SiteModel> getSitePrimayer()
        {
            List<SiteModel> list = new List<SiteModel>();

            Connect connect = new Connect();

            var siteCollection = connect.db.GetCollection<SiteModel>("t_Sites");

            list = siteCollection.Find(s => s.IsPrimayer == true).ToList();

            return list;
        }
    }
}
