using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using DTRC.Services.Commands;
using DTRC.Utility;

using DTRC.Droid.Services.Commands;
using DTRC.Droid.Services.Commands.RecordAudio;
using Android.Media;
using DTRC.Server;

[assembly: Xamarin.Forms.Dependency(typeof(RecordAudioCommand))]
namespace DTRC.Droid.Services.Commands {
    public class RecordAudioCommand : ARecordAudioCommand {
        private delegate void Callback(bool result, string message, string path = null);

        private RecordAudioClass _recorder;
        private ServerSendFile _serverSendFile;

        private TimeSpan timerSpanToRecord;
        private double millisecondsToRecord = 0;

        private const string localFolderName = "AudioRecorded";
        private const string audioDefaultFileName = "audioFile";

        public RecordAudioCommand() {
            _recorder = new RecordAudioClass();
            _serverSendFile = new ServerSendFile();
        }


         public override void SetData(CommandParameter commandParams) {
            if (commandParams.dict.ContainsKey(TIMER_ID)) {
                string timerStr = commandParams.dict[TIMER_ID];
                try {
                    timerSpanToRecord = TimeSpan.Parse(timerStr);
                    millisecondsToRecord = timerSpanToRecord.TotalMilliseconds+500;
                }
                catch(Exception ex) {
                    Debug.WriteLine(string.Format("Unable to parse input param {0} in TimeSpan", timerStr));
                    Debug.WriteLine(ex.StackTrace);
                }
            }
        }


        public override bool Execute() {
            bool result = true;

            if (millisecondsToRecord > 0) {
                StartRecording((startResult, message, currentFilePath) => {
                    if (startResult && message == null) {
                        Thread.Sleep((int)millisecondsToRecord);
                        StopRecording(out message);
                        SendFileAudioToServer(currentFilePath, "file");
                    }
                    else {
                        Debug.WriteLine(string.Format("Unable to start recording. Message : {0}", message));
                    }
                });
            }
            else {
                result = false;
            }

            return result;
        }

        private async void StartRecording(Callback callback) {
            bool result = true;
            string message = null;
            string fileExtension = ".3gp";

            int counter = 0;
            string audioFileName = StorageUtility.UpdateFilename(audioDefaultFileName, counter) + fileExtension;
            bool fileExists = await StorageUtility.IsFileExistAsync(audioFileName, localFolderName);
            while (fileExists) {
                counter++;
                audioFileName = StorageUtility.UpdateFilename(audioFileName, counter) + fileExtension;
                fileExists = await StorageUtility.IsFileExistAsync(audioFileName, localFolderName);
            }

            string currentFilePath = StorageUtility.GetAppLocalPath() + "/" + localFolderName + "/" + audioFileName;

            result = result && await StorageUtility.CreateEmptyFileInFolder(localFolderName, audioFileName);

            result = result && _recorder.StartRecording(currentFilePath, out message);
            
            callback?.Invoke(result, message, currentFilePath);
        }

        private void StopRecording(out string message) {
            _recorder.StopRecording(out message);
            _recorder.EndRecording();
        }


        private async void SendFileAudioToServer(string filePath, string name, ServerSendFileCallback callback = null) {

            RequestBuilder requestBuilder = new RequestBuilder();
            requestBuilder
                .SetDevice_id(App.config.GetDeviceId())
                .SetDevice_tokenFirebase(App.firebaseInstanceId.Token);
            if(timerSpanToRecord != null) {
                requestBuilder.SetLength(timerSpanToRecord.ToString());
            }
            _serverSendFile.request = requestBuilder.Build();

            bool result = 
                await _serverSendFile.SendGenericFileToServer(ServerConfig.Instance.server_url_send_audio, filePath, name, callback);
        }

    }
}