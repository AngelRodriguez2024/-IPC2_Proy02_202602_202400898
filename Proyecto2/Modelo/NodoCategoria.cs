namespace Proyecto2.Models
{
    public class NodoCategoria
    {
        public string Nombre { get; set; }
        public ArbolAVL Libros { get; set; } // TDA no lineal

        // Representación de árbol N-ario mediante punteros binarios
        public NodoCategoria PrimerHijo { get; set; }       // Subcategoría directa
        public NodoCategoria SiguienteHermano { get; set; } // Subcategoría hermana

        public NodoCategoria(string nombre)
        {
            Nombre = nombre;
            Libros = new ArbolAVL();
            PrimerHijo = null;
            SiguienteHermano = null;
        }
    }
}