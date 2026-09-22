using System.Text;

namespace Proyecto2.Models
{
    public static class GraphvizService
    {
        public static string GenerarDotCategorias(NodoCategoria raiz)
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph ArbolCategorias {");
            dot.AppendLine("    node [shape=box, style=filled, fillcolor=lightyellow];");

            if (raiz == null)
            {
                dot.AppendLine("    empty [label=\"Sin categorías\"];");
            }
            else
            {
                GenerarDotCategoriasRecursivo(raiz, dot);
            }

            dot.AppendLine("}");
            return dot.ToString();
        }

        private static void GenerarDotCategoriasRecursivo(NodoCategoria nodo, StringBuilder dot)
        {
            if (nodo == null) return;

            if (nodo.PrimerHijo != null)
            {
                dot.AppendLine($"    \"{nodo.Nombre}\" -> \"{nodo.PrimerHijo.Nombre}\" [label=\"hijo\", color=blue];");
                GenerarDotCategoriasRecursivo(nodo.PrimerHijo, dot);
            }

            if (nodo.SiguienteHermano != null)
            {
                dot.AppendLine($"    \"{nodo.Nombre}\" -> \"{nodo.SiguienteHermano.Nombre}\" [label=\"hermano\", constraint=false, color=green];");
                GenerarDotCategoriasRecursivo(nodo.SiguienteHermano, dot);
            }
        }

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