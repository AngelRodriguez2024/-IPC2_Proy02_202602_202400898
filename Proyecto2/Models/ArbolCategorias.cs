namespace Proyecto2.Models
{
    public class ArbolCategorias
    {
        public NodoCategoria Raiz { get; set; }

        public ArbolCategorias()
        {
            Raiz = null;
        }

        public NodoCategoria BuscarCategoria(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;
            return BuscarRecursivo(Raiz, nombre.Trim());
        }

        private NodoCategoria BuscarRecursivo(NodoCategoria nodo, string nombre)
        {
            if (nodo == null) return null;

            if (nodo.Nombre.Equals(nombre, System.StringComparison.OrdinalIgnoreCase))
                return nodo;

            // Buscar primero en el hijo (profundidad)
            NodoCategoria encontrado = BuscarRecursivo(nodo.PrimerHijo, nombre);
            if (encontrado != null) return encontrado;

            // Buscar luego en el hermano (amplitud)
            return BuscarRecursivo(nodo.SiguienteHermano, nombre);
        }

        public bool AgregarCategoria(string nombreNueva, string nombrePadre)
        {
            if (string.IsNullOrWhiteSpace(nombreNueva)) return false;

            nombreNueva = nombreNueva.Trim();
            nombrePadre = string.IsNullOrWhiteSpace(nombrePadre) ? null : nombrePadre.Trim();

            // Si ya existe en el árbol, no la volvemos a crear
            if (BuscarCategoria(nombreNueva) != null) return false;

            NodoCategoria nueva = new NodoCategoria(nombreNueva);

            // CASO 1: Es una categoría sin padre (va como hermana en el nivel superior)
            if (nombrePadre == null)
            {
                Raiz = InsertarHermanoOrdenado(Raiz, nueva);
                return true;
            }

            // CASO 2: Tiene padre especificado
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
            if (actual == null) return nueva;

            if (string.Compare(nueva.Nombre, actual.Nombre, System.StringComparison.OrdinalIgnoreCase) < 0)
            {
                nueva.SiguienteHermano = actual;
                return nueva;
            }

            actual.SiguienteHermano = InsertarHermanoOrdenado(actual.SiguienteHermano, nueva);
            return actual;
        }
    }
}