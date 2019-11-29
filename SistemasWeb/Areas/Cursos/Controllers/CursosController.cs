using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SistemasWeb.Areas.Cursos.Controllers
{
    [Area("Cursos")]
    public class CursosController : Controller
    {
        public IActionResult Cursos()
        {
            return View();
        }
    }
}