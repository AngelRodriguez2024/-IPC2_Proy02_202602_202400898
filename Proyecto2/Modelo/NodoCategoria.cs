namespace Proyecto2.Models
{
    public class NodoCategoria
    {
        public string Nombre { get; set; }
        public ArbolAVL Libros { get; set; } // Su propio árbol AVL de libros
        public ListaCategorias Subcategorias { get; set; } // Lista de hijas

        public NodoCategoria(string nombre)
        {
            Nombre = nombre;
            Libros = new ArbolAVL();
            Subcategorias = new ListaCategorias();
        }
    }
}