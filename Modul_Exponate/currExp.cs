using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeineSammlungen_2.Modul_Exponate
{
    public class currExp
    {
        public Int32 cID { get; set; }
        public string cFundstelle_Land { get; set; }
        public string cFundstelle_Ort { get; set; }
        public string cKoordinaten { get; set; }
        public string cHinweise { get; set; }
        public string cFunddatum { get; set; }
        public Int32 cGrunddaten_ID { get; set; }
    }

    public static class editEx
    {
        public static void editExponat(Exponate exponate)
        {
            Exponate ex = (from e in Admin. Admin.conn.Exponate where e.ID == exponate.ID select e).FirstOrDefault();
            ex.Fundstelle_Land = exponate.Fundstelle_Land;
            ex.Fundstelle_Ort = exponate.Fundstelle_Ort;
            ex.Koordinaten = exponate.Koordinaten;
            ex.Hinweise = exponate.Hinweise;
            ex.Fund_Datum = exponate.Fund_Datum;
            ex.Grunddaten_ID = exponate.Grunddaten_ID;

            Admin. Admin.conn.SubmitChanges();
        }

    }
}
