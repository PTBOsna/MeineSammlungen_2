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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using MeineSammlungen_2.Modul_Grunddaten;

namespace MeineSammlungen_2
{

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class ShowIPTC : Window
    {

        string openArgs; //enthält currImg und Objekt-Nr mit * getrennt. Aus Objekt-Nr. wird gdID extrahiert 
        Int32 gdID;
        string currImg;
        string cTitel;
        ImgHandling.IPTCDaten iptc;
        public ShowIPTC(string _openArgs)
        {
            InitializeComponent();
            this.openArgs = _openArgs;
            String[] strlist = openArgs.Split('*');

            currImg = strlist[0];
            string[] strlist2 = strlist[1].Split('-');
            cTitel = strlist[1];
            gdID = int.Parse(strlist2[1]);
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            this.Title = "Metadaten für Objekt-Nr " + cTitel;
            iptc = new ImgHandling.IPTCDaten(currImg);
            //ImgHandling.IPTCDaten.myIPTC_Daten(currImg);

            txtObject.Text = iptc.iObjekt;
            txtDetail.Text = iptc.iDeteil;
            txtBemerkung.Text = iptc.iBemerkung;
            txtQuelle.Text = iptc.iQuelle;
            txtSpezial.Text = iptc.iSpezial;
            txtOrt.Text = iptc.iFundstelleOrt;
            txtCountry.Text = iptc.iFundstelleCountry;
            txtLand.Text = iptc.iFundstelleLand;
            txtPosition.Text = iptc.iPostition;
            txtErstellt.Text = iptc.iErstellt;
            txtDErstellt.Text = iptc.iDigitalErstellt;
            txtAutor.Text = iptc.iAutor;
            txtCRight.Text = iptc.iCopyright;
            txtHinweise.Text = iptc.iHinweise;
            txtStichworte.Text = iptc.iStichwortText;
            ShowListBox();
            BitmapImage myBitMap = new BitmapImage();
            myBitMap.BeginInit();
            myBitMap.CacheOption = BitmapCacheOption.OnLoad;
            myBitMap.UriSource = new Uri(currImg);
            myBitMap.EndInit();
            ImgDisplay.Source = myBitMap;

            //EXIF-Daten
            ImgHandling.EXIF.ReadEXIF(currImg);
            txtKamera.Text = ImgHandling.ExifDaten.Kamera;
            txtBlende.Text = ImgHandling.ExifDaten.Blende;
            txtBelichtung.Text = ImgHandling.ExifDaten.Belichtung;
            txtISO.Text = ImgHandling.ExifDaten.ISO;
            txtBrennweite.Text = ImgHandling.ExifDaten.Brennweite;
            txtAufnahmeDat.Text = ImgHandling.ExifDaten.AufnahmeDat;
            txtLat.Content = "Latitude (Länge): " + ImgHandling.ExifDaten.Latitude;
            txtLatDez.Content = "Latitude " + ImgHandling.ExifDaten.LatitudeDez;
            txtLong.Content = "Longitude (Breite): " + ImgHandling.ExifDaten.Longitude;
            txtLongDez.Content = "Longitude " + ImgHandling.ExifDaten.LongitudeDez;
            txtAlt.Content = ImgHandling.ExifDaten.Altitude;
            ImgHandling.EXIF.clearExif();
            //Quellenliste laden und zuweisen
            var quellen = from q in Admin.Admin.conn.Bildtyp orderby q.Bildtyp1 select q.Bildtyp1;

            cbQuelle.ItemsSource = quellen.ToList();
        }

        private void ShowListBox()
        {
            if (iptc.iStichworte != null)
            {
                listStichwort.ItemsSource = iptc.iStichworte.ToList();
                //cbStichworte.ItemsSource = IPTCDaten.iStichworte.ToList();
            }
            else { listStichwort.Items.Clear(); }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (iptc.iStichworte == null)
            {
                List<string> lst = new List<string>();
                lst.Add(txtAddStichworte.Text);
                iptc.iStichworte = lst.ToList();
            }
            else
            iptc.iStichworte.Add(txtAddStichworte.Text);
            txtAddStichworte.Text = null;
            ShowListBox();
        }

       
        private void BtSaveClick(object sender, RoutedEventArgs e)
        {
            //ImgHandling.myIPTCDaten ipd =  ImgHandling.myIPTCDaten();
            if (string.IsNullOrEmpty(txtHinweise.Text) == true)
            {
                txtHinweise.Text = txtHinweise.Text = "In DB als Objekt Nr. '" + gdID.ToString() + "' aufgenommen.";
            }
            ImgHandling.myIPTCDaten.iObjekt = txtObject.Text;
            ImgHandling.myIPTCDaten.iDeteil = txtDetail.Text;
            ImgHandling.myIPTCDaten.iBemerkung = txtBemerkung.Text;
            ImgHandling.myIPTCDaten.iQuelle = txtQuelle.Text;
            ImgHandling.myIPTCDaten.iSpezial = txtSpezial.Text;
            ImgHandling.myIPTCDaten.iFundstelleOrt = txtOrt.Text;
            ImgHandling.myIPTCDaten.iFundstelleCountry = txtCountry.Text;
            ImgHandling.myIPTCDaten.iFundstelleLand = txtLand.Text;
            ImgHandling.myIPTCDaten.iPostition = txtPosition.Text;
            if (string.IsNullOrEmpty(txtErstellt.Text) == false)
            {
                try
                {
                    DateTime da = DateTime.Parse(txtErstellt.Text);
                    ImgHandling.myIPTCDaten.iErstellt = da.ToString("yyyyMMdd", System.Globalization.CultureInfo.GetCultureInfo("de-DE"));

                }
                catch (Exception)
                {
                    MessageBox.Show("Bitte Datum manuell eintragen!", "Feld 'Erstellt' enthält falsches Datumsformat!");
                }
            }
            if (string.IsNullOrEmpty(txtDErstellt.Text) == false)
            {
                try
                {
                    DateTime oDate = DateTime.Parse(txtDErstellt.Text);
                    ImgHandling.myIPTCDaten.iDigitalErstellt = oDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.GetCultureInfo("de-DE"));

                }
                catch (Exception)
                {

                    MessageBox.Show("Bitte Datum manuell eintragen!", "Feld 'Digital erstellt' enthält falsches Datumsformat!");
                }
            }
            ImgHandling.myIPTCDaten.iDigitalErstellt = txtDErstellt.Text;
            ImgHandling.myIPTCDaten.iAutor = txtAutor.Text;
            ImgHandling.myIPTCDaten.iCopyright = txtCountry.Text;
            ImgHandling.myIPTCDaten.iHinweise = txtHinweise.Text;
            ImgHandling.myIPTCDaten.iStichworte = iptc.iStichworte;
            // ImgHandling.myIPTCDaten.iCopyright = txtCountry.Text;

            ImgHandling.IPTCDaten.WriteIPTC(currImg);
            System.Windows.Forms.MessageBox.Show("Daten übernommen");
            DialogResult = false;
        }

