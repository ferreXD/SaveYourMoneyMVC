using SaveYourMoneyMVC.Entities;
using static SaveYourMoneyMVC.Common.Enums.Enums;
using System.ComponentModel.DataAnnotations;

namespace SaveYourMoneyMVC.DTOs.Gastos
{
    public class GastoCsvDTO
    {
        public TipoGasto Tipo { get; set; }
        public double Valor { get; set; }
        public string? Descripcion { get; set; }
        public string Created { get; set; }
        public string? FileName { get; set; }
    }
}
