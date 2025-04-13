namespace JbFinanceAPI.Models
{
    // Criacação como se fosse um banco de dados, informando qual dados serão passados e se poderá inserir ou pegar os dados
    public class Pagamento
    {
        public int Id { get; set; }
        public string CpfCnpj { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
    }
}
