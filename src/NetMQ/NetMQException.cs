using System;
using System.Diagnostics;
using System.Net.Sockets;
using JetBrains.Annotations;

using NetMQ.zmq;

namespace NetMQ
{
    /// <summary>
    /// Class NetMQException is the parent-class for Exceptions that occur within the NetMQ library.
    /// </summary>
    [Serializable]
    public class NetMQException : Exception
    {
        /// <summary>
        /// Create a new NetMQException containing the given Exception, Message and ErrorCode.
        /// </summary>
        /// <param name="innerException">an Exception that this exception will expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        /// <param name="errorCode">an ErrorCode that this exception will expose via it's ErrorCode property</param>
        protected NetMQException(Exception innerException, string message, ErrorCode errorCode)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; private set; }

        /// <summary>
        /// Create and return a new NetMQException with no Message, containing only the given SocketError.
        /// </summary>
        /// <param name="error">a SocketError that this exception will carry within it's ErrorCode property</param>
        /// <returns>a new NetMQException</returns>
        [NotNull]
        public static NetMQException Create(SocketError error)
        {
            return Create(error, null);
        }

        /// <summary>
        /// Create and return a new NetMQException with no Message containing only the given SocketException.
        /// </summary>
        /// <param name="innerException">a SocketException that this exception will expose via it's InnerException property</param>
        /// <returns>a new NetMQException</returns>
        [NotNull]
        public static NetMQException Create(SocketException innerException)
        {
            return Create(innerException.SocketErrorCode, innerException);
        }

        /// <summary>
        /// Create and return a new NetMQException with no Message containing the given SocketError and Exception.
        /// </summary>
        /// <param name="error">a SocketError that this exception will carry and expose via it's ErrorCode property</param>
        /// <param name="innerException">an Exception that this exception will expose via it's InnerException property</param>
        /// <returns>a new NetMQException</returns>
        public static NetMQException Create(SocketError error, [CanBeNull] Exception innerException)
        {
            var errorCode = ErrorHelper.SocketErrorToErrorCode(error);

            return Create(errorCode, innerException);
        }

        /// <summary>
        /// Create and return a new NetMQException with no Message containing the given ErrorCode and Exception.
        /// </summary>
        /// <param name="errorCode">an ErrorCode for this exception to contain and expose via it's ErrorCode property</param>
        /// <param name="innerException">an Exception for this exception to contain and expose via it's InnerException property</param>
        /// <returns>a new NetMQException</returns>
        [NotNull]
        public static NetMQException Create(ErrorCode errorCode, [CanBeNull] Exception innerException)
        {
            return Create(errorCode, "", innerException);
        }

        /// <summary>
        /// Create and return a new NetMQException with no Message containing only the given ErrorCode.
        /// </summary>
        /// <param name="errorCode">an ErrorCode that this exception will carry and expose via it's ErrorCode property</param>
        /// <returns>a new NetMQException</returns>
        [NotNull]
        public static NetMQException Create(ErrorCode errorCode)
        {
            return Create("", errorCode);
        }

        /// <summary>
        /// Create and return a new NetMQException with the given Message and ErrorCode.
        /// </summary>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        /// <param name="errorCode">an ErrorCode that this exception will carry and expose via it's ErrorCode property</param>
        /// <returns>a new NetMQException</returns>
        [NotNull]
        public static NetMQException Create([CanBeNull] string message, ErrorCode errorCode)
        {
            return Create(errorCode, message, null);
        }

