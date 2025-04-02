using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace GraphicalWPF
{
    class CreateTSProject
    {
        private string filePath = null;
        private string projectName = null;
        MainWindow _mainWindow;

        public CreateTSProject(MainWindow _mainWindow)
        {
            this._mainWindow = _mainWindow;
        }

        public void setFilePath(string filePath)
        {
            this.filePath = filePath;
        }
        public void setProjectName(string projectName)
        {
            this.projectName = projectName;
        }

        private string createFolders()
        {
            if (System.IO.Directory.Exists(Path.Combine(filePath, projectName)))
            {
                throw new Exception("Project already exists!");
            }
            System.IO.Directory.CreateDirectory(Path.Combine(filePath, projectName));
            System.IO.Directory.CreateDirectory(Path.Combine(filePath, projectName, "src"));
            System.IO.Directory.CreateDirectory(Path.Combine(filePath, projectName, ".vscode"));
            string fullDir = Path.Combine(filePath, projectName);
            return fullDir;
        }

        public void createProject()
        {
            try
            {
                string vsCode = OpenVscode();
                string fullDir = createFolders();

                string npmInit = "npm init -y";
                string tscInit = "tsc --init --sourceMap --rootDir src --outDir dist";
                string srcindexDir = Path.Combine(fullDir, "src", "index.ts");

                CreateFiles(fullDir);

                string watchtsc = "npm i --save-dev typescript";
                string notifyCompletion = "echo Project created successfully!";

                string command = $"/c cd {fullDir} && dir && {npmInit} && {tscInit} && tsc && {watchtsc} && dir && {notifyCompletion} {vsCode}";

                Process.Start("CMD.exe", command);
            }
            catch (Exception e)
            {
                _mainWindow.lblNotifyCreation.Content = e.Message;
                Console.WriteLine(e);
            }
        }

        private string OpenVscode()
        {
            string messageBoxText = "Open project in VSCode?";
            string caption = "Open VSCode";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Cancel)
            {
                throw new Exception("");
            }
            else if (result == MessageBoxResult.No)
            {
                return "";
            }
            return "&& code .";
        }

        private void CreateFiles(string fullDir)
        {
            string precode = "console.log(\"Hello, World!\");";
            if (System.IO.Directory.Exists(Path.Combine(fullDir, ".vscode")))
            {
                System.IO.File.WriteAllText(Path.Combine(fullDir, ".vscode", "launch.json"), Configs("launch.json"));
                System.IO.File.WriteAllText(Path.Combine(fullDir, ".vscode", "tasks.json"), Configs("tasks.json"));
            }
            else
            {
                throw new Exception("Directory .vscode not found!");
            }

            if (System.IO.Directory.Exists(Path.Combine(fullDir, "src")))
            {
                System.IO.File.WriteAllText(Path.Combine(fullDir, "src", "index.ts"), precode);
            }
            else
            {
                throw new Exception("Directory src not found!");
            }
        }

        private string Configs(string file)
        {
            string configFile = null;
            if (file == "launch.json")
                configFile = "{\r\n  // Use IntelliSense to learn about possible attributes.\r\n " +
                " // Hover to view descriptions of existing attributes.\r\n " +
                " // For more information, visit: https://go.microsoft.com/fwlink/?linkid=830387\r\n " +
                " \"version\": \"0.2.0\",\r\n " +
                " \"configurations\": [\r\n\r\n    {\r\n     " +
                " \"type\": \"node\",\r\n   " +
                "   \"request\": \"launch\",\r\n   " +
                "   \"name\": \"Launch Program\",\r\n     " +
                " \"skipFiles\": [\r\n     " +
                "   \"<node_internals>/**\"\r\n      ],\r\n   " +
                "   \"program\": \"${workspaceFolder}\\\\dist\\\\index.js\",\r\n " +
                "     \"outFiles\": [\r\n    " +
                "    \"${workspaceFolder}/**/*.js\"\r\n  " +
                "    ]\r\n  " +
                "  }\r\n " +
                " ]\r\n}";
            else if (file == "tasks.json")
                configFile = "{\r\n  \"version\": \"2.0.0\",\r\n  " +
                "\"tasks\": [\r\n    {\r\n     " +
                " \"type\": \"typescript\",\r\n    " +
                "  \"tsconfig\": \"tsconfig.json\",\r\n    " +
                "  \"option\": \"watch\",\r\n " +
                "     \"problemMatcher\": [\r\n    " +
                "    \"$tsc-watch\"\r\n      ],\r\n  " +
                "    \"group\": {\r\n     " +
                "   \"kind\": \"build\",\r\n    " +
                "    \"isDefault\": true\r\n    " +
                "  },\r\n  " +
                "    \"label\": \"tsc: watch - tsconfig.json\"\r\n   " +
                " }\r\n  " +
                "]\r\n}";
            return configFile;
        }
    }
}