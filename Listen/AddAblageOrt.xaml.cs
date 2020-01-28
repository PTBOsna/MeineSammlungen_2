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

namespace MeineSammlungen_2.Listen
{
    /// <summary>
    /// Interaktionslogik für AddAblageOrt.xaml
    /// </summary>
    public partial class AddAblageOrt : Window
    {
        public AddAblageOrt()
        {
            InitializeComponent();
        }

        private void Add_Ablage_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(txtAblageort.Text) == true)
            {
                MessageBox.Show("Ablageort fehlt!");
            }
            if (string.IsNullOrEmpty(txtBeschreibung.Text) == true)
            {
                MessageBox.Show("Beschreibung fehlt!");
            }
            else
            {
                Ablage abl = new Ablage();
                abl.Ablageort = txtAblageort.Text.Trim();
                abl.Beschreibung = txtBeschreibung.Text.Trim();
                using (SammlungDataClassDataContext conn = new SammlungDataClassDataContext())
                {
                    conn.Ablage.InsertOnSubmit(abl);
                    conn.SubmitChanges();
                }
                MessageBox.Show(abl.Ablageort + " wurde hinzugefügt.");
                    DialogResult = true;
               
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
