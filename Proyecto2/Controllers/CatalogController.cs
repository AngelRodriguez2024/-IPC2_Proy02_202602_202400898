using Microsoft.AspNetCore.Mvc;
using Proyecto2.Models;

namespace Proyecto2.Controllers
{
    public class CatalogController : Controller
    {
        private static ArbolCategorias sistemaCategorias = new ArbolCategorias();

        public IActionResult Index()
        {
            return View(sistemaCategorias);
        }

        // a. Inicialización - Limpia todo el sistema
        [HttpPost]
        public IActionResult Inicializar()
        {
            sistemaCategorias = new ArbolCategorias();
            ViewBag.Mensaje = "Sistema inicializado correctamente. Memoria limpia.";
            return View("Index", sistemaCategorias);
        }

        // b. Cargar XML
        [HttpPost]
        public IActionResult CargarXml(IFormFile archivoXml)
        {
            if (archivoXml != null && archivoXml.Length > 0)
            {
                sistemaCategorias = new ArbolCategorias();
                using (var stream = archivoXml.OpenReadStream())
                {
                    XmlParser.CargarXmlDesdeStream(stream, sistemaCategorias);
                }
                
                string dotCode = GraphvizService.GenerarDotCategorias(sistemaCategorias.Raiz);
                ViewBag.DotCategorias = dotCode;
                
                ViewBag.UrlCategorias = $"https://quickchart.io/graphviz?graph={System.Net.WebUtility.UrlEncode(dotCode)}";
                
                ViewBag.Mensaje = "Archivo XML cargado exitosamente.";
            }
            return View("Index", sistemaCategorias);
        }

        // c.a Estructura de categorías (N-Ario)
        [HttpPost]
        [HttpGet]
        public IActionResult ReporteCategorias()
        {
            string dotCode = GraphvizService.GenerarDotCategorias(sistemaCategorias.Raiz);
            ViewBag.DotCategorias = dotCode;
            
            ViewBag.UrlCategorias = $"https://quickchart.io/graphviz?graph={System.Net.WebUtility.UrlEncode(dotCode)}";
            
            return View("Index", sistemaCategorias);
        }

        // c.b Mostrar AVL
        public IActionResult ReporteAVL(string categoria)
        {
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                NodoCategoria cat = sistemaCategorias.BuscarCategoria(categoria.Trim());
                if (cat != null)
                {
                    string dotCode = GraphvizService.GenerarDotAVL(cat.Libros.Raiz);
                    ViewBag.DotAVL = dotCode;
                    ViewBag.CategoriaReporte = categoria;
                    // NUEVA LÍNEA: Genera el enlace de la imagen AVL
                    ViewBag.UrlAVL = $"https://quickchart.io/graphviz?graph={System.Net.WebUtility.UrlEncode(dotCode)}";
                }
                else
                {
                    ViewBag.MensajeError = "Categoría no encontrada.";
                }
            }
            return View("Index", sistemaCategorias);
        }

        // c.c Agregar categoría
        [HttpPost]
        public IActionResult AgregarCategoria(string nombre, string padre)
        {
            bool exito = sistemaCategorias.AgregarCategoria(nombre, padre);
            ViewBag.Mensaje = exito ? "Categoría agregada correctamente." : "Error: La categoría ya existe o datos inválidos.";
            return View("Index", sistemaCategorias);
        }

        // d.a Registrar libro
        [HttpPost]
        public IActionResult RegistrarLibro(int isbn, string titulo, string autor, string categoria)
        {
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                NodoCategoria cat = sistemaCategorias.BuscarCategoria(categoria.Trim());
                if (cat == null)
                {
                    sistemaCategorias.AgregarCategoria(categoria.Trim(), null);
                    cat = sistemaCategorias.BuscarCategoria(categoria.Trim());
                }
                cat.Libros.Insertar(new Libro(isbn, titulo, autor, categoria.Trim()));
                ViewBag.Mensaje = "Libro registrado exitosamente.";
            }
            return View("Index", sistemaCategorias);
        }

        // d.b Eliminar libro
        [HttpPost]
        public IActionResult EliminarLibro(int isbn, string categoria)
        {
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                NodoCategoria cat = sistemaCategorias.BuscarCategoria(categoria.Trim());
                if (cat != null)
                {
                    cat.Libros.Eliminar(isbn);
                    ViewBag.Mensaje = $"Libro con ISBN {isbn} eliminado.";
                }
            }
            return View("Index", sistemaCategorias);
        }

        // d.c Mostrar libro con el MENOR ISBN
        [HttpPost]
        public IActionResult ObtenerMenorIsbn(string categoria)
        {
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                NodoCategoria cat = sistemaCategorias.BuscarCategoria(categoria.Trim());
                if (cat != null)
                {
                    Libro menor = cat.Libros.ObtenerMenor();
                    ViewBag.LibroResultado = menor;
                    ViewBag.MensajeBusqueda = menor != null ? "Libro con el MENOR ISBN:" : "No hay libros en esta categoría.";
                }
            }
            return View("Index", sistemaCategorias);
        }

        // d.d Mostrar libro con el MAYOR ISBN
        [HttpPost]
        public IActionResult ObtenerMayorIsbn(string categoria)
        {
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                NodoCategoria cat = sistemaCategorias.BuscarCategoria(categoria.Trim());
                if (cat != null)
                {
                    Libro mayor = cat.Libros.ObtenerMayor();
                    ViewBag.LibroResultado = mayor;
                    ViewBag.MensajeBusqueda = mayor != null ? "Libro con el MAYOR ISBN:" : "No hay libros en esta categoría.";
                }
            }
            return View("Index", sistemaCategorias);
        }

        // d.e Buscar por ISBN
        [HttpPost]
        public IActionResult BuscarLibro(int isbn, string categoria)
        {
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                NodoCategoria cat = sistemaCategorias.BuscarCategoria(categoria.Trim());
                if (cat != null)
                {
                    Libro encontrado = cat.Libros.Buscar(isbn);
                    ViewBag.LibroResultado = encontrado;
                    ViewBag.MensajeBusqueda = encontrado != null ? "Libro Encontrado:" : "Libro no encontrado.";
                }
            }
            return View("Index", sistemaCategorias);
        }

        // e. Ayuda
        public IActionResult Ayuda()
        {
            return View();
        }
    }
}
