using System.Collections.Generic;
using System.IO;

/// <summary>
/// Class			:FileIO
/// Description		:Class to read and write text files
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
    public static class FileIO
    {       
        /// <summary>
        /// Write a text stream to a file
        /// </summary>
        /// <param name="file">Path and name of file to write to</param>
        /// <param name="text">Text to write to file</param>
        /// <returns>true if there was data to write and a file was persisted</returns>
        public static bool Write(string file, IEnumerable<string> text)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file))) Directory.CreateDirectory(Path.GetDirectoryName(file));
            if (File.Exists(file)) File.Delete(file);

            int rowCount = 0;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, true))
            {
                foreach (string line in text)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        sw.WriteLine(line);
                        rowCount++;
                    }
                }
            }
            if (rowCount < 1)
            {
                File.Delete(file);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Read a text file into a stream
        /// </summary>
        /// <param name="file">Path and name of file to read from</param>
        /// <param name="skipHeaderRow">Should the first row be read</param>
        /// <returns>An enumerable steam of text</returns>
        public static IEnumerable<string> Read(string file, bool skipHeaderRow=true)
        {
            if (File.Exists(file))
            {
                using (StreamReader reader = File.OpenText(file))
                {
                    if (skipHeaderRow) reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string text = reader.ReadLine();
                        if (!string.IsNullOrEmpty(text)) yield return text;
                    }
                }
            }
            else
            {
                string error = string.Format("File missing: {0}", file);
                throw new FileNotFoundException(error);
            }
        }
    }
}
