using Android.Media;
using DTRC.Services.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using DTRC.Droid.Services.Commands;

[assembly: Xamarin.Forms.Dependency(typeof(PlayBeepCommand))]
namespace DTRC.Droid.Services.Commands {
    
    /// <summary>
    /// Command semplice che riproduce un beep
    /// </summary>
    public class PlayBeepCommand : APlayBeepCommand {

        public new static string Id {
            get {
                return "PLAY_BEEP";
            }
        }
        
        public PlayBeepCommand() {
            _player = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.wolfs);
        }


        public override void SetData() {

        }


        MediaPlayer _player;

        public override bool Execute() {
            bool result = true;

            _player.Start();

            return result;
        }

    }
}
