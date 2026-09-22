namespace Proyecto2.Models
{
    public class Libro
    {
        public int ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }

        public Libro(int isbn, string titulo, string autor, string categoria)
        {
            ISBN = isbn;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
        }

        public override string ToString()
        {
            return $"[{ISBN}] {Titulo} - {Autor} ({Categoria})";
        }
    }
}