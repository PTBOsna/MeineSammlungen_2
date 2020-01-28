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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeineSammlungen_2.Modul_MikroMakro
{
    /// <summary>
    /// Interaktionslogik für PageMikroMakro.xaml
    /// </summary>
    public partial class PageMikroMakro : Page
    {
        private Int32 gdID;
        public PageMikroMakro(Int32 _gdID)
        {
            InitializeComponent();
            this.gdID = _gdID;
        }

       

        private void ucPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var currM = from m in Admin.Admin.conn.ModulMikro where m.Grunddaten_ID == gdID select m;

            foreach (var mMikro in currM)
            {
                SchnittText.Text = mMikro.Schnittebene;
                SchnittartText.Text = mMikro.Schnittart;
                FarbeText.Text = mMikro.Farbung;
                HellText.Text = mMikro.Aufhellung;
                FixierungText.Text = mMikro.Fixierung;
                EinschlussText.Text = mMikro.Einschluss;
                HinweiseText.Text = mMikro.Hineise;

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
