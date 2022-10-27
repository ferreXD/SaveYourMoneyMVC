using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using SaveYourMoneyMVC.Context;
using SaveYourMoneyMVC.DTOs.Analisis;
using SaveYourMoneyMVC.DTOs.Analisis.Filtros;
using SaveYourMoneyMVC.Entities;
using SaveYourMoneyMVC.Models.Analisis;
using static SaveYourMoneyMVC.Common.Constants.Contants;
using static SaveYourMoneyMVC.Common.Enums.Enums;

namespace SaveYourMoneyMVC.Controllers
{
    public class AnalisisController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;

        public AnalisisController(UserManager<User> userManager, AppDbContext ctx, IMapper mapper)
        {
            _userManager = userManager;
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AnalisisFiltroDTO filtro = new AnalisisFiltroDTO() { IntervaloInicio = DateTime.UtcNow.AddMonths(-11), IntervaloFin = DateTime.UtcNow };

            var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var allExpends = _ctx.Gastos.Where(g => g.UserId.Equals(currentUser.Id));

            if (filtro.TipoGasto.HasValue)
                allExpends.Where(e => e.Tipo.Equals(filtro.TipoGasto));

            var groupedExpendsByType = allExpends.GroupBy(
                expend => expend.Tipo,
                expend => expend,
                (key, group) => new { Key = key, Expends = group.ToList() }
            );

            Dictionary<TipoGasto, List<AnalisisResponseDTO>> dictionary = new Dictionary<TipoGasto, List<AnalisisResponseDTO>>();

            foreach (var group in groupedExpendsByType)
            {
                dictionary.Add(group.Key, new List<AnalisisResponseDTO>());

                foreach(var expend in group.Expends)
                {
                    if(!dictionary.GetValueOrDefault(group.Key).Any(v => v.Year.Equals(expend.Created.Year) && v.Month.Equals(expend.Created.Month)))
                    {
                        var toAdd = new AnalisisResponseDTO() { Month = expend.Created.Month, Year = expend.Created.Year, Value = expend.Valor };
                        dictionary.GetValueOrDefault(group.Key).Add(toAdd);
                    }
                    else
                    {
                        var prevExpend = dictionary.GetValueOrDefault(group.Key).FirstOrDefault(v => v.Year.Equals(expend.Created.Year) && v.Month.Equals(expend.Created.Month));
                        prevExpend.Value += expend.Valor;
                    }
                }
            }

            foreach(var key in dictionary.Keys)
            {
                var expendByMonth = dictionary[key].Where(entry => new DateTime(entry.Year, entry.Month, DateTime.DaysInMonth(entry.Year, entry.Month)) >= filtro.IntervaloInicio.Value && new DateTime(entry.Year, entry.Month, 1) <= filtro.IntervaloFin.Value).ToList();

                for (DateTime inicio = filtro.IntervaloInicio.Value.Date; inicio < filtro.IntervaloFin.Value.Date; inicio = inicio.AddMonths(1))
                {
                    if (!expendByMonth.Any(e => e.Month == inicio.Month && e.Year == inicio.Year))
                        expendByMonth.Add(new AnalisisResponseDTO() { Month = inicio.Month, Year = inicio.Year, Value = 0 });
                }

                dictionary[key] = expendByMonth.OrderBy(e => e.Year).ThenBy(e => e.Month).ToList();
            }

            Dictionary<string, List<AnalisisResponseDTO>> toret = new Dictionary<string, List<AnalisisResponseDTO>>();

            if (dictionary.Values.All(v => v == null))
            {
                dictionary = new Dictionary<TipoGasto, List<AnalisisResponseDTO>>();
            }
            else
            {
                var userLang = currentUser.Language;
                var types = userLang.Equals(LANGUAGE_ES) ? EXPENDS_TYPE_ES : EXPENDS_TYPE_EN;

                foreach (var key in dictionary.Keys)
                {
                    toret.Add(types[(int) key], dictionary[key]);
                }
            }

            var viewModel = new AnalisisViewModel() {
                Dictionary = toret,
                JsonData = JsonConvert.SerializeObject(new { Dictionary = toret }),
                TipoGasto = null,
                IntervaloInicio = null,
                IntervaloFin = null,
                Months = JsonConvert.SerializeObject(new { Months = currentUser.Language.Equals(LANGUAGE_ES) ? MONTHS_ES : MONTHS_EN })
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("JsonData, TipoGasto, IntervaloInicio, IntervaloFin")] AnalisisViewModel model)
        {
             if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.Where(v => v.ValidationState.Equals(ModelValidationState.Invalid));
                var modelStateVal = ViewData.ModelState[ModelState.Keys.FirstOrDefault(k => k.Equals("TipoGasto"))];

                if (!(errors.Count() == 1 && modelStateVal.Errors.Count > 0))
                    return View(model);
            }

            AnalisisFiltroDTO filtro = _mapper.Map<AnalisisFiltroDTO>(model);

            if (!filtro.IntervaloInicio.HasValue)
                filtro.IntervaloInicio = DateTime.UtcNow.AddMonths(-11);

            if (!filtro.IntervaloFin.HasValue)
                filtro.IntervaloFin = filtro.IntervaloInicio.Value.AddMonths(12);

            if (model.TipoGasto.HasValue)
                filtro.TipoGasto = model.TipoGasto;

            var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var allExpends = _ctx.Gastos.Where(g => g.UserId.Equals(currentUser.Id));

            if (filtro.TipoGasto.HasValue)
                allExpends = allExpends.Where(e => e.Tipo.Equals(filtro.TipoGasto));

            var groupedExpendsByType = allExpends.GroupBy(
                expend => expend.Tipo,
                expend => expend,
                (key, group) => new { Key = key, Expends = group.ToList() }
            );

            Dictionary<TipoGasto, List<AnalisisResponseDTO>> dictionary = new Dictionary<TipoGasto, List<AnalisisResponseDTO>>();

            foreach (var group in groupedExpendsByType)
            {
                dictionary.Add(group.Key, new List<AnalisisResponseDTO>());

                foreach (var expend in group.Expends)
                {
                    if (!dictionary.GetValueOrDefault(group.Key).Any(v => v.Year.Equals(expend.Created.Year) && v.Month.Equals(expend.Created.Month)))
                    {
                        var toAdd = new AnalisisResponseDTO() { Month = expend.Created.Month, Year = expend.Created.Year, Value = expend.Valor };
                        dictionary.GetValueOrDefault(group.Key).Add(toAdd);
                    }
                    else
                    {
                        var prevExpend = dictionary.GetValueOrDefault(group.Key).FirstOrDefault(v => v.Year.Equals(expend.Created.Year) && v.Month.Equals(expend.Created.Month));
                        prevExpend.Value += expend.Valor;
                    }
                }
            }

            foreach (var key in dictionary.Keys)
            {
                var expendByMonth = dictionary[key].Where(entry => new DateTime(entry.Year, entry.Month, DateTime.DaysInMonth(entry.Year, entry.Month)) >= filtro.IntervaloInicio.Value && new DateTime(entry.Year, entry.Month, 1) <= filtro.IntervaloFin.Value).ToList();

                for (DateTime inicio = filtro.IntervaloInicio.Value.Date; inicio < filtro.IntervaloFin.Value.Date; inicio = inicio.AddMonths(1))
                {
                    if (!expendByMonth.Any(e => e.Month == inicio.Month && e.Year == inicio.Year))
                        expendByMonth.Add(new AnalisisResponseDTO() { Month = inicio.Month, Year = inicio.Year, Value = 0 });
                }

                if (expendByMonth.All(ex => ex.Value == 0))
                    dictionary[key] = null;
                else
                    dictionary[key] = expendByMonth.OrderBy(e => e.Year).ThenBy(e => e.Month).ToList();
            }

            Dictionary<string, List<AnalisisResponseDTO>> toret = new Dictionary<string, List<AnalisisResponseDTO>>();

            if (dictionary.Values.All(v => v == null))
            {
                dictionary = new Dictionary<TipoGasto, List<AnalisisResponseDTO>>();
            }
            else
            {
                var userLang = currentUser.Language;
                var types = userLang.Equals(LANGUAGE_ES) ? EXPENDS_TYPE_ES : EXPENDS_TYPE_EN;

                foreach(var key in dictionary.Keys)
                {
                    if (dictionary[key] != null)
                        toret.Add(types[(int) key], dictionary[key]);
                }
            }

            var viewModel = new AnalisisViewModel()
            {
                Dictionary = toret,
                JsonData = JsonConvert.SerializeObject(new { Dictionary = toret }),
                TipoGasto = model.TipoGasto.HasValue ? model.TipoGasto.Value : null,
                IntervaloInicio = model.IntervaloInicio.HasValue ? model.IntervaloInicio.Value : null,
                IntervaloFin = model.IntervaloFin.HasValue ? model.IntervaloFin.Value : null,
                Months = JsonConvert.SerializeObject(new { Months = currentUser.Language.Equals(LANGUAGE_ES) ? MONTHS_ES : MONTHS_EN })
            };

            return View(viewModel);
        }
    }
}
