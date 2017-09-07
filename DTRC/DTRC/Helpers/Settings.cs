// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace DTRC.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		public static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

        public const string RecordAudioKey = "RECORD_AUDIO";
        private static readonly bool RecordAudioDefault = true;

        public const string TakePictureKey = "TAKE_PICTURE";
        private static readonly bool TakePictureDefault = true;

        public const string PlayBeepKey = "PLAY_BEEP";
        private static readonly bool PlayBeepDefault = true;

        #endregion

        /// <summary>
        /// Check if commandId is enabled
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        public static bool CommandKeyEnabled(string commandId) {
            return AppSettings.GetValueOrDefault(commandId, true);
        }
        
        public static bool RecordAudioEnabled {
            get {
                return AppSettings.GetValueOrDefault(RecordAudioKey, RecordAudioDefault);
            }
            set {
                AppSettings.AddOrUpdateValue(RecordAudioKey, value);
            }
        }

        public static bool TakePictureEnabled {
            get {
                return AppSettings.GetValueOrDefault(TakePictureKey, TakePictureDefault);
            }
            set {
                AppSettings.AddOrUpdateValue(TakePictureKey, value);
            }
        }

        public static bool PlayBeepEnabled {
            get {
                return AppSettings.GetValueOrDefault(PlayBeepKey, PlayBeepDefault);
            }
            set {
                AppSettings.AddOrUpdateValue(PlayBeepKey, value);
            }
        }
        
    }
}