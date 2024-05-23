
namespace BonVinoGrupo12.Entidades
{
    public class Usuario
    {
        private string nombre;
        private string contrasenia;
        private bool premium;

        public Usuario() { }

        public Usuario(string nombre, string contrasenia, bool premium)
        {
            this.nombre = nombre;
            this.contrasenia = contrasenia;
            this.premium = premium;
        }

        #region Paso 7 Caso de Uso

        public string getNombre()
        {
            return nombre;
        }

        #endregion

    }
}
