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

namespace BonVinoGrupo12.Pantallas
{
    /// <summary>
    /// Lógica de interacción para PantallaAlternativa1.xaml
    /// </summary>
    public partial class PantallaAlternativa1 : UserControl
    {
        private List<string> listadoBodegasParaSeleccion;
        public PantallaAlternativa1()
        {
            InitializeComponent();
        }

        #region Paso 4 del Caso de Uso
        private void alternativo1Control_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ATENCION: No se encontro ninguna bodega para actualizar", "ATENCION", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void seleccionBodegaBoton_Click(object sender, RoutedEventArgs e) => selOpTomarSeleccionBodega();
        private void grillaBodegasActualizar_MouseDoubleClick(object sender, MouseButtonEventArgs e) => selOpTomarSeleccionBodega();
        private void selOpTomarSeleccionBodega()
        {
            int index = grillaBodegasActualizar.SelectedIndex;
            //Esto es para evitar selecciones nulas
            if (index != -1)
            {
                string nombre = listadoBodegasParaSeleccion[index];
                bodegaSeleccionadaTextbox.Text = nombre;
                //gestorImportar.tomarSeleccionBodega(nombre);
            }
            else
            {
                MessageBox.Show("ERROR: No se selecciono ninguna bodega", "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
        #endregion

    }
}
