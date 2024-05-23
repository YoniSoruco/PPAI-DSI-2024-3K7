using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonVinoGrupo12.Entidades
{
    public class Maridaje
    {
        public string nombre;
        public string descripcion;

        public Maridaje()
        {

        }

        public Maridaje(string nombre, string descripcion)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
        }

        #region Paso 6 del Caso de Uso
        public bool esMaridaje(Maridaje mar)
        {
            if(mar == this) return true;
            else return false;
        }
        #endregion
    }
}
