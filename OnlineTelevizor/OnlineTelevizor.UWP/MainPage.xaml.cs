﻿using OnlineTelevizor.Models;
using OnlineTelevizor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;

namespace OnlineTelevizor.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new OnlineTelevizor.Views.App(new UWPOnlineTelevizorConfiguration()));

            MessagingCenter.Subscribe<string>(this, BaseViewModel.UriMessage, (url) =>
            {
                Task.Run(async () => await LaunchUrl(url));
            });

            KeyDown += MainPage_KeyDown;    
        }

        private void MainPage_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Plugin.Toast does not work in UWP: Could not load file or assembly Plugin.Toast, Version=2.1.0.0
            // and keys are consumed in ALL pages (for example in settings when type in username entry )!
            //MessagingCenter.Send(e.Key.ToString(), BaseViewModel.KeyMessage);
        }

        private async Task LaunchUrl(string url)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(
              new Action(
                  async () =>
                  {
                      await Launcher.LaunchUriAsync(new Uri($"vlc://openstream/?from=url&url={url}"));
                  }));
        }
    }
}
