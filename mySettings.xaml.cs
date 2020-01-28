using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MeineSammlungen_2.Admin;

namespace MeineSammlungen_2
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class mySettings : Window
    {
        Int32 sID;
        public mySettings()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var setting = from s in Admin.Admin.conn.Settings select s;
            foreach (var item in setting)
            {
                txtImgPath.Text = item.Bildpfad;
                sID = item.ID;
            }
        }
        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowNewFolderButton = true;
            folder.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK) ;

            txtImgPath.Text = folder.SelectedPath;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Settings sx = (from s in Admin.Admin.conn.Settings where s.ID == sID select s).FirstOrDefault();
                sx.Bildpfad = txtImgPath.Text.Trim();
                Admin.Admin.conn.SubmitChanges();
                Admin.Admin.ImgPath = txtImgPath.Text.Trim();
                System.Windows.Forms.MessageBox.Show("Änderungen übernommen");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Fehler beim Speichern!" + "\r\n" + ex.Message);
                return;
            }

            DialogResult = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }


    }
}
