namespace Proyecto2.Models
{
    public class ArbolCategorias
    {
        public NodoCategoria Raiz { get; set; }

        public ArbolCategorias()
        {
            Raiz = null;
        }

        // Búsqueda en profundidad dentro del árbol N-ario
        public NodoCategoria BuscarCategoria(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return null;
            return BuscarRecursivo(Raiz, nombre);
        }

        private NodoCategoria BuscarRecursivo(NodoCategoria nodo, string nombre)
        {
            if (nodo == null) return null;

            if (nodo.Nombre.Equals(nombre, System.StringComparison.OrdinalIgnoreCase))
                return nodo;

            // Buscar en sus subcategorías (hijos)
            NodoCategoria encontrado = BuscarRecursivo(nodo.PrimerHijo, nombre);
            if (encontrado != null) return encontrado;

            // Buscar en el mismo nivel (hermanos)
            return BuscarRecursivo(nodo.SiguienteHermano, nombre);
        }

        // Inserción ordenada alfabéticamente usando únicamente punteros de hermano
        public bool AgregarCategoria(string nombreNueva, string nombrePadre)
        {
            if (BuscarCategoria(nombreNueva) != null) return false;

            NodoCategoria nueva = new NodoCategoria(nombreNueva);

            if (string.IsNullOrEmpty(nombrePadre) || Raiz == null)
            {
                if (Raiz == null)
                {
                    Raiz = nueva;
                    return true;
                }
                return false;
            }

            NodoCategoria padre = BuscarCategoria(nombrePadre);
            if (padre != null)
            {
                padre.PrimerHijo = InsertarHermanoOrdenado(padre.PrimerHijo, nueva);
                return true;
            }

            return false;
        }

        private NodoCategoria InsertarHermanoOrdenado(NodoCategoria actual, NodoCategoria nueva)
        {
            if (actual == null || string.Compare(nueva.Nombre, actual.Nombre, System.StringComparison.OrdinalIgnoreCase) < 0)
            {
                nueva.SiguienteHermano = actual;
                return nueva;
            }

            actual.SiguienteHermano = InsertarHermanoOrdenado(actual.SiguienteHermano, nueva);
            return actual;
        }
    }
}