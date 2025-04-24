using Microsoft.AspNetCore.Mvc;
using JbFinanceAPI.Models;
using JbFinanceAPI.Data;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


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

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u =>
                u.Email == login.Login && u.Senha == login.Senha);

            if (usuario == null)
                return Unauthorized(new { mensagem = "Credenciais inválidas." });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("chave_extramamente_forte_que_nem_a_nasa_sabeasokdmfasokdmfaoi931i90i23k9i4"); // mesma do Program.cs

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { token = tokenString });
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

        [HttpDelete("{id}")]
        // DELETE: api/usuarios/id
        // requisição para que o usuário consiga excluir seu usuário
        public IActionResult DeletarUsuario(int id, [FromBody] Usuario usuarioDeletado)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado" });

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return Ok(usuario);
        }

    }
}
