using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff
{
    public static class FileUtility
    {
        public static void SaveDataToFile(string FileName, string Data, bool isOverwrite = false)
        {
            if (File.Exists(FileName))
            {
                if (isOverwrite)
                {
                    File.WriteAllText(FileName, Data);
                }
                else
                {
                    string fileContents = File.ReadAllText(FileName);
                    fileContents = fileContents + Data;
                    File.WriteAllText(FileName, fileContents);
                }
            }
            else
            {
                Directory.CreateDirectory(new FileInfo(FileName).DirectoryName);
                File.WriteAllText(FileName, Data);
            }
        }

        public static FileStream GetFileStream(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                Directory.CreateDirectory(new FileInfo(FilePath).DirectoryName);
            }

            FileStream file = new FileStream(FilePath, FileMode.Create, System.IO.FileAccess.Write);

            return file;
        }
    }
}
