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
    public class IndexRepository : RepositoryConfig
    {
        private PMAC_Controller _pmac;
        private string channel_id;
        IMongoCollection<DataModel> collection;
        string pmac_path;

        public IndexRepository(string channel_id, PMAC_Controller pmac)
        {
            this.channel_id = channel_id;
            this._pmac = pmac;
            this.collection = database.GetCollection<DataModel>(Common.Constant.t_dt_index + channel_id);
            this.pmac_path = "C:\\PMAC\\Web_TMP\\" + channel_id + ".dat";
        }

        public bool InsertData()
        {
            try
            {
                if (File.Exists(pmac_path))
                {
                    if (_pmac.strategy == 10) //là kênh có index
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
                            if (_pmac.last_time_index > last_time && _pmac.interval != null)
                            {
                                startTime = last_time.AddSeconds(_pmac.interval ?? 0);
                                last_time = last_time.AddSeconds(_pmac.interval ?? 0);
                                //Nếu PMAC lớn hơn, tiến hành insert trước vào 1 list
                                while (_pmac.last_time_index > last_time)
                                {
                                    var data = new DataModel() { TimeStamp = last_time, Value = _pmac.GetIndex(last_time) };
                                    //Nếu Index == null thì tính theo data logger cộng vào
                                    if (data.Value == null)
                                    {
                                        var previous_time = data.TimeStamp.AddSeconds(-_pmac.interval ?? 0);
                                        var record_previous_time = collection.AsQueryable().FirstOrDefault(x => x.TimeStamp == previous_time);
                                        if (record_previous_time != null)
                                        {
                                            var previous_index = record_previous_time.Value;
                                            var data_logger = _pmac.GetValue(previous_time);
                                            if (previous_index != null)
                                            {
                                                if (data_logger != null)
                                                {
                                                    data.Value = previous_index + (data_logger * _pmac.interval / 3600);
                                                }
                                                else data.Value = previous_index;
                                            }
                                        }
                                    }
                                    newData.Add(data);
                                    last_time = last_time.AddSeconds(_pmac.interval ?? 0);
                                }
                                if (newData.Count > 0)
                                {
                                    //Insert list bằng insert many
                                    collection.InsertMany(newData);
                                    //Write Log
                                    _log.WriteLog("timeD_" + startTime + "->" + last_time, "update index on channel " + channel_id, false);
                                }

                            }
                        }
                        else //Add từ time đầu tiên trong PMAC
                        {
                            if (_pmac.first_time_index != null && _pmac.interval != null)
                            {
                                startTime = (DateTime)_pmac.first_time_index;
                                last_time = (DateTime)_pmac.first_time_index;
                                while (_pmac.last_time_index >= last_time)
                                {
                                    var data = new DataModel() { TimeStamp = last_time, Value = _pmac.GetIndex(last_time) };
                                    //Nếu Index == null thì tính theo data logger cộng vào
                                    if (data.Value == null && newData.Count > 0)
                                    {
                                        var previous_time = data.TimeStamp.AddSeconds(-_pmac.interval ?? 0);
                                        var previous_index = newData[newData.Count - 1].Value;
                                        var data_logger = _pmac.GetValue(previous_time);
                                        if (previous_index != null)
                                        {
                                            if (data_logger != null)
                                            {
                                                data.Value = previous_index + (data_logger * _pmac.interval / 3600);
                                            }
                                            else data.Value = previous_index;
                                        }
                                    }
                                    newData.Add(data);
                                    last_time = last_time.AddSeconds(_pmac.interval ?? 0);
                                }

                                if (newData.Count > 0)
                                {
                                    //Insert list bằng insert many
                                    collection.InsertMany(newData);
                                    //Write Log
                                    _log.WriteLog("timeD_" + startTime + "->" + last_time.AddSeconds(-_pmac.interval ?? 0), "update index on channel " + channel_id, false);
                                }

                            }
                            else
                            {
                                _log.WriteLog("No data on channel " + channel_id, "", true);
                                return false;
                            }
                        }
                        //Handle Delay Index
                        HandleDelayIndex();
                    }
                    return true;
                }
                //Write Log
                _log.WriteLog(".dat file not found on channel " + channel_id, "", true);
                return false;
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.ToString(), "error insert new index data on channel " + channel_id, true);
                return false;
            }
        }

        private void HandleDelayIndex()
        {
            var newData = new List<DataModel>();
            var startTime = new DateTime();

            var last_time = collection.AsQueryable().Select(x => x.TimeStamp).Max().ToLocalTime();
            var last_time_logger = (_pmac.last_time ?? new DateTime(1990, 12, 2));
            if (_pmac.interval != null)
            {
                if (last_time_logger.Subtract(last_time).TotalSeconds > _pmac.interval)
                {
                    startTime = last_time.AddSeconds(_pmac.interval ?? 0);
                    last_time = last_time.AddSeconds(_pmac.interval ?? 0);

                    while (last_time_logger >= last_time)
                    {
                        var previous_time = last_time.AddSeconds(-_pmac.interval ?? 0);
                        var record_previous_time = collection.AsQueryable().FirstOrDefault(x => x.TimeStamp == previous_time);
                        if (record_previous_time == null && newData.Count > 0) record_previous_time = newData[newData.Count - 1];

                        var previous_index = record_previous_time.Value;
                        var data_logger = _pmac.GetValue(previous_time);
                        if (previous_index != null)
                        {
                            var index = previous_index;
                            if (data_logger != null)
                            {
                                index += (data_logger * _pmac.interval / 3600);
                            }
                            newData.Add(new DataModel() { TimeStamp = last_time, Value = index });
                        }

                        last_time = last_time.AddSeconds(_pmac.interval ?? 0);
                    }
                }
            }
            if (newData.Count > 0)
            {
                //Insert list bằng insert many
                collection.InsertMany(newData);
                //Write Log
                _log.WriteLog("timeD_" + startTime + "->" + last_time_logger, "update index base on logger data on channel " + channel_id, false);
            }

        }
    }
}
