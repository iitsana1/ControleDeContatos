using System.ComponentModel.DataAnnotations;

namespace ControleDeContatos.Models
{
    public class ContatoModels
    {
        public int id { get; set; }
        [Required(ErrorMessage ="Digite o nome do contato!")]

        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite o email do contato!")]
        [EmailAddress(ErrorMessage ="O email digitado não é válido!")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Digite o celular do contato!")]
        [Phone(ErrorMessage ="O celular digitado não é válido")]

        public string Celular { get; set;}
    }
}
