using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Exception
{
    /// <summary>
    /// MyFramework's queue exception.
    /// </summary>
    public class QueueException: MyException 
    {
        /// <summary>
        /// Initializes a new instance of the MyException class. 
        /// </summary>
        public QueueException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message. 
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public QueueException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public QueueException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Throw when queue timeoue occured.
    /// </summary>
    public class QueueTimeoutException : QueueException
    {
        /// <summary>
        /// Initializes a new instance of the MyException class. 
        /// </summary>
        public QueueTimeoutException()
            : base("Queue timeout")
        {

        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message. 
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public QueueTimeoutException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public QueueTimeoutException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