        private void BtExitClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtAddClick(object sender, RoutedEventArgs e)
        {
            var mb = MessageBox.Show("Achtung, die vorhandenen IPTC-Daten werden überschrieben!", "Grunddaten übernehmen!", MessageBoxButton.OKCancel);
            if (mb == MessageBoxResult.No)
            { return; }
            if (gdID != 0)
            {
                currGD gd = new currGD(gdID);
                txtObject.Text = gd.cObjekt;
                txtDetail.Text = gd.cDetail;
                txtBemerkung.Text = gd.cBemerkung;
                txtHinweise.Text = "In DB als Objekt Nr. '" + gd.cNr + "' aufgenommen.";
                txtCRight.Text = "© PBBerlin";
                txtAutor.Text = "PTBBerlin";
                txtSpezial.Text = Admin.Admin.cName; 
            }

        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txtErstellt.Text = txtErstelltPicker.SelectedDate.ToString();
        }

        private void cbQuelle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtQuelle.Text = cbQuelle.SelectedItem.ToString();
        }

        private void Button_Del_Click(object sender, RoutedEventArgs e)
        {
            if (listStichwort.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte erst ein Stichwort auswählen!");
                return;
            }
            string lbItem = listStichwort.SelectedItem.ToString();
            
            //int lbIndex = listStichwort.SelectedIndex;
            //System.Windows.Forms.MessageBox.Show("Index " + lbIndex.ToString() + " - Item " + lbItem);
            iptc.iStichworte.Remove(lbItem);
            txtStichworte.Text = "";
                ShowListBox();
            if (this.listStichwort.Items.Count > 0)
            {
                
                List<string> lst = new List<string>();
                foreach (var el in listStichwort.Items)
                {
                    lst.Add(el.ToString());
                    
                }
                txtStichworte.Text = String.Join(String.Empty, lst.ToArray());
                //    lst.Add(listStichwort.Items.
                iptc.iStichworte = lst.ToList();
           
            }
               
        }

        private void btn_ToIptc_clilck(object sender, RoutedEventArgs e)
        {
            txtPosition.Text = txtLongDez.Content + "\r\n" + txtLatDez.Content + "\r\n" + txtAlt.Content;
        }
    }
}
