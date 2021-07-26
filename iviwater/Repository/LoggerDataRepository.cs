using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF_iPMAC.Controller;
using MongoDB.Driver;
using MongoDB.Bson;
using WF_iPMAC.Model;
using System.IO;

namespace WF_iPMAC.Repository
{
    public class LoggerDataRepository: RepositoryConfig
    {
        private PMAC_Controller _pmac;
        private string channel_id;
        IMongoCollection<DataModel> collection;
        string pmac_path;

        public LoggerDataRepository(string channel_id, PMAC_Controller pmac)
        {
            this.channel_id = channel_id;
            this._pmac = pmac;
            this.collection = database.GetCollection<DataModel>(Common.Constant.t_dt_logger + channel_id);
            this.pmac_path = "C:\\PMAC\\Web\\"+ channel_id + ".dat";
        }

        public bool InsertData()
        {
            try
            {
                if (File.Exists(pmac_path))
                {
                    List<DataModel> newData = new List<DataModel>();
                    var startTime = new DateTime();
                    var last_time = new DateTime();

                    //Nếu collection có ít nhất 1 bản ghi
                    if (collection.AsQueryable().Count() > 0)
                    {
                        //Get last time in database
                        last_time = collection.AsQueryable().Select(x => x.TimeStamp).Max().ToLocalTime();
                        //So sánh last time trong database với PMAC
                        if (_pmac.last_time > last_time && _pmac.interval != null)
                        {
                            startTime = last_time.AddSeconds(_pmac.interval ?? 0);
                            last_time = last_time.AddSeconds(_pmac.interval ?? 0);
                            //Nếu PMAC lớn hơn, tiến hành insert trước vào 1 list
                            while (_pmac.last_time > last_time)
                            {
                                newData.Add(new DataModel() { TimeStamp = last_time, Value = _pmac.GetValue(last_time) });
                                last_time = last_time.AddSeconds(_pmac.interval ?? 0);
                            }

                            if(newData.Count > 0)
                            {
                                //Insert list bằng insert many
                                collection.InsertMany(newData);
                                //Write Log
                                _log.WriteLog("timeD_" + startTime + "->" + last_time, "update data on channel " + channel_id, false);
                            }
                            
                        }
                    }
                    else //Add từ time đầu tiên trong PMAC
                    {
                        if (_pmac.first_time != null && _pmac.interval != null)
                        {
                            startTime = (DateTime)_pmac.first_time;
                            last_time = (DateTime)_pmac.first_time;
                            while (_pmac.last_time >= last_time)
                            {
                                newData.Add(new DataModel() { TimeStamp = last_time, Value = _pmac.GetValue(last_time) });
                                last_time = last_time.AddSeconds(_pmac.interval ?? 0);
                            }
                            if(newData.Count > 0)
                            {
                                //Insert list bằng insert many
                                collection.InsertMany(newData);
                                //Write Log
                                _log.WriteLog("timeD_" + startTime + "->" + last_time.AddSeconds(-_pmac.interval ?? 0), "update data on channel " + channel_id, false);
                            }
                            
                        }
                        else
                        {
                            _log.WriteLog("", "No data on channel " + channel_id, true);
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    //Write Log
                    _log.WriteLog("", ".dat file not found on channel " + channel_id, true);
                    return false;
                }
            }
            catch(Exception ex)
            {
                _log.WriteLog(ex.ToString(), "error insert new logger data on channel " + channel_id, true);
                return false;
            }
        }
    }
}
