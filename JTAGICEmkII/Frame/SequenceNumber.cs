using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.HostService;

namespace JTAGICEmkII.Frame
{
    [DebuggerDisplay("NumberValue={NumberValue}")]
    internal class SequenceNumber : IEquatable<SequenceNumber>, IComparable<SequenceNumber>
    {

        #region Constructors 

        internal SequenceNumber(int startValue)
        {
            _sequenceNumber = startValue;
        }

        internal SequenceNumber()
        {
            _sequenceNumber = -1;
        }

        internal SequenceNumber(SequenceNumber src)
        {
            _sequenceNumber = src._sequenceNumber;
            _sequenceNumberModulo = src._sequenceNumberModulo;
            _sequenceNumberWrap = src._sequenceNumberWrap;
        }

        #endregion


        #region Fields 


        private int _sequenceNumber = -1;
        private int _sequenceNumberModulo = 0xFFFF;
        private int _sequenceNumberWrap = 0xFFFE;

        #endregion


        #region Properties 

        internal bool IsInitial => _sequenceNumber == -1;

        internal int NumberValue => _sequenceNumber;

        private byte NumberValueLowByte => (byte)(NumberValue & 0xFF);

        private byte NumberValueHighByte => (byte)((NumberValue >> 8) & 0xFF);

        public int SequenceNumberModulo { get => this._sequenceNumberModulo; set => this._sequenceNumberModulo = value; }

        public int SequenceNumberWrap { get => this._sequenceNumberWrap; set => this._sequenceNumberWrap = value; }

        #endregion

        #region Public Methods 

        internal byte[] GetUInt16LittleEndian()
        {
            return new byte[] { NumberValueLowByte, NumberValueHighByte };
        }


        internal SequenceNumber Increment()
        {
            return new SequenceNumber((_sequenceNumber + 1) % _sequenceNumberModulo);
        }

        internal SequenceNumber Decrement()
        {
            var sequenceNumber = _sequenceNumber - 1;

            if (sequenceNumber < 0)
            {
                sequenceNumber = _sequenceNumberWrap;
            }
        
            return new SequenceNumber(sequenceNumber);
        }

        internal Int16 Difference(UInt16 other)
        {
            Int16 diffSequenceNumber = (Int16)(_sequenceNumber - other);

            return diffSequenceNumber;
        }

        bool IEquatable<SequenceNumber>.Equals(SequenceNumber? other) 
            => other is not null && other._sequenceNumber == _sequenceNumber;

        public override bool Equals(object? obj) 
            => obj is SequenceNumber other && other._sequenceNumber == _sequenceNumber;

        public override int GetHashCode() => _sequenceNumber.GetHashCode();

        // Implement the generic CompareTo method with the SequenceNumber
        // class as the Type parameter.
        //
        public int CompareTo(SequenceNumber? other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other! == null!) return 1;

            // The sequence number comparison depends on the comparison of
            // the underlying integer values.
            return _sequenceNumber.CompareTo(other._sequenceNumber);
        }

        // Define the is greater than operator.
        public static bool operator >(SequenceNumber operand1, SequenceNumber operand2)
        {
            return operand1.CompareTo(operand2) > 0;
        }

        // Define the is less than operator.
        public static bool operator <(SequenceNumber operand1, SequenceNumber operand2)
        {
            return operand1.CompareTo(operand2) < 0;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(SequenceNumber operand1, SequenceNumber operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(SequenceNumber operand1, SequenceNumber   operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }
        public static bool operator ==(SequenceNumber sequenceNumber1, SequenceNumber sequenceNumber2)
        {
            if (sequenceNumber1 is null)
            {
                return sequenceNumber2 is null;
            }

            return sequenceNumber1.Equals(sequenceNumber2);
        }

        public static bool operator !=(SequenceNumber sequenceNumber1, SequenceNumber sequenceNumber2)
        {
            if (sequenceNumber1 is null)
            {
                return sequenceNumber2 is not null;
            }

            return !sequenceNumber1.Equals(sequenceNumber2);
        }

        public static SequenceNumber operator ++(SequenceNumber sequenceNumber)
        {
            return new SequenceNumber((sequenceNumber._sequenceNumber + 1) % sequenceNumber._sequenceNumberModulo);
        }

        public static SequenceNumber operator --(SequenceNumber sequenceNumber)
        {
            return sequenceNumber.Decrement(); ;
        }

        public static explicit operator SequenceNumber(UInt16 value)
        {
            return new SequenceNumber(value);
        }

        public static implicit operator UInt16(SequenceNumber sequenceNumber)
        {
            return (UInt16)sequenceNumber._sequenceNumber;
        }

        #endregion

    }
}
