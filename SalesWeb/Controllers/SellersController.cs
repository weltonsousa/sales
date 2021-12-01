using Microsoft.AspNetCore.Mvc;
using SalesWeb.Models;
using SalesWeb.Models.ViewModels;
using SalesWeb.Services;
using System.Collections.Generic;
using SalesWeb.Services.Exception;
using System.Diagnostics;
using System;

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
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();

            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };

            return View(viewModel);
        }

        [HttpPost] //obtenm do  resquest
        [ValidateAntiForgeryToken] // validacao contra ataque crsf
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {

                //objeto anonimo  new { message = ("Id mismatch") })
                return RedirectToAction(nameof(Error), new { message = ("Id not provided") });
            }
            
            var obj = _sellerService.FindById(id.Value);

            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = ("Id not found") });
            }

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value);

            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found"}) ;
            }

            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
           if(id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not mismatch"});
            }
            try
            {
                _sellerService.Update(seller);

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
