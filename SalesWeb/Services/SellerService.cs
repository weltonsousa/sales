using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWeb.Models;
using SalesWeb.Data;
using Microsoft.EntityFrameworkCore;
using SalesWeb.Services.Exception;

namespace SalesWeb.Services
{
    public class SellerService
    {
        private readonly SalesWebContext _context;

        public SellerService(SalesWebContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            //retorna uma lista com todos os vendedores
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            //obj.Department = _context.Department.First();
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            // Include(obj => obj.Department) eager loading faz um relacionamento entre tabelas
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);

            _context.Seller.Remove(obj);

            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            //expressao lambda (x => x.Id == obj.Id)
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch(DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
