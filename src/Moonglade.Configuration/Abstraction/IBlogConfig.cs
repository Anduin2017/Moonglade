﻿using Edi.Practice.RequestResponseModel;
using System.Threading.Tasks;

namespace Moonglade.Configuration.Abstraction
{
    public interface IBlogConfig
    {
        GeneralSettings GeneralSettings { get; set; }
        ContentSettings ContentSettings { get; set; }
        NotificationSettings NotificationSettings { get; set; }
        FeedSettings FeedSettings { get; set; }
        WatermarkSettings WatermarkSettings { get; set; }
        FriendLinksSettings FriendLinksSettings { get; set; }
        AdvancedSettings AdvancedSettings { get; set; }

        Task<Response> SaveConfigurationAsync<T>(T moongladeSettings) where T : MoongladeSettings;

        void RequireRefresh();
    }
}