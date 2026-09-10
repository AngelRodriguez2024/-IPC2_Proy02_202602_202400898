using System;
using System.Xml;

namespace Proyecto2.Models
{
    public class XmlParser
    {
        public static void CargarXml(string rutaArchivo, ArbolCategorias sistemaCategorias)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(rutaArchivo);

            // 1. Procesar Lista de Categorías
            XmlNodeList nodosCategorias = doc.SelectNodes("//listaCategorias/categoria");
            if (nodosCategorias != null)
            {
                foreach (XmlNode catNode in nodosCategorias)
                {
                    string nombreCat = catNode.InnerText.Trim();
                    string nombrePadre = catNode.Attributes["padre"]?.Value?.Trim();

                    // Intentar insertar la categoría en la estructura jerárquica
                    sistemaCategorias.AgregarCategoria(nombreCat, nombrePadre);
                }
            }

            // 2. Procesar Lista de Libros
            XmlNodeList nodosLibros = doc.SelectNodes("//listaLibros/libro");
            if (nodosLibros != null)
            {
                foreach (XmlNode libroNode in nodosLibros)
                {
                    int isbn = int.Parse(libroNode["ISBN"].InnerText.Trim());
                    string titulo = libroNode["titulo"].InnerText.Trim();
                    string autor = libroNode["autor"].InnerText.Trim();
                    string nombreCategoria = libroNode["categoria"].InnerText.Trim();

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