using BonVinoGrupo12.Entidades;
using BonVinoGrupo12.Pantallas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace BonVinoGrupo12.Gestores
{
    public class GestorImportarActualizacion
    {
        #region Atributos

        private readonly PantallaImportarActualizacion pantalla;
        private List<Bodega> listadoBodegasCompleto = new();
        private List<string> nombresBodegaActualizar = new();
        private List<Bodega> listadoParaActualizar = new();

        // private List<Vino> listadoVinosCompleto = new();
        private List<Vino> listadoVinosActualizarCompleto = new(); //No se pone en el diagrama de clases porque estaria en la API
        private List<Vino> listadoVinosActualizarBodega = new();
        private Bodega BodegaSeleccionada;

        //Este diccionario es necesario para solucionar el problema de la actualizacion usando bodega
        private Dictionary<Bodega, List<Vino>> bodegasXvino;


        private List<Maridaje> listadoMaridajeCompleto = new();

        private List<TipoUva> listadoTipoUvaCompleto = new();

        private List<Varietal> listadoVariedadesCompleto = new();

        private List<Enofilo> listadoEnofilosAplicacion = new();

        #region Alternativo 2
        private bool activarAlternativo3 = false;
        #endregion

        #endregion

        #region Paso 1 del Caso de Uso
        //El constructor obliga a que le pases una pantalla asi queda inicializado
        public GestorImportarActualizacion(PantallaImportarActualizacion pan)
        {
            this.pantalla = pan;
        }

        public void opcionImportarActualizacion()
        {
            crearListadoVinos();
            //Finaliza el Paso 1
            buscarBodegasParaActualizar();
        }
        #endregion

        #region Paso 2 del Caso de Uso
        private void buscarBodegasParaActualizar()
        {
            DateTime fechaHoy = getFechaActual();
            //Se limpia la lista por las dudas
            listadoParaActualizar.Clear();
            nombresBodegaActualizar.Clear();
            //Mientras haya Bodegas
            foreach (Bodega bod in listadoBodegasCompleto)
            {

                if(bod.estaEnPeriodoDeActualizacion(fechaHoy))
                {
                    //Guardamos los nombres y el listado de las bodegas filtradas para su seleccion futura
                    nombresBodegaActualizar.Add(bod.getNombre());
                    listadoParaActualizar.Add(bod);
                }
            }
            pantalla.mostrarBodegasParaActualizar(nombresBodegaActualizar);
        }
        private DateTime getFechaActual()
        {
            return DateTime.Now;
        }
        #endregion

        #region Paso 4 del Caso de Uso
        public void tomarSeleccionBodega(string nombre)
        {
            obtenerActualizacionVinosBodega(nombre);
        }
        private void obtenerActualizacionVinosBodega(string nombre)
        {
            listadoVinosActualizarBodega = obtenerActualizacionVinos(nombre);

            //Buscamos en el listado de bodegas
            BodegaSeleccionada = listadoParaActualizar.Find(x => x.getNombre() == nombre)??new();
            if (BodegaSeleccionada == null || BodegaSeleccionada.getNombre() == null) return; //Esto es solo si no lo encuentra por nombre y nos da error
            
            //Se realiza la operacion en el caso que haya al menos un vino para actualizar en esta bodega
            if(listadoVinosActualizarBodega.Count>0) actualizarOCrearVinos();

        }
        //Se crea el metodo pero se nos informa que no es necesario la comunicacion con ningun tipo de API por lo que devolvemos un listado
        // que contenga un listado de vinos a actualizar creado por nosotros.
        private List<Vino> obtenerActualizacionVinos(string nombre)
        {
            //Esto hace que cuando cambiemos la variable muestre el caso Alternativo 3 donde no responde la API
            if (activarAlternativo3)
            {
                MessageBox.Show("ERROR: Imposible conectarse con la API, intente mas tarde", "ERROR",MessageBoxButton.OK,MessageBoxImage.Hand);
                return new();
            }
            else return listadoVinosActualizarCompleto.Where(x => x.getBodega().getNombre() == nombre).ToList();
            //Esto simula una busqueda en la API porque nosotros ya tenemos un listado actualizar completo

        }

        private bool existeEsteVino(Vino vinoActualizado)
        {
            if (bodegasXvino.ContainsKey(vinoActualizado.getBodega()))
            {
                List<Vino> listadoVinos = bodegasXvino[vinoActualizado.getBodega()];

                foreach (var vinosBodegas in listadoVinos)
                {
                    if(vinosBodegas.getNombre() == vinoActualizado.getNombre()) return true;
                }
                return false;
            }
            //Esto deberia ser error porque no existiria la bodega en la lista actual
            else return false;
        }
        #endregion

        #region Paso 5 y 6 del Caso de Uso
        private void actualizarOCrearVinos()
        {
            DataTable resumenVinos = new();
            resumenVinos.Columns.Add("Bodega");
            resumenVinos.Columns.Add("Nombre Vino");
            resumenVinos.Columns.Add("Nota Cata");
            resumenVinos.Columns.Add("Precio ARS");
            resumenVinos.Columns.Add("Varietal");
            resumenVinos.Columns.Add("Actualizado o Creado");

            //No es necesario comprobar si es de la bodega ya que para esto le mandamos a la API el parametro
            foreach (Vino vinoActualizar in listadoVinosActualizarBodega)
            {
                bool alternativa = existeEsteVino(vinoActualizar);
                //Esto nos parecio mas comodo que crear dos listas por separado y recorrerlas

                if(alternativa)//Esta alternativa tiene que ver con que el vino si existe en la bodega actualmente
                {
                    actualizarCaracteristicaVinoExistente(vinoActualizar);
                    resumenVinos.Rows.Add(
                        vinoActualizar.getBodega().getNombre(),
                        vinoActualizar.getNombre(),
                        vinoActualizar.getNota(),
                        vinoActualizar.getPrecio(),
                        vinoActualizar.getVariedades()[0].getDescripcion(),
                        "ACTUALIZADO"
                        );
                }
                else//Este caso es cuando no es asi
                {
                    crearVinoNuevo(vinoActualizar);
                    resumenVinos.Rows.Add(
                    vinoActualizar.getBodega().getNombre(),
                    vinoActualizar.getNombre(),
                    vinoActualizar.getNota(),
                    vinoActualizar.getPrecio(),
                    vinoActualizar.getVariedades()[0].getDescripcion(),
                    "CREADO"
                    );
                }
            }
            //Actualizamos la bodega una vez que terminamos
            BodegaSeleccionada.setUltimaActualizacion(DateTime.Now);

            //Esta es la ultima parte que manda para mostrar
            pantalla.mostrarResumenVinos(resumenVinos);

            //Inicio paso 7
            buscarSeguidoresBodega();
        }

        //Alternativa de actualizar
        private void actualizarCaracteristicaVinoExistente(Vino vinoActualizar)
        {
            List<Vino> listadoVinosXBodega = bodegasXvino[vinoActualizar.getBodega()];

            //Esto es necesario porque bodega no contiene un listado de vinos propio y no se puede mandar a actualizar desde bodega sino lo conoce
            BodegaSeleccionada.actualizarDatosVinos(listadoVinosXBodega,vinoActualizar);
        }
        //Alternativa de crear
        private void crearVinoNuevo(Vino vinoACrear)
        {
            //Le mando el maridaje del vino traido desde la API
            //El signo de pregunta es que puede ser nulo sino lo encuentra
            Maridaje? maridajeActual = buscarMaridaje(vinoACrear.getMaridaje());

            TipoUva? tipoUva = buscarTipoUva(vinoACrear.getVariedades()[0].getTipoUva());

            //Esto es para evitar errores en caso que no encuentre
            if(maridajeActual != null && tipoUva != null) CrearVino(vinoACrear, tipoUva, maridajeActual);

        }

        private Maridaje? buscarMaridaje(Maridaje maridajeBuscar)
        {
            foreach (var maridaje in listadoMaridajeCompleto)
            {
                if (maridaje.esMaridaje(maridajeBuscar)) return maridaje;
            }
            return null;
        }
        private TipoUva? buscarTipoUva(TipoUva tipoUva)
        {
            foreach (var uvas in listadoTipoUvaCompleto)
            {
                if (uvas.esTipoUva(tipoUva)) return uvas;
            }
            return null;
        }

        private Vino CrearVino(Vino vin,TipoUva tip,Maridaje mar)
        {

            Varietal vari = vin.getVariedades()[0];
            //Esto es para crear la variedad y asignarle nuestro tipo de Uva
            vari.setTipoUva(tip);

            Vino vinoNuevo = new Vino(
                BodegaSeleccionada,
                vin.getAnianada(),
                vin.getFechaActualizacion(),
                vin.getImagenEtiqueta(),
                vin.getNombre(),
                vin.getNota(),
                vin.getPrecio(), //Desconozco porque esto no lo recuperamos como hacemos con el resto y listo
                mar,
                //Esto es la creacion del varietal

                new() { vari } //Crea el listado de variedades con el varietal principal
                );
           

            //Esta complicacion es que recuperamos el listado de vinos de la bodega seleccionada y agregamos el nuevo vino al listado
            //Se evita volver a crear con esto
            List<Vino> listadoVinosXBodega = bodegasXvino[vinoNuevo.getBodega()];
            listadoVinosXBodega.Add( vinoNuevo );
            bodegasXvino[vinoNuevo.getBodega()] = listadoVinosXBodega;

            return vinoNuevo;
        }
        #endregion

        #region Paso 7 del Caso de Uso
        private void buscarSeguidoresBodega()
        {
            List<Enofilo> listadoUsuarioANotificar = new();
            //Recorremos los usuarios porque es mas comodo para programar
            foreach (Enofilo enofilo in listadoEnofilosAplicacion)
            {
                if (enofilo.sigueABodega(BodegaSeleccionada))
                {
                    listadoUsuarioANotificar.Add(enofilo);
                }
            }

            foreach (Enofilo notificaciones in listadoUsuarioANotificar)
            {
                notificarActualizacionesDeVino(notificaciones);
            }

            MessageBox.Show("EXITO: los mensajes fueron enviados correctamente","EXITO",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void notificarActualizacionesDeVino(Enofilo notificacion)
        {
            //No pasa nada porque no tenemos una interfaz para comunicar esto (TP3 Se hara un Observer?)
        }
        #endregion

        #region Persistencia
        private void crearListadoVinos()
        {
            listadoMaridajeCompleto = new()
            {
                new("Pastas","Comidas de origen italiano"),
                new("Carnes","Origen animal"),
                new("Ensaladas","Origen Vegetal"),
                new("Postres","Lo que viene despues de la comida"),
            };

            TipoUva tipo1 = new("Rojas", "Rojas");
            TipoUva tipo2 = new("Morenas", "Morenas");
            TipoUva tipo3 = new("Al", "Rojas grandes");
            TipoUva tipo4 = new("Negras", "Negras");
            TipoUva tipo5 = new("Verdes", "Verdes");

            listadoTipoUvaCompleto = new()
            {
                tipo1,
                tipo2,
                tipo3,
                tipo4,
                tipo5
            };

            listadoVariedadesCompleto = new()
            {
                    new("Malbec",10,tipo1),
                    new("Cabernet Sauvignon",15,tipo2),
                    new("Merlot",8,tipo3),
                    new("Syrah",10,tipo4),
                    new("Barbera",12,tipo5),
            };

            //Actualizar son: Bonvino, Santa Julia, El quemada y el Becerra
            Bodega bodega1 = new ("Bonvino", DateTime.Parse("01/01/2024"), 3);
            Bodega bodega2 = new ("Cavani Bodega", DateTime.Parse("01/01/2024"), 3);
            Bodega bodega3 = new ("Estancia Mendoza", DateTime.Parse("01/01/2024"), 4);
            Bodega bodega4 = new ("Santa Julia", DateTime.Parse("01/01/2024"), 6);
            Bodega bodega5 = new ("El Quemado", DateTime.Parse("01/01/2024"), 3);
            Bodega bodega6 = new ("Toro", DateTime.Parse("01/01/2024"), 6);
            Bodega bodega7 = new ("Becerra", DateTime.Parse("01/01/2024"), 1);

            listadoBodegasCompleto = new()
            {
                bodega1,
                bodega2,
                bodega3,
                bodega4,
                bodega5,
                bodega6,
                bodega7,
            };

            Vino vino1 = new Vino(bodega1, 2021, DateTime.Parse("01/01/2025"), new(), "Vinito", "Flojo", 1000.00, 
                listadoMaridajeCompleto[0], listadoVariedadesCompleto.GetRange(0,1));
            Vino vino2 = new Vino(bodega1, 2015, DateTime.Parse("01/05/2024"), new(), "El Federal", "Una obra de Arte", 5000.00,
                listadoMaridajeCompleto[1], listadoVariedadesCompleto.GetRange(1, 2));
            Vino vino3 = new Vino(bodega2, 2012, DateTime.Parse("01/05/2025"), new(), "Cavani Wine", "Podria ser Mejor", 1400.00,
                listadoMaridajeCompleto[0], listadoVariedadesCompleto.GetRange(1, 2));
            Vino vino4 = new Vino(bodega3, 2020, DateTime.Parse("01/05/2025"), new(), "Chacabuco", "Estable", 2400.00,
                listadoMaridajeCompleto[1], listadoVariedadesCompleto.GetRange(2, 1));
            Vino vino5 = new Vino(bodega4, 2021, DateTime.Parse("06/05/2024"), new(), "Mora", "Cumple", 2100.00, 
                listadoMaridajeCompleto[2], listadoVariedadesCompleto.GetRange(1, 2));
            Vino vino6 = new Vino(bodega4, 2019, DateTime.Parse("06/05/2025"), new(), "Varietal", "Buen precio calidad", 3100.00,
                listadoMaridajeCompleto[3], listadoVariedadesCompleto.GetRange(4, 1));
            Vino vino7 = new Vino(bodega5, 2021, DateTime.Parse("06/05/2025"), new(), "Malbec Seleccion", "Buen precio calidad", 4100.00,
                listadoMaridajeCompleto[1], listadoVariedadesCompleto.GetRange(4, 1));
            Vino vino8 = new Vino(bodega5, 2016, DateTime.Parse("12/05/2024"), new(), "Torreon", "Amargo", 1200.00,
                listadoMaridajeCompleto[2], listadoVariedadesCompleto.GetRange(4, 1));
            Vino vino9 = new Vino(bodega6, 2016, DateTime.Parse("12/05/2025"), new(), "Amigo", "Muy dulce", 2200.00,
                listadoMaridajeCompleto[1], listadoVariedadesCompleto.GetRange(1, 3));
            Vino vino10 = new Vino(bodega6, 2012, DateTime.Parse("12/05/2025"), new(), "Uvita", "Barato pero feo", 1000.00,
                listadoMaridajeCompleto[0], listadoVariedadesCompleto.GetRange(2, 2));
            Vino vino11 = new Vino(bodega7, 2006, DateTime.Parse("16/05/2024"), new(), "Maria", "Bastante rico", 2000.00,
                listadoMaridajeCompleto[3], listadoVariedadesCompleto.GetRange(1, 1));
            Vino vino12 = new Vino(bodega7, 2006, DateTime.Parse("16/05/2024"), new(), "Establos", "Pasable", 1000.00,
            listadoMaridajeCompleto[2], listadoVariedadesCompleto.GetRange(1, 1));

            bodegasXvino = new()
            {
                {bodega1,new(){vino1,vino2 } },
                {bodega2,new(){vino3 } },
                {bodega3,new(){vino4 } },
                {bodega4,new(){vino5, vino6 } },
                {bodega5,new(){vino7, vino8 } },
                {bodega6,new(){vino9, vino10 } },
                {bodega7,new(){vino11 } },
            };
            //listadoVinosCompleto = new()
            //{
            //   vino1,
            //   vino2,
            //   vino3,
            //   vino4,
            //   vino5,
            //   vino6,
            //   vino7,
            //   vino8,
            //   vino9,
            //   vino10,
            //   vino11,
            //};

           listadoVinosActualizarCompleto = new()
           {
                vino2,
                vino5,
                vino8,
                vino11,
                vino12 //Este vino sirve para resolver el caso alternativo en el cual se crean
           };

            DateTime today = DateTime.Today;

            Siguiendo seg1 = new(today, today.AddYears(2),bodega1);
            Siguiendo seg2 = new(today, today.AddYears(2), bodega2);
            Siguiendo seg3 = new(today, today.AddYears(2), bodega3);
            Siguiendo seg4 = new(today, today.AddYears(2), bodega4);
            Siguiendo seg5 = new(today, today.AddYears(2), bodega5);
            Siguiendo seg6 = new(today, today.AddYears(2), bodega6);
            Siguiendo seg7 = new(today, today.AddYears(2), bodega7);
            Siguiendo seg8 = new(today, today.AddYears(2), bodega2);
            Siguiendo seg9 = new(today, today.AddYears(2), bodega5);
            Siguiendo seg10 = new(today, today.AddYears(2), bodega7);
            Siguiendo seg11 = new(today, today.AddYears(2), bodega2);
            Siguiendo seg12 = new(today, today.AddYears(2), bodega4);

            Usuario user1 = new("FERM0N", "jswrqa", true);
            Usuario user2 = new("BoxaLove", "cavaniWine", true);
            Usuario user3 = new("Yon1", "2aasnf", true);
            Usuario user4 = new("Amanda", "sfacvasc", true);
            Usuario user5 = new("Moya", "jswr231qa", true);
            Usuario user6 = new("ArriGasp", "031563", true);

            Enofilo eno1 = new("Stura", new(), "Fermin", new() { seg1, seg2 },user1);
            Enofilo eno2 = new("Capellan", new(), "Gaston", new() { seg3, seg4,seg5 }, user2);
            Enofilo eno3 = new("Soruco", new(), "Yoni", new() { seg6 }, user3);
            Enofilo eno4 = new("Stura", new(), "Amanda", new() { seg7,seg8, seg9 }, user4);
            Enofilo eno5 = new("Moyano", new(), "Tomas", new() { seg10, seg11 }, user5);
            Enofilo eno6 = new("Arrigo", new(), "Gaspar", new() { seg12 }, user6);

            listadoEnofilosAplicacion = new()
            {
                eno1,
                eno2,
                eno3,
                eno4,
                eno5,
                eno6
            };
        }
        #endregion
    }
}
