// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace HaxOnTheWay.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

        private const string IsLoggedInTokenKey = "isloggedid_key";
        private static readonly bool IsLoggedInTokenDefault = false;

        private const string IsSignature = "isSignature_key";
        private static readonly bool IsSignatureDefault = false;

        private const string idCommand = "isIdCommand_key";
        private static readonly int idCommandDefault = 0;

		#endregion


		public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(SettingsKey, value);
			}
		}

        public static bool IsLoggedIn
        {
            get { return AppSettings.GetValueOrDefault(IsLoggedInTokenKey, IsLoggedInTokenDefault); }
            set { AppSettings.AddOrUpdateValue(IsLoggedInTokenKey, value); }
        }

        public static bool IsSinature
        {
            get { return AppSettings.GetValueOrDefault(IsSignature, IsSignatureDefault); }
            set { AppSettings.AddOrUpdateValue(IsSignature, value); }
        }

        public static int IdCommand
        {
            get
            {
                return AppSettings.GetValueOrDefault(idCommand, idCommandDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(idCommand, value);
            }
        }

	}
}