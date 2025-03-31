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
        private string vsCode = "";
        MainWindow _mainWindow;

        public CreateTSProject(MainWindow _mainWindow) {
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
            if ((String.IsNullOrWhiteSpace(filePath) && String.IsNullOrWhiteSpace(projectName)))
            {              
                throw new Exception("File Path and Project Name not set!");
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
                if (!OpenVscode()) return;
                string fullDir = createFolders();

                string npmInit = "npm init -y";
                string tscInit = "tsc --init --sourceMap --rootDir src --outDir dist";
                string srcindexDir = Path.Combine(fullDir, "src", "index.ts");
                string precode = $"echo console.log(^\"Hello, World!^\"); > \"{srcindexDir}\"";

                CopyFiles(fullDir);

                string watchtsc = "npm i --save-dev typescript";
                string notifyCompletion = "echo Project created successfully! You can close this window if it hangs.";
                string command = $"/c cd {fullDir} && dir && {precode} && {npmInit} && {tscInit} && tsc && {watchtsc} && dir && {notifyCompletion}{vsCode}";
                Process.Start("CMD.exe", command);
            }
            catch (Exception e)
            {
                _mainWindow.lblNotifyCreation.Content = e.Message;
                Console.WriteLine(e);
            }
        }

        private bool OpenVscode()
        {
            string messageBoxText = "Open project in VSCode?";
            string caption = "Open VSCode";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
            {
                return false;
            }
            vsCode = " && code .";
            return true;
        }
        private void CopyFiles(string fullDir)
        {
            string dataFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string launchconfig = Path.Combine(dataFolderPath, "launch.json");
            if (!File.Exists(launchconfig))
            {
                _mainWindow.lblNotifyCreation.Content = "launch.json file not found!";
                throw new Exception("launch.json file not found!");
            }
            string tasksconfig = Path.Combine(dataFolderPath, "tasks.json");
            if (!File.Exists(tasksconfig))
            {
                _mainWindow.lblNotifyCreation.Content = "tasks.json file not found!";
                throw new Exception("tasks.json file not found!");
            }
            string copyTasks = Path.Combine(fullDir, ".vscode", "tasks.json");
            string copyLaunch = Path.Combine(fullDir, ".vscode", "launch.json");
            File.Copy(tasksconfig, copyTasks);
            File.Copy(launchconfig, copyLaunch);
        }
    }
}