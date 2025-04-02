using System;
using System.Diagnostics;
using System.IO;

using System.Windows;

namespace GraphicalWPF
{
    class CreateRustProject
    {
        private string filePath = null;
        private string projectName = null;
        MainWindow _mainWindow;

        public CreateRustProject(MainWindow _mainWindow)
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
            if ((String.IsNullOrWhiteSpace(filePath) && String.IsNullOrWhiteSpace(projectName)))
            {
                throw new Exception("File Path and Project Name not set!");
            }
            if (System.IO.Directory.Exists(Path.Combine(filePath, projectName)))
            {
                throw new Exception("Project already exists!");
            }
            string fullDir = Path.Combine(filePath, projectName);
            return fullDir;
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

        public void createProject()
        {
            try
            {
                string vsCode = OpenVscode();
                string fullDir = createFolders();
                string cargoNew = $"cargo new {projectName}", compileRs = "cargo build";
                string notifyCompletion = "echo Project created successfully!";
                string command = $"/c cd \"{filePath}\" && dir && {cargoNew} && cd \"{fullDir}\" && {compileRs} && dir && {notifyCompletion} {vsCode}";
                Process.Start("CMD.exe", command);
            }
            catch (Exception e)
            {
                _mainWindow.lblNotifyCreation.Content = e.Message;
                Console.WriteLine(e);
            }
        }
    }
}
