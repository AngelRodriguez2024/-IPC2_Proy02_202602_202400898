namespace Proyecto2.Models
{
    public class ArbolCategorias
    {
        public NodoCategoria Raiz { get; set; }

        public ArbolCategorias()
        {
            Raiz = null;
        }

        // Busca una categoría por su nombre en todo el árbol de forma recursiva
        public NodoCategoria BuscarCategoria(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return null;
            return BuscarRecursivo(Raiz, nombre);
        }

        private NodoCategoria BuscarRecursivo(NodoCategoria actual, string nombre)
        {
            if (actual == null) return null;

            // Si coincide el nombre
            if (actual.Nombre.Equals(nombre, System.StringComparison.OrdinalIgnoreCase))
            {
                return actual;
            }

            // Buscar en sus subcategorías (hijas)
            ElementoCategoria subActual = actual.Subcategorias.Cabeza;
            while (subActual != null)
            {
                NodoCategoria resultado = BuscarRecursivo(subActual.Categoria, nombre);
                if (resultado != null)
                {
                    return resultado;
                }
                subActual = subActual.Siguiente;
            }

            return null;
        }

        // Inserta una nueva categoría indicando quién es su padre (si padre == null, es la raíz principal)
        public bool AgregarCategoria(string nombreNueva, string nombrePadre)
        {
            // El enunciado especifica que no pueden existir nombres duplicados de categorías
            if (BuscarCategoria(nombreNueva) != null)
            {
                return false; // Ya existe
            }

            NodoCategoria nuevaCat = new NodoCategoria(nombreNueva);

            // Si no tiene padre, se establece como la categoría raíz principal
            if (string.IsNullOrEmpty(nombrePadre) || Raiz == null)
            {
                if (Raiz == null)
                {
                    Raiz = nuevaCat;
                    return true;
                }
                return false; // Ya hay una raíz principal definida
            }

            // Si tiene padre, buscamos al padre en la jerarquía
            NodoCategoria nodoPadre = BuscarCategoria(nombrePadre);
            if (nodoPadre != null)
            {
                nodoPadre.Subcategorias.AgregarOrdenado(nuevaCat);
                return true;
            }

            return false; // El padre especificado no existe
        }
    }
}