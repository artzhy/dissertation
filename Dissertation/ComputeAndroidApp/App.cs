using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Util;

using Newtonsoft.Json;
using Android.Content.PM;

namespace ComputeAndroidApp {
    [Application]
    class App : Application, BackgroundService.IAppConn {
        #region "Variables"
        public const String GCM_SENDER_ID = "348279368873";
        private BackgroundService.ControllerServiceBinder _binder;
        private Boolean _binderSet;
        public BackgroundService.ServiceConnection sc;
        private static String _authToken;

        #endregion

        #region "App Functions"
        public App(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer) {
        }

        public override void OnCreate() {
            base.OnCreate();

            ApplicationContext.StartService(new Intent(ApplicationContext, typeof(BackgroundService.ControllerService)));
            BindControllerService();
        }

        public void BindControllerService() {
            sc = new BackgroundService.ServiceConnection(this);

            this.ApplicationContext.BindService(new Intent(this, typeof(BackgroundService.ControllerService)), sc, Bind.AutoCreate);

        }

        public BackgroundService.ControllerServiceBinder ServiceBinder {
            get {
                return this._binder;
            }
            set {
                this._binder = value;
            }
        }

        public bool binderSet {
            get {
                return this._binderSet;
            }
            set {
                this._binderSet = value;
            }
        }
        #endregion

        #region "Device Functions"

        /// <summary>
        ///  
        /// </summary>
        /// <param name="context"></param>
        /// <returns>-1 for no device id</returns>
        public static int GetDeviceId(Context context) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);

            return prefs.GetInt("DeviceID", -1);
        }

        public static void setDeviceId(Context context, int deviceId) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);

            prefs.Edit().PutInt("DeviceID", deviceId).Commit();
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns "" for no user</returns>
        public static String GetUsername(Context context) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            return prefs.GetString("Username", "");
        }

        public static void setUsername(Context context, string username) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);

            prefs.Edit().PutString("Username", username).Commit();

        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns "" for no password</returns>
        public static String GetPassword(Context context) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            return prefs.GetString("Password", "");
        }

        public static void setPassword(Context context, string password) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);

            prefs.Edit().PutString("Password", password).Commit();
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns "" for no GCMCode</returns>
        public static String GetGCMCode(Context context) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            return prefs.GetString("GCMCode", "");
        }

        public static void setGCMCode(Context context, string GCMCode) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);

            prefs.Edit().PutString("GCMCode", GCMCode).Commit();
        }

        public static String GetAuthToken(String username = "", String password = "", int deviceId = -2) {
            if (_authToken == null) {
                if (username == "" || password == "")
                    throw new Exception("Username/Password cannot be null");

                Boolean deviceSpecified = false;

                if (deviceId != -2) {
                    deviceSpecified = true;
                }

                _authToken = new AuthWS.AuthSvc().GetAuthToken(username, password, deviceId, deviceSpecified);
            }
            return _authToken;
        }

        //new AuthWS.AuthSvc().GetAuthToken(username, password);

        public static void ShowDialog(String title, String message, Context context) {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetCancelable(true);
            builder.SetTitle(title);
            builder.SetInverseBackgroundForced(true);
            builder.SetMessage(message);
            builder.SetPositiveButton("OK", delegate {

            });
            AlertDialog alert = builder.Create();
            alert.Show();
        }

        public static void HandleException(Exception e, Context context, Boolean showDialog = true) {
            if (showDialog) {
                AlertDialog.Builder builder = new AlertDialog.Builder(context);
                builder.SetCancelable(true);
                builder.SetTitle("Error Logged");
                builder.SetInverseBackgroundForced(true);
                builder.SetMessage(e.Message);
                builder.SetPositiveButton("OK", delegate {
                });
                AlertDialog alert = builder.Create();
                alert.Show();
            }

            Log.Error(e.Source, e.Message + " \n   " + e.InnerException);
        }

        public static void RegisterDeviceToUserNoGCM(Context context, String username) {
                UserWS.UserDevice ud = new UserWS.UserSvc().AddUserDeviceNoGCMCode(username, Android.OS.Build.Model, 500, true, 0, false);

                App.setDeviceId(context, ud.DeviceIdk__BackingField);

                RegisterDeviceGCM(context);
        }

        public static void RegisterDeviceGCM(Context context) {
                // do GCM registration
                GCMSharp.Client.GCMRegistrar.CheckDevice(context);
                GCMSharp.Client.GCMRegistrar.CheckManifest(context);

                GCMSharp.Client.GCMRegistrar.Register(context, BroadcastReceiver.SENDER_ID);

            // return gcmRegId;

        }

        public static void DeregisterDevice(Context context) {
            try {
                new UserWS.UserSvc().DeleteUserDevice(App.GetAuthToken(), App.GetDeviceId(context), true);
                App.setGCMCode(context, "");
                App.setDeviceId(context, -1);

                context.StartActivity(typeof(LogonActivity));
            } catch (Exception ex) {
                App.HandleException(ex, context);

            }
        }

        public static Dictionary<String, int> GetComputeInstalledApps(Context context) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            return JsonConvert.DeserializeObject<Dictionary<String, int>>(prefs.GetString("InstalledComputeApps", null));

        }

        public static void SetComputeInstalledApps(Context context, Dictionary<String, int> apps) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            prefs.Edit().PutString("InstalledComputeApps", JsonConvert.SerializeObject(apps)).Commit();

        }


        public static void GetInstalledApplications() {

            IEnumerable<ApplicationInfo> packages = App.Context.PackageManager.GetInstalledApplications(PackageInfoFlags.MetaData);

            foreach (ApplicationInfo packageInfo in packages) {

                Log.Error("TAG", "Installed package :" + packageInfo.PackageName);

                // the getLaunchIntentForPackage returns an intent that you can use with startActivity()
            }

        }


        #endregion

        #region "General Utilites"
        public static String SerializeToJson(Object o) {
            return JsonConvert.SerializeObject(o);
        }

        #endregion

    }
}