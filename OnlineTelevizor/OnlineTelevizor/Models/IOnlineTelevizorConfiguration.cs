﻿using LoggerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineTelevizor.Models
{
    public interface IOnlineTelevizorConfiguration
    {
        TVAPIEnum TVApi { get; set; }

        // credentials
        string Username { get; set; }
        string Password { get; set; }
        string ChildLockPIN { get; set; }

        string KUKIsn { get; set; }

        //  user settings
        string StreamQuality { get; set; }
        string ChannelFilterGroup { get; set; }
        string ChannelFilterType { get; set; }
        string ChannelFilterName { get; set; }

        bool ShowLocked { get; set; }
        bool ShowAdultChannels { get; set; }

        /// <summary>
        /// -1    : no chanel
        /// 0     : last channel
        /// 1...n : channel number
        /// </summary>
        string AutoPlayChannelNumber { get; set; }
        string LastChannelNumber { get; set; }

        AppFontSizeEnum AppFontSize { get; set; }

        bool EnableLogging { get; set; }
        LoggingLevelEnum LoggingLevel { get; set; }

        bool Purchased { get; set; }
        bool NotPurchased { get; }

        bool AnimatedScrolling { get; set; }

        bool DoNotSplitScreenOnLandscape { get; set; }

        string PurchaseProductId { get; }

        bool DebugMode { get; set; }

        // private cached login credentailes
        string DeviceId { get; set; }
        string DevicePassword { get; set; }
    }
}
