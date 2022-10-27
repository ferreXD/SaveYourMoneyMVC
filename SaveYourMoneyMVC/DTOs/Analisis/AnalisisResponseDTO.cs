using static SaveYourMoneyMVC.Common.Enums.Enums;

namespace SaveYourMoneyMVC.DTOs.Analisis
{
    public class AnalisisResponseDTO
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public double Value { get; set; }
    }
}
