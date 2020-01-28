using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeineSammlungen_2.Modul_MikroMakro
{
    class delMM
    {
        public static bool deleteMM(int gdID)
        {
            var deleteGD = from gd in Admin.Admin.conn.ModulMikro
                           where gd.Grunddaten_ID == gdID
                           select gd;

            foreach (var eintrag in deleteGD)
            {
                Admin.Admin.conn.ModulMikro.DeleteOnSubmit(eintrag);
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
