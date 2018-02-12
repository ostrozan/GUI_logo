using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI_logo
{
    /// <summary>
    /// Interakční logika pro NewProjWindow.xaml
    /// </summary>
    public partial class NewProjWindow : Window
    {
        private string pathProj;
        public NewProjWindow(string path)
        {
            InitializeComponent();
            this.pathProj = path;


        }

        private void btnFileBrowse_Click(object sender, RoutedEventArgs e)
        {
            
            SaveFileDialog sfd = new SaveFileDialog();
            pathProj = System.IO.Path.Combine(pathProj, tbxName.Text);
            if (!Directory.Exists(pathProj)) Directory.CreateDirectory(pathProj);
            sfd.InitialDirectory = pathProj;
            sfd.FileName = tbxName.Text;
            sfd.DefaultExt = "xml";
            sfd.ShowDialog();
            
            
            if (!File.Exists(sfd.FileName)) File.Create(sfd.FileName);
            pathProj = tbxPath.Text = System.IO.Path.GetFullPath(sfd.FileName);
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.pathProj = pathProj;
            MainWindow.projPaths.Add(pathProj);
            MainWindow.projectName = tbxName.Text;
            DialogResult = true;
            this.Close();
        }
    }
}
