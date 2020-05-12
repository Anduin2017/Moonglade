using Edi.Practice.RequestResponseModel;
using Moonglade.Model;
using System;
using System.Threading.Tasks;

namespace Moonglade.Core.Notification
{
    public interface IMoongladeNotificationClient
    {
        Task<Response> SendTestNotificationAsync();

        Task SendNewCommentNotificationAsync(CommentListItem model, Func<string, string> funcCommentContentFormat);

        Task SendCommentReplyNotificationAsync(CommentReplyDetail model, string postLink);

        Task SendPingNotificationAsync(PingbackHistory model);
    }
}
