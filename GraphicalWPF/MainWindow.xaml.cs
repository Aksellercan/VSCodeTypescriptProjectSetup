using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GraphicalWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath = null;
        string projectName = null;
        private CreateTSProject project;

        public MainWindow()
        {
            InitializeComponent();
            project = new CreateTSProject(this);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (checkEmpty())
            {      
                return;
            }
            else if (projectName.StartsWith(" ")) projectName = projectName.TrimStart();
            project.setProjectName(projectName);
            project.setFilePath(filePath);
            project.createProject();            
        }

        private bool checkEmpty()
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                lblNotifyCreation.Content = "Path is empty";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(projectName))
            {
                lblNotifyCreation.Content = "Project Name missing";
                return true;
            }
            return false;
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowser.ShowDialog();
            filePath = folderBrowser.SelectedPath;
            lblDirectory.Content = "Workspace folder: " + filePath;
            Console.WriteLine(filePath);
        }

        private void lblDirectory_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(filePath))
            {
                Process.Start(filePath);
                return;
            }
            Console.WriteLine("Label clicked but path is empty");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            projectName = textBox.Text;
            lblStructure.Content = $"Structure:\n{textBox.Text}\n .vscode\n   launch.json\n   " +
                $"tasks.json\n dist\n node_modules\n src\n   index.ts\n package.json\n package-lock.json\n tsconfig.json";
        }
    }
}