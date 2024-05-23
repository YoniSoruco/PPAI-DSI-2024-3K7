using BonVinoGrupo12.Pantallas;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace BonVinoGrupo12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Caso Uso Alternativo Inventado
        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            //Limpia la pantalla en cualquier momento del CU
            ShowGrid.Children.Clear();
        }
        #endregion

        #region Paso 1 del Caso de Uso
        //Primer Paso del CU
        private void selOpcionImportarActualizacion(object sender, RoutedEventArgs e)
        {
            //Este metodo crea la pantalla asi tenemos mas flexibilidad si queremos agregar cosas al menu
            habilitarPantalla();
        }
        
        private void habilitarPantalla()
        {
            PantallaImportarActualizacion pantalla = new();
            ShowGrid.Children.Clear();
            ShowGrid.Children.Add(pantalla);
        }

        #endregion

        #region Caso Uso Alternativo 1
        private void casoAlternativo1Button_Click(object sender, RoutedEventArgs e)
        {
            //Este se llamaria igual que selOpcionImportarActualizacion() pero para evitar errores le dejamos este nombre
            ShowGrid.Children.Clear();

            PantallaAlternativa1 alt1 = new();
            ShowGrid.Children.Add(alt1);
        }
        #endregion
    }
}