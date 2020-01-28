using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeineSammlungen_2.Admin;

namespace MeineSammlungen_2.Modul_Grunddaten
{
    class currGD
    {
        public Int32 cID { get; set; }
        public string cNr { get; set; }
        public Int32 cModul { get; set; }
        public string cObjekt { get; set; }
        public string cDetail { get; set; }
        public DateTime cErstellt { get; set; }
        public string cBemerkung { get; set; }
        public string cAblage { get; set; }
        public DateTime cGeaendert { get; set; }
        public Int32 cImgCount { get; set; }
        public Int32 cAblage_neu { get; set; }
        public bool cChecked { get; set; }

        public currGD(Int32 gdID)
        {

            var selGD = from g in Admin.Admin.conn.Grunddaten where g.ID == gdID select g;
            foreach (var item in selGD)
            {
                cNr = item.Nr;
                cObjekt = item.Objekt;
                cDetail = item.Detail;
                cAblage = item.Ablageort;
                cBemerkung = item.Bemerkung;
                cErstellt = (DateTime)item.Erstellt;
                if (string.IsNullOrEmpty(item.Geaendert.ToString()) == true)
                { cGeaendert = DateTime.Now; }
                else
                { cGeaendert = (DateTime)item.Geaendert; }

                cID = item.ID;
                cModul = item.Modul;
                cImgCount = item.ImgCount;
                cAblage_neu = item.Ablageort_neu;
                if (item.Checked == true)
                {
                    cChecked = true;
                }

            }

        }
        public static Int32  addGD(Int32 mID)
        {
           
            // Objekt speichern um neuen Datensatz zu erzeugen
            Grunddaten addGD = new Grunddaten();
            addGD.Objekt = "Objekt";
            addGD.Modul = mID;
            addGD.Erstellt = DateTime.Now;
            addGD.Nr = mID.ToString();
            addGD.LfdNr = 0; //zunächst mit 0 vorbelegen
            addGD.Ablageort_neu = 1; //zunächst Standardablage
            Admin.Admin.conn.Grunddaten.InsertOnSubmit(addGD);
            Admin.Admin.conn.SubmitChanges();
            //neue GD-ID holen
            Int32 cID = addGD.ID; 
            // und neu max Nr abholen
            var max = from m in Admin.Admin.conn.Grunddaten select m.LfdNr;
            Int32 clfNr = (Int32) max.Max() + 1;
            // Nr generieren
            Grunddaten cGD = (from g in Admin.Admin.conn.Grunddaten where g.ID == cID select g).FirstOrDefault();
            cGD.LfdNr = clfNr;
            cGD.Nr = mID.ToString() + "-" + clfNr.ToString();
            Admin.Admin.conn.SubmitChanges();
           

            return cID;
        }
    }

    public static class editGD
    {
        public static void editGrunddaten(Grunddaten grunddaten)
        {
            Grunddaten gd = (from g in Admin.Admin.conn.Grunddaten where g.ID == grunddaten.ID select g).FirstOrDefault();
            if (string.IsNullOrEmpty(grunddaten.Nr) == true)
            { gd.Nr = "0"; }
            else gd.Nr = grunddaten.Nr;
            gd.Objekt = grunddaten.Objekt;
            gd.Detail = grunddaten.Detail;
            gd.Ablageort = grunddaten.Ablageort;
            gd.Ablageort_neu = grunddaten.Ablageort_neu;
            gd.Bemerkung = grunddaten.Bemerkung;
            gd.Erstellt = grunddaten.Erstellt;
            gd.Geaendert = grunddaten.Geaendert;
            gd.ImgCount = grunddaten.ImgCount;
            gd.Ablageort_neu = grunddaten.Ablageort_neu;
            gd.Checked = grunddaten.Checked;

            Admin.Admin.conn.SubmitChanges();
        }
    }

   
}