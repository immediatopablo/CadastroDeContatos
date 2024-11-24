namespace CadastroDeContatos.Controllers
{
    public class Contato // Alterado para 'public'
    {
        public int Id { get; set; } // Definindo a chave primária
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Cidade { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Email { get; set; }
    }
}
