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
    class App : Application {
        #region "Variables"
        public const String GCM_SENDER_ID = "348279368873";
        public static BackgroundService.ControllerServiceBinder _binder;
        private static Boolean _binderSet;
        public static BackgroundService.ServiceConnection sc;
        private static String _authToken;
        private static DateTime _lastTransmitCloud;


        #endregion

        #region "App Functions"
        public App(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer) {
        }

        public override void OnCreate() {
            base.OnCreate();
      //      StrictMode.SetThreadPolicy(new StrictMode.ThreadPolicy.Builder().PermitAll().Build());


            ApplicationContext.StartService(new Intent(ApplicationContext, typeof(BackgroundService.ControllerService)));
            BindControllerService();

            try {
                int deviceId = App.GetDeviceId(ApplicationContext) ;
                if (deviceId == 0)
                    deviceId = -2;
                

                GetAuthToken(ApplicationContext, GetUsername(ApplicationContext), GetPassword(ApplicationContext), deviceId);
            } catch {

            }

        }

        public void BindControllerService() {
            sc = new BackgroundService.ServiceConnection(this);

            this.ApplicationContext.BindService(new Intent(this, typeof(BackgroundService.ControllerService)), sc, Bind.AutoCreate);

        }

        public static BackgroundService.ControllerServiceBinder GetServiceBinder() {
                return _binder;
        }

        public static void SetServiceBinder(BackgroundService.ControllerServiceBinder value) {
            _binder = value;
        }

        public bool binderSet {
            get {
                return _binderSet;
            }
            set {
                _binderSet = value;
            }
        }
        #endregion

        #region "Device Functions"

        public static void UpdateLastTransmit() {
            _lastTransmitCloud = DateTime.Now;
        }

        public static DateTime GetLastTransmit() {
            return _lastTransmitCloud;
        }

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


        public static String GetAuthToken(Context context) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            return prefs.GetString("AuthToken", "");
        }

        public static void SetAuthToken(Context context, string authToken) {
            ISharedPreferences prefs = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);

            prefs.Edit().PutString("AuthToken", authToken).Commit();

        }

        public static String GetAuthToken(Context context, String username = "", String password = "", int deviceId = -2) {
            if (_authToken == null) {
                if (GetAuthToken(context) == "") {
                    if (username == "" || password == "")
                        throw new Exception("Username/Password cannot be null");

                    Boolean deviceSpecified = false;

                    if (deviceId != -2) {
                        deviceSpecified = true;
                    }

                    _authToken = new AuthWS.AuthSvc().GetAuthToken(username, password, deviceId, deviceSpecified);
                    SetAuthToken(context, _authToken);

                } else
                    _authToken = GetAuthToken(context);
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
                UserWS.UserDevice ud = new UserWS.UserSvc().AddUserDeviceNoGCMCode(App.GetAuthToken(context), username, Android.OS.Build.Model, 500, true, 0, false);

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
                new UserWS.UserSvc().DeleteUserDevice(App.GetAuthToken(context), App.GetDeviceId(context), true);
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