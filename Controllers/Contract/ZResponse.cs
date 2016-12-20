using System;
using System.Runtime.Serialization;

namespace Zippy.Controllers.Contract
{
    [DataContract]
    public class ZResponse
    {
        private const int SUCCESS_CODE = 0;
        private const int INTERNAL_SERVER_ERROR = 500;
        private const string SUCCESS_MSG = "Success";
        private const string FAILURE_MSG = "Failure";

        public ZResponse()
        {
        }

        public ZResponse(object payload) : this(SUCCESS_CODE, SUCCESS_MSG, payload)
        {
        }

        public ZResponse(int status, string message, object payload = null)
        {
            Status = status;
            Message = string.IsNullOrEmpty(message) ? FAILURE_MSG : message;
            Payload = payload;
        }

        public ZResponse(Exception ex) : this(
            INTERNAL_SERVER_ERROR,
            ex?.Message ?? "Request operation encountered an error",
            // Provide stack trace only in DEV mode
            true ? ex?.StackTrace : null
        )
        {
        }

        [DataMember]
        public int Status { get; }

        [DataMember]
        public string Message { get; }

        [DataMember]
        public object Payload { get; }
    }
}