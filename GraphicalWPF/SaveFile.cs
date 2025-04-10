using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalWPF
{
    internal class SaveFile
    {
        private string filePath;
        private String currentDomain = AppDomain.CurrentDomain.BaseDirectory;

        public SaveFile(string filePath)
        {
            this.filePath = filePath;
        }

        public void setFilePath(string newFilePath)
        {
            filePath = newFilePath;
        }

        public string getFilePath()
        {
            return filePath;
        }

        public string FileFormat()
        {
            return $"\"{filePath}\"";
        }
        public void CreateConfig()
        {
            System.IO.File.WriteAllText(Path.Combine(currentDomain + "config.txt"), FileFormat());
        }

        public void ReadConfig()
        {
            var reader = new System.IO.StreamReader(Path.Combine(currentDomain + "config.txt"));
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string cmd = line;
                if (cmd.StartsWith(" "))
                {
                    cmd = cmd.TrimStart();
                }
                setFilePath(cmd.Replace("\"", ""));
            }
        }

        public bool CheckExists()
        {
            if (System.IO.File.Exists(Path.Combine(currentDomain + "config.txt")))
            {
                ReadConfig();
                return true;
            }
            return false;
        }
    }
}
