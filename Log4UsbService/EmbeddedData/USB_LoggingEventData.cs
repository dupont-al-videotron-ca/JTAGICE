using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace Log4UsbService.EmbeddedData
{
    internal class USB_LoggingEventData
    {


        #region Constructors 

        internal USB_LoggingEventData()
        {
            this.Level = Level.Off;
            Message = string.Empty;
            FileName = string.Empty;
            LoggerName = string.Empty;
        }

        #endregion


        #region Properties 

        public Level Level { get; set; }

        public UInt32 Tick { get; set; }

        public string Message { get; set; }
        
        public string LoggerName { get; set; }
        
        public string FileName { get; set; }
        
        #endregion
        #region Public Methods 

        #endregion


    }
}
