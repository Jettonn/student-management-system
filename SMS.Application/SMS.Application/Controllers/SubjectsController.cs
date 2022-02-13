using Microsoft.AspNetCore.Mvc;
using SMS.Application.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Controllers
{
    public class SubjectsController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
