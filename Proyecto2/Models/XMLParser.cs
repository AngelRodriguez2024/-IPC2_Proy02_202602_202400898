using System;
using System.IO;
using System.Xml;

namespace Proyecto2.Models
{
    public static class XmlParser
    {
        public static void CargarXmlDesdeStream(Stream stream, ArbolCategorias arbol)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                using (StreamReader reader = new StreamReader(stream))
                {
                    doc.Load(reader);
                }
                XmlNodeList listaCategorias = doc.GetElementsByTagName("categoria");    
                foreach (XmlNode nodo in listaCategorias)
                {
                    XmlAttribute attrPadre = nodo.Attributes["padre"];
                    if (attrPadre != null && !string.IsNullOrWhiteSpace(attrPadre.Value))
                    {
                        string padre = attrPadre.Value.Trim();
                        if (arbol.BuscarCategoria(padre) == null)
                        {
                            arbol.AgregarCategoria(padre, null);
                        }
                    }
                }

                foreach (XmlNode nodo in listaCategorias)
                {
                    string nombre = nodo.InnerText.Trim();
                    XmlAttribute attrPadre = nodo.Attributes["padre"];
                    string padre = (attrPadre != null && !string.IsNullOrWhiteSpace(attrPadre.Value)) ? attrPadre.Value.Trim() : null;

                    arbol.AgregarCategoria(nombre, padre);
                }

                XmlNodeList listaLibros = doc.GetElementsByTagName("libro");
                foreach (XmlNode nodo in listaLibros)
                {
                    int isbn = int.Parse(nodo.SelectSingleNode("ISBN").InnerText.Trim());
                    string titulo = nodo.SelectSingleNode("titulo").InnerText.Trim();
                    string autor = nodo.SelectSingleNode("autor").InnerText.Trim();
                    string categoria = nodo.SelectSingleNode("categoria").InnerText.Trim();

                    NodoCategoria catNode = arbol.BuscarCategoria(categoria);
                    if (catNode != null)
                    {
                        catNode.Libros.Insertar(new Libro(isbn, titulo, autor, categoria));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en XmlParser: " + ex.Message);
            }
        }
    }
}