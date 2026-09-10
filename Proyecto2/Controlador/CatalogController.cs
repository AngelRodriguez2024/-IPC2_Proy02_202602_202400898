using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Proyecto2.Models;
using System.IO;

namespace Proyecto2.Controllers
{
    public class CatalogController : Controller
    {
        // Instancia única en memoria para mantener los datos de la aplicación
        private static ArbolCategorias sistemaCategorias = new ArbolCategorias();

        // Vista principal del catálogo
        public IActionResult Index()
        {
            return View(sistemaCategorias);
        }

        // POST: Cargar archivo XML
        [HttpPost]
        public IActionResult CargarXml(IFormFile archivoXml)
        {
            if (archivoXml != null && archivoXml.Length > 0)
            {
                // Guardar temporalmente el archivo para procesarlo
                string rutaTemporal = Path.GetTempFileName();
                using (var stream = new FileStream(rutaTemporal, FileMode.Create))
                {
                    archivoXml.CopyTo(stream);
                }

                // Procesar el XML con nuestro Parser
                XmlParser.CargarXml(rutaTemporal, sistemaCategorias);

                // Eliminar el archivo temporal
                if (System.IO.File.Exists(rutaTemporal))
                {
                    System.IO.File.Delete(rutaTemporal);
                }
            }

            return RedirectToAction("Index");
        }

        // POST: Agregar una categoría manualmente
        [HttpPost]
        public IActionResult AgregarCategoria(string nombre, string padre)
        {
            if (!string.IsNullOrEmpty(nombre))
            {
                sistemaCategorias.AgregarCategoria(nombre, padre);
            }
            return RedirectToAction("Index");
        }

        // POST: Registrar un nuevo libro manualmente
        [HttpPost]
        public IActionResult RegistrarLibro(int isbn, string titulo, string autor, string categoria)
        {
            NodoCategoria nodoCat = sistemaCategorias.BuscarCategoria(categoria);
            if (nodoCat != null)
            {
                Libro nuevoLibro = new Libro(isbn, titulo, autor, categoria);
                nodoCat.Libros.Insertar(nuevoLibro);
            }
            return RedirectToAction("Index");
        }

        // POST: Eliminar un libro por su ISBN
        [HttpPost]
        public IActionResult EliminarLibro(int isbn, string categoria)
        {
            NodoCategoria nodoCat = sistemaCategorias.BuscarCategoria(categoria);
            if (nodoCat != null)
            {
                nodoCat.Libros.Eliminar(isbn);
            }
            return RedirectToAction("Index");
        }

        // GET: Buscar libro por ISBN en una categoría
        public IActionResult BuscarLibro(int isbn, string categoria)
        {
            NodoCategoria nodoCat = sistemaCategorias.BuscarCategoria(categoria);
            Libro encontrado = null;

            if (nodoCat != null)
            {
                encontrado = nodoCat.Libros.Buscar(isbn);
            }

            ViewBag.LibroEncontrado = encontrado;
            return View("Index", sistemaCategorias);
        }
    }
}