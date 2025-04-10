using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
        private SaveFile saveFile;

        public MainWindow()
        {
            InitializeComponent();
            rustProject = new CreateRustProject(this);
            tsProject = new CreateTSProject(this);
            btnComboBox.Items.Add("TypeScript");
            btnComboBox.Items.Add("Rust");
            btnComboBox.SelectedIndex = 0;
            lblDirectory.Visibility = Visibility.Hidden;
            saveFile = new SaveFile(filePath);
            if (saveFile.CheckExists()) {
                lblDirectory.Visibility = Visibility.Visible;
                saveFile.getFilePath(); 
                filePath = saveFile.getFilePath();
                lblDirectory.Content = "Workspace folder: " + filePath;
            }
        }

        private void ConfigPath() 
        {
            saveFile.setFilePath(filePath);
            saveFile.CreateConfig();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (checkEmpty())
            {
                return;
            }
            else if (projectName.Contains(" ")) projectName = projectName.Replace(" ", "");

            string selected = btnComboBox.SelectedItem.ToString();
            if (selected == "Rust")
            {
                rustProject.setProjectName(projectName);
                rustProject.setFilePath(filePath);
                rustProject.createProject();
                ConfigPath();
            }
            else
            {
                tsProject.setProjectName(projectName);
                tsProject.setFilePath(filePath);
                tsProject.createProject();
                ConfigPath();
            }
        }

        private bool checkEmpty()
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                lblDirectory.Visibility = Visibility.Visible;
                lblDirectory.Content = "Path is empty";
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
                lblDirectory.Visibility = Visibility.Visible;
                lblDirectory.Content = "Workspace folder: " + filePath;
            }
            Console.WriteLine(filePath);
        }

        private void lblDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(filePath))
                {
                    throw new Exception("Invalid path, FilePath: " + filePath);
                }
                Process.Start($@"{filePath}");
                return;
            }
            catch (Exception ex)
            {
                lblNotifyCreation.Content = ex.Message;
                Console.WriteLine(ex);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            projectName = textBox.Text;
        }
    }
}