using Microsoft.AspNetCore.Mvc;
using JbFinanceAPI.Models;
using JbFinanceAPI.Data;
using System.Linq;

namespace JbFinanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagamentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/pagamentos
        [HttpGet]
        public IActionResult ObterTodos()
        {
            var lista = _context.Pagamentos.ToList();
            return Ok(lista);
        }

        // POST: /api/pagamentos
        [HttpPost]
        public IActionResult CriarPagamento([FromBody] Pagamento novoPagamento)
        {
            _context.Pagamentos.Add(novoPagamento);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterTodos), novoPagamento);
        }

        // PUT: /api/pagamentos/id
        [HttpPut("{id}")]
        public IActionResult AtualizarPagamento(int id, [FromBody] Pagamento pagamentoAtualizado)
        {
            var pagamento = _context.Pagamentos.FirstOrDefault(p => p.Id == id);

            if (pagamento == null)
                return NotFound(new { mensagem = "Pagamento não encontrado." });

            pagamento.CpfCnpj = pagamentoAtualizado.CpfCnpj;
            pagamento.Categoria = pagamentoAtualizado.Categoria;
            pagamento.Valor = pagamentoAtualizado.Valor;
            pagamento.Data = pagamentoAtualizado.Data;

            _context.SaveChanges();

            return Ok(pagamento);
        }

        // DELETE: /api/pagamentos/id
        [HttpDelete("{id}")]
        public IActionResult DeletarPagamento(int id, [FromBody] Pagamento pagamentoDeleteado)
        {
            // essa linha está servindo para conseguir identificar qual o pagamento pelo ID, e se ele existe ou não.
            var pagamento = _context.Pagamentos.FirstOrDefault(p => p.Id == id);

            if (pagamento == null)
                return NotFound(new { mensagem = "Pagamento não encontrado." });

            _context.Pagamentos.Remove(pagamento);

            _context.SaveChanges();
            return Ok(pagamento);
        }


        // FILTROS


        // Filtragem por CPF ou CNPJ
        // api/pagamentos/cpfcnpj/12345678910
        [HttpGet("cpfcnpj/{cpfcpnj}")]
        public IActionResult BuscarPorCpfCnpj(string cpfcpnj)
        {
            var resultado = _context.Pagamentos.Where(p => p.CpfCnpj == cpfcpnj).ToList();

            if (resultado == null || resultado.Count == 0)
                return NotFound(new { mensagem = "Pagamento não encontrado" });

            return Ok(resultado);
        }
    }
}
