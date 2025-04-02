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
        private CreateTSProject tsProject;
        private CreateRustProject rustProject;

        public MainWindow()
        {
            InitializeComponent();
            rustProject = new CreateRustProject(this);
            tsProject = new CreateTSProject(this);
            btnComboBox.Items.Add("TypeScript");
            btnComboBox.Items.Add("Rust");
            btnComboBox.SelectedIndex = 0;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (checkEmpty())
            {
                return;
            }
            else if (projectName.StartsWith(" ")) projectName = projectName.TrimStart();

            string selected = btnComboBox.SelectedItem.ToString();
            Console.WriteLine(selected);
            if (selected == "Rust")
            {
                rustProject.setProjectName(projectName);
                rustProject.setFilePath(filePath);
                rustProject.createProject();
            }
            else
            {
                tsProject.setProjectName(projectName);
                tsProject.setFilePath(filePath);
                tsProject.createProject();
            }
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
            if (!String.IsNullOrWhiteSpace(filePath))
            {
                lblDirectory.Content = "Workspace folder: " + filePath;
            }
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
        }    
    }
}