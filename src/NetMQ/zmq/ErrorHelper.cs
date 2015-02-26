﻿using System.Diagnostics;
using System.Net.Sockets;

namespace NetMQ.zmq
{
    /// <summary>
    /// Class ErrorHelper provides one static method - SocketErrorToErrorCode, for converting a SocketError to the equivalent ErrorCode.
    /// </summary>
    internal static class ErrorHelper
    {
        /// <summary>
        /// Return the ErrorCode that is the closest equivalent to the given SocketError.
        /// </summary>
        /// <param name="error">the SocketError to convert from</param>
        /// <returns>an ErrorCode that corresponds to the given SocketError</returns>
        public static ErrorCode SocketErrorToErrorCode(SocketError error)
        {
            switch (error)
            {
                case SocketError.SocketError:
                    return ErrorCode.Unspecified;
                case SocketError.AccessDenied:
                    return ErrorCode.AccessDenied;
                case SocketError.Fault:
                    return ErrorCode.Fault;
                case SocketError.InvalidArgument:
                    return ErrorCode.Invalid;
                case SocketError.TooManyOpenSockets:
                    return ErrorCode.TooManyOpenSockets;
                case SocketError.InProgress:
                    return ErrorCode.TryAgain;
                case SocketError.MessageSize:
                    return ErrorCode.MessageSize;
                case SocketError.ProtocolNotSupported:
                    return ErrorCode.ProtocolNotSupported;
                case SocketError.AddressFamilyNotSupported:
                    return ErrorCode.AddressFamilyNotSupported;
                case SocketError.AddressNotAvailable:
                    return ErrorCode.AddressNotAvailable;
                case SocketError.NetworkDown:
                    return ErrorCode.NetworkDown;
                case SocketError.NetworkUnreachable:
                    return ErrorCode.NetworkUnreachable;
                case SocketError.NetworkReset:
                    return ErrorCode.NetworkReset;
                case SocketError.ConnectionAborted:
                    return ErrorCode.ConnectionAborted;
                case SocketError.ConnectionReset:
                    return ErrorCode.ConnectionReset;
                case SocketError.NoBufferSpaceAvailable:
                    return ErrorCode.NoBufferSpaceAvailable;
                case SocketError.NotConnected:
                    return ErrorCode.NotConnected;
                case SocketError.TimedOut:
                    return ErrorCode.TimedOut;
                case SocketError.ConnectionRefused:
                    return ErrorCode.ConnectionRefused;
                case SocketError.HostUnreachable:
                    return ErrorCode.HostUnreachable;
                default:
                    Debug.Assert(false);
                    return ErrorCode.Unknown;
            }
        }
    }
}
