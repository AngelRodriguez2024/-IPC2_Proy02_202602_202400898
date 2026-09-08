namespace Proyecto2.Models
{
    public class NodoAVL
    {
        public Libro Dato { get; set; }
        public NodoAVL Izquierdo { get; set; }
        public NodoAVL Derecho { get; set; }
        public int Altura { get; set; }

        public NodoAVL(Libro libro)
        {
            Dato = libro;
            Izquierdo = null;
            Derecho = null;
            Altura = 1; // Un nodo hoja nuevo inicia con altura 1
        }
    }
}