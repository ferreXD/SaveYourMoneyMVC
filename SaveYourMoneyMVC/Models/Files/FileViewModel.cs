using SaveYourMoneyMVC.Entities;

namespace SaveYourMoneyMVC.Models.Files
{
    public class FileViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public Guid GastoId { get; set; }
        public virtual Gasto gasto { get; set; }
    }
}
