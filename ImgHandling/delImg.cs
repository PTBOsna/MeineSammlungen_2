using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MeineSammlungen_2.ImgHandling
{
    public class delImg
    {
        public static bool ImgDel(string args)
        {
            string curPath = Admin.Admin.ImgPath;
            // Files to be deleted   
            try
            {
                foreach (var myFile in Directory.GetFiles(curPath))
                {
                    if (myFile.Contains(args) == true)
                    {
                        System.Windows.Forms.MessageBox.Show(myFile);
                        File.Delete(myFile);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //Console.ReadKey();
    }
}

