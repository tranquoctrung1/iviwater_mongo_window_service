using SyncPrimayerToMongoDB.Action;
using SyncPrimayerToMongoDB.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF_iPMAC.Controller;

namespace SyncPrimayerToMongoDB.Controller
{
    public class MainController
    {
        public void Main()
        {
            FileAction fileAction = new FileAction();
            DataLoggerAction dataLoggerAction = new DataLoggerAction();
            SiteAction siteAction = new SiteAction();
            ChannelConfigAction channelConfigAction = new ChannelConfigAction();
            Log_Controller log_Controller = new Log_Controller();

            var sites = siteAction.getSitePrimayer();

            string path = ConfigurationManager.ConnectionStrings["path"].ConnectionString;

            foreach(var site in sites)
            {
                try
                {
                    string filePressure = fileAction.checkFileExistsFilePressure(path, site.SiteId);
                    string fileFlow = fileAction.checkFileExistsFileFlow(path, site.SiteId);
                    string fileIndex = fileAction.checkFileExistsFileIndex(path, site.SiteId);

                    if (filePressure != "nothing")
                    {

                        string channelPressure = $"{site.LoggerId}_01";

                        DateTime? currentTimePressure = dataLoggerAction.getCurrentTimeStamp(channelPressure);

                        List<DataLoggerModel> list = fileAction.readFileXml(filePressure, currentTimePressure);

                        if (list.Count > 0)
                        {
                            try
                            {
                                // insert t_Data_Logger collection
                                dataLoggerAction.InsertDataLogger(channelPressure, list);
                                log_Controller.WriteLog("Inserted Success To t_Data_Logger_" + channelPressure, "Insert", false);
                            }
                            catch(Exception ex)
                            {
                                log_Controller.WriteLog(ex.Message, "Error", true);
                            }
                           
                            try
                            {
                                if(list[list.Count - 1].TimeStamp != null && list[list.Count - 1].Value != null )
                                {
                                    // update t_Channel_Configurations collection
                                    channelConfigAction.UpdateChannel(list[list.Count - 1].TimeStamp.Value, list[list.Count - 1].Value.Value, channelPressure);

                                    log_Controller.WriteLog("Update Success To t_Channel_Configurations by ChannelId" + channelPressure, "Update", false);
                                }
                                

                            }
                            catch(Exception ex)
                            {
                                log_Controller.WriteLog(ex.Message, "Error", true);
                            }

                            
    
                        }
                        else
                        {
                            //log_Controller.WriteLog("Inserted Nothing To t_Data_Logger_" + channelPressure, "Insert", true);
                        }


                    }

                    if (fileFlow != "nothing")
                    {
                        string channelFlow = $"{site.LoggerId}_02";

                        DateTime? currentTimeFlow = dataLoggerAction.getCurrentTimeStamp(channelFlow);

                        List<DataLoggerModel> list = fileAction.readFileXml(fileFlow, currentTimeFlow);

                        if (list.Count > 0)
                        {
                            try
                            {
                                // insert t_Data_Logger collection
                                dataLoggerAction.InsertDataLogger(channelFlow, list);

                                log_Controller.WriteLog("Inserted Success To t_Data_Logger_" + channelFlow, "Insert", false);
                            }
                            catch (Exception ex)
                            {
                                log_Controller.WriteLog(ex.Message, "Error", true);
                            }

                            try
                            {
                                if (list[list.Count - 1].TimeStamp != null && list[list.Count - 1].Value != null)
                                {
                                    channelConfigAction.UpdateChannel(list[list.Count - 1].TimeStamp.Value, list[list.Count - 1].Value.Value, channelFlow);

                                    log_Controller.WriteLog("Update Success To t_Channel_Configurations by ChannelId" + channelFlow, "Update", false);
                                }
                            }
                            catch(Exception ex)
                            {
                                log_Controller.WriteLog(ex.Message, "Error", true);
                            }
                        }
                        else
                        {
                            //log_Controller.WriteLog("Inserted Nothing To t_Data_Logger_" + channelFlow, "Insert", true);
                        }
                    }

                    if (fileIndex != "nothing")
                    {
                        string channelFlow = $"{site.LoggerId}_02";

                        DateTime? currentTimeIndex = dataLoggerAction.getCurrentTimeStampIndex(channelFlow);

                        List<DataLoggerModel> listIndex = fileAction.readFileXml(fileIndex, currentTimeIndex);

                        if (listIndex.Count > 0)
                        {
                            try
                            {
                                // insert t_Index_logger collection 
                                dataLoggerAction.InsertIndexLogger(channelFlow, listIndex);

                                log_Controller.WriteLog("Inserted Success To t_Index_Logger_" + channelFlow, "Insert", true);

                            }
                            catch (Exception ex)
                            {
                                log_Controller.WriteLog(ex.Message, "Error", true);
                            }
                            try
                            {
                                if(listIndex[listIndex.Count - 1].TimeStamp != null && listIndex[listIndex.Count - 1].Value != null)
                                {
                                    channelConfigAction.UpdateChannelIndex(listIndex[listIndex.Count - 1].TimeStamp.Value, listIndex[listIndex.Count - 1].Value.Value, channelFlow);
                                }
                            }
                            catch(Exception ex)
                            {
                                log_Controller.WriteLog(ex.Message, "Error", true);
                            }
                        }
                        else
                        {
                            //log_Controller.WriteLog("Inserted Nothing To t_Index_Logger" + channelFlow, "Insert", true);
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    log_Controller.WriteLog(ex.Message, "Error", true);
                }
                

            }
        }
    }
}
