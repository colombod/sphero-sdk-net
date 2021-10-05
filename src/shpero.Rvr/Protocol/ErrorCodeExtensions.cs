using System;

namespace shpero.Rvr.Protocol
{
    public static class ErrorCodeExtensions
    {
        public static string getApiErrorMessageFromCode(this ErrorCode errorCode)  {
            var errorMessage = "Unknown";

            switch (errorCode)
            {
                case ErrorCode.success:
                    errorMessage = "Success";
                    break;
                case ErrorCode.bad_did:
                    errorMessage = "Bad Device ID";
                    break;
                case ErrorCode.bad_cid:
                    errorMessage = "Bad Command ID";
                    break;
                case ErrorCode.not_yet_implemented:
                    errorMessage = "Command Not Implemented";
                    break;
                case ErrorCode.restricted:
                    errorMessage = "Restricted Command";
                    break;
                case ErrorCode.bad_data_length:
                    errorMessage = "Bad Data Length";
                    break;
                case ErrorCode.failed:
                    errorMessage = "Command Failed";
                    break;
                case ErrorCode.bad_data_value:
                    errorMessage = "Bad Parameter Value";
                    break;
                case ErrorCode.busy:
                    errorMessage = "Busy";
                    break;
                case ErrorCode.bad_tid:
                    errorMessage = "Bad Target ID";
                    break;
                case ErrorCode.target_unavailable:
                    errorMessage = "Target Unavailable";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(errorCode), errorCode, null);
            }

            return errorMessage;
        }
    }
}