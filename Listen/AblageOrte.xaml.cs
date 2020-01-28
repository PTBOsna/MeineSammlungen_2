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
    /// Interaktionslogik für AblageOrte.xaml
    /// </summary>
    public partial class AblageOrte : Window
    {
        public AblageOrte()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDGAblage();

          
        }

        private void LoadDGAblage()
        {
            SammlungDataClassDataContext con = new SammlungDataClassDataContext();
            List<Ablage> ablage = (from a in con.Ablage select a).ToList();
            DGAblage.ItemsSource = ablage;
        }

        private void Button_Click_Info(object sender, RoutedEventArgs e)
        {
            Ablage selected = DGAblage.SelectedItem as Ablage;
            if (selected == null)
            {
                MessageBox.Show("Bitte einen Ablageort auswählen!");
                return;
            }
            UpdateAblageOrte updateAblage = new UpdateAblageOrte(selected);
            updateAblage.ShowDialog();
            LoadDGAblage();
            //DialogResult = false;
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            Ablage selected = DGAblage.SelectedItem as Ablage;
            if (selected == null)
            {
                MessageBox.Show("Bitte den zu löschenden Ablagort auswählen!");
            }
            else
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Wirklich löschen?", "Ablageort löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    using (SammlungDataClassDataContext conn = new SammlungDataClassDataContext())
                    {
                        var abl = from a in conn.Ablage where a.ID == selected.ID select a;
                        conn.Ablage.DeleteAllOnSubmit(abl);
                        conn.SubmitChanges();
                    }

                  LoadDGAblage();
                }
            }

        }

        private void Button_cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Click_new(object sender, RoutedEventArgs e)
        {
            AddAblageOrt adAbl = new AddAblageOrt();
            adAbl.ShowDialog();
            LoadDGAblage();

        }
    }
}
