using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeineSammlungen_2.Modul_Mineral
{
    class delMineral
    {
        public static bool deleteMin(int gdID)
        {
            var deleteMin = from gd in Admin.Admin.conn.Mineralien
                           where gd.ID == gdID
                           select gd;

            foreach (var eintrag in deleteMin)
            {
                Admin.Admin.conn.Mineralien.DeleteOnSubmit(eintrag);
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