        /// <summary>
        /// Create and return a new NetMQException with the given ErrorCode, Messag, and Exception.
        /// </summary>
        /// <param name="errorCode">an ErrorCode that this exception will contain and expose via it's ErrorCode property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        /// <param name="innerException">an Exception that this exception will expose via it's InnerException property</param>
        /// <returns>a new NetMQException, or sublcass of NetMQException that corresponds to the given ErrorCode</returns>
        [NotNull]
        private static NetMQException Create(ErrorCode errorCode, [CanBeNull] string message, [CanBeNull] Exception innerException)
        {
            switch (errorCode)
            {
                case ErrorCode.TryAgain:
                    return new AgainException(innerException, message);
                case ErrorCode.ContextTerminated:
                    return new TerminatingException(innerException, message);
                case ErrorCode.Invalid:
                    return new InvalidException(innerException, message);
                case ErrorCode.EndpointNotFound:
                    return new EndpointNotFoundException(innerException, message);
                case ErrorCode.AddressAlreadyInUse:
                    return new AddressAlreadyInUseException(innerException, message);
                case ErrorCode.ProtocolNotSupported:
                    return new ProtocolNotSupportedException(innerException, message);
                case ErrorCode.HostUnreachable:
                    return new HostUnreachableException(innerException, message);
                case ErrorCode.FiniteStateMachine:
                    return new FiniteStateMachineException(innerException, message);
                case ErrorCode.Fault:
                    return new FaultException(innerException, message);
                default:
                    return new NetMQException(innerException, message, errorCode);
            }
        }
    }

    /// <summary>
    /// AddressAlreadyInUseException is an Exception that is used within SocketBase.Bind to signal an address-conflict.
    /// </summary>
    public class AddressAlreadyInUseException : NetMQException
    {
        /// <summary>
        /// Create a new AddressAlreadyInUseException with a given inner-exception and message.
        /// </summary>
        /// <param name="innerException">an Exception for this new exception to contain and expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        public AddressAlreadyInUseException(Exception innerException, string message)
            : base(innerException, message, ErrorCode.AddressAlreadyInUse)
        {
        }

        /// <summary>
        /// Create a new AddressAlreadyInUseException with a given message.
        /// </summary>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        public AddressAlreadyInUseException(string message)
            : this(null, message)
        {
        }
    }

    /// <summary>
    /// EndpointNotFoundException is an Exception that is used within Ctx.FindEndpoint to signal a failure to find a specified address.
    /// </summary>
    [Serializable]
    public class EndpointNotFoundException : NetMQException
    {
        /// <summary>
        /// Create a new EndpointNotFoundException with a given inner-exception and message.
        /// </summary>
        /// <param name="innerException">an Exception for this new exception to contain and expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        public EndpointNotFoundException(Exception innerException, string message)
            : base(innerException, message, ErrorCode.EndpointNotFound)
        {
        }

        /// <summary>
        /// Create a new EndpointNotFoundException with a given message.
        /// </summary>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        public EndpointNotFoundException(string message)
            : this(null, message)
        {
        }

        /// <summary>
        /// Create a new EndpointNotFoundException with no message nor inner-exception.
        /// </summary>
        public EndpointNotFoundException()
            : this("")
        {
        }
    }

    /// <summary>
    /// AgainException is an Exception that is used within SocketBase.Send and SocketBase.Recv to signal failures
    /// (as when the Send/Recv fails and and DontWait is set or no timeout is specified)
    /// and is raised within Sub.XSetSocketOption if sending the queued-message fails.
    /// </summary>
    [Serializable]
    public class AgainException : NetMQException
    {
        /// <summary>
        /// Create a new AgainException with a given inner-exception and message.
        /// </summary>
        /// <param name="innerException">an Exception for this new exception to contain and expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal AgainException(Exception innerException, string message)
            : base(innerException, message, ErrorCode.TryAgain)
        {
        }

        /// <summary>
        /// Create a new AgainException with no message nor inner-exception.
        /// </summary>
        public AgainException()
            : this(null, "")
        {
        }
    }

    /// <summary>
    /// TerminatingException is an Exception that is used within SocketBase and Ctx to signal
    /// that you're trying to do further work after terminating the message-queuing system.
    /// </summary>
    [Serializable]
    public class TerminatingException : NetMQException
    {
        /// <summary>
        /// Create a new TerminatingException with a given inner-exception and message.
        /// </summary>
        /// <param name="innerException">an Exception for this new exception to contain and expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal TerminatingException(Exception innerException, string message)
            : base(innerException, message, ErrorCode.ContextTerminated)
        {
        }

        /// <summary>
        /// Create a new TerminatingException with no message nor inner-exception.
        /// </summary>
        internal TerminatingException()
            : this(null, "")
        {
        }
    }

    /// <summary>
    /// InvalidException is an Exception that is used within within the message-queuing system to signal invalid value errors.
    /// </summary>
    [Serializable]
    public class InvalidException : NetMQException
    {
        /// <summary>
        /// Create a new InvalidException with a given inner-exception and message.
        /// </summary>
        /// <param name="innerException">an Exception for this new exception to contain and expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal InvalidException(Exception innerException, string message)
            : base(innerException, message, ErrorCode.Invalid)
        {
        }

        /// <summary>
        /// Create a new InvalidException with the given message.
        /// </summary>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal InvalidException(string message)
            : this(null, message)
        {
        }

        /// <summary>
        /// Create a new InvalidException with no message nor inner-exception.
        /// </summary>
        public InvalidException()
            : this(null, "")
        {
        }
    }

    /// <summary>
    /// FaultException is an Exception that is used within within the message-queuing system to signal general fault conditions.
    /// </summary>
    [Serializable]
    public class FaultException : NetMQException
    {
        /// <summary>
        /// Create a new FaultException with a given inner-exception and message.
        /// </summary>
        /// <param name="innerException">an Exception for this new exception to contain and expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal FaultException(Exception innerException, string message)
            : base(innerException, message, ErrorCode.Fault)
        {
        }

        /// <summary>
        /// Create a new FaultException with the given message.
        /// </summary>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal FaultException(string message)
            : this(null, message)
        {
        }

        /// <summary>
        /// Create a new FaultException with no message nor inner-exception.
        /// </summary>
        public FaultException()
            : this(null, "")
        {
        }
    }

    /// <summary>
    /// ProtocolNotSupportedException is an Exception that is used within within the message-queuing system to signal
    /// mistakes in properly utilizing the communications protocols.
    /// </summary>
    [Serializable]
    public class ProtocolNotSupportedException : NetMQException
    {
        /// <summary>
        /// Create a new ProtocolNotSupportedException with a given inner-exception and message.
        /// </summary>
        /// <param name="innerException">an Exception for this new exception to contain and expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal ProtocolNotSupportedException(Exception innerException, string message)
            : base(innerException, message, ErrorCode.ProtocolNotSupported)
        {
        }

        /// <summary>
        /// Create a new ProtocolNotSupportedException with the given message.
        /// </summary>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal ProtocolNotSupportedException(string message)
            : this(null, message)
        {
        }

        /// <summary>
        /// Create a new ProtocolNotSupportedException with no message nor inner-exception.
        /// </summary>
        public ProtocolNotSupportedException()
            : this(null, "")
        {
        }
    }

    /// <summary>
    /// HostUnreachableException is an Exception that is used within within the message-queuing system
    /// to signal failures to communicate with a host.
    /// </summary>
    [Serializable]
    public class HostUnreachableException : NetMQException
    {
        /// <summary>
        /// Create a new HostUnreachableException with a given inner-exception and message.
        /// </summary>
        /// <param name="innerException">an Exception for this new exception to contain and expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal HostUnreachableException(Exception innerException, string message)
            : base(innerException, message, ErrorCode.HostUnreachable)
        {
        }

        /// <summary>
        /// Create a new HostUnreachableException with the given message.
        /// </summary>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal HostUnreachableException(string message)
            : this(null, message)
        {
        }

        /// <summary>
        /// Create a new HostUnreachableException with no message nor inner-exception.
        /// </summary>
        public HostUnreachableException()
            : this(null, "")
        {
        }
    }

    /// <summary>
    /// FiniteStateMachineException is an Exception that is used within within the message-queuing system
    /// to signal failures to communicate with a host.
    /// </summary>
    [Serializable]
    public class FiniteStateMachineException : NetMQException
    {
        /// <summary>
        /// Create a new FiniteStateMachineException with a given inner-exception and message.
        /// </summary>
        /// <param name="innerException">an Exception for this new exception to contain and expose via it's InnerException property</param>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal FiniteStateMachineException(Exception innerException, string message)
            : base(innerException, message, ErrorCode.FiniteStateMachine)
        {
        }

        /// <summary>
        /// Create a new FiniteStateMachineException with the given message.
        /// </summary>
        /// <param name="message">the textual description of what gave rise to this exception, to expose via the Message property</param>
        internal FiniteStateMachineException(string message)
            : this(null, message)
        {
        }

        /// <summary>
        /// Create a new FiniteStateMachineException with no message nor inner-exception.
        /// </summary>
        public FiniteStateMachineException()
            : this(null, "")
        {
        }
    }
}
