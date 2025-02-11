using System.ComponentModel.DataAnnotations;

namespace DevSteamAPI.Models
{
    public class ItemCarrinho
    {
        public Guid ItemCarrinhoId { get; set; }
        public Guid CarrinhoId { get; set; }
        public Guid JogoId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }
        [Required(ErrorMessage = "O Campo é obrigatória")]
        [Range(0.01, 9999.9, ErrorMessage = "A Valor deve ser maior que zero e menor que R")]
        public decimal Valor { get; set; }
        public decimal Total { get; set; }
    }
}
