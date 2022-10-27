using System.ComponentModel.DataAnnotations.Schema;

namespace SaveYourMoneyMVC.Entities
{
    public class FileEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public Guid GastoId { get; set; }
        public virtual Gasto Gasto { get; set; }
    }
}
