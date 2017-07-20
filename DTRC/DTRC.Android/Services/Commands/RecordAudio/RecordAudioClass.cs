using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using System.Diagnostics;
using DTRC.Utility;

namespace DTRC.Droid.Services.Commands.RecordAudio {
    class RecordAudioClass {
        private MediaRecorder _recorder;
        

        public RecordAudioClass() {
            InitRecorder();
        }

        private void InitRecorder() {
            if (_recorder == null) {
                _recorder = new MediaRecorder();
            }
        }


        public bool StartRecording(string pathFile, out string message) {
            bool result = true;
            message = null;

            try {
                InitRecorder();
                
                _recorder.SetAudioSource(AudioSource.Mic);
                _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
                _recorder.SetOutputFile(pathFile);
                _recorder.Prepare();
                _recorder.Start();
            } catch(Exception ex) {
                result = false;
                message = string.Format("Unable to start recording audio. Error: {0}", ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }

            return result;
        }

        public bool StopRecording(out string message) {
            bool result = true;
            message = null;

            try {
                _recorder.Stop();
                _recorder.Reset();
            }
            catch (Exception ex) {
                result = false;
                message = string.Format("Unable to stop recording audio. Error: {0}", ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            return result;
        }

        public void EndRecording() {
            _recorder.Release();
            _recorder.Dispose();
            _recorder = null;
        }

    }
}