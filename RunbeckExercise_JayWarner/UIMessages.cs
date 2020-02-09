/// <summary>
/// Class			:UIMessages
/// Description		:Static class to grab entries from the app config file
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
    public static class UIMessages
    {        
        //files to write to 
        public static string FilePath_ValidData
        {
            get =>"FilePath_ValidData".GetAppSettingConfigEntry();
        }

        public static string FilePath_InValidData
        {
            get => "FilePath_InValidData".GetAppSettingConfigEntry();
        }

        //keyboard keys for specific actions
        public static string RequestForHelpKey
        {
            get => "RequestForHelpKey".GetAppSettingConfigEntry();
        }
        public static string RequetToExitKey
        {
            get => "RequetToExitKey".GetAppSettingConfigEntry();
        }
        public static string RequestToRestartKey
        {
            get => "RequestToRestartKey".GetAppSettingConfigEntry();
        }


        //messages to the user 
        public static string MessageHelpExitRestart
        {
            get
            {
                string appEntry = "MessageHelpExitRestart".GetAppSettingConfigEntry(); 
                string message = string.Format(appEntry, UIMessages.RequestForHelpKey, UIMessages.RequetToExitKey, UIMessages.RequestToRestartKey);

                return message;
            }
        }
        public static string MessageWelcome
        {
            get => "MessageWelcome".GetAppSettingConfigEntry();
        }
        public static string MessageGoodbye
        {
            get => "MessageGoodbye".GetAppSettingConfigEntry();
        }
        public static string MessageAnyKey
        {
            get => "MessageAnyKey".GetAppSettingConfigEntry();
        }
        public static string MessageHelpTip
        {
            get => "MessageHelpTip".GetAppSettingConfigEntry();
        }
        public static string MessageEmptyFile
        {
            get => "MessageEmptyFile".GetAppSettingConfigEntry();
        }
        public static string MessageProcessingComplete
        {
            get => "MessageProcessingComplete".GetAppSettingConfigEntry();
        }
        public static string MessageOutputLocation
        {
            get => "MessageOutputLocation".GetAppSettingConfigEntry();
        }
        public static string MessageError
        {
            get => "MessageError".GetAppSettingConfigEntry();
        }
        public static string MessageErrorUnexpected
        {
            get => "MessageErrorUnexpected".GetAppSettingConfigEntry();
        }

        //user prompts and help text
        public static string PromptFileLocation
        {
            get => "PromptFileLocation".GetAppSettingConfigEntry();
        }
        public static string PromptFileLocationHelp
        {
            get => "PromptFileLocationHelp".GetAppSettingConfigEntry();
        }

        public static string PromptFileDelimiter
        {
            get => "PromptFileDelimiter".GetAppSettingConfigEntry();
        }
        public static string PromptFileDelimiterHelp
        {
            get => "PromptFileDelimiterHelp".GetAppSettingConfigEntry();
        }

        public static string PromptFieldCount
        {
            get => "PromptFieldCount".GetAppSettingConfigEntry();
        }
        public static string PromptFieldCountHelp
        {
            get => "PromptFieldCountHelp".GetAppSettingConfigEntry();
        }
    }
}
