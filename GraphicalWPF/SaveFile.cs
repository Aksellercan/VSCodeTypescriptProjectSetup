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
        private string type;
        private String currentDomain = AppDomain.CurrentDomain.BaseDirectory;
        private TypeObject[] typeObjects = new TypeObject[2];

        public SaveFile(string filePath, string type)
        {
            this.filePath = filePath;
            this.type = type;
        }

        public void setFilePath(string newFilePath)
        {
            this.filePath = newFilePath;
        }

        public void setType(string newType)
        {
            this.type = newType;
        }
        public string getType()
        {
            return type;
        }

        private string FileFormat()
        {
            return $"{type},\"{filePath}\"";
        }

        public void CreateConfig()
        {
            if (CheckExists())
            {
                StreamWriter sw = File.AppendText(Path.Combine(currentDomain + "config.txt"));
                sw.WriteLine($"\n{FileFormat()}");
                sw.Close();
            }
            else
            {
                System.IO.File.WriteAllText(Path.Combine(currentDomain + "config.txt"), FileFormat());
            }
        }

        public void ReadConfig()
        {
            var reader = new System.IO.StreamReader(Path.Combine(currentDomain + "config.txt"));
            string line;
            int count = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                values[0] = values[0].Trim(); //type
                values[1] = values[1].Trim(); //directory

                string cmd = values[1];
                string types = values[0];
                if (cmd.StartsWith(" "))
                {
                    cmd = cmd.TrimStart();
                }
                typeObjects[count] = new TypeObject(types, cmd.Replace("\"", ""));
                count++;
            }
        }

        public string getObjectProp(string type) 
        {
            if (String.IsNullOrWhiteSpace(type)) return "null";
            switch (type) 
            {
                case "TypeScript":
                    return (typeObjects[0] == null ? null: typeObjects[0].getfilePath());
                case "Rust":
                    return (typeObjects[1] == null ? null : typeObjects[1].getfilePath());
                default:
                    return null;
            }
        }

        public bool CheckExists()
        {
            if (System.IO.File.Exists(Path.Combine(currentDomain + "config.txt")))
            {
                return true;
            }
            return false;
        }
    }
}