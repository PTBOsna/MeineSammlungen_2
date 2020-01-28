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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MeineSammlungen_2.Admin;

namespace MeineSammlungen_2.Modul_Mineral
{
    /// <summary>
    /// Interaktionslogik für AddEditMineral.xaml
    /// </summary>
    public partial class AddEditMineral : Window
    {
        string openArgs;
        Int32 istNeu;
        Int32 myVarID; // enthält entweder die Modul-ID (bei Add Daten) oder die Grunddaten-ID (bei Edit-Daten)
        Int32 myModID; //Modul-ID
        Int32 myExID;
        Int32 myImgCount;
        Int32 ablageID;
        string imgPath;
        string ObjNr;
        public AddEditMineral(string _openArgs)
        {
            InitializeComponent();
            this.openArgs = _openArgs;
            String[] strlist = openArgs.Split('#');

            myVarID = Int32.Parse(strlist[0]);
            istNeu = Int32.Parse(strlist[1]);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var abl = from a in Admin.Admin.conn.Ablage select a;
            cbAblage.DataContext = abl;
            cbAblage.ItemsSource = abl;
            //cbAblage.ItemsSource = abl.ToList();

            if (istNeu == 1)
            {
                myModID = myVarID;
                myVarID = Modul_Grunddaten.currGD.addGD(myVarID);

                //Neuer Exponate Datensatz
                Mineralien addEx = new Mineralien();
                addEx.Grunddaten_ID = myVarID;
                //IDLabel.Content = "Grunddaten-ID; " + addEx.Grunddaten_ID;
                Admin.Admin.conn.Mineralien.InsertOnSubmit(addEx);
                Admin.Admin.conn.SubmitChanges();
            }

                //jetzt alles neu laden:
                var myDat = from ex in Admin.Admin.conn.Mineralien
                            from g in Admin.Admin.conn.Grunddaten
                            from a in Admin.Admin.conn.Ablage
                            where ex.Grunddaten_ID == myVarID && g.ID == myVarID && g.Ablageort_neu == a.ID
                            select new { ex, g, a };
                //und anzeigen
                foreach (var item in myDat)
                {
                    ObjektText.Text = item.g.Objekt;
                    DetailText.Text = item.g.Detail;
                    AblageortText.Text = item.a.Ablageort; //item.g.Ablageort;
                    ablageID = item.a.ID;
                    BemerkungText.Text = item.g.Bemerkung;
                    ErstelltText.Text = item.g.Erstellt.ToString();
                    GeaendertText.Text = item.g.Geaendert.ToString();
                    myImgCount = item.g.ImgCount;
                    myModID = item.g.Modul;
                    LblImgCount.Content = "Zugehörige Bilder: " + myImgCount.ToString();
                //item.g.Nr = item.g.Modul.ToString() + "-" + item.g.ID.ToString();
                ObjNr = item.g.Nr;

                lblObjektNr.Content = "Objekt-Nr.: " + item.g.Nr.Trim();
                    if (item.g.Checked == true)
                    {
                        ckbWeitereBearbeitung.IsChecked = true;
                    }
                    else ckbWeitereBearbeitung.IsChecked = false;

                    LandText.Text = item.ex.Fundstelle_Land;
                    OrtTExt.Text = item.ex.Fundstelle_Ort;
                    KoordinatenText.Text = item.ex.Koordinaten;
                    FunddatumText.Text = item.ex.Fund_Datum;
                    BemerkungText.Text = item.ex.Hinweise;
                    //IDLabel.Content = item.ex.Grunddaten_ID;
                    GewichtText.Text = item.ex.Gewicht.ToString();
                    VolumenText.Text = item.ex.Volumen.ToString();
                    DichteText.Text = item.ex.Dichte.ToString();
                    ZusammensetzungText.Text = item.ex.Zusammensetzung;
                    myExID = item.ex.ID;
                    //Titel anzeigen
                    this.Title = "Details zu Objekt '" + item.g.Nr.Trim() + "' ansehen/ändern";

                }
                if (myImgCount > 0)
                {
                    PictureList selPicture = new PictureList(myVarID.ToString());
                    imgListBox.ItemsSource = selPicture;
                }
            }
        
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FunddatumText.Text = FdDatePicker.SelectedDate.Value.ToShortDateString();
        }

        private void cbAblage_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show(cbAblage.SelectedValue.ToString());
            var ab = from a in Admin.Admin.conn.Ablage where a.ID == (Int32)cbAblage.SelectedValue select a;
            foreach (var item in ab)
            {
                ablageID = item.ID;
                AblageortText.Text = item.Ablageort;
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveGD();

            //dann die Änderungen in den Detaildaten speichern
            Mineralien gEx = new Mineralien();
            gEx.ID = myExID;
            gEx.Fundstelle_Land = LandText.Text;
            gEx.Fundstelle_Ort = OrtTExt.Text;
            gEx.Koordinaten = KoordinatenText.Text;
            gEx.Fund_Datum = FunddatumText.Text;
            gEx.Hinweise = HinweiseExpoText.Text;
            gEx.Grunddaten_ID = myVarID;
          
            editMineral.edit(gEx);


            DialogResult = false;
        }

