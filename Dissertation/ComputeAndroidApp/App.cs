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

namespace ComputeAndroidApp {
    class App {
        public const String GCM_SENDER_ID = "348279368873";
        private static UserWS.UserSvc _userWS;

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

        public static UserWS.UserSvc UserWS {
            get { 
                if(_userWS == null) {
                    _userWS = new UserWS.UserSvc();
                }
                
                return _userWS;
            }
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

        public static String RegisterDevice(Context context) {
            // do GCM registration
            GCMSharp.Client.GCMRegistrar.CheckDevice(context);
            GCMSharp.Client.GCMRegistrar.CheckManifest(context);
            GCMSharp.Client.GCMRegistrar.Register(context, BroadcastReceiver.SENDER_ID);
            String gcmRegId = GCMSharp.Client.GCMRegistrar.GetRegistrationId(context);
            setGCMCode(context, gcmRegId);

            int deviceId;
            bool outDeviceId;
            // do compue app registration
            new UserWS.UserSvc().GetDeviceId(GetUsername(context), GetPassword(context), gcmRegId, out deviceId, out outDeviceId);

            if (outDeviceId == false || deviceId == null) {
                setDeviceId(context, -1);
            } else {
                setDeviceId(context, deviceId);
            }

            if (GetDeviceId(context) == -1) {
                
                UserWS.UserDevice ud = new UserWS.UserSvc().AddUserDevice(GetUsername(context), GetPassword(context), Android.OS.Build.Model, 500, true, 0, false, gcmRegId);
                setDeviceId(context, ud.DeviceIdk__BackingField);

            }
            
            return gcmRegId;
            
        }
    }
}