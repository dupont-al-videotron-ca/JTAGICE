using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    /// <summary>
    /// Class contaning the message identification.
    /// </summary>
    public class MsgId
    {
        /// <summary>
        /// Application base message identification number
        /// </summary>
        public const int BaseParkingApplication = 0x1000;

        /// <summary>
        /// Device driver base message identification number
        /// </summary>
        public const int BaseRbCom = 0x2000;
        public const int BaseMDB = 0x2100;
        public const int BaseccTalk = 0x2200;
        public const int BaseK800 = 0x2300;

        /// <summary>
        /// Component base message identification number
        /// </summary>
        public const int BasePaymentEngine = 0x3000;
        public const int BaseElectronicPaymentEngine = 0x3100;

        /// <summary>
        /// Base message for journal module
        /// </summary>
        public const int BaseJournalEvent = 0x5000;

        /// <summary>
        /// Base message for CfgDistribution
        /// </summary>
        public const int BaseCfgDitribution = 0x6000;

        /// <summary>
        /// Base message for DbAgent
        /// </summary>
        public const int BaseDbAgent = 0x7000;

        /// <summary>
        /// Base message for DbAgent
        /// </summary>
        public const int BaseWCFCommunication = 0x8000;

        /// <summary>
        /// Message identification number
        /// </summary>
        private int mId;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Message's identification.</param>
        public MsgId(int id)
        {
            mId = id;
        }

        /// <summary>
        /// Binary comparaison.
        /// </summary>
        /// <param name="obj">Source message to compare from</param>
        /// <returns>true if message Id are equal.</returns>
        public override bool Equals(object? obj)
        {
            // Is same reference ?
            if (!base.Equals(obj))
            {
                // No compare value
                MsgId? MsgIdObj = obj as MsgId;

                // Is obj a MsgId ?
                if (MsgIdObj != null)
                {
                    // Yes,
                    return Id == MsgIdObj.Id;
                }
                else
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Generates a number corresponding to the value of the object to support the use of a hash table.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return mId;
        }

        /// <summary>
        /// Return the message identification as a string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return string.Format("0x{0}", Id.ToString("X"));
        }

        /// <summary>
        /// Gets the message identification.
        /// </summary>
        public int Id
        {
            get
            {
                return mId;
            }
        }
    }

    /// <summary>
    /// List of MsgId.
    /// </summary>
    public class MsgIdList : List<MsgId>
    {

    }

    /// <summary>
    /// Dictionary of MsgId.
    /// </summary>
    public class MsgIdDictionary : Dictionary<int, MsgId>
    {

    }

}
