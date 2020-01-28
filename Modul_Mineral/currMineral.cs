using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeineSammlungen_2.Modul_Mineral
{
    public class currMineral
    {
        public Int32 cID { get; set; }
        public string cFundstelle_Land { get; set; }
        public string cFundstelle_Ort { get; set; }
        public string cKoordinaten { get; set; }
        public string cHinweise { get; set; }
        public string cFunddatum { get; set; }
        public Int32 cGrunddaten_ID { get; set; }
        public float cGewicht { get; set; }
        public float cVolumen { get; set; }
        public float cDichte { get; set; }
        public string cZusammensetzung { get; set; }

    }

    public static class editMineral
    {
        public static void edit(Mineralien mineralien)
        {
            Mineralien ex = (from e in Admin.Admin.conn.Mineralien where e.ID == mineralien.ID select e).FirstOrDefault();
            ex.Fundstelle_Land = mineralien.Fundstelle_Land;
            ex.Fundstelle_Ort = mineralien.Fundstelle_Ort;
            ex.Koordinaten = mineralien.Koordinaten;
            ex.Hinweise = mineralien.Hinweise;
            ex.Fund_Datum = mineralien.Fund_Datum;
            ex.Grunddaten_ID = mineralien.Grunddaten_ID;
            ex.Gewicht = mineralien.Gewicht;
            ex.Volumen = mineralien.Volumen;
            ex.Dichte = mineralien.Dichte;
            ex.Zusammensetzung = mineralien.Zusammensetzung;

            Admin.Admin.conn.SubmitChanges();
        }

    }
}

