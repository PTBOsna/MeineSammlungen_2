using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Windows.Shapes;
using MeineSammlungen_2.Admin;
using MeineSammlungen_2.Modul_MikroMakro;
using MeineSammlungen_2.Modul_Exponate;
using MeineSammlungen_2.Modul_Grunddaten;

namespace MeineSammlungen_2.Modul_MikroMakro
{
    /// <summary>
    /// Interaktionslogik für AddMikroMakro.xaml
    /// </summary>
    public partial class AddMikroMakro : Window
    {
        string openArgs;
        Int32 istNeu;
        Int32 myVarID; // enthält entweder die Modul-ID (bei Add Daten) oder die Grunddaten-ID (bei Edit-Daten)
        Int32 myModID; // Modul-ID
        Int32 myMID;
        Int32 myImgCount;
        Int32 ablageID;
        string imgPath;
        string ObjNr; //Angezeigte Nr zusammengesetzt aus Modul + gdID

        public AddMikroMakro(string _openArgs)
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

            if (istNeu == 1)
            {
                myModID = myVarID;
                myVarID = Modul_Grunddaten.currGD.addGD(myVarID);
                //Neuer MikroMakro Datensatz
                ModulMikro addM = new ModulMikro();
                addM.Grunddaten_ID = myVarID;
                //IDLabel.Content = "Grunddaten-ID; " + addM.Grunddaten_ID;
                Admin.Admin.conn.ModulMikro.InsertOnSubmit(addM);
                Admin.Admin.conn.SubmitChanges();
            }

            //jetzt alles neu laden:
            var myDat = from m in Admin.Admin.conn.ModulMikro
                        from g in Admin.Admin.conn.Grunddaten
                        from a in Admin.Admin.conn.Ablage
                        where m.Grunddaten_ID == g.ID && g.ID == myVarID && g.Ablageort_neu == a.ID
                        select new { m, g, a };
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
                LblImgCount.Content = "Zugehörige Bilder: " + myImgCount.ToString();
                //myModID = item.g.Modul;
                //if (string.IsNullOrEmpty( item.g.Nr)==true)
                //{item.g.Nr = item.g.Modul.ToString() +"-" + item.g.ID.ToString(); }
                //item.g.Nr = item.g.Modul.ToString() + "-" + item.g.ID.ToString();
                ObjNr = item.g.Nr;
                lblObjektNr.Content = "Objekt Nr.: " + ObjNr;
                if (item.g.Checked == true)
                {
                    ckbWeitereBearbeitung.IsChecked = true;
                }
                else ckbWeitereBearbeitung.IsChecked = false;

                SchnittText.Text = item.m.Schnittebene;
                //IDLabel.Content = item.m.ID;
                SchnittartText.Text = item.m.Schnittart;
                FarbeText.Text = item.m.Farbung;
                HellText.Text = item.m.Aufhellung;
                FixierungText.Text = item.m.Fixierung;
                EinschlussText.Text = item.m.Einschluss;
                HinweiseText.Text = item.m.Hineise;
                myMID = item.m.ID;
                // Titel anzeigen
                this.Title = "Details zu Objekt '"  + item.g.Nr.Trim() + "' ansehen/ändern";
            }
            if (myImgCount > 0)
            {
                PictureList selPicture = new PictureList(myVarID.ToString());
                imgListBox.ItemsSource = selPicture;
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            //zunächst Änderungen in Grunddaten speichern

            SaveGD();
            Grunddaten gD = new Grunddaten();
            SaveGD();
            //gD.Nr = ObjNr;
            //gD.Objekt = ObjektText.Text;
            //gD.Detail = DetailText.Text;
            //gD.Ablageort = AblageortText.Text;
            //gD.Modul = myModID;
            //gD.Bemerkung = BemerkungText.Text;
            //if (string.IsNullOrEmpty(ErstelltText.Text) == true)
            //{ gD.Erstellt = DateTime.Now; }
            //else { gD.Erstellt = DateTime.Parse(ErstelltText.Text); }
            //gD.Geaendert = DateTime.Now;
            //gD.ID = myVarID;
            //gD.ImgCount = myImgCount;
            //gD.Ablageort_neu = ablageID;
            //if (ckbWeitereBearbeitung.IsChecked == true)
            //{ gD.Checked = true; }
            //else gD.Checked = false;
            //Modul_Grunddaten.editGD.editGrunddaten(gD);
            //System.Windows.Forms.MessageBox.Show("Objekt '" + gD.Objekt + "' wurde geändert.");

            //dann die Änderungen in den Detaildaten speichern
            ModulMikro gm = new ModulMikro();
            gm.ID = myMID;
            gm.Schnittart = SchnittartText.Text;
            gm.Schnittebene = SchnittText.Text;
            gm.Farbung = FarbeText.Text;
            gm.Aufhellung = HellText.Text;
            gm.Fixierung = FixierungText.Text;
            gm.Einschluss = EinschlussText.Text;
            gm.Hineise = HinweiseText.Text;
            gm.Grunddaten_ID = myVarID;

            currMM.editMM.editMikro(gm);
            System.Windows.MessageBox.Show("Objekt '" + gD.Objekt + "' wurde geändert.");
            DialogResult = false;

        }

        private void SaveGD()
        {
            Grunddaten gD = new Grunddaten();
            gD.Nr =ObjNr;
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
            editGD.editGrunddaten(gD);


            // ImgHandling.myIPTCDaten.iCopyright = txtCountry.Text;

            //ImgHandling.IPTCDaten.WriteIPTC(imgPath);
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
            ofd.Filter = "Image Files(*.JPG;|*.JPG| All files(*.*) | *.*";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //FilePath = ofd.FileName;
                Admin.Admin.cName = System.IO.Path.GetFileName(ofd.FileName);
                myImgCount += 1;
                String _NewName = myVarID.ToString() + "#" + myImgCount + System.IO.Path.GetExtension(ofd.FileName);
                string NewName = System.IO.Path.Combine(Admin.Admin.ImgPath, _NewName);
                // System.Windows.Forms.MessageBox.Show(NewName);
                try
                {
                    File.Copy(ofd.FileName, NewName);
                    curName = System.IO.Path.GetFileName(NewName);
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
            txtBemerkung.Text = iptc.iBemerkung;
            txtStichworte.Text = iptc.iStichwortText;
            txtKamera.Text = ImgHandling.ExifDaten.Kamera;
            txtBelichtung.Text = ImgHandling.ExifDaten.Belichtung;
            txtBlende.Text = ImgHandling.ExifDaten.Blende;
            txtBrennweite.Text = ImgHandling.ExifDaten.Brennweite;
            txtIso.Text = ImgHandling.ExifDaten.ISO;
            txtAufnahmeDat.Text = ImgHandling.ExifDaten.AufnahmeDat;
        }

        private void imgListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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
        /// <summary>
        /// IPTC ändern.
        /// Übergeben wird der Img-Pfad und die Objekt-Nr.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ChangeIPTC_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(imgPath) == true)
            {
                System.Windows.Forms.MessageBox.Show("Bitte zunächst ein Bild auswählen!");
                return;
            }
            string objNr = myModID.ToString() + "-" + myVarID.ToString();
            ShowIPTC iptcchange = new ShowIPTC(imgPath + "*" + objNr);
            iptcchange.ShowDialog();
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

        private void cbAblage_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var ab = from a in Admin.Admin.conn.Ablage where a.ID == (Int32)cbAblage.SelectedValue select a;
            foreach (var item in ab)
            {
                ablageID = item.ID;
                AblageortText.Text = item.Ablageort;
            }
        }
    }
}
