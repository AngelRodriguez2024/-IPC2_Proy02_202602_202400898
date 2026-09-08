namespace Proyecto2.Models
{
    public class ElementoCategoria
    {
        public NodoCategoria Categoria { get; set; }
        public ElementoCategoria Siguiente { get; set; }

        public ElementoCategoria(NodoCategoria categoria)
        {
            Categoria = categoria;
            Siguiente = null;
        }
    }

    public class ListaCategorias
    {
        public ElementoCategoria Cabeza { get; set; }

        // Inserción ordenada alfabéticamente
        public void AgregarOrdenado(NodoCategoria nuevaCat)
        {
            ElementoCategoria nuevo = new ElementoCategoria(nuevaCat);

            if (Cabeza == null || string.Compare(nuevaCat.Nombre, Cabeza.Categoria.Nombre, System.StringComparison.OrdinalIgnoreCase) < 0)
            {
                nuevo.Siguiente = Cabeza;
                Cabeza = nuevo;
                return;
            }

            ElementoCategoria actual = Cabeza;
            while (actual.Siguiente != null && 
                   string.Compare(nuevaCat.Nombre, actual.Siguiente.Categoria.Nombre, System.StringComparison.OrdinalIgnoreCase) > 0)
            {
                actual = actual.Siguiente;
            }

            nuevo.Siguiente = actual.Siguiente;
            actual.Siguiente = nuevo;
        }

        public NodoCategoria Buscar(string nombre)
        {
            ElementoCategoria actual = Cabeza;
            while (actual != null)
            {
                if (actual.Categoria.Nombre.Equals(nombre, System.StringComparison.OrdinalIgnoreCase))
                {
                    return actual.Categoria;
                }
                actual = actual.Siguiente;
            }
            return null;
        }
    }
}