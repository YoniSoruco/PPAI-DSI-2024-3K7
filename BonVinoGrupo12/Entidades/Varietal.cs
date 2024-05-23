using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonVinoGrupo12.Entidades
{
    public class Varietal
    {
        private string descripcion;
        private double porcentajeComposicion;
        private TipoUva tipoUva;
        public Varietal()
        {

        }

        public Varietal(string descripcion, double porcentajeComposicion, TipoUva tipoUva)
        {
            this.descripcion = descripcion;
            this.porcentajeComposicion = porcentajeComposicion;
            this.tipoUva = tipoUva;
        }
        #region Paso 6 del Caso de Uso
        public TipoUva getTipoUva()
        {
            return tipoUva;
        }
        public string getDescripcion()
        {
            return descripcion;
        }
        public void setTipoUva(TipoUva tipoUva)
        {
            this.tipoUva= tipoUva;  
        }
        #endregion
    }
}
