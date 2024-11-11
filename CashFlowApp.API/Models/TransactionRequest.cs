using System.ComponentModel.DataAnnotations;

namespace CashFlowApp.API.Models
{
    public class TransactionRequest
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression("Credito|Debito", ErrorMessage = "Tipo deve ser 'Credito' ou 'Debito'.")]
        public string Type { get; set; }

        public string Description { get; set; }
    }
}
