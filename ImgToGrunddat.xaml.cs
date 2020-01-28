using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;
using MeineSammlungen_2.Admin;
using MeineSammlungen_2.Modul_MikroMakro;
using MeineSammlungen_2.Modul_Exponate;
using MeineSammlungen_2.Modul_Mineral;


namespace MeineSammlungen_2
{
    


    /// <summary>
    /// Interaktionslogik für ImgToGrunddat.xaml
    /// </summary>
    public partial class ImgToGrunddat : Window
    {
        string ImgPath;
        public ImgToGrunddat()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var gdListe = from gd in Admin.Admin.conn.Grunddaten select gd;
            DGGrundDat.ItemsSource = gdListe.ToList();
        }

        private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            string openArgs = "*#" + @"H:\Mikro-Makro\MeineSammlungen\Eingang";
            this.Cursor = System.Windows.Input.Cursors.Wait;
            PictureList selPicture = new PictureList(openArgs);

            //lbItem.ItemsSource = selPicture;
            lbImg.ItemsSource = selPicture;
            this.Cursor = System.Windows.Input.Cursors.Arrow;
        }

        private void lbImg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic row = lbImg.SelectedItem;
            if (row == null)
            {
                //ImgDisplay.Source = null;
                //readExif.ClearExit();
                return;
            }
            //MessageBox.Show(row.Path);
            ImgPath = row.Path;

            BitmapImage myBitMap = new BitmapImage();
            myBitMap.BeginInit();
            myBitMap.CacheOption = BitmapCacheOption.OnLoad;
            myBitMap.UriSource = new Uri(ImgPath);
            myBitMap.EndInit();
            ImgDisplay.Source = myBitMap;
            //Modul_Image.readIPTC.ReadIPTC(ImgPath);
            //Modul_Image.readExif.ShowEXIF(ImgPath);
            //ShowMetaDaten();
            ImgHandling.IPTCDaten iptc = new ImgHandling.IPTCDaten(ImgPath);
            //ImgHandling.IPTCDaten.myIPTC_Daten(currImg);

            txtObject.Text = iptc.iObjekt;
            txtDetail.Text = iptc.iDeteil;
            //txtBemerkung.Text = iptc.iBemerkung;
            txtQuelle.Text = iptc.iQuelle;
            txtOrt.Text = iptc.iFundstelleOrt;
            //txtCountry.Text = iptc.iFundstelleCountry;
            //txtLand.Text = iptc.iFundstelleLand;
            //txtPosition.Text = iptc.iPostition;
            //txtErstellt.Text = iptc.iErstellt;
            //txtDErstellt.Text = iptc.iDigitalErstellt;
            //txtAutor.Text = iptc.iAutor;
            //txtCRight.Text = iptc.iCopyright;
            //txtHinweise.Text = iptc.iHinweise;
            txtStichworte.Text = iptc.iStichwortText;

            //Exif-Daten
            ImgHandling.EXIF.clearExif();
            ImgHandling.EXIF.ReadEXIF(ImgPath);
            txtKamera.Text = ImgHandling.ExifDaten.Kamera;
            txtBlende.Text = ImgHandling.ExifDaten.Blende;
            txtBelichtung.Text = ImgHandling.ExifDaten.Belichtung;
            txtIso.Text = ImgHandling.ExifDaten.ISO;
            txtBrennweiste.Text = ImgHandling.ExifDaten.Brennweite;
            txtAufnahmeDat.Text = ImgHandling.ExifDaten.AufnahmeDat;
        }

        private void DGGrundDat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void txtSuche_TextChanged(object sender, TextChangedEventArgs e)
        {
            var gdListe = from gd in Admin.Admin.conn.Grunddaten where gd.Objekt.Contains(txtSuche.Text) select gd;
            DGGrundDat.ItemsSource = gdListe.ToList();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSuche.Text = null;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                Grunddaten selected = DGGrundDat.SelectedItem as Grunddaten;
                switch (selected.Modul)
                {
                    case 1:
                        string openArgs = selected.ID.ToString() + "#2"; //1 -> neuer Datensatz
                        AddMikroMakro mmEdit = new AddMikroMakro(openArgs);
                        mmEdit.ShowDialog();
                        break;
                    case 2:
                        string oaExpo = selected.ID.ToString() + "#2"; //1 -> neuer Datensatz
                        AddEditExponate exEdit = new AddEditExponate(oaExpo);
                        exEdit.ShowDialog();
                        break;
                    case 3:
                        string oaMineral = selected.ID.ToString() + "#2"; //1 -> neuer Datensatz
                        AddEditMineral miEdit = new AddEditMineral(oaMineral);
                        miEdit.ShowDialog();
                        break;
                }
            }
        }
    }
    }

