using System.Text;

namespace Proyecto2.Models
{
    public class ArbolAVL
    {
        public NodoAVL Raiz = null;

        private int ObtenerAltura(NodoAVL nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        private int ObtenerFactorEquilibrio(NodoAVL nodo)
        {
            return nodo == null ? 0 : ObtenerAltura(nodo.Izquierdo) - ObtenerAltura(nodo.Derecho);
        }

        private int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        private NodoAVL RotacionDerecha(NodoAVL y)
        {
            if (y == null || y.Izquierdo == null) return y;

            NodoAVL x = y.Izquierdo;
            NodoAVL T2 = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = T2;

            y.Altura = Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;
            x.Altura = Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;

            return x;
        }

        private NodoAVL RotacionIzquierda(NodoAVL x)
        {
            if (x == null || x.Derecho == null) return x;

            NodoAVL y = x.Derecho;
            NodoAVL T2 = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = T2;

            x.Altura = Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;
            y.Altura = Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;

            return y;
        }

        public void Insertar(Libro nuevoLibro)
        {
            if (nuevoLibro == null) return;
            Raiz = InsertarRecursivo(Raiz, nuevoLibro);
        }

        private NodoAVL InsertarRecursivo(NodoAVL nodo, Libro nuevoLibro)
        {
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
                return nodo;
            }

            nodo.Altura = 1 + Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));

            int fe = ObtenerFactorEquilibrio(nodo);

            if (fe > 1 && nodo.Izquierdo != null && nuevoLibro.ISBN < nodo.Izquierdo.Dato.ISBN)
            {
                return RotacionDerecha(nodo);
            }

            if (fe < -1 && nodo.Derecho != null && nuevoLibro.ISBN > nodo.Derecho.Dato.ISBN)
            {
                return RotacionIzquierda(nodo);
            }

            if (fe > 1 && nodo.Izquierdo != null && nuevoLibro.ISBN > nodo.Izquierdo.Dato.ISBN)
            {
                nodo.Izquierdo = RotacionIzquierda(nodo.Izquierdo);
                return RotacionDerecha(nodo);
            }

            if (fe < -1 && nodo.Derecho != null && nuevoLibro.ISBN < nodo.Derecho.Dato.ISBN)
            {
                nodo.Derecho = RotacionDerecha(nodo.Derecho);
                return RotacionIzquierda(nodo);
            }

            return nodo;
        }

        public Libro Buscar(int isbn)
        {
            NodoAVL actual = Raiz;
            while (actual != null)
            {
                if (isbn == actual.Dato.ISBN)
                    return actual.Dato;

                actual = (isbn < actual.Dato.ISBN) ? actual.Izquierdo : actual.Derecho;
            }
            return null;
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
                if ((nodo.Izquierdo == null) || (nodo.Derecho == null))
                {
                    NodoAVL temporal = nodo.Izquierdo ?? nodo.Derecho;
                    nodo = temporal;
                }
                else
                {
                    NodoAVL sucesor = ObtenerNodoMinimo(nodo.Derecho);
                    nodo.Dato = sucesor.Dato;
                    nodo.Derecho = EliminarRecursivo(nodo.Derecho, sucesor.Dato.ISBN);
                }
            }

            if (nodo == null) return null;

            nodo.Altura = 1 + Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));

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
            while (actual != null && actual.Izquierdo != null)
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