using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonVinoGrupo12.Entidades
{
    public class TipoUva
    {
        private string descripcion;
        private string nombre;
        public TipoUva()
        {

        }

        public TipoUva(string descripcion, string nombre)
        {
            this.descripcion = descripcion;
            this.nombre = nombre;
        }

        #region Paso 6 del Caso de Uso
        public bool esTipoUva(TipoUva tipo)
        {
            if (tipo == this) return true;
            else return false;
        }
        #endregion
    }
}
