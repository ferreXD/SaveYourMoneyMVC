using SaveYourMoneyMVC.Entities;
using SaveYourMoneyMVC.Models.Files;
using SaveYourMoneyMVC.Validators;
using System.ComponentModel.DataAnnotations;
using static SaveYourMoneyMVC.Common.Enums.Enums;

namespace SaveYourMoneyMVC.Models.Gastos
{
    public class GastoViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public TipoGasto Tipo { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be between {1} - {2}")]
        public double Valor { get; set; }
        public string? Descripcion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [ValidateYears(ErrorMessage = "Date must be between {0} and {1}.")]
        public DateTime Created { get; set; } = DateTime.Now;
        public User? User { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? FileContent { get; set; }
        public string? FileType { get; set; }
    }
}
