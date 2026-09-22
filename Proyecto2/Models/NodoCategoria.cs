namespace Proyecto2.Models
{
    public class NodoCategoria
    {
        public string Nombre { get; set; }
        public ArbolAVL Libros { get; set; } 

        public NodoCategoria PrimerHijo { get; set; }       
        public NodoCategoria SiguienteHermano { get; set; } 

        public NodoCategoria(string nombre)
        {
            Nombre = nombre;
            Libros = new ArbolAVL();
            PrimerHijo = null;
            SiguienteHermano = null;
        }
    }
}