using BlazorBiblioteca.Context;
using BlazorBiblioteca.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorBiblioteca.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {

        // Peticion Http Get que conecta con el servidor
        [HttpGet("ConexionServidor")]
        public async Task<ActionResult<string>> Conexion()
        {
            return "Conectado con el servidor";
        }

        // Inicializar context
        private readonly LibroDBContext _dbContext;

        public LibroController(LibroDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Peticion Http Get que conecta la tabla Libros
        [HttpGet("ConexionLibros")]
        public async Task<ActionResult<string>> GetConexionUsuarios()
        {
            try
            {
                var respuesta = await _dbContext.Libros.ToListAsync();
                return "Conectado a la base de datos y a la tabla Libros";
            }
            catch (Exception ex)
            {
                return "Error de conexion con Libros";
            }
        }

        // AGREGAR Libro
        [HttpPost("LibroAgregar")]
        public async Task<ActionResult<string>> HandleCreateLibro([FromBody] Libro libro)
        {
            await _dbContext.Libros.AddAsync(libro);
            var res = await _dbContext.SaveChangesAsync();

            if (res == 1) return Created();
            else return BadRequest();
        }

        // LISTAR Libros
        [HttpGet("LibrosListar")]
        public async Task<ActionResult<List<Libro>>> ListarLibros()
        {
            var res = await _dbContext.Libros.ToListAsync();
            return res;
        }

        // ELIMINAR Libros
        [HttpDelete("libro/{id}")]
        public async Task<ActionResult<string>> HandleDeleteLibro([FromRoute] int id)
        {
            var find = await _dbContext.Libros.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (find == 1) return Ok();
            else return BadRequest();
        }

        // ACTUALIZAR Libro
        [HttpPut("libro/{id}")]
        public async Task<ActionResult<string>> HandleUpdateLibro([FromRoute] int id, [FromBody] Libro libroActualizado)
        {
            var libro = await _dbContext.Libros.FindAsync(id);

            libro.NombreLibro = libroActualizado.NombreLibro;
            libro.Autor = libroActualizado.Autor;
            libro.NumPaginas = libroActualizado.NumPaginas;
            libro.FechaPublicacion = libroActualizado.FechaPublicacion;

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
