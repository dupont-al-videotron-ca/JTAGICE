using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    /// <summary>
    /// Class storing message handler with its MsgId filters.
    /// </summary>
    internal class MsgDispatcherData
    {

        #region Members

        /// <summary>
        /// Message handler's filters
        /// </summary>
        private MsgIdList mFilters;
        private IMsgHandler mNotify;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Notify">Message handler.</param>
        internal MsgDispatcherData(IMsgHandler Notify)
        {
            mNotify = Notify;
            mFilters = new MsgIdList();
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Notify">Message handler.</param>
        /// <param name="Filter">MsgId filter.</param>
        internal MsgDispatcherData(IMsgHandler Notify, MsgId Filter)
        {
            mNotify = Notify;
            mFilters = new MsgIdList();
            mFilters.Add(Filter);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Notify">Message handler.</param>
        /// <param name="Filters">MsgId filters.</param>
        internal MsgDispatcherData(IMsgHandler Notify, MsgId[] Filters)
        {
            mNotify = Notify;
            mFilters = new MsgIdList();
            mFilters.AddRange(Filters);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets message handler
        /// </summary>
        public IMsgHandler Handler
        {
            get
            {
                return mNotify;
            }
        }


        #endregion
        /// <summary>
        /// Called by the message reactor.
        /// Return true if the message is accepting to receive this message.
        /// </summary>
        /// <param name="MsgToAccept">Message received by the message reactor.</param>
        /// <returns></returns>
        internal bool AcceptMsg(Msg MsgToAccept)
        {
            if (mFilters.Count != 0)
            {
                return mFilters.Contains(MsgToAccept.Id);
            }
            else
                // Accept all message.
                return true;
        }

    }


    internal class MsgHandlerDataList : List<MsgDispatcherData>
    {
    }

}
