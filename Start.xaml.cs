using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using MeineSammlungen_2.Admin;
using MeineSammlungen_2.Modul_Exponate;
using MeineSammlungen_2.Modul_MikroMakro;
using MeineSammlungen_2.Listen;
using MeineSammlungen_2.Modul_Mineral;



namespace MeineSammlungen_2
{

    /// <summary>
    /// Interaktionslogik für Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        private UserControl uc;
        private Int32 cModul;
        private string ImgPath;
        private Int32 gdID;
        private Int32 imgCount;

        public Start()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrids();

            Admin.Admin.GetSettings();
        }

        private void LoadDataGrids()
        {
            var mod = from m in Admin.Admin.conn.Module select m;
            DGModule.ItemsSource = mod.ToList();
            DGModule.DataContext = mod.ToList();
        }

        private void DGModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Module selected = DGModule.SelectedItem as Module;
            if (selected == null)
            {
                return;
            }
            //currModul.GetDataModul(selected.ID);
            cModul = selected.ID;
            DGGrundDat.ItemsSource = Admin.Admin.GetDataGrunddatenListe(selected.ID);
            BtnNeu.Content = "Neuer Eintrag für " + selected.Modul;

            ClearDisplay();
        }

        private void DGGrundDat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cModul > 0)
            {
                ShowDaten();
            }

        }

        private void ShowDaten()
        {
            Grunddaten selected = DGGrundDat.SelectedItem as Grunddaten;
            if (selected == null)
            {
                return;
            }

            ClearDisplay();
            var abl = from a in Admin.Admin.conn.Ablage where a.ID == selected.Ablageort_neu select a;
            foreach (var item in abl)
            {
                AblageortText.Text = item.Ablageort;
            }
            // Anzeige der Grunddaten
            lblObjektNr.Content = "Objekt Nr.: " + selected.Nr;
            ObjektText.Text = selected.Objekt;
            DetailText.Text = selected.Detail;
            //AblageortText.Text = abl;
            BemerkungText.Text = selected.Bemerkung;
            ErstelltText.Text = selected.Erstellt.ToString();
            GeaendertText.Text = selected.Geaendert.ToString();
            imgCount = selected.ImgCount;
            AnzahlBilderText.Text = imgCount.ToString();
            gdID = selected.ID;
            if (selected.Checked == true)
            { lblBearbeitung.Content = "Weitere Bearbeitung erforderlich"; }
            else lblBearbeitung.Content = null;

            

            
            //Anzeige der UserControls für Details
            if (selected.Modul == 1)
            {
                {
                    if (selected == null)
                    {
                        MessageBox.Show("Bitte erst auswählen!");
                        return;
                    }
                    PageModul.Content = new PageMikroMakro(gdID);

                }
            }
            else if (selected.Modul == 2)
            {
                if (selected == null)
                {
                    MessageBox.Show("Bitte erst einen auswählen!");
                    return;
                }
                PageModul.Content = new PageExponate(gdID);
            }
            else if (selected.Modul == 3)
            {
                if (selected == null)
                {
                    MessageBox.Show("Bitte erst einen auswählen!");
                    return;
                }
                PageModul.Content = new PageMineral(gdID);
            }

            //Imageliste erstellen

            //List<string> myImgList = new List<string>();
            // string LocalimgPath = Admin. Admin.ImgPath;
            PictureList selPicture = new PictureList(gdID.ToString());

            imgListBox.ItemsSource = selPicture;
        }

       
        private void Btn_Neu_Click(object sender, RoutedEventArgs e)
        {
            if (cModul == 1)
            {
                string openArgs = cModul.ToString() + "#1"; //1 -> neuer Datensatz
                AddMikroMakro mmNeu = new AddMikroMakro(openArgs);
                mmNeu.ShowDialog();


            }
            if (cModul == 2)
            {
                string openArgs = cModul.ToString() + "#1"; //1 -> neuer Datensatz
                AddEditExponate exEdit = new AddEditExponate(openArgs);
                exEdit.ShowDialog();


            }
            if (cModul == 3)
            {
                string openArgs = cModul.ToString() + "#1"; //1 -> neuer Datensatz
                AddEditMineral exEdit = new AddEditMineral(openArgs);
                exEdit.ShowDialog();


            }
            DGGrundDat.ItemsSource = Admin.Admin.GetDataGrunddatenListe(cModul);
            ShowDaten();
        }

        private void Button_edit_Click(object sender, RoutedEventArgs e)
        {
            if (cModul == 1)
            {
                string openArgs = gdID.ToString() + "#2"; //1 -> neuer Datensatz
                AddMikroMakro mmEdit = new AddMikroMakro(openArgs);
                mmEdit.ShowDialog();


            }
            if (cModul == 2)
            {
                string openArgs = gdID.ToString() + "#2"; //1 -> neuer Datensatz
                AddEditExponate exEdit = new AddEditExponate(openArgs);
                exEdit.ShowDialog();


            }
            if (cModul == 3)
            {
                string openArgs = gdID.ToString() + "#2"; //1 -> neuer Datensatz
                AddEditMineral exEdit = new AddEditMineral(openArgs);
                exEdit.ShowDialog();


            }
            DGGrundDat.ItemsSource = Admin.Admin.GetDataGrunddatenListe(cModul);
            ShowDaten();
        }

        private void Del_Butten_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gelöscht wird " + gdID.ToString());
            if (imgCount > 0)
            {
                var result = MessageBox.Show("Es sind " + imgCount.ToString() + " Bilder vorhanden." + Environment.NewLine + "Löschen?", "Fehler", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    ImgHandling.delImg.ImgDel(gdID.ToString());
                }

            }
            if (Modul_Grunddaten.delGD.deleteGD(gdID) == false)
            {
                MessageBox.Show("Es ist ein Fehler beim Löschen der Detaildaaten aufgetreten!");
                // MessageBox.Show("Daten wurden gelöscht.");
            }
            //dann details in Modulen löschen
            if (cModul == 1)
            {
                if (Modul_MikroMakro.delMM.deleteMM(gdID) == true)
                {
                    MessageBox.Show("Daten wurden gelöscht.");
                }
                else
                    MessageBox.Show("Es ist ein Fehler beim Löschen der Detaildaaten aufgetreten!");
            }
            if (cModul == 2)
            {
                if (Modul_Exponate.delExp.deleteExp(gdID) == true)
                {
                    MessageBox.Show("Daten wurden gelöscht.");
                }
                else
                    MessageBox.Show("Es ist ein Fehler beim Löschen der Detaildaaten aufgetreten!");
            }
            DGGrundDat.ItemsSource = Admin.Admin.GetDataGrunddatenListe(cModul);
            object item = DGGrundDat.Items[0];
            DGGrundDat.SelectedItem = item;
            DGGrundDat.ScrollIntoView(item);

            ShowDaten();
            //System.Windows.Forms.MessageBox.Show("Hinweis: Ggf. vorhandene Bilder müssen z.Z. noch manuell gelöscht werden!");
        }

        private void Click_NewImg(object sender, RoutedEventArgs e)
        {

        }

        private void Click_DelImg(object sender, RoutedEventArgs e)
        {
            //ImgHandling.delImg.Main(gdID.ToString());
        }

        private void Click_ShowSelectImg(object sender, RoutedEventArgs e)
        {
            ShowImage si = new ShowImage(ImgPath);
            si.ShowDialog();
        }

        private void Click_EditImgData(object sender, RoutedEventArgs e)
        {

            string objNr = cModul.ToString() + "-" + gdID.ToString();
            ShowIPTC iptcchange = new ShowIPTC(ImgPath + "*" + objNr);
            iptcchange.ShowDialog();


        }



        private void ImgListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic row = imgListBox.SelectedItem;
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

        private void Click_ExitMnu(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PageModul_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void ClearDisplay()
        {
            //readExif.ClearExit();
            //readIPTC.clearIPTC();
            //ShowMetaDaten();
            imgListBox.ItemsSource = null;
            ImgDisplay.Source = null;
            ImgDisplay.Source = null;
            //IPTC und EXIF leeren
            //ImgHandling.EXIF.clearExif();
            txtObject.Text = null;
            txtDetail.Text = null;
            txtQuelle.Text = null;
            txtOrt.Text = null;
            txtStichworte.Text = null;
            txtKamera.Text = null;
            txtBlende.Text = null;
            txtBelichtung.Text = null;
            txtIso.Text = null;
            txtBrennweiste.Text = null;
            txtAufnahmeDat.Text = null;
        }


        private void MenuItem_Ablage_Click(object sender, RoutedEventArgs e)
        {
            AblageOrte abl = new AblageOrte();
            abl.ShowDialog();
        }

        private void Click_SettingMnu(object sender, RoutedEventArgs e)
        {
            mySettings st = new mySettings();
            st.ShowDialog();
        }

        private void MenuItem_ImgToGD_Click(object sender, RoutedEventArgs e)
        {
            ImgToGrunddat igd = new ImgToGrunddat();
            igd.ShowDialog();
        }
    }
}
