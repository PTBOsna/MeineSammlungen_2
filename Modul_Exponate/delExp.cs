using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeineSammlungen_2.Modul_Exponate
{
    class delExp
    {
        public static bool deleteExp(int gdID)
        {
            var deleteEx = from gd in Admin.Admin.conn.Exponate
                           where gd.ID == gdID
                           select gd;

            foreach (var eintrag in deleteEx)
            {
                Admin.Admin.conn.Exponate.DeleteOnSubmit(eintrag);
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
