using static SaveYourMoneyMVC.Common.Enums.Enums;

namespace SaveYourMoneyMVC.DTOs.Analisis.Filtros
{
    public class AnalisisFiltroDTO
    {
        public TipoGasto? TipoGasto { get; set; }
        public DateTime? IntervaloInicio { get; set; }
        public DateTime? IntervaloFin { get; set; }
    }
}
