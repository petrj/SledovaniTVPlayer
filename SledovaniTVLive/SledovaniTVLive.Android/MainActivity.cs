﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using NLog;
using SledovaniTVLive.Services;
using SledovaniTVLive.Views;
using Plugin.Permissions;
using Plugin.InAppBilling;

namespace SledovaniTVLive.Droid
{
    [Activity(Label = "SledovaniTVLive", Icon = "@drawable/Icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private App _app;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // workaround pro ne-pouziti FileProvideru:
            // https://stackoverflow.com/questions/38200282/android-os-fileuriexposedexception-file-storage-emulated-0-test-txt-exposed
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            _app = new App();

            LoadApplication(_app);
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

        /* when uncommented, HW BackButton is not working when navigating with PushAsync!
         
        [Android.Runtime.Register("onKeyDown", "(ILandroid/view/KeyEvent;)Z", "GetOnKeyDown_ILandroid_view_KeyEvent_Handler")]
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            return _app.OnKeyDown(keyCode, e);
        }

        [Android.Runtime.Register("onKeyUp", "(ILandroid/view/KeyEvent;)Z", "GetOnKeyUp_ILandroid_view_KeyEvent_Handler")]
        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            return _app.OnKeyDown(keyCode, e);
        }*/
    }
}