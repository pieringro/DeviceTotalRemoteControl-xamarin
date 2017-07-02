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
        private delegate void Callback(bool result, string message);

        private RecordAudioClass _recorder;
        private double millisecondsToRecord = 0;

        private const string localFolderName = "AudioRecorded";
        private const string audioDefaultFileName = "audioFile";

        public RecordAudioCommand() {
            _recorder = new RecordAudioClass();
        }


        public override void SetData(CommandParameter commandParams) {
            if (commandParams.dict.ContainsKey(TIMER_ID)) {
                string timerStr = commandParams.dict[TIMER_ID];
                try {
                    TimeSpan timerSpan = TimeSpan.Parse(timerStr);
                    millisecondsToRecord = timerSpan.TotalMilliseconds+500;
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
                StartRecording((startResult, message) => {
                    if (startResult && message == null) {
                        Thread.Sleep((int)millisecondsToRecord);
                        StopRecording(out message);
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

            if (result) {
                result = await StorageUtility.CreateEmptyFileInFolder(localFolderName, audioFileName);
            }

            if (result) {
                result = _recorder.StartRecording(localFolderName+"/"+audioFileName, out message);
            }

            callback?.Invoke(result, message);
        }

        private void StopRecording(out string message) {
            _recorder.StopRecording(out message);
            _recorder.EndRecording();


        }


        private async void SendFileAudioToServer(string filePath, string name, Callback callback = null) {
            bool result = true;
            string message = null;
            try {
                ServerRequest serverRequest = new ServerRequest();

                RequestBuilder requestBuilder = new RequestBuilder();
                Request request = requestBuilder
                    .SetDevice_id(App.config.GetDeviceId())
                    .SetDevice_tokenFirebase(App.firebaseInstanceId.Token)
                    .Build();

                string responseString = await serverRequest.SendFileToServerAsync(ServerConfig.Instance.server_url_send_audio,
                    filePath, name, request);
                Response response = ServerResponse.ParsingJsonResponse(responseString);
                if (!response.Error) {
                    Debug.WriteLine(string.Format("File {0} inviato con successo"));
                }
                else {
                    Debug.WriteLine(
                        string.Format("Il server ha restituito un errore durante l'invio del file {0}. Messaggio : {1}",
                            name, response.Message));
                    result = false;
                    message = string.Format("Il server ha restituito un errore durante l'invio del file {0}. Messaggio : {1}",
                            name, response.Message);
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(string.Format("Non e' stato possibile l'invio del file {0}.", name));
            }

            callback?.Invoke(result, message);
        }

    }
}