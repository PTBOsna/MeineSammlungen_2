using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MeineSammlungen_2
{
    /// <summary>
    /// Interaktionslogik für ShowImage.xaml
    /// </summary>
    public partial class ShowImage : Window
    { private string myImg;
        public ShowImage(string _myImg)
        {
            InitializeComponent();
            this.myImg = _myImg;

            BitmapImage myBitMap = new BitmapImage();
            myBitMap.BeginInit();
            myBitMap.CacheOption = BitmapCacheOption.OnLoad;
            myBitMap.UriSource = new Uri(myImg);
            myBitMap.EndInit();
            MyImage.Source = myBitMap;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
