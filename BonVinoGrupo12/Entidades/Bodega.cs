using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonVinoGrupo12.Entidades
{
    public class Bodega
    {
        private string coordenadasUbicacion;
        private string descripcion;
        private string historia;
        private DateTime fechaUltimaActualizacion;
        private string nombre;


        //Este atributo representa los meses para actualizar
        private int periodoActualizacion;

        public Bodega()
        {
        }
        public Bodega(string nombre,DateTime fechaUltima,int periodo)
        {
            this.nombre = nombre;
            this.fechaUltimaActualizacion= fechaUltima;
            this.periodoActualizacion = periodo;
        }
        #region Paso 2 del Caso de Uso
        public bool estaEnPeriodoDeActualizacion(DateTime hoy)
        {
            //Calculamos la fecha la ultima fecha de actualizacion mas periodo actualizacion (meses) y lo comparamos con la fecha de hoy
            if(hoy > fechaUltimaActualizacion.AddMonths(periodoActualizacion))
            {
                //Hoy es 1 un dia mas que esta suma
                return true;
            }
            //Es el mismo o anterior
            else return false;
        }
        public string getNombre()
        {
            return nombre;
        }
        #endregion

        #region Paso 5 y 6 del Caso de Uso
       
        public void actualizarDatosVinos(List<Vino> vinosDeBodega, Vino vinoActualizado)
        {
            foreach (Vino vinosBodega in vinosDeBodega)
            {
                if (vinoActualizado == vinosBodega)
                {
                    if (vinosBodega.sosVinoParaActualizar())
                    {
                        //Debemos crear todos los get porque sus atributos son privados
                        vinosBodega.setPrecio(vinoActualizado.getPrecio());
                        vinosBodega.setNotaCata(vinoActualizado.getNota());
                        vinosBodega.setImagenEtiqueta(vinoActualizado.getImagenEtiqueta());

                        //Esto lo dejamos porque sino no se actualizaria
                        DateTime fechaActualizarNueva = DateTime.Now.AddMonths(periodoActualizacion);
                        vinosBodega.setFechaActualizacion(fechaActualizarNueva);
                    }//Si lo encuentra pero no se actualiza se corta el ciclo
                    else return;
                }
            }
        }

        public void setUltimaActualizacion(DateTime fecha)
        {
            this.fechaUltimaActualizacion = fecha;
        }
        #endregion
    }
}
