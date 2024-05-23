using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BonVinoGrupo12.Entidades
{
    public class Vino
    {
        //Es aniada es por no utilizar ñ en un atributo
        private Bodega bodega;
        private int aniada;
        private DateTime fechaActualizacion;
        //Este tambien puede ser byte[]
        private Image imagenEtiqueta;
        private string nombre;
        private string notaDeCataBodega;
        private double precioARS;
        private Maridaje maridaje;
        private List<Varietal> variedades;
        public Vino()
        {

        }

        public Vino(Bodega bodega, int aniada, DateTime fechaActualizacion, Image imagenEtiqueta, string nombre, string notaDeCataBodega, double precioARS,Maridaje maridaje,List<Varietal> variedades)
        {
            this.bodega= bodega;
            this.aniada = aniada;
            this.fechaActualizacion = fechaActualizacion;
            this.imagenEtiqueta = imagenEtiqueta;
            this.nombre = nombre;
            this.notaDeCataBodega = notaDeCataBodega;
            this.precioARS = precioARS;
            this.maridaje = maridaje;
            this.variedades = variedades;
        }
        #region Paso 6 del Caso de Uso
        public bool sosVinoParaActualizar()
        {
            if (DateTime.Now > fechaActualizacion) return true;
            else return false;
        }
        public string getNombre()
        {
            return nombre;
        }
        public Bodega getBodega()
        {
            return bodega;
        }

        public double getPrecio()
        {
            return precioARS;
        }
        public void setPrecio(double precio)
        {
            precioARS = precio;
        }

        public void setNotaCata(string nota)
        {
            notaDeCataBodega = nota;
        }
        public string getNota()
        {
            return notaDeCataBodega;
        }

        public void setImagenEtiqueta(Image imag)
        {
            imagenEtiqueta = imag;
        }
        public Image getImagenEtiqueta()
        {
            return imagenEtiqueta;
        }

        public void setFechaActualizacion(DateTime fecha)
        {
            fechaActualizacion = fecha;
        }
        public DateTime getFechaActualizacion()
        {
            return fechaActualizacion;
        }
        /*------------------------- PARA CREAR NUEVO VINO -----------------------*/
        public Maridaje getMaridaje()
        {
            return maridaje;
        }
        public List<Varietal> getVariedades()
        {
            return variedades;
        }
        public int getAnianada()
        {
            return aniada;
        }
        #endregion
    }
}
