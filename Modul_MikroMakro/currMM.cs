using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeineSammlungen_2.Modul_MikroMakro
{
    class currMM
    {
        class currMikroMakro
        {
            public Int32 cID { get; set; }
            public string cSchnittebene { get; set; }
            public string cSchnittart { get; set; }
            public string cFarbung { get; set; }
            public string cAufhellung { get; set; }
            public string cFixierung { get; set; }
            public string cEinschluss { get; set; }
            public string cHinweise { get; set; }
            public Int32 cGrunddaten_ID { get; set; }
        }
        public static class editMM
        {
            public static void editMikro(ModulMikro modulMikro)
            {
                //using (SammlungDataClassDataContext conn = new SammlungDataClassDataContext())
                //{
                ModulMikro md = (from g in Admin. Admin.conn.ModulMikro where g.ID == modulMikro.ID select g).FirstOrDefault();
                md.Schnittebene = modulMikro.Schnittebene;
                md.Schnittart = modulMikro.Schnittart;
                md.Farbung = modulMikro.Farbung;
                md.Aufhellung = modulMikro.Aufhellung;
                md.Fixierung = modulMikro.Fixierung;
                md.Einschluss = modulMikro.Einschluss;
                md.Hineise = modulMikro.Hineise;
                md.Grunddaten_ID = modulMikro.Grunddaten_ID;

                Admin. Admin.conn.SubmitChanges();

                //}
            }
        }
    }
}
