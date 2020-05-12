using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Moonglade.Pingback
{
    public interface IPingbackReceiver
    {
        ILogger<PingbackReceiver> Logger { get; set; }
        int RemoteTimeout { get; set; }
        HttpContext HttpContext { get; }
        event PingbackReceiver.PingSuccessHandler OnPingSuccess;
        Task<PingbackValidationResult> ValidatePingRequest(HttpContext context);
        Task<PingRequest> GetPingRequest();
        PingbackServiceResponse ProcessReceivedPingback(PingRequest req, Func<bool> ifTargetResourceExists, Func<bool> ifAlreadyBeenPinged);
    }
}