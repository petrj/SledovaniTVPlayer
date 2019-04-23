﻿using Android.Content;
using LoggerService;
using SledovaniTVLive.Models;
using SledovaniTVLive.Services;
using SledovaniTVLive.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SledovaniTVLive.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPage : ContentPage
    {
        private FilterPageViewModel _viewModel;
        private ISledovaniTVConfiguration _config;

        public FilterPage(ILoggingService loggingService, ISledovaniTVConfiguration config, Context context, TVService service)
        {
            InitializeComponent();

            _config = config;
            var dialogService = new DialogService(this);

            GroupPicker.SelectedIndexChanged += GroupPicker_SelectedIndexChanged;            

            BindingContext = _viewModel = new FilterPageViewModel(loggingService, config, dialogService, context, service);            

        }

        private void GroupPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GroupPicker.SelectedItem != null &&
                GroupPicker.SelectedItem is FilterItem)
                {
                    _config.ChannelGroup = (GroupPicker.SelectedItem as FilterItem).Name;
                }
        }

        public string ChannelNameFilter
        {
            get
            {
                return _viewModel.ChannelNameFilter;
            }
            set
            {
                _viewModel.ChannelNameFilter = value;
            }
        }


        public FilterItem SelectedGroupItem
        {
            get
            {
                return _viewModel.SelectedGroupItem;
            }
            set
            {
                _viewModel.SelectedGroupItem = value;
            }
        }

        

        private async void Group_Tapped(object sender, ItemTappedEventArgs e)
        {
            await Task.Run(() =>           
            {
                var filterItem = e.Item as FilterItem;
                if (filterItem == _viewModel.Groups[0])
                {
                    _config.ChannelGroup = "*";
                }
                else
                {
                    _config.ChannelGroup = filterItem.Name;
                }
            });            
        }

        private async void Type_Tapped(object sender, ItemTappedEventArgs e)
        {
            await Task.Run(() =>
            {
                var filterItem = e.Item as FilterItem;

                if (filterItem == _viewModel.Types[0])
                {
                    _config.ChannelType = "*";
                }
                else
                {
                    _config.ChannelType = filterItem.Name;
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.RefreshCommand.Execute(null);
        }
    }
}