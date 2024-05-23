using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BonVinoGrupo12.Entidades
{
    public class Enofilo
    {
        private string apellido;
        private Image imagenPerfil;
        private string nombre;
        private List<Siguiendo> seguido; //Puede estar vacio
        private Usuario usuario;

        public Enofilo(string apellido, Image imagenPerfil, string nombre, List<Siguiendo> seguido, Usuario usuario)
        {
            this.apellido = apellido;
            this.imagenPerfil = imagenPerfil;
            this.nombre = nombre;
            this.seguido = seguido;
            this.usuario = usuario;
        }

        #region Paso 7 Caso de Uso
        public bool sigueABodega(Bodega bod)
        {
            foreach (Siguiendo seg in seguido)
            {
                if(seg.sosDeBodega(bod))return true;
            }
            return false;
        }
        public string getNombreUsuario()
        {
            return usuario.getNombre();
        }
        #endregion
    }
}
