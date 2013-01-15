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

namespace ComputeAndroidApp {
    class App {
        public const String GCM_SENDER_ID = "348279368873";

        public static BackgroundService.ControllerServiceBinder binder;

        public static BackgroundService.ServiceConnection sc = new BackgroundService.ServiceConnection();

        public static void BindService(Context context) {
           
            context.BindService(new Intent(context, typeof(BackgroundService.ControllerService)), ComputeAndroidApp.App.sc, Bind.AutoCreate);
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

        public static String RegisterDevice(Context context) {
            // do GCM registration
            GCMSharp.Client.GCMRegistrar.CheckDevice(context);
            GCMSharp.Client.GCMRegistrar.CheckManifest(context);
            GCMSharp.Client.GCMRegistrar.Register(context, BroadcastReceiver.SENDER_ID);
            String gcmRegId = GCMSharp.Client.GCMRegistrar.GetRegistrationId(context);
            setGCMCode(context, gcmRegId);

            int deviceId = -1;
            bool outDeviceId = false;
            // do compue app registration
            try {
                new UserWS.UserSvc().GetDeviceId(GetUsername(context), GetPassword(context), gcmRegId, out deviceId, out outDeviceId);
            } catch (Exception ex) {
                // Ignore excptions thrown hfrom ehre
            }
            
             setDeviceId(context, deviceId);

            if (GetDeviceId(context) == -1) {
                
                UserWS.UserDevice ud = new UserWS.UserSvc().AddUserDevice(GetUsername(context), GetPassword(context), Android.OS.Build.Model, 500, true, 0, false, gcmRegId);
                setDeviceId(context, ud.DeviceIdk__BackingField);

            }
            
            return gcmRegId;
            
        }

        public static void DeregisterDevice(Context context) {
            try {
                new UserWS.UserSvc().DeleteUserDevice(App.GetUsername(context), App.GetPassword(context), App.GetDeviceId(context), true);
                App.setGCMCode(context, "");
                App.setDeviceId(context, -1);

                context.StartActivity(typeof(LogonActivity));
            } catch (Exception ex) {
                App.HandleException(ex, context);

            }
        }
    }
}