using SalesWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWeb.Models;
using Microsoft.EntityFrameworkCore;


namespace SalesWeb.Services
{
    public class DepartmentService
    {
        private readonly SalesWebContext _context;


        public DepartmentService(SalesWebContext context)
        {
            _context = context;
        }

        //load data async
        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }

    }
}
