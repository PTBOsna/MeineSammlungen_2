using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeineSammlungen_2.Modul_Grunddaten
{
    class delGD
    {
        public static bool deleteGD(int gdID)
        {
            var deleteGD = from gd in Admin.Admin.conn.Grunddaten
                           where gd.ID == gdID
                           select gd;

            foreach (var eintrag in deleteGD)
            {
                Admin.Admin.conn.Grunddaten.DeleteOnSubmit(eintrag);
            }

            try
            {
                Admin.Admin.conn.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                // Provide for exceptions.
            }
        }
    }
}
