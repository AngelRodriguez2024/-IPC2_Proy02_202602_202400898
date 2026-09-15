namespace Proyecto2.Modelo
{
    public class ArbolAVL
    {
        public NodoAVL Raiz = null;
        
        // Devuelve la altura de un nodo (0 si es null para evitar NullReferenceException)
        private int ObtenerAltura(NodoAVL nodo)
        {
            if (nodo == null)
            {
                return 0; 
            }
            return nodo.Altura;
        }

        // Calcula la diferencia entre el subárbol izquierdo y el derecho
        private int ObtenerFactorEquilibrio(NodoAVL nodo)
        {
            if (nodo == null)
            {
                return 0;
            }
            return ObtenerAltura(nodo.Izquierdo) - ObtenerAltura(nodo.Derecho);
        }

        // Función auxiliar para determinar la mayor de dos alturas
        private int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        private NodoAVL RotacionDerecha(NodoAVL y)
        {
            // 1. Guardamos las referencias necesarias
            NodoAVL x = y.Izquierdo; // y es la raiz, x el nodo central
            NodoAVL T2 = x.Derecho;

            // 2. Realizamos la rotación (reacomodo de punteros)
            x.Derecho = y;
            y.Izquierdo = T2;

            // 3. Recalculamos la altura de los nodos afectados (primero 'y', luego 'x')
            y.Altura = Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;
            x.Altura = Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;

            // 4. 'x' es la nueva raíz del subárbol
            return x;
        }

        private NodoAVL RotacionIzquierda(NodoAVL x)
        {
            // 1. Guardamos referencias
            NodoAVL y = x.Derecho; // x es la raiz, y el nodo central
            NodoAVL T2 = y.Izquierdo;

            // 2. Realizamos la rotación
            y.Izquierdo = x;
            x.Derecho = T2;

            // 3. Recalculamos las alturas (primero 'x', luego 'y')
            x.Altura = Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;
            y.Altura = Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;

            // 4. 'y' es la nueva raíz del subárbol
            return y;
        }

        // Método público accesible para el controlador/sistema
        public void Insertar(Libro nuevoLibro)
        {
            Raiz = InsertarRecursivo(Raiz, nuevoLibro);
        }

        // Método recursivo interno que inserta y rebalancea el árbol
        private NodoAVL InsertarRecursivo(NodoAVL nodo, Libro nuevoLibro)
        {
            // 1. Inserción normal de Árbol Binario de Búsqueda
            if (nodo == null)
            {
                return new NodoAVL(nuevoLibro);
            }

            if (nuevoLibro.ISBN < nodo.Dato.ISBN)
            {
                nodo.Izquierdo = InsertarRecursivo(nodo.Izquierdo, nuevoLibro);
            }
            else if (nuevoLibro.ISBN > nodo.Dato.ISBN)
            {
                nodo.Derecho = InsertarRecursivo(nodo.Derecho, nuevoLibro);
            }
            else
            {
                // El ISBN ya existe en el catálogo, no se admiten duplicados
                return nodo;
            }

            // 2. Actualizar la altura de este nodo padre
            nodo.Altura = 1 + Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));

            // 3. Obtener el factor de equilibrio para verificar desbalanceo
            int fe = ObtenerFactorEquilibrio(nodo);

            // 4. Casos de Rebalanceo

            // Caso Izquierda - Izquierda
            if (fe > 1 && nuevoLibro.ISBN < nodo.Izquierdo.Dato.ISBN)
            {
                return RotacionDerecha(nodo);
            }

            // Caso Derecha - Derecha
            if (fe < -1 && nuevoLibro.ISBN > nodo.Derecho.Dato.ISBN)
            {
                return RotacionIzquierda(nodo);
            }

            // Caso Izquierda - Derecha
            if (fe > 1 && nuevoLibro.ISBN > nodo.Izquierdo.Dato.ISBN)
            {
                nodo.Izquierdo = RotacionIzquierda(nodo.Izquierdo);
                return RotacionDerecha(nodo);
            }

            // Caso Derecha - Izquierda
            if (fe < -1 && nuevoLibro.ISBN < nodo.Derecho.Dato.ISBN)
            {
                nodo.Derecho = RotacionDerecha(nodo.Derecho);
                return RotacionIzquierda(nodo);
            }

            // Si el nodo sigue balanceado, se retorna intacto
            return nodo;
        }

        public Libro Buscar(int isbn)
        {
            NodoAVL actual = Raiz;
            while (actual != null)
            {
                if (isbn == actual.Dato.ISBN)
                    return actual.Dato; // Encontrado
                
                if (isbn < actual.Dato.ISBN)
                    actual = actual.Izquierdo;
                else
                    actual = actual.Derecho;
            }
            return null; // No existe en el árbol
        }

        public Libro ObtenerMenor()
        {
            if (Raiz == null) return null;
            
            NodoAVL actual = Raiz;
            while (actual.Izquierdo != null)
            {
                actual = actual.Izquierdo;
            }
            return actual.Dato;
        }

        public Libro ObtenerMayor()
        {
            if (Raiz == null) return null;

            NodoAVL actual = Raiz;
            while (actual.Derecho != null)
            {
                actual = actual.Derecho;
            }
            return actual.Dato;
        }

        public void Eliminar(int isbn)
        {
            Raiz = EliminarRecursivo(Raiz, isbn);
        }

        private NodoAVL EliminarRecursivo(NodoAVL nodo, int isbn)
        {
            if (nodo == null) return null;

            // 1. Busqueda del nodo a eliminar
            if (isbn < nodo.Dato.ISBN)
            {
                nodo.Izquierdo = EliminarRecursivo(nodo.Izquierdo, isbn);
            }
            else if (isbn > nodo.Dato.ISBN)
            {
                nodo.Derecho = EliminarRecursivo(nodo.Derecho, isbn);
            }
            else
            {
                // Nodo encontrado: Manejo de casos de eliminación
                if ((nodo.Izquierdo == null) || (nodo.Derecho == null))
                {
                    NodoAVL temporal = nodo.Izquierdo ?? nodo.Derecho;

                    if (temporal == null) // Caso 0 hijos
                    {
                        nodo = null;
                    }
                    else // Caso 1 hijo
                    {
                        nodo = temporal;
                    }
                }
                else // Caso 2 hijos: Obtener el sucesor en in-orden (menor del subárbol derecho)
                {
                    NodoAVL sucesor = ObtenerNodoMinimo(nodo.Derecho);
                    nodo.Dato = sucesor.Dato;
                    nodo.Derecho = EliminarRecursivo(nodo.Derecho, sucesor.Dato.ISBN);
                }
            }

            if (nodo == null) return null;

            // 2. Actualizar altura
            nodo.Altura = 1 + Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));

            // 3. Rebalancear
            int fe = ObtenerFactorEquilibrio(nodo);

            if (fe > 1 && ObtenerFactorEquilibrio(nodo.Izquierdo) >= 0)
                return RotacionDerecha(nodo);

            if (fe > 1 && ObtenerFactorEquilibrio(nodo.Izquierdo) < 0)
            {
                nodo.Izquierdo = RotacionIzquierda(nodo.Izquierdo);
                return RotacionDerecha(nodo);
            }

            if (fe < -1 && ObtenerFactorEquilibrio(nodo.Derecho) <= 0)
                return RotacionIzquierda(nodo);

            if (fe < -1 && ObtenerFactorEquilibrio(nodo.Derecho) > 0)
            {
                nodo.Derecho = RotacionDerecha(nodo.Derecho);
                return RotacionIzquierda(nodo);
            }

            return nodo;
        }

        private NodoAVL ObtenerNodoMinimo(NodoAVL nodo)
        {
            NodoAVL actual = nodo;
            while (actual.Izquierdo != null)
            {
                actual = actual.Izquierdo;
            }
            return actual;
        }

        public string ObtenerTextoEnOrden()
        {
            StringBuilder sb = new StringBuilder();
            ObtenerEnOrdenRecursivo(Raiz, sb);
            return sb.ToString();
        }

        private void ObtenerEnOrdenRecursivo(NodoAVL nodo, StringBuilder sb)
        {
            if (nodo != null)
            {
                ObtenerEnOrdenRecursivo(nodo.Izquierdo, sb);
                sb.AppendLine($"[ISBN: {nodo.Dato.ISBN}] {nodo.Dato.Titulo} - {nodo.Dato.Autor}");
                ObtenerEnOrdenRecursivo(nodo.Derecho, sb);
            }
        }
    }
}
