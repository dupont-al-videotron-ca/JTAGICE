using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    /// <summary>
    /// Message class to store event in a message queue with .Net EventArgs object (derived).
    /// </summary>
    public class MsgEventArgs: Msg
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessageId">Message identifier.</param>
        /// <param name="e">.Net EventArgs object.</param>
        public MsgEventArgs(MsgId MessageId, EventArgs e): base(MessageId, e)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessageId">Message identifier.</param>
        /// <param name="e">.Net EventArgs object.</param>
        public MsgEventArgs(int MessageId, EventArgs e): base(MessageId, e)
        {
        }

        /// <summary>
        /// Overrided. Return class name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("%0, %1", base.ToString(), Event?.ToString()); ;
        }

        /// <summary>
        /// Compare messages.
        /// </summary>
        /// <param name="obj">Messsage to compare with.</param>
        /// <returns>bool</returns>
        public override bool Equals(object? obj)
        {
            MsgEventArgs? MsgObj = obj as MsgEventArgs;

            if (base.Equals(obj))
                return true;
            else if (MsgObj == null)
                return false;
            else
                return (Event == MsgObj.Event);
        }

        /// <summary>
        /// Generates a number corresponding to the value of the object to support the use of a hash table.
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets / Sets .Net EventArgs object to / from message.
        /// </summary>
        public EventArgs? Event
        {
            get
            {
                return (EventArgs?) Data;
            }
            set
            {
                Data = value;
            }
        }
    }
}
