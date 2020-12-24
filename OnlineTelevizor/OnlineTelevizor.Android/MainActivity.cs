﻿using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using OnlineTelevizor.Services;
using OnlineTelevizor.Views;
using Plugin.Permissions;
using Plugin.InAppBilling;
using Xamarin.Forms;
using OnlineTelevizor.ViewModels;
using OnlineTelevizor.Models;
using Android.Content;
using Android.Graphics;
using AndroidX.Core.Graphics;
using Android.Hardware.Input;

namespace OnlineTelevizor.Droid
{
    [Activity(Label = "OnlineTelevizor", Icon = "@drawable/Icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, InputManager.IInputDeviceListener
    {
        private App _app;
        private AndroidOnlineTelevizorConfiguration _cfg;
        private Toast _toastInstance = null;

        private int _defaultUiOptions;
        private int _fullscreenUiOptions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            _cfg = new AndroidOnlineTelevizorConfiguration();
#if DEBUG
            Console.WriteLine("Debug version");

            _cfg.TVApi = TVAPIEnum.SledovaniTV;
            _cfg.Username = "luis99@seznam.cz";
            _cfg.Password = "123456";
            _cfg.InternalPlayer = true;
#else
      
#endif


            _app = new App(_cfg);

            MessagingCenter.Subscribe<string>(this, BaseViewModel.ToastMessage, (message) =>
            {
                ShowToastMessage(message, "#FFFFFF", "#3F51B5");
            });

            MessagingCenter.Subscribe<string>(this, BaseViewModel.UriMessage, (url) =>
            {
                var intent = new Intent(Intent.ActionView);
                var uri = Android.Net.Uri.Parse(url);
                intent.SetDataAndType(uri, "video/*");
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask); // necessary for Android 5
                Android.App.Application.Context.StartActivity(intent);
            });

            MessagingCenter.Subscribe<string>(this, BaseViewModel.EnableFullScreen, (msg) =>
            {
                SetFullScreen(true);
            });
            MessagingCenter.Subscribe<string>(this, BaseViewModel.DisableFullScreen, (msg) =>
            {
                SetFullScreen(false);
            });

            // prevent sleep:
            Window window = (Forms.Context as Activity).Window;
            window.AddFlags(WindowManagerFlags.KeepScreenOn);

            // https://stackoverflow.com/questions/39248138/how-to-hide-bottom-bar-of-android-back-home-in-xamarin-forms
            _defaultUiOptions = (int)Window.DecorView.SystemUiVisibility;

            _fullscreenUiOptions = _defaultUiOptions;
            _fullscreenUiOptions |= (int)SystemUiFlags.LowProfile;
            _fullscreenUiOptions |= (int)SystemUiFlags.Fullscreen;
            _fullscreenUiOptions |= (int)SystemUiFlags.HideNavigation;
            _fullscreenUiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            if (_cfg.Fullscreen)
            {
                SetFullScreen(true);
            }

            LoadApplication(_app);
        }

        private void SetFullScreen(bool on)
        {
            if (on)
            {
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)_fullscreenUiOptions;
            } else
            {
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)_defaultUiOptions;
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {

            MessagingCenter.Send(keyCode.ToString(), BaseViewModel.KeyMessage);

            return base.OnKeyDown(keyCode, e);
        }

        private void ShowToastMessage(string message, string foregroundColor = "#FFFFFF", string backgroundColor = "#3F51B5")
        {
            var length = Android.Widget.ToastLength.Short;

            // To dismiss existing toast, otherwise, the screen will be populated with it if the user do so
            _toastInstance?.Cancel();

            _toastInstance = Toast.MakeText(Android.App.Application.Context, message, length);

            var view = _toastInstance.View;
            view.Background.SetColorFilter(BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(Android.Graphics.Color.ParseColor(backgroundColor), BlendModeCompat.SrcIn));

            var textView = (TextView)view.FindViewById(Android.Resource.Id.Message);
            textView.SetTextColor(Android.Graphics.Color.ParseColor(foregroundColor));
            textView.TextSize = BaseViewModel.GetScaledSize(_cfg, 25);

            _toastInstance.SetGravity(GravityFlags.Center, 0, 0);

            _toastInstance.Show();
        }

        public void OnInputDeviceAdded(int deviceId)
        {

        }

        public void OnInputDeviceChanged(int deviceId)
        {

        }

        public void OnInputDeviceRemoved(int deviceId)
        {

        }
    }
}