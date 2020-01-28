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
    /// Interaktionslogik für UpdateAblageOrte.xaml
    /// </summary>
    public partial class UpdateAblageOrte : Window
    {
        private Ablage ablage;
        public UpdateAblageOrte(Ablage ablage)
        {
            InitializeComponent();
            this.ablage = ablage;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtAblageort.Text = ablage.Ablageort;
            txtBeschreibung.Text = ablage.Beschreibung;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Update_Ablage_Click(object sender, RoutedEventArgs e)
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
                ablage.Ablageort = txtAblageort.Text.Trim();
                ablage.Beschreibung = txtBeschreibung.Text.Trim();
                using (SammlungDataClassDataContext conn = new SammlungDataClassDataContext())
                {
                    Ablage abl = (from a in conn.Ablage where a.ID == ablage.ID select a).FirstOrDefault();
                    abl.Ablageort = ablage.Ablageort;
                    abl.Beschreibung = ablage.Beschreibung;
                    conn.SubmitChanges();
                    MessageBox.Show(abl.Ablageort + " wurde übenommen.");
                    DialogResult = false;
                }
            }

        }

    }
}
