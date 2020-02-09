using System.Configuration;

/// <summary>
/// Class			:Extensions
/// Description		:Class that contains extensions for .net types
/// Source			:
/// Copyright		:
/// ___________________________________________________________________________
/// Revisions
/// Date			Programmer			Description of Change
/// ___________________________________________________________________________
/// 02.09.2020		Jay Warner			Original Write
/// </summary>

namespace RunbeckExercise_JayWarner
{
    public static class Extensions
    {
        /// <summary>
        /// Read a appsetting entry from the config file 
        /// </summary>
        /// <param name="value">AppSetting entry to read</param>
        /// <returns>Value of appsettings entry or if not found an exception</returns>
        public static string GetAppSettingConfigEntry(this string value)
        {
            string results = @ConfigurationManager.AppSettings[value];
            if (results == null)
            {
                string error = string.Format("Configuration Entry not found: {0}", value);
                throw new ConfigurationErrorsException(error);
            }
            return results;
        }
    }
}
