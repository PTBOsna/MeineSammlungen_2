using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MeineSammlungen_2.Admin
{
    class Admin
    {
        public static SammlungDataClassDataContext conn = new SammlungDataClassDataContext();

        #region Grunddaten
        /// <summary>
        /// Grunddaten mit Modul-ID neu laden
        /// </summary>
        /// <param name="mID"></param>
        /// <returns></returns>
        public static List<Grunddaten> GetDataGrunddatenListe(Int32 mID)
        {
           
            List<Grunddaten> fillList = (from g in Admin.conn.Grunddaten
                                         where g.Modul == mID
                                         select g).ToList();
            //System.Windows.Forms.MessageBox.Show(fillList.Count.ToString());
            return fillList;
        }
        #endregion
        #region Settings
        public static string ImgPath;

        public static void GetSettings()
        {
            var st = from s in Admin.conn.Settings select s;
            foreach (var item in st)
            {
                ImgPath = item.Bildpfad;
            }
        }


        #endregion
        #region Allgemeines
        public static string cName; //Original Dateiname der übernommenen Bilder
        public static bool FileIsLocked(string strFullFileName)
        {
            bool blnReturn = false;
            System.IO.FileStream fs;
            try
            {
                fs = File.Open(strFullFileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                fs.Close();
            }
            catch (System.IO.IOException ex)
            {
                blnReturn = true;
            }
            return blnReturn;
        }
        #endregion

    }
}

