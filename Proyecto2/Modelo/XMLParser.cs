using System;
using System.Xml;

namespace Proyecto2.Modelo
{
    public class XmlParser
    {
        public static void CargarXml(string rutaArchivo, ArbolCategorias sistemaCategorias)
        {
            if (sistemaCategorias == null) return;

            XmlDocument doc = new XmlDocument();
            doc.Load(rutaArchivo);

            // 1. Procesar Lista de Categorías (opcional según enunciado)
            XmlNodeList nodosCategorias = doc.SelectNodes("//listaCategorias/categoria");
            if (nodosCategorias != null)
            {
                foreach (XmlNode catNode in nodosCategorias)
                {
                    string nombreCat = catNode.InnerText?.Trim();
                    string nombrePadre = catNode.Attributes?["padre"]?.Value?.Trim();

                    if (!string.IsNullOrEmpty(nombreCat))
                    {
                        sistemaCategorias.AgregarCategoria(nombreCat, nombrePadre);
                    }
                }
            }

            // 2. Procesar Lista de Libros (opcional según enunciado)
            XmlNodeList nodosLibros = doc.SelectNodes("//listaLibros/libro");
            if (nodosLibros != null)
            {
                foreach (XmlNode libroNode in nodosLibros)
                {
                    XmlNode nodeIsbn = libroNode["ISBN"];
                    XmlNode nodeTitulo = libroNode["titulo"];
                    XmlNode nodeAutor = libroNode["autor"];
                    XmlNode nodeCategoria = libroNode["categoria"];

                    if (nodeIsbn != null && nodeTitulo != null && nodeAutor != null && nodeCategoria != null)
                    {
                        if (int.TryParse(nodeIsbn.InnerText.Trim(), out int isbn))
                        {
                            string titulo = nodeTitulo.InnerText.Trim();
                            string autor = nodeAutor.InnerText.Trim();
                            string nombreCategoria = nodeCategoria.InnerText.Trim();

                            // Buscar la categoría asignada al libro
                            NodoCategoria nodoCat = sistemaCategorias.BuscarCategoria(nombreCategoria);

                            // Si la categoría existe, se inserta el libro en su árbol AVL
                            if (nodoCat != null)
                            {
                                Libro nuevoLibro = new Libro(isbn, titulo, autor, nombreCategoria);
                                nodoCat.Libros.Insertar(nuevoLibro);
                            }
                        }
                    }
                }
            }
        }
    }
}