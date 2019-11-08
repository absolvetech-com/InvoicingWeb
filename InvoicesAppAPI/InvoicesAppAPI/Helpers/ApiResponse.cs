using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Helpers
{
    public class ApiResponse
    {
        public int status{ get; }
        public bool success { get; } = false;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; }
        public bool userstatus { get; } = false;

        public ApiResponse(int statusCode, string msg = null)
        {
            status = statusCode;
            message = msg ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            { 
            case 401:
                return "Invalid token or unauthorized user access";
            //case 404:
            //    return "Resource not found";
            //case 500:
            //    return "An unhandled error occurred";
            default:
                return null;
        }
    }
}
}