        private void SaveGD()
        {
            Grunddaten gD = new Grunddaten();
            gD.Nr = ObjNr;
            gD.Objekt = ObjektText.Text;
            gD.Detail = DetailText.Text;
            gD.Ablageort = AblageortText.Text;
            gD.Modul = myModID;
            gD.Bemerkung = BemerkungText.Text;
            if (string.IsNullOrEmpty(ErstelltText.Text) == true)
            { gD.Erstellt = DateTime.Now; }
            else { gD.Erstellt = DateTime.Parse(ErstelltText.Text); }
            gD.Geaendert = DateTime.Now;
            gD.ID = myVarID;
            gD.ImgCount = myImgCount;
            gD.Ablageort_neu = ablageID;
            if (ckbWeitereBearbeitung.IsChecked == true)
            { gD.Checked = true; }
            else gD.Checked = false;

            Modul_Grunddaten.editGD.editGrunddaten(gD);

            System.Windows.Forms.MessageBox.Show("Objekt '" + gD.Objekt + "' wurde geändert.");
        }

        private void Btn_Return_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Btn_Img_new(object sender, RoutedEventArgs e)
        {
            if (myImgCount == 0)
            { SaveGD(); }
            string curName = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Bilder einfügen";
            ofd.Filter = "Image Files(*.PNG; *.JPG;| *.PNG; *.JPG| All files(*.*) | *.*";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //FilePath = ofd.FileName;
                //System.Windows.Forms.MessageBox.Show(FilePath);
                Admin.Admin.cName = System.IO.Path.GetFileName(ofd.FileName);
                myImgCount += 1;
                String _NewName = myVarID.ToString() + "#" + myImgCount + System.IO.Path.GetExtension(ofd.FileName);
                string NewName = System.IO.Path.Combine(Admin.Admin.ImgPath, _NewName);
                // System.Windows.Forms.MessageBox.Show(NewName);
                try
                {
                    File.Copy(ofd.FileName, NewName);
                    curName = System.IO.Path.GetFileName(NewName);
                    //den alten FileName speichern
                    //ImgHandling.myIPTCDaten.iSpezial = cFileName;
                    //ImgHandling.IPTCDaten.WriteIPTC(cName);
                    PictureList selPicture = new PictureList(curName);
                    imgListBox.ItemsSource = selPicture;
                    LblImgCount.Content = "Zugehörige Bilder: " + myImgCount.ToString();
                }
                catch (Exception ex)
                {

                    System.Windows.Forms.MessageBox.Show("Bild bereits vorhaden!" + Environment.NewLine + ex.Message);
                }
                string objNr = myModID.ToString() + "-" + myVarID.ToString();
                string cImg = System.IO.Path.Combine(Admin.Admin.ImgPath, curName);
                ShowIPTC iptcchange = new ShowIPTC(cImg + "*" + objNr);
                iptcchange.ShowDialog();
                    ShowMetaDaten(curName);

            }

        }

        private void ShowMetaDaten(string myPath)
        {
            ImgHandling.IPTCDaten iptc = new ImgHandling.IPTCDaten(myPath);

            ImgHandling.EXIF.ReadEXIF(myPath);

            txtObject.Text = iptc.iObjekt;
            txtDetail.Text = iptc.iDeteil;
            txtQuelle.Text = iptc.iQuelle;
            txtOrt.Text = iptc.iFundstelleOrt;
            txtStichworte.Text = iptc.iStichwortText;
            txtBemerkung.Text = iptc.iBemerkung;
            txtKamera.Text = ImgHandling.ExifDaten.Kamera;
            txtBelichtung.Text = ImgHandling.ExifDaten.Belichtung;
            txtBlende.Text = ImgHandling.ExifDaten.Blende;
            txtBrennweite.Text = ImgHandling.ExifDaten.Brennweite;
            txtIso.Text = ImgHandling.ExifDaten.ISO;
            txtAufnahmeDat.Text = ImgHandling.ExifDaten.AufnahmeDat;
        }

        private void Btn_DelImg(object sender, RoutedEventArgs e)
        {
            string fileName = System.IO.Path.GetFileName(imgPath);
            if (ImgHandling.delImg.ImgDel(fileName) == true)
            {
                System.Windows.Forms.MessageBox.Show("Bild wurde entfernt");
                myImgCount = myImgCount - 1;
                LblImgCount.Content = "Zugehörige Bilder: " + myImgCount.ToString();
                SaveGD();
                //imgListBox.Items.Clear();
                //if (myImgCount > 0)
                {
                    PictureList selPicture = new PictureList(myVarID.ToString());
                    imgListBox.ItemsSource = selPicture;
                }
            }

        }

        private void Btn_ChangeIPTC_click(object sender, RoutedEventArgs e)
        {
            var ab = from a in Admin.Admin.conn.Ablage where a.ID == (Int32)cbAblage.SelectedValue select a;
            foreach (var item in ab)
            {
                ablageID = item.ID;
                AblageortText.Text = item.Ablageort;
            }

        }

        private void imgListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            dynamic row = imgListBox.SelectedItem;
            if (row == null)
            {
                //ImgDisplay.Source = null;
                //readExif.ClearExit();
                return;
            }
            //MessageBox.Show(row.Path);
            imgPath = row.Path;
            ShowMetaDaten(imgPath);
        }
    }
}
