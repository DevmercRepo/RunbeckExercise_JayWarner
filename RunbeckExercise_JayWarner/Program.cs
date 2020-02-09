using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;

/// <summary>
/// Class			:Program 
/// Description		:Runbeck Exercise main entry point for console app
///                  This application will 
///                  Prompt a user for a file to process
///                  Prompt a user for the file delimiter
///                  Prompt a user for the number of fields in the file
///                  The App will read the file and write the data that has the correct number of fields in one file 
///                  and write the data that has the incorrect number of fields in another file
/// Source			:
/// Copyright		:
/// ___________________________________________________________________________
/// Revisions
/// Date			Programmer			Description of Change
/// ___________________________________________________________________________
/// 02.09.2019		Jay Warner			Original Write
/// </summary>
namespace RunbeckExercise_JayWarner
{
    class Program
    {
        private static InputValidator validationHelper;               
        
        static void Main(string[] args)
        {
            validationHelper = new InputValidator();
            validationHelper.OnValidationErrorSent += Helper_OnMessageSent;

            try
            {
                AppStart();
            }
            catch(ConfigurationErrorsException ex)//we are missing a config entry .. display error and exit app
            {
                string errorMessage = string.Format("{0} {1}", UIMessages.MessageErrorUnexpected, ex.Message);
                PrintMessage(errorMessage);
                AppStall(false);
            }
            catch (Exception ex)//we had an error that we may be able recovered from .. display error and restart app
            {
                string errorMessage = string.Format("{0} {1}", UIMessages.MessageErrorUnexpected, ex.Message);
                PrintMessage(errorMessage);
                AppStall(true);
            }
        }
        /// <summary>
        /// Prompt the user for required information and process the input
        /// </summary>
        private static void AppStart()
        {
            //\\DT010G\Passport\Files
            //"c:\\jdev\\r\\Good.txt"

            Console.Clear();
            PrintFancyMessage(UIMessages.MessageWelcome);

            //grab (and validate) the necessary info from the user 
            string sourcFile = PromptUser<string>(validationHelper.IsValidFilePath, UIMessages.PromptFileLocation, UIMessages.PromptFileLocationHelp);
            InputValidator.FileDelimiter delimiter = PromptUser<InputValidator.FileDelimiter>(validationHelper.IsValidFileFormat, UIMessages.PromptFileDelimiter, UIMessages.PromptFileDelimiterHelp);
            int sourceFileFieldCount = PromptUser<int>(validationHelper.IsValidFieldCount, UIMessages.PromptFieldCount, UIMessages.PromptFieldCountHelp);

            //process the information..
            ProcessFile(sourcFile, delimiter, sourceFileFieldCount);
            AppStall(true);
        }

        /// <summary>
        /// Pause the app and give the user a press any key message
        /// </summary>
        /// <param name="restart">Should the app be restarted</param>
        private static void AppStall(bool restart)
        {
            PrintMessage(UIMessages.MessageAnyKey);
            Console.ReadLine();

            if (restart) AppStart();
        }

        /// <summary>
        /// End the application
        /// </summary>
        private static void AppExit()
        {
            PrintFancyMessage(UIMessages.MessageGoodbye);
            PrintMessage(UIMessages.MessageAnyKey);
            Console.ReadKey();
            Environment.Exit(0);
        }

        /// <summary>
        /// process the files provided and output the results
        /// </summary>
        /// <param name="sourceFile">the file that contains the data to process</param>
        /// <param name="delimiter">the file delimiter</param>
        /// <param name="sourceFileFieldCount">the number of fields in the file</param>
        private static void ProcessFile(string sourceFile, InputValidator.FileDelimiter delimiter, int sourceFileFieldCount)
        {
            char spliter = ToDelimiter(delimiter);

            //keep track of if we actual wrote data to a file
            bool validFileCreated = false;
            bool inValidFileCreated = false;
            
            IEnumerable<string> results = FileIO.Read(sourceFile, true);

            //write the rows that are valid to a file (has the correct number of fields)
            validFileCreated = FileIO.Write(UIMessages.FilePath_ValidData, results.Where(x => x.Split(spliter).Length == sourceFileFieldCount));

            //write the rows that are invalid to a file (does the have the correct number of fields)
            inValidFileCreated = FileIO.Write(UIMessages.FilePath_InValidData, results.Where(x => x.Split(spliter).Length != sourceFileFieldCount));

            PrintMessage(UIMessages.MessageProcessingComplete);


            //if we wrote data to a file display the file name and path
            if (validFileCreated | inValidFileCreated)
            {
                PrintMessage(UIMessages.MessageOutputLocation, Path.GetDirectoryName(UIMessages.FilePath_ValidData));
            }
            else PrintMessage(UIMessages.MessageEmptyFile);//no data was written let the user know
        }

        /// <summary>
        /// Prompt a user for input and validate the input is acceptable 
        /// Acceptable input will be converted to the correct type     
        /// </summary>
        /// <typeparam name="T">The allowed data type for the given input</typeparam>
        /// <param name="validator">A delegate that will validate the input for acceptable input</param>
        /// <param name="request">The text that prompts the user for a specific input</param>
        /// <param name="helpText">Help text for the prompt</param>
        /// <returns></returns>
        private static T PromptUser<T>(InputValidator.Validator validator, string request, string helpText)
        {           
            string response;
            //prompt the user until acceptable input is received
            //the user can also choose to ask for help, restart or exit the application
            do
            {
                Console.Write(string.Format("{0} {1}", request, UIMessages.MessageHelpExitRestart));
                response = Console.ReadLine().Trim().ToUpper();
            }
            while 
            (
                response != UIMessages.RequestForHelpKey
                && response != UIMessages.RequetToExitKey
                && response!= UIMessages.RequestToRestartKey
                && !validationHelper.ValidateUserInput(validator, response)
            );

            if (response == UIMessages.RequestToRestartKey) AppStart();//restart the application 
            if (response == UIMessages.RequetToExitKey) AppExit();// exit the application
            if (response == UIMessages.RequestForHelpKey)//display help and prompt the user again
            {
                PrintFancyMessage(UIMessages.MessageHelpTip, helpText);
                return PromptUser<T>(validator, request, helpText);
            }

            try
            {
                //convert the user input into the required data type
                return validationHelper.ConvertInput<T>(validator, response);
            }
            catch (InvalidCastException ex)//we could not cast the input to the expected type.. ask for the input again
            {
                string errorMessage = string.Format("{0}. Please re-enter the value", ex.Message);
                PrintMessage(UIMessages.MessageError, errorMessage);
                return PromptUser<T>(validator, request, helpText);
            }
        }      

        /// <summary>
        /// Convert the FileDelimiter enum to an acutal delimiter
        /// </summary>
        /// <param name="value">Enum to convert to a delimiter</param>
        /// <returns></returns>
        private static char ToDelimiter(InputValidator.FileDelimiter value)
        {
            switch (value)
            {
                case InputValidator.FileDelimiter.CSV:
                    return ',';
                case InputValidator.FileDelimiter.TSV:
                    return '\t';
                default://we did not code against this delimiter type... throw out..
                    string errorMessage = string.Format("Unsupported file delimiter detected: {0}", value.ToString());
                    throw new InvalidEnumArgumentException(errorMessage);
            }
        }
       
        /// <summary>
        /// Handle any messages sent from the InputValidator
        /// </summary>
        /// <param name="message">text of message sent from InputValidator</param>
        private static void Helper_OnMessageSent(string message)
        {
            PrintMessage(UIMessages.MessageError, message);
        }       
        
        private static void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        private static void PrintMessage(string header, string message)
        {
            if (!String.IsNullOrEmpty(header)) Console.WriteLine(header);
            if (!String.IsNullOrEmpty(message)) Console.WriteLine(message);
        }

        private static void PrintFancyMessage(string message)
        {
            PrintFancyMessage(message, string.Empty);
        }
        private static void PrintFancyMessage(string header, string message)
        {
            string headerFooter = string.Concat(Enumerable.Repeat("*", 50));

            Console.WriteLine();
            Console.WriteLine(headerFooter);
            if (!String.IsNullOrEmpty(header)) Console.WriteLine(header);
            if (!String.IsNullOrEmpty(message)) Console.WriteLine(message);
            Console.WriteLine(headerFooter);
            Console.WriteLine();
        }
    }
}
