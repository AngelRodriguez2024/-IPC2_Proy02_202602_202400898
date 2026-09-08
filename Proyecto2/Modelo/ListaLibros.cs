namespace Proyecto2.Models
{
    public class NodoListaLibro
    {
        public Libro Libro { get; set; }
        public NodoListaLibro Siguiente { get; set; }

        public NodoListaLibro(Libro libro)
        {
            Libro = libro;
            Siguiente = null;
        }
    }

    public class ListaLibros
    {
        public NodoListaLibro Cabeza { get; set; }
        private NodoListaLibro Cola { get; set; }

        public void Agregar(Libro libro)
        {
            NodoListaLibro nuevo = new NodoListaLibro(libro);
            if (Cabeza == null)
            {
                Cabeza = nuevo;
                Cola = nuevo;
            }
            else
            {
                Cola.Siguiente = nuevo;
                Cola = nuevo;
            }
        }
    }
}