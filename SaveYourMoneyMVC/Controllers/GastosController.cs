using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaveYourMoneyMVC.Common.Helpers;
using SaveYourMoneyMVC.Context;
using SaveYourMoneyMVC.DTOs.Analisis.Filtros;
using SaveYourMoneyMVC.DTOs.Gastos;
using SaveYourMoneyMVC.Entities;
using SaveYourMoneyMVC.Models.Gastos;
using System.Collections.Generic;
using System.Globalization;

using SystemFile = System.IO.File;
using static SaveYourMoneyMVC.Common.Constants.Contants;
using System.ComponentModel.DataAnnotations;
using System;

namespace SaveYourMoneyMVC.Controllers
{
    public class GastosController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _ctx;

        private readonly string _container;

        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;

        public GastosController(AppDbContext ctx, UserManager<User> userManager, IMapper mapper, IFileStorage fileStorage)
        {
            _ctx = ctx;
            _userManager = userManager;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _container = "files";
        }

        [HttpGet]
        public async Task<IActionResult> Index(PopupViewModel model)
        {
            var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var gastos = _ctx.Gastos.Where(g => g.UserId.Equals(currentUser.Id)).OrderByDescending(g => g.Created).Include(g => g.User).Include(g => g.File);

            var gastoViewModel = _mapper.Map<IEnumerable<Gasto>, IEnumerable <GastoViewModel>>(gastos);

            var viewModel = new GastoEditViewModel() {
                GastoViewModels = gastoViewModel,
                EditViewModel = new GastoViewModel(),
                MessageType = !string.IsNullOrWhiteSpace(model.Type) ? model.Type : "",
                PopupMessage = !string.IsNullOrWhiteSpace(model.PopupMessage) ? model.PopupMessage : ""
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new GastoViewModel());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Crear([Bind("Valor, Tipo, Descripcion, Created, FileName, FileContent, FileType")] GastoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if(user != null)
            {
                var gasto = _mapper.Map<GastoViewModel, Gasto>(model);
                gasto.UserId = user.Id;

                if (!string.IsNullOrWhiteSpace(model.FileContent))
                {
                    var ba = Convert.FromBase64String(model.FileContent);
                    var path = await _fileStorage.SaveFile(ba, model.FileType.Split("/")[1], _container);

                    gasto.File = new FileEntity() { Name = model.FileName, Type = model.FileType, Path = path};
                    await _ctx.Files.AddAsync(gasto.File);
                }

                await _ctx.Gastos.AddAsync(gasto);
                await _ctx.SaveChangesAsync();
            }

            PopupViewModel vm = new PopupViewModel() { Type = "Success", PopupMessage = user.Language.Equals(LANGUAGE_ES) ? Properties.Resources.Expends_NewExpend_Es : Properties.Resources.Expends_NewExpend_En };

            return RedirectToAction("Index", "Gastos", vm);
        }

        [HttpPost("[controller]/Update")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update([Bind("EditViewModel")] GastoEditViewModel model)
        {
            if (!ModelState.IsValid && model.GastoViewModels != null)
                return RedirectToAction("Index", "Gastos", model);

            var context = new ValidationContext(model.EditViewModel, null, null);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(model.EditViewModel, context, results, true);

            if(results.Count > 0)
            {
                PopupViewModel errorVm = new PopupViewModel() { Type = "Error", PopupMessage = results.FirstOrDefault().ErrorMessage };
                return RedirectToAction("Index", "Gastos", errorVm);
            }

            var dbEntity = _ctx.Gastos.Where(gasto => gasto.Id.Equals(model.EditViewModel.Id)).Include(gasto => gasto.File).FirstOrDefault();

            if (!dbEntity.Valor.Equals(model.EditViewModel.Valor))
                dbEntity.Valor = model.EditViewModel.Valor;

            if (!dbEntity.Created.Equals(model.EditViewModel.Created))
                dbEntity.Created = model.EditViewModel.Created;

            if (string.IsNullOrWhiteSpace(dbEntity.Descripcion) || !dbEntity.Descripcion.Equals(model.EditViewModel.Descripcion))
                dbEntity.Descripcion = model.EditViewModel.Descripcion;

            if (!dbEntity.Tipo.Equals(model.EditViewModel.Tipo))
                dbEntity.Tipo = model.EditViewModel.Tipo; 
            
            if (!string.IsNullOrWhiteSpace(model.EditViewModel.FileContent))
            {
                string path = string.Empty;
                var ba = Convert.FromBase64String(model.EditViewModel.FileContent);

                if (dbEntity.File != null)
                {
                    path = await _fileStorage.EditFile(ba, model.EditViewModel.FileType.Split("/")[1], _container, dbEntity.File.Path);

                    dbEntity.File.Name = model.EditViewModel.FileName;
                    dbEntity.File.Type = model.EditViewModel.FileType;
                    dbEntity.File.Path = path;
                }
                else
                {
                    path = await _fileStorage.SaveFile(ba, model.EditViewModel.FileType.Split("/")[1], _container);
                    dbEntity.File = new FileEntity() { Path = path, Name = model.EditViewModel.FileName, Type = model.EditViewModel.FileType };
                }
            }

            _ctx.Gastos.Update(dbEntity);
            await _ctx.SaveChangesAsync();

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            PopupViewModel vm = new PopupViewModel() { Type = "Success", PopupMessage = user.Language.Equals(LANGUAGE_ES) ? Properties.Resources.Expends_EditedExpend_Es : Properties.Resources.Expends_EditedExpend_En };

            return RedirectToAction("Index", "Gastos", vm);
        }

        [HttpPost("[controller]/Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dbEntity = await _ctx.Gastos.FindAsync(id);
            _ctx.Gastos.Remove(dbEntity);
            await _ctx.SaveChangesAsync();

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            PopupViewModel vm = new PopupViewModel() { Type = "Success", PopupMessage = user.Language.Equals(LANGUAGE_ES) ? Properties.Resources.Expends_DeleteExpend_Es : Properties.Resources.Expends_DeleteExpend_En };

            return RedirectToAction("Index", "Gastos", vm);
        }

        [HttpPost("[controller]/DownloadCsv")]
        public async Task<IActionResult> DownloadCsv()
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var gastos = _mapper.Map<IEnumerable<GastoCsvDTO>>(_ctx.Gastos.Where(g => g.UserId.Equals(user.Id)).Include(g => g.File));

            var fileName = KnownFolders.GetPath(KnownFolder.Downloads) + "\\" + DateTime.Now.ToString("yyyyMMdd") + "-" + user.UserName + ".csv";

            using (var stream = SystemFile.OpenWrite(fileName))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(gastos);
            }

            PopupViewModel vm = new PopupViewModel() { Type = "Success", PopupMessage = user.Language.Equals(LANGUAGE_ES) ? Properties.Resources.Expends_PopupMessage_Es : Properties.Resources.Expends_PopupMessage_En };

            return RedirectToAction("Index", "Gastos", vm);
        }
    }
}
