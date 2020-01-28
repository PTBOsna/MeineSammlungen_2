using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;


namespace MeineSammlungen_2.Admin
{
    public class Picture
    {
        public string Name { get; set; }
        public BitmapImage BitmapImage { get; set; }
        public DateTime DateCreated { get; set; }
        public string Path { get; set; }
        public string FileType { get; set; }
        
    }

    public class PictureList : ObservableCollection<Picture>
    {
        public PictureList(string myImges) //selPicture muss den Stamm des FileNmaens enthalten (z.B. Filename = 12#1.jpg, Stamm = 12)
        {
            Refresh(myImges);
        }

        private void Refresh(string _myImges)
        {
            string cPath = null;
            string pattern = null;
            String[] stamm = _myImges.Split('#'); //Wenn _myImages "*#<Path>" enthält, dann erfolgt Auflistung alle Bilder im Pfad <Path>
            //string pathPictures =@"H:\Mikro-Makro\TestOrdner";
            if (stamm[0] == "*")
            {
                cPath = stamm[1];
                pattern = "*.jpg";
            }
            else
            {
                pattern = stamm[0] + "#*.jpg";
                cPath = Admin.ImgPath;
            }
            string[] fileNames = Directory.GetFiles(cPath,  pattern);
            //.Union(Directory.GetFiles(pathPictures, imgPath + "*.jpg"))
            //                           .Union(Directory.GetFiles(pathPictures,imgPath + "*.jpeg"))
            //                           .OrderBy(f => f)
            //                           .ToArray();
            foreach (string fileName in fileNames)
            {
              
                //if (fileName.Contains(stamm[0]) == true)
                //{

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(fileName);
                    bitmap.DecodePixelWidth = 100;
                    bitmap.EndInit();
                   


                    this.Add(new Picture()
                    {
                        Name = Path.GetFileNameWithoutExtension(fileName),
                        BitmapImage = bitmap,
                        DateCreated = File.GetCreationTime(fileName),
                        Path = fileName,
                        FileType = Path.GetExtension(fileName),
                        
                    });
                    
                //}
               
            }
            
        }
    }
}
