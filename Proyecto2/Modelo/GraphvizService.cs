using System.Text;

namespace Proyecto2.Models
{
    public class GraphvizService
    {
        // Genera el código DOT para el Árbol AVL de libros
        public static string GenerarDotAVL(NodoAVL raiz)
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph ArbolAVL {");
            dot.AppendLine("    node [shape=record, style=filled, fillcolor=lightblue];");

            if (raiz == null)
            {
                dot.AppendLine("    empty [label=\"Categoría sin libros\"];");
            }
            else
            {
                GenerarDotAVLRecursivo(raiz, dot);
            }

            dot.AppendLine("}");
            return dot.ToString();
        }

        private static void GenerarDotAVLRecursivo(NodoAVL nodo, StringBuilder dot)
        {
            if (nodo != null)
            {
                // Formato del nodo: ISBN, Título y Altura
                dot.AppendLine($"    node{nodo.Dato.ISBN} [label=\"<f0> |<f1> ISBN: {nodo.Dato.ISBN}\\n{nodo.Dato.Titulo}|<f2>\"];");

                if (nodo.Izquierdo != null)
                {
                    dot.AppendLine($"    node{nodo.Dato.ISBN}:f0 -> node{nodo.Izquierdo.Dato.ISBN}:f1;");
                    GenerarDotAVLRecursivo(nodo.Izquierdo, dot);
                }

                if (nodo.Derecho != null)
                {
                    dot.AppendLine($"    node{nodo.Dato.ISBN}:f2 -> node{nodo.Derecho.Dato.ISBN}:f1;");
                    GenerarDotAVLRecursivo(nodo.Derecho, dot);
                }
            }
        }
    }
}