using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace MeineSammlungen_2.ImgHandling
{
    public class ExifDaten
    {
        public static string Kamera { get; set; }
        public static string AufnahmeDat { get; set; }
        public static string Blende { get; set; }
        public static string Belichtung { get; set; }
        public static string ISO { get; set; }
        public static string Brennweite { get; set; }
        public static string Longitude { get; set; }
        public static string Latitude { get; set; }
        public static string Altitude { get; set; }
        public static string LongitudeDez { get; set; }
        public static string LatitudeDez { get; set; }
       
    }

    public static class EXIF
    {

        public static void ReadEXIF(string curImg)
        {
            //curImg = _curImg;
            //ExifDaten currExif = new ExifDaten();
            string sDocument = curImg;
            if (File.Exists(sDocument) == false)
            {
                return;
            }
            //Klassen.Admin.FileIsLocked(sDocument);
            //FileStream myStream = new FileStream(sDocument, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            //JpegBitmapDecoder Decoder = new JpegBitmapDecoder(myStream, BitmapCreateOptions.None, BitmapCacheOption.None);
            //var metadata = 
            //Image img;
            //using (var bmpTemp = new Bitmap(sDocument))
            //{
            //    img = new Bitmap(bmpTemp);
            //}
            //Image imgLoaded = img;
            Image imgLoaded = Image.FromFile(sDocument);
            Encoding enc = Encoding.Default;
            string lat_Ref = null;
            string long_Ref = null;
            foreach (PropertyItem item in imgLoaded.PropertyItems)
            {
                //alle Infos auslesen - item.Id beinhaltet die Position der spezifischen Information
                //Geografische Position
                //Case 1
                //    lat_Ref = Replace(System.Text.Encoding.ASCII.GetString(Info.Value), vbNullChar, "")
                if (item.Id == 1)
                {
                    lat_Ref = enc.GetString(item.Value, 0, 1);
                    //MessageBox.Show(lat_Ref);
                }
               
                if (item.Id == 2)
                {
                   decimal degrees = (System.BitConverter.ToInt32(item.Value, 0) / System.BitConverter.ToInt32(item.Value, 4));
                   decimal minutes = (System.BitConverter.ToInt32(item.Value, 8) / System.BitConverter.ToInt32(item.Value, 12));
                   decimal seconds = (System.BitConverter.ToInt32(item.Value, 16) / System.BitConverter.ToInt32(item.Value, 20));

                    decimal lat_dd = System.Math.Round( degrees + (minutes / 60) + (seconds / 3600),6);
                    ExifDaten.Latitude = degrees.ToString() + "°" + minutes + "'" + seconds + (char)34 + " " + lat_Ref;
                    ExifDaten.LatitudeDez = "Dezimal: " + lat_dd.ToString() + " " + lat_Ref;
                }

                if (item.Id == 3)
                {
                    long_Ref = enc.GetString(item.Value, 0, 1);
                }
                if (item.Id == 4)
                {
                    decimal degrees = (System.BitConverter.ToInt32(item.Value, 0) / System.BitConverter.ToInt32(item.Value, 4));
                   decimal minutes = (System.BitConverter.ToInt32(item.Value, 8) / System.BitConverter.ToInt32(item.Value, 12));
                   decimal seconds = (System.BitConverter.ToInt32(item.Value, 16) / System.BitConverter.ToInt32(item.Value, 20));
                   
                   decimal long_dd = System.Math.Round( degrees + (minutes / 60) + (seconds / 3600),6);
                    ExifDaten.Longitude = degrees.ToString() + "°" + minutes + "'" + seconds + (char)34 + " " + long_Ref;
                    ExifDaten.LongitudeDez = "Dezimal: " + long_dd.ToString() + " " + long_Ref;
                }
               

                if (item.Id == 6)
                {
                    ExifDaten.Altitude = "Höhe über NN: " + (((Int32)System.BitConverter.ToInt32(item.Value, 0)) / 1000).ToString() + " Meter";
                }
              
               
                if (item.Id == 272) ExifDaten.Kamera = enc.GetString(item.Value, 0, item.Len - 1);
                //if (item.Id == 36867) ExifDaten.AufnahmeDat = enc.GetString(item.Value, 0, item.Len - 1);
                if (item.Id == 33437) ExifDaten.Blende = (BitConverter.ToInt32(item.Value, 0) / BitConverter.ToInt32(item.Value, 4)).ToString();

                if (item.Id == 33434) ExifDaten.Belichtung = (BitConverter.ToInt32(item.Value, 0)).ToString() + "/" + (BitConverter.ToInt32(item.Value, 4)).ToString() + " sec.";
                if (item.Id == 34855) ExifDaten.ISO = (BitConverter.ToInt16(item.Value, 0)).ToString();
                if (item.Id == 37386) ExifDaten.Brennweite = (BitConverter.ToInt32(item.Value, 0) / BitConverter.ToInt32(item.Value, 4)).ToString() + " mm";
                // Datum und Uhrzeit auslesen
                if (item.Id == 306) ExifDaten.AufnahmeDat = System.Text.Encoding.Default.GetString(item.Value);

            }
            imgLoaded.Dispose();
            //MessageBox.Show(ExifDaten.Latitude + "\r\n" + ExifDaten.LatitudeDez + "\r\n" + ExifDaten.Longitude + "\r\n" + ExifDaten.LongitudeDez + "\r\n" + ExifDaten.Altitude);

        }
        public static void clearExif()
        {
            ExifDaten.Altitude = null;
            ExifDaten.AufnahmeDat = null;
            ExifDaten.Belichtung = null;
            ExifDaten.Blende = null;
            ExifDaten.Brennweite = null;
            ExifDaten.ISO = null;
            ExifDaten.Kamera = null;
            ExifDaten.Latitude = null;
            ExifDaten.LatitudeDez = null;
            ExifDaten.Longitude = null;
            ExifDaten.LongitudeDez = null;
           
        }
    }

    public class IPTC_Const
    {
        public const string IPTC_Headline = @"/app13/irb/8bimiptc/iptc/Headline"; // ; //Detail
        public const string IPTC_Caption = @"/app13/irb/8bimiptc/iptc/Caption"; // ; //Bemerkung
        public const string IPTC_Object = @"/app13/irb/8bimiptc/iptc/Object name"; //Objekt
        public const string IPTC_City = @"/app13/irb/8bimiptc/iptc/city"; //Fundstelle Ort
        public const string IPTC_Loacation = @"/app13/irb/8bimiptc/iptc/sub-location"; //Position
        public const string IPTC_State = @"/app13/irb/8bimiptc/iptc/Province\/State"; // Land
        public const string IPTC_Country = @"/app13/irb/8bimiptc/iptc/Country\/Primary Location Name"; // Country
        public const string IPTC_Quelle = @"/app13/irb/8bimiptc/iptc/source"; //Quelle (Negativ)
        public const string IPTC_Copyright = @"/app13/irb/8bimiptc/iptc/copyright notice"; //Copyright
        public const string IPTC_Author = @"/app13/irb/8bimiptc/iptc/by-line"; //Autor
        public const string IPTC_Instr = @"/app13/irb/8bimiptc/iptc/Edit Status"; //Hinweis (in DB aufgenommen)
        public const string IPTC_DigitCreateDate = @"/app13/irb/8bimiptc/iptc/Digital Creation Date"; //Form yyyyMMdd
        public const string IPTC_Created = @"/app13/irb/8bimiptc/iptc/Date Created"; //Form yyyyMMdd
        public const string IPTC_Special = @"/app13/irb/8bimiptc/iptc/Special Instructions";
    }

    public class myIPTCDaten
    {
        public static string iObjekt; //Feld Objekt in Grunddaten
        public static string iDeteil; //Feld Detail in Grunddaten
        public static string iBemerkung; //Feld Bemerkung in Grunddaten
        public static string iFundstelleOrt; //Feld Fundstelle Ort in Exponate
        public static string iPostition; //Feld Postition in Exponate (ggf. auf GPS-Daten)
        public static string iFundstelleLand; //Feld Fundstelle Land in Exponate
        public static string iFundstelleCountry; //Feld Fundstelle Bundesland in Exponate
        public static string iQuelle; //Herkunft (Digitalbild, Analolgbild, Internet, Scann) aus Externer Tabelle
        public static string iSpezial; //nähere Bezeichung der Quelle (Negativ-Nr, Datei-Name des Originals, Webseite bei Internet, Scann-Quelle)
        public static string iCopyright; //PTBBerlin    
        public static string iAutor; //PTBBerlin
        public static string iHinweise;//Text: In DB als Objekt Nr. xxx aufgenommen.
        public static string iDigitalErstellt;
        public static string iErstellt;
        public static List<string> iStichworte;//
        public static string iStichwortText; //iStichworte als ein String kombiniert
    }
    public class IPTCDaten
    {
        public string iObjekt { get; set; }
        public string iDeteil { get; set; }
        public string iBemerkung { get; set; }
        public string iFundstelleOrt { get; set; }
        public string iPostition { get; set; }
        public string iFundstelleLand { get; set; }
        public string iFundstelleCountry { get; set; }
        public string iQuelle { get; set; }
        public string iSpezial { get; set; }
        public string iCopyright { get; set; }
        public string iAutor { get; set; }
        public string iHinweise { get; set; }
        public string iDigitalErstellt { get; set; }
        public string iErstellt { get; set; }
        public List<string> iStichworte { get; set; }
        public string iStichwortText { get; set; }
        /// <summary>
        /// Konstrukton IPTCDaten
        /// mit curImg als Übergabeparameter
        /// </summary>
        /// <param name="curImg"></param>
        public IPTCDaten(string curImg)
        {
            //if (curImg == null)
            // {
            //     return;
            // }
            string dummy = null;
            string sDocument = curImg;
            if (File.Exists(sDocument) == false)
            {
                return;
            }
            var stream = new FileStream(sDocument, FileMode.Open, FileAccess.Read);
            var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
            var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
            if (metadata != null)
            {
                //Anzeige StichworteText
                try
                {
                    dummy = metadata.Keywords.Aggregate((old, val) => old + "; " + val);
                    iStichwortText = dummy;
                }
                catch (Exception)
                {
                    iStichwortText = "keine Angaben";
                }
                //Anzeige Stichworte (Liste)

                List<string> lst = new List<string>();
                if (metadata.Keywords != null)
                {
                    //iStichworte = metadata.Keywords.ToList();
                    lst = metadata.Keywords.ToList();
                    iStichworte = lst.ToList();
                }
                else
                {
                    //lst.Add("ohne");
                    //iStichworte = lst.ToList();
                }
                //Anzeige Beschreibung
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Headline);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iDeteil = dummy;
                }
                else
                    iDeteil = "keine Angaben";
                //Anzeige Detail
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Caption);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iBemerkung = dummy;
                }
                else
                    iBemerkung = "keine Angaben";
                //Anzeige Art
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Object);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iObjekt = dummy;
                }
                else
                    iObjekt = "keine Angaben";
                //Anzeige Fundstelle Ort
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_City);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iFundstelleOrt = dummy;
                }
                else
                    iFundstelleOrt = "keine Angaben";
                //Anzeige Position Fundstelle
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Loacation);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iPostition = dummy;
                }
                else
                    iPostition = "keine Angaben";
                //Anzeige Land
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_State);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iFundstelleLand = dummy;
                }
                else
                    iFundstelleLand = "keine Angaben";
                //Aneige Country/Bundesland
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Country);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iFundstelleCountry = dummy;
                }
                else
                    iFundstelleCountry = "keine Angaben";
                //Anzeige Quelle (z.B.Digaitalbild...)
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Quelle);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iQuelle = dummy;
                }
                else
                    iQuelle = "keine Angaben";
                //Anzeige Spezial (z.B. Negati Nr.)
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Special);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iSpezial = dummy;
                }
                else
                    iSpezial = "keine Angaben";
                //Anzeige Copyright
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Copyright);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iCopyright = dummy;
                }
                else
                    iCopyright = "keine Angaben";
                //Anzeige Autor
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Author);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iAutor = dummy;
                }
                else
                    iAutor = "keine Angaben";
                //Anzeige Hinweise (z.B. in DB aufgenommen)
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Instr);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    iHinweise = dummy;
                }
                else
                    iHinweise = null;
                // Anzeige Digital erstellt
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_DigitCreateDate);
                if (String.IsNullOrEmpty(dummy) == false)
                {
                    try
                    {
                        iDigitalErstellt = DateTime.ParseExact(dummy, "yyyyMMdd", CultureInfo.CurrentCulture).ToString();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ungültiges Datumsformat. Datum 'Digital erstellt' wird als " + dummy + " eingetragen!");
                        iDigitalErstellt = dummy;
                    }
                }
                else
                    iDigitalErstellt = null;
                //Anzeige Erstellt
                dummy = (string)metadata.GetQuery(IPTC_Const.IPTC_Created);
                if (String.IsNullOrEmpty(dummy) == false)
                    try
                    {
                        iErstellt = DateTime.ParseExact(dummy, "yyyyMMdd", CultureInfo.CurrentCulture).ToString();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ungültiges Datumsformat. Datum 'Erstellt' wird als " + dummy + " eingetragen!");
                        iErstellt = dummy;
                    }
                else
                    iErstellt = null;
            }


            stream.Close();
            stream = null;
        }

        /// <summary>
        /// Speichern der IPTC-Daten
        /// der Bilddatei myFile
        /// </summary>
        /// <param name="myFile"></param>
        public static void WriteIPTC(string myFile)
        {
            if (string.IsNullOrEmpty(myFile) == true)
            { return; }

            var originalImage = new FileInfo(myFile);
            BitmapFrame bitmapFrame = null;
            // load the jpg file with a PngBitmapDecoder    
            using (Stream pngStreamIn = File.Open(myFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                var decoder = new JpegBitmapDecoder(pngStreamIn, BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.OnLoad);
                bitmapFrame = decoder.Frames[0];
            }
            var metadata = bitmapFrame.Metadata.Clone() as BitmapMetadata;

            if ((myIPTCDaten.iStichworte != null))
            {
                metadata.Keywords = new ReadOnlyCollection<string>(myIPTCDaten.iStichworte);
                //replace keywords
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iObjekt))
            {
                metadata.SetQuery(IPTC_Const.IPTC_Object, myIPTCDaten.iObjekt);
                //  set new description
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iBemerkung))
            {
                metadata.SetQuery(IPTC_Const.IPTC_Caption, myIPTCDaten.iBemerkung);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iDeteil))
            {
                metadata.SetQuery(IPTC_Const.IPTC_Headline, myIPTCDaten.iDeteil);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iAutor))
            {
                metadata.SetQuery(IPTC_Const.IPTC_Author, myIPTCDaten.iAutor);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iPostition))
            {
                metadata.SetQuery(IPTC_Const.IPTC_Loacation, myIPTCDaten.iPostition);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iFundstelleCountry))
            {
                metadata.SetQuery(IPTC_Const.IPTC_Country, myIPTCDaten.iFundstelleCountry);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iFundstelleOrt))
            {
                metadata.SetQuery(IPTC_Const.IPTC_City, myIPTCDaten.iFundstelleOrt);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iFundstelleLand))
            {
                metadata.SetQuery(IPTC_Const.IPTC_State, myIPTCDaten.iFundstelleLand);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iQuelle))
            {
                //MessageBox.Show(myIPTCDaten.iQuelle);
                metadata.SetQuery(IPTC_Const.IPTC_Quelle, myIPTCDaten.iQuelle);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iSpezial))
            {
                //MessageBox.Show(myIPTCDaten.iSpezial);
                metadata.SetQuery(IPTC_Const.IPTC_Special, myIPTCDaten.iSpezial);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iHinweise))
            {
                metadata.SetQuery(IPTC_Const.IPTC_Instr, myIPTCDaten.iHinweise);
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iDigitalErstellt))
            {

                metadata.SetQuery(IPTC_Const.IPTC_DigitCreateDate, DateTime.Now.ToString("yyyyMMdd"));
            }
            if (!string.IsNullOrWhiteSpace(myIPTCDaten.iErstellt))
            {
                metadata.SetQuery(IPTC_Const.IPTC_Created, myIPTCDaten.iErstellt);
            }
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapFrame, bitmapFrame.Thumbnail, metadata, bitmapFrame.ColorContexts));
            originalImage.Delete();
            using (Stream pngStreamOut = File.Open(myFile, FileMode.Create, FileAccess.ReadWrite))
            {
                encoder.Save(pngStreamOut);
            }
        }

       

        //public string GetPngDescription(string path)
        //{
        //    using (var pngStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
        //    {
        //        var pngDecoder = new PngBitmapDecoder(pngStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
        //        var pngInplace = pngDecoder.Frames[0].Metadata as BitmapMetadata;
        //        var description = pngInplace == null ? null : pngInplace.GetQuery("/Text/Description");
        //        return description == null ? null : description.ToString();
        //    }
        //}
    }


}
