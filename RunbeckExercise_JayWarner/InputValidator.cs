using System;
using System.IO;

/// <summary>
/// Class			:InputValidator
/// Description		:Class that validates a users input
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
    public class InputValidator
    {
        public enum FileDelimiter { CSV, TSV };//acceptable file formats        

        public delegate void SendValidationError(string message);
        public event SendValidationError OnValidationErrorSent;//event for sending a error message back to the caller
        
        public delegate bool Validator(string value);//delegate for validating user input        
      
        private const string INVALID_FIELDCOUNT = "Field count must be a number";
        private const string INVALID_FILEPATH = "File was not found.";
        private const string INVALID_FILEFORMAT = "File format is not supported";
        private const string INVALID_INPUT = "Invalid input detected";
        
        /// <summary>
        /// Check if the input is valid for the required type
        /// </summary>
        /// <param name="validator">Method to validate the input with</param>
        /// <param name="value">value to be validated</param>
        /// <returns>true if the value is valid for the desired type</returns>
        public bool ValidateUserInput(Validator validator, string value)
        {            
            return validator(value);
        }

        /// <summary>
        /// Convert the user input to the correct type
        /// This will also validate the input
        /// </summary>
        /// <typeparam name="T">Type the input should be converted to</typeparam>
        /// <param name="validator">Method to validate against</param>
        /// <param name="value">value to be validated and converted</param>
        /// <returns></returns>
        public T ConvertInput<T>(Validator validator,string value)
        {
            if (ValidateUserInput(validator, value))//double check the validation
            {
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), value, true);
                }
                else return (T)Convert.ChangeType(value, typeof(T));
            }

            //if we don't pass validation throw an exception..
            throw new InvalidCastException(INVALID_INPUT);
        }      
                
        /// <summary>
        /// Validate a string is a path and file to an existing file
        /// </summary>
        /// <param name="value">Path and file to validate</param>
        /// <returns>true if valid</returns>
        public bool IsValidFilePath(string value)
        {
            if (File.Exists(value)) return true;

            RaiseInvalidInput(INVALID_FILEPATH);
            return false;
        }
     
        /// <summary>
        /// Validate if the input is of type FileFormat a valid file format 
        /// </summary>
        /// <param name="value">file format to validate</param>
        /// <returns>true if valid</returns>
        public bool IsValidFileFormat(string value)
        {
            FileDelimiter result;
            if (Enum.TryParse(value, true, out result)) return true;

            RaiseInvalidInput(INVALID_FILEFORMAT);
            return false;
        }
        
        /// <summary>
        /// Validate if the input is of type INT and a valid field count
        /// </summary>
        /// <param name="value">filed count to validate</param>
        /// <returns>true if valid</returns>
        public bool IsValidFieldCount(string value)
        {
            int result;
            if (int.TryParse(value, out result)) return true;
            
            RaiseInvalidInput(INVALID_FIELDCOUNT);
            return false;
        }

        /// <summary>
        /// Raise a error message to any callers
        /// </summary>
        /// <param name="errorDetail">Error message to send to caller</param>
        private void RaiseInvalidInput(string errorDetail)
        {
            if (!string.IsNullOrEmpty(errorDetail))
            {
                OnValidationErrorSent?.Invoke(errorDetail);
            }
        }      
    }
}

