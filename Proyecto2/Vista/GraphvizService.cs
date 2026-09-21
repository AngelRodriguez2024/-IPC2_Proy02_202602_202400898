using System.Text;

namespace Proyecto2.Modelo
{
    public static class GraphvizService
    {
        // Genera el código DOT para la jerarquía de categorías (N-ario mediante Hijo/Hermano)
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

            // Enlace con el primer hijo (relación vertical)
            if (nodo.PrimerHijo != null)
            {
                dot.AppendLine($"    \"{nodo.Nombre}\" -> \"{nodo.PrimerHijo.Nombre}\" [label=\"hijo\", color=blue];");
                GenerarDotCategoriasRecursivo(nodo.PrimerHijo, dot);
            }

            // Enlace con el siguiente hermano (relación horizontal)
            if (nodo.SiguienteHermano != null)
            {
                dot.AppendLine($"    \"{nodo.Nombre}\" -> \"{nodo.SiguienteHermano.Nombre}\" [label=\"hermano\", constraint=false, color=green];");
                GenerarDotCategoriasRecursivo(nodo.SiguienteHermano, dot);
            }
        }
    }
}