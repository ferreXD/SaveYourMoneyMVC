using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SaveYourMoneyMVC.Common.Enums.Enums;

namespace SaveYourMoneyMVC.Entities
{
    public class Gasto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public TipoGasto Tipo { get; set; }
        [Required]
        public double Valor { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public DateTime Created { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual FileEntity? File { get; set; }
    }
}
