using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonVinoGrupo12.Entidades
{
    public class Siguiendo
    {
        private DateTime fechaInicio;
        private DateTime fechaFin;
        private Bodega bodega;

        public Siguiendo(DateTime fechaInicio, DateTime fechaFin, Bodega bodega)
        {
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
            this.bodega = bodega;
        }

        #region Paso 7 Caso de Uso
        public bool sosDeBodega(Bodega bod)
        {
            if(bod == bodega)return true;
            else return false;
        }
        #endregion
    }
}
