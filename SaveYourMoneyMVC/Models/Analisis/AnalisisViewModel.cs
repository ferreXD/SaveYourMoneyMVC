using SaveYourMoneyMVC.DTOs.Analisis;
using SaveYourMoneyMVC.Validators;
using System.ComponentModel.DataAnnotations;
using static SaveYourMoneyMVC.Common.Enums.Enums;

namespace SaveYourMoneyMVC.Models.Analisis
{
    public class AnalisisViewModel
    {
        public Dictionary<string, List<AnalisisResponseDTO>>? Dictionary { get; set; }
        public string JsonData { get; set; }

        public TipoGasto? TipoGasto { get; set; }

        [DataType(DataType.Date)]
        [ValidateYears(ErrorMessage = "Initial date must be between {0} and {1}.")]
        public DateTime? IntervaloInicio { get; set; }

        [DataType(DataType.Date)]
        [ValidateYears(ErrorMessage = "End date must be between {0} and {1}.")]
        [DateGreaterThan("IntervaloInicio", ErrorMessage = "Date not valid")]
        public DateTime? IntervaloFin { get; set; }

        public string? Months { get; set; }
    }
}
