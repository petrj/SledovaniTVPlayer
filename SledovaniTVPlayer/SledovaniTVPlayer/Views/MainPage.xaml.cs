﻿using LoggerService;
using SledovaniTVAPI;
using SledovaniTVPlayer.Models;
using SledovaniTVPlayer.Services;
using SledovaniTVPlayer.ViewModels;
using SledovaniTVPlayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Content;

namespace SledovaniTVPlayer.Views
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;
        private SettingsPage _settingsPage;
        private DialogService _dialogService;
        private Context _context;
        private string _credentialsTmp;

        public MainPage(ILoggingService loggingService, ISledovaniTVConfiguration config, Context context)
        {
            InitializeComponent();

            _settingsPage = new SettingsPage(loggingService, config);
            _settingsPage.Disappearing += _settingsPage_Disappearing;

            _dialogService = new DialogService(this);
            _context = context;

            BindingContext = _viewModel = new MainPageViewModel(loggingService, config, _dialogService, context);
        }

        private void _settingsPage_Disappearing(object sender, EventArgs e)
        {
            _viewModel.ResetConnection();
        }

        private async void ToolbarItemSettings_Clicked(object sender, EventArgs e)
        {
            await _viewModel.NavigateToPage(_settingsPage, Navigation);

        }

        private async void Channel_Tapped(object sender, ItemTappedEventArgs e)
        {
            var channel = e.Item as TVChannel;

            //var dlgResult = await _dialogService.Confirm($"Stream {channel.Name}?");
            //if (dlgResult)
            //{
            //    ((ListView)sender).SelectedItem = null;
            //    await _viewModel.PlayStream(channel.Url);
            //}

            await _viewModel.PlayStream(channel.Url);
        }
    }
}
