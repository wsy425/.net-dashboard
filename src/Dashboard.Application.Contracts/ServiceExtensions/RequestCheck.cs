using System;
using Dashboard.ManualShared;
using Dashboard.Parameters;

namespace Dashboard.ServiceExtensions
{
    public class RequestCheck
    {
        public static bool ManualRequestCheckError(ManualRequest request)
        {
            return !(Enum.IsDefined(typeof(Status), request.State) && Enum.IsDefined(typeof(Algorithm),request.Name));
        }
    }
}