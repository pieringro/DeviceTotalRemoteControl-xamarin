using System;
using System.Diagnostics;
using Firebase.Messaging;
using Android.App;

using DTRC.Services.Commands;
using System.Collections.Generic;

namespace DTRC.Services {
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService {
        const string TAG = "MyFirebaseMsgService";

        public override void OnCreate() {
            base.OnCreate();
            DTRC.Droid.Utility.IOCContainer.InitXLabResolver();
            DTRC.Droid.Utility.IOCContainer.InitFabricCrashlytics(this);
        }

        /**
         * Called when message is received.
         */
        public override void OnMessageReceived(RemoteMessage remoteMessage) {

            // There are two types of messages data messages and notification messages. Data messages are handled
            // here in onMessageReceived whether the app is in the foreground or background. Data messages are the type
            // traditionally used with GCM. Notification messages are only received here in onMessageReceived when the app
            // is in the foreground. When the app is in the background an automatically generated notification is displayed.
            // When the user taps on the notification they are returned to the app. Messages containing both notification
            // and data payloads are treated as notification messages. The Firebase console always sends notification
            // messages. For more see: https://firebase.google.com/docs/cloud-messaging/concept-options

            string remoteCommandId = string.Empty;
            IDictionary<string, string> commandParams = null;

            Debug.WriteLine("From: " + remoteMessage.From, TAG);

            if (remoteMessage.GetNotification() != null) {
                Debug.WriteLine("Notification Message Body: " + remoteMessage.GetNotification().Body, TAG);
                remoteCommandId = remoteMessage.GetNotification().Body;
            }
            else if(remoteMessage.Data != null) {
                Debug.WriteLine("Data Message: Keys: " + remoteMessage.Data.Keys + ";   "+
                    "Values: "+remoteMessage.Data.Values, 
                    TAG);
                if (remoteMessage.Data.ContainsKey("CommandId")) {
                    remoteCommandId = remoteMessage.Data["CommandId"];
                }
                commandParams = remoteMessage.Data;
            }

            bool commandResult = true;
            if (commandParams != null && commandParams.Count > 0) {
                commandResult = CommandDispatcher.getInstance().ExecuteCommandWithParams(remoteCommandId, commandParams);
            }
            else {
                commandResult = CommandDispatcher.getInstance().ExecuteCommand(remoteCommandId);
            }

            if (commandResult) {
                Debug.WriteLine(string.Format("Command \"{0}\" executed successfull!", remoteCommandId));
            }
        }

        
        

        

        
        
        /**
         * Create and show a simple notification containing the received FCM message.
         */
        void SendNotification(string messageBody) {
            Debug.WriteLine("SendNotification("+messageBody+") called", TAG);

            //Intent intent = new Intent(this, typeof(MainActivity));
            //intent.AddFlags(ActivityFlags.ClearTop);
            //PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.OneShot);
            //
            //var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            //var notificationBuilder = new NotificationCompat.Builder(this)
            //    .SetSmallIcon(Resource.Drawable.ic_stat_ic_notification)
            //    .SetContentTitle("FCM Message")
            //    .SetContentText(messageBody)
            //    .SetAutoCancel(true)
            //    .SetSound(defaultSoundUri)
            //    .SetContentIntent(pendingIntent);
            //
            //var notificationManager = NotificationManager.FromContext(this);
            //
            //notificationManager.Notify(0 /* ID of notification */, notificationBuilder.Build());
        }
    }

}
