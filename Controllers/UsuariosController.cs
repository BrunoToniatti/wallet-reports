using Microsoft.AspNetCore.Mvc;
using JbFinanceAPI.Models;
using JbFinanceAPI.Data;
using System.Linq;

namespace JbFinanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context) 
        {
            _context = context; 
        }

        // GET: api/usuarios
        // Get para que o administrador consiga ter acesso aos usuários
        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            var lista = _context.Usuarios.ToList();
            return Ok(lista);
        }

        [HttpPost]
        // POST: api/usuarios
        public IActionResult CriarUsuarios([FromBody] Usuario novoUsuario)
        {
            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ListarUsuarios), novoUsuario);
        }

        [HttpPut("{id}")]
        // PUT: api/usuarios/id
        // requisição para atualizar o usuário

        public IActionResult AtualizarUsuario(int id, [FromBody] Usuario usuarioAlterado)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado" });

            usuario.Nome = usuarioAlterado.Nome;
            usuario.Senha = usuarioAlterado.Senha;
            usuario.Email = usuarioAlterado.Email;

            _context.SaveChanges();
            return Ok(usuario);
        }

    }
}
