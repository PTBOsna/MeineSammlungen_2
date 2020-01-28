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

namespace MeineSammlungen_2.Modul_Exponate
{
    /// <summary>
    /// Interaktionslogik für PageExponate.xaml
    /// </summary>
    public partial class PageExponate : Page
    {
        private Int32 gdID;
        public PageExponate(Int32 _gdID)
        {
            InitializeComponent();
            this.gdID = _gdID;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var currEx = from ex in Admin. Admin.conn.Exponate where ex.Grunddaten_ID == gdID select ex;
            foreach (var ex in currEx)
            {
                LandText.Text = ex.Fundstelle_Land;
                OrtTExt.Text = ex.Fundstelle_Ort;
                KoordinatenText.Text = ex.Koordinaten;
                HinweiseExpoText.Text = ex.Hinweise;
                FunddatumText.Text = ex.Fund_Datum;
            }
        }
    }
}
