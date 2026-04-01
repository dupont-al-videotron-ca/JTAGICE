using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    public interface IMsgReactor
    {
        /// <summary>
        /// Gets the number of handlers
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Demultiplex and dispatch the message queue.
        /// </summary>
        /// <returns></returns>
        void HandleQueueEvent();

        /// <summary>
        /// Register a Queue handler to the reactor
        /// </summary>
        /// <param name="h">Message queue handler</param>
        void RegisterHandle(IMsgQueueHandle h);
        
        /// <summary>
        /// Resume the Queue handler (put handler in the ready list)
        /// </summary>
        /// <param name="h">Message queue handler</param>
        void ResumeHandle(IMsgQueueHandle h);

        /// <summary>
        /// Suspend the Queue handler (put handler in the suspended list)
        /// </summary>
        /// <param name="h">Message queue handler</param>
        void SuspendHandle(IMsgQueueHandle h);

        /// <summary>
        /// Unregister the Queue handler (remove handler from the reactor)
        /// </summary>
        /// <param name="h">Message queue handler</param>
        void UnregisterHandle(IMsgQueueHandle h);

    }
}
