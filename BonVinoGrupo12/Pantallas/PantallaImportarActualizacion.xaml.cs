using BonVinoGrupo12.Gestores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace BonVinoGrupo12.Pantallas
{
    /// <summary>
    /// Lógica de interacción para PantallaCU5.xaml
    /// </summary>
    public partial class PantallaImportarActualizacion : UserControl
    {
        #region Atributos

        private GestorImportarActualizacion gestorImportar;
        private List<string> listadoBodegasParaSeleccion;


        #endregion

        public PantallaImportarActualizacion()
        {
            InitializeComponent();
        }

        #region Paso 1 del Caso de Uso
        private void pantallaActualizacion_Loaded(object sender, RoutedEventArgs e)
        {
            //Esto es para no mostrar la siguiente pantalla hasta que ocurra el evento
            gridMostrarResumenVino.Visibility = Visibility.Collapsed;

            //Esto lo hacemos ya que el gestor no se encuentra creado. En el constructor hacemos que tenga de atributo la pantalla
            gestorImportar = new(this);
            gestorImportar.opcionImportarActualizacion();
        }
        #endregion

        #region Paso 2 y 3 del Caso de Uso
        public void mostrarBodegasParaActualizar(List<string> bodegasActualizar)
        {
            listadoBodegasParaSeleccion = bodegasActualizar;
            DataTable datosMostrar = new();
            datosMostrar.Columns.Add("Nombre de Bodega");
            
            //Esto lo hacemos para poder mostrar el nombre que deseamos
            foreach (var nombres in listadoBodegasParaSeleccion) datosMostrar.Rows.Add(nombres);

            grillaBodegasActualizar.ItemsSource = datosMostrar.DefaultView;
        }
        #endregion

        #region Paso 4 del Caso de Uso
        //Creamos un metodo por separado para poder llamarlo desde el doble click y desde el boton seleccionar
        private void grillaBodegasActualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => selOpTomarSeleccionBodega();
        private void seleccionBodegaBoton_Click(object sender, RoutedEventArgs e) => selOpTomarSeleccionBodega();
        private void selOpTomarSeleccionBodega()
        {
            grillaResumenesVino.ItemsSource = null;
            int index = grillaBodegasActualizar.SelectedIndex;
            //Esto es para evitar selecciones nulas
            if (index != -1)
            {
                string nombre = listadoBodegasParaSeleccion[index];
                bodegaSeleccionadaTextbox.Text = nombre;
                gestorImportar.tomarSeleccionBodega(nombre);
            }
            else
            {
                //Esto tambien seria el caso Alternativo 1
                MessageBox.Show("ERROR: No se selecciono ninguna bodega","ERROR",MessageBoxButton.OK,MessageBoxImage.Stop);
            }
        }
        #endregion

        #region Paso 6 del Caso de Uso

        public void mostrarResumenVinos(DataTable datos)
        {
            gridMostrarResumenVino.Visibility = Visibility.Visible;
            grillaResumenesVino.ItemsSource = datos.DefaultView;
        }
        #endregion
    }
}
