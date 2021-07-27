using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF_iPMAC.Controller;
using WF_iPMAC.Repository;

namespace iviwater.Controller
{
    public class ReadCelloController
    {
        Log_Controller log = new Log_Controller();
        public void SyncData()
        {
            //Copy files
            File_Controller _file = new File_Controller();
            string source_path = @"C:\PMAC\DATA";
            string des_path = @"C:\PMAC\Web_TMP";
            _file.Copy_Files(source_path, des_path);

            source_path = @"C:\PMAC\DATA\Index";
            des_path = @"C:\PMAC\Web_TMP\Index";
            _file.Copy_Files(source_path, des_path);

            source_path = @"C:\PMAC\Loggers";
            des_path = @"C:\PMAC\Web_TMP\Loggers";
            _file.Copy_Files(source_path, des_path);

            long totalTime = 0;

            var channelIds = (new ChannelConfigRepository()).GetChannelIds();


            totalTime = SyncAll(channelIds);
           
        }

        private void SyncPerChannel(string channelId)
        {
            var _pmac = new PMAC_Controller(channelId);
            var _channelConfig = new ChannelConfigRepository(channelId, _pmac);
            var _loggerConfig = new LoggerConfigRepository(channelId, _pmac);
            var _index = new IndexRepository(channelId, _pmac);
            var _logger = new LoggerDataRepository(channelId, _pmac);
            _channelConfig.UpdateChannelConfig();
            _loggerConfig.UpdateLoggerConfig();
            _logger.InsertData();
            _index.InsertData();
        }

        private long SyncAll(List<string> channelIds)
        {
            long totalTime = 0;
            foreach (var channelId in channelIds)
            {
                try
                {
                    var _pmac = new PMAC_Controller(channelId);
                    var _channelConfig = new ChannelConfigRepository(channelId, _pmac);
                    var _loggerConfig = new LoggerConfigRepository(channelId, _pmac);
                    var _index = new IndexRepository(channelId, _pmac);
                    var _logger = new LoggerDataRepository(channelId, _pmac);
                    _channelConfig.UpdateChannelConfig();
                    _loggerConfig.UpdateLoggerConfig();
                    _logger.InsertData();
                    _index.InsertData();
                }
                catch(Exception ex)
                {
                    log.WriteLog(ex.Message, "Error", true);
                }
               
            }
            return totalTime;
        }
    }
}
