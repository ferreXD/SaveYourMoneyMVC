using System.ComponentModel.DataAnnotations;

namespace SaveYourMoneyMVC.Models.Gastos
{
    public class GastoEditViewModel
    {
        public IEnumerable<GastoViewModel> GastoViewModels { get; set; }
        public GastoViewModel EditViewModel { get; set; }
        public string? MessageType { get; set; }
        public string? PopupMessage { get; set; }
    }
}
