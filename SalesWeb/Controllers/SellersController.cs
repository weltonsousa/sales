using Microsoft.AspNetCore.Mvc;
using SalesWeb.Models;
using SalesWeb.Models.ViewModels;
using SalesWeb.Services;
using System.Collections.Generic;
using SalesWeb.Services.Exception;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace SalesWeb.Controllers
{
    public class SellersController : Controller
    {
        //injecao de dependencia
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        //*************************************
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();

            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };

            return View(viewModel);
        }

        [HttpPost] //obtenm do  resquest
        [ValidateAntiForgeryToken] // validacao contra ataque crsf
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departaments = await _departmentService.FindAllAsync();

                var viweModel = new SellerFormViewModel { Seller = seller, Departments = departaments };
                return View(viweModel);
            }
            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {

                //objeto anonimo  new { message = ("Id mismatch") })
                return RedirectToAction(nameof(Error), new { message = ("Id not provided") });
            }
            
            var obj = await _sellerService.FindByIdAsync(id.Value);

            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = ("Id not found") });
            }

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id);

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found"}) ;
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departaments = await _departmentService.FindAllAsync();

                var viweModel = new SellerFormViewModel { Seller = seller, Departments = departaments };
                return View(viweModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not mismatch"});
            }
            try
            {
               await _sellerService.UpdateAsync(seller);

                return RedirectToAction(nameof(Index));
            }
            // a claase applicationexception traz as excessoes (generica) para as duas excessoes
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new {e.Message});
            }

            //catch (NotFoundException e)
            //{
            //    return RedirectToAction(nameof(Error), new {e.Message});
            //}
            //catch(DbConcurrencyException e){

            //    return RedirectToAction(nameof(Error), new { e.Message });
            //}                   

            }

            public IActionResult Error(string message)
            {
                var viewModel = new ErrorViewModel
                {
                    Message = message,

                    //pega o id interno da requisicao (?? Operador de coliencia nula)
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier

                };

                return View(viewModel);
        }
    }
}
