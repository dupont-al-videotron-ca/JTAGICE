using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    /// <summary>
    /// Interface to register notification queue.
    /// </summary>
    /// <remarks>Must implement this interface when notification is sent.</remarks>
    public interface IMsgNotify
    {

        #region Properties

        /// <summary>
        /// Return true when a notification queue is available
        /// </summary>
        bool IsNotifyQueue
        {
            get;
        }

        /// <summary>
        /// Get the notification queue.
        /// </summary>
        MsgQueue NotifyQueue
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attach / Add a notification queue.
        /// </summary>
        /// <param name="Q">The notification queue.</param>
        void AttachNotifyQueue(MsgQueue Q);

        /// <summary>
        /// Detach / remove the notification queue from the Com. unit.
        /// </summary>
        /// <param name="Q">The notification queue.</param>
        void DetachNotifyQueue(MsgQueue Q);
        
        #endregion


    }
}
