using SyncPrimayerToMongoDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SyncPrimayerToMongoDB.Action
{
    public class FileAction
    {
        public string checkFileExistsFilePressure(string path, string siteid)
        {
            string file = $"{path}/{siteid}/15 Minute Average Pressure A1.log.xml";

            if(File.Exists(file))
            {
                return file;
            }
            else
            {
                return "nothing";
            }
        }

        public string checkFileExistsFileFlow(string path, string siteid)
        {
            string file = $"{path}/{siteid}/15 Minute Average Flow D1a.log.xml";

            if (File.Exists(file))
            {
                return file;
            }
            else
            {
                return "nothing";
            }
        }

        public string checkFileExistsFileIndex(string path, string siteid)
        {
            string file = $"{path}/{siteid}/1 Day Cumulative Total Flow D1a.log.xml";

            if (File.Exists(file))
            {
                return file;
            }
            else
            {
                return "nothing";
            }
        }

        public List<DataLoggerModel> readFileXml(string path, Nullable<DateTime> time)
        {
            List<DataLoggerModel> list = new List<DataLoggerModel>();

            if(time == null)
            {
                using (XmlReader xmlReader = XmlReader.Create(path))
                {
                    DateTime? start = null;
                    bool checkFirstNode = false;

                    while(xmlReader.Read())
                    {
                        if(xmlReader.Name == "LoggedData")
                        {
                            string datetime = xmlReader["DTG"];
                            if(datetime != null && datetime.Trim() != "")
                            {
                                string[] split_string = datetime.Split(new char[] { ' ' }, StringSplitOptions.None);
                                string[] date = split_string[0].Split(new char[] { '/' }, StringSplitOptions.None);
                                string[] times = split_string[1].Split(new char[] { ':' }, StringSplitOptions.None);

                                int year = int.Parse(date[2]);
                                int month = int.Parse(date[1]);
                                int day = int.Parse(date[0]);

                                int hour = int.Parse(times[0]);
                                int minute = int.Parse(times[1]);
                                int second = int.Parse(times[2]);


                                start = new DateTime(year, month, day, hour, minute, second);
                                start = start.Value.AddHours(7);

                                checkFirstNode = true;
                            }
                        }
                        else if(xmlReader.Name == "d")
                        {
                            xmlReader.Read();
                            DataLoggerModel el = new DataLoggerModel();
                            if(checkFirstNode == true)
                            {
                                el.TimeStamp = start;
                                checkFirstNode = false;
                            }
                            else
                            {
                                start = start.Value.AddMinutes(15);
                                el.TimeStamp = start;
                            }
                            double value = xmlReader.Value != null ? double.Parse(xmlReader.Value) : 0;

                            xmlReader.Read();

                            el.Value = Math.Round(value,2);
                            list.Add(el);
                        }

                    }
                }
            }
            else
            {
                using (XmlReader xmlReader = XmlReader.Create(path))
                {
                    DateTime? datetimeOfNode = null;
                    bool checkFirstNode = false;

                    while (xmlReader.Read())
                    {
                        if(xmlReader.Name == "LoggedData")
                        {
                            string datetime = xmlReader["DTG"];
                            if (datetime != null && datetime.Trim() != "")
                            {
                                string[] split_string = datetime.Split(new char[] { ' ' }, StringSplitOptions.None);
                                string[] date = split_string[0].Split(new char[] { '/' }, StringSplitOptions.None);
                                string[] times = split_string[1].Split(new char[] { ':' }, StringSplitOptions.None);

                                int year = int.Parse(date[2]);
                                int month = int.Parse(date[1]);
                                int day = int.Parse(date[0]);

                                int hour = int.Parse(times[0]);
                                int minute = int.Parse(times[1]);
                                int second = int.Parse(times[2]);


                                datetimeOfNode = new DateTime(year, month, day, hour, minute, second);

                                checkFirstNode = true;
                            }
                        }
                        else if (xmlReader.Name == "d")
                        {
                            xmlReader.Read();
                            if(checkFirstNode == true)
                            {
                                checkFirstNode = false;
                            }
                            else
                            {
                                datetimeOfNode = datetimeOfNode.Value.AddMinutes(15);
                            }


                            if(DateTime.Compare(time.Value, datetimeOfNode.Value) < 0)
                            {
                                DataLoggerModel el = new DataLoggerModel();
                                el.TimeStamp = datetimeOfNode.Value.AddHours(7);
                                el.Value = xmlReader.Value != null ? double.Parse(xmlReader.Value) : 0;
                                el.Value = Math.Round(el.Value.Value, 2);

                                list.Add(el);
                            }
                            xmlReader.Read();
                        }
                    }
                }
            }

            return list;
        }

    }
}
