using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ACS.MDB.Library.DataAccess
{
    public class ReleaseNoteDAL:BaseDAL
    {
        /// <summary>
        /// This method is used to read the Release Notes from ReleaseNotes.txt file
        /// </summary>
        /// <returns>List of String</returns>
        public List<String> GetReleaseNote(string filePath)
        {
            List<String> releaseNote = new List<String>();
            FileStream fileStream = null;
            StreamReader streamReader = null;
            
            if (File.Exists(filePath))
            {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                streamReader = new StreamReader(fileStream, Encoding.GetEncoding("Windows-1252"));

                while (streamReader.Peek() >= 0)
                {
                    string line = streamReader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        releaseNote.Add(line);
                    }
                }

                streamReader.Close();
                fileStream.Close();
            }
            else
            {
                releaseNote.Add("Release notes not found");
            }

            return releaseNote;
        }
    }
}
