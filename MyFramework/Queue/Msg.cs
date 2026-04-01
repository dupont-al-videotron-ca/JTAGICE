using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    /// <summary>
    /// Message class to store event in a message queue.
    /// </summary>
    public class Msg
    {
        /// <summary>
        /// Message identification
        /// </summary>
        private MsgId mId;

        /// <summary>
        /// Message's data
        /// </summary>
        protected Object? mSenderData;
        
        /// <summary>
        /// Message timestamp
        /// </summary>
        private DateTime mDateTime;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessageId">Message identifier.</param>
        public Msg(MsgId MessageId)
        {
            mId = MessageId;
            mDateTime = DateTime.Now;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessageId">Message identifier.</param>
        /// <param name="UserData">Specific user data.</param>
        public Msg(MsgId MessageId, Object UserData)
        {
            mId = MessageId;
            mDateTime = DateTime.Now;
            mSenderData = UserData;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessageId">Message identifier.</param>
        public Msg(int MessageId)
        {
            mId = new MsgId(MessageId);
            mDateTime = DateTime.Now;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessageId">Message identifier.</param>
        /// <param name="UserData">Specific user data.</param>
        public Msg(int MessageId, Object UserData)
        {
            mId = new MsgId(MessageId);
            mDateTime = DateTime.Now;
            mSenderData = UserData;
        }

        /// <summary>
        /// Overrided. Return class name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}, {1}", Id.ToString(), TimeStamp.ToString()); ;
        }

        /// <summary>
        /// Compare messages.
        /// </summary>
        /// <param name="obj">Messsage to compare with.</param>
        /// <returns>bool</returns>
        public override bool Equals(object? obj)
        {
            Msg? MsgObj = obj as Msg;

            if (base.Equals(obj))
                return true;
            else if (MsgObj == null)
                return false;
            else
                return (Id == MsgObj.Id && TimeStamp == MsgObj.TimeStamp);
                
        }
        /// <summary>
        /// Generates a number corresponding to the value of the object to support the use of a hash table.
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Gets the Message identifier.
        /// </summary>
        public MsgId Id
        {
            get
            {
                return mId;
            }
        }

        /// <summary>
        /// Gets / Sets the message's timestamp.
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                return mDateTime;
            }
            set
            {
                mDateTime = value;
            }
        }

        /// <summary>
        /// Gets / Sets message's data.
        /// </summary>
        public Object? Data
        {
            get
            {
                return mSenderData;
            }
            set
            {
                mSenderData = value;
            }
        }

        /// <summary>
        /// Return true when user data is attached.
        /// </summary>
        /// <returns>bool</returns>
        public bool IsData()
        {
            return mSenderData != null;
        }
    }

    /// <summary>
    /// List of Msg class.
    /// </summary>
    public class MsgLinkedList : LinkedList<Msg>
    {

    }
}
