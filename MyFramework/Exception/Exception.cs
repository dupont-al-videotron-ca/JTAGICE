using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Exception
{
    /// <summary>
    /// Base exception of all MyFramework exception.
    /// </summary>
    public class MyException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the MyException class. 
        /// </summary>
        public MyException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message. 
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MyException(String message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MyException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets the message from Inner exception or from this exception
        /// </summary>
        /// 
        public override string Message
        {
            get
            {
                if (InnerException != null)
                    return InnerException.Message;
                else
                    return base.Message;
            }
        }
    }

    /// <summary>
    /// MyFramework application exception.
    /// </summary>
    public class MyApplicationException : MyException
    {
        /// <summary>
        /// Initializes a new instance of the MyException class. 
        /// </summary>
        public MyApplicationException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message. 
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MyApplicationException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MyApplicationException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }


    /// <summary>
    /// MyFramework internal runtime exception.
    /// </summary>
    public class MyInternalException : MyException
    {
        /// <summary>
        /// Initializes a new instance of the MyException class. 
        /// </summary>
        public MyInternalException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message. 
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MyInternalException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MyInternalException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Not initialize exception, throw when a class is not readey to be used.
    /// </summary>
    public class MyNotInitializeException : MyException
    {
        /// <summary>
        /// Initializes a new instance of the MyException class. 
        /// </summary>
        public MyNotInitializeException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message. 
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MyNotInitializeException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MyNotInitializeException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
