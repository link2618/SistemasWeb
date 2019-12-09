using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SistemasWeb.Areas.Inscripciones.Controllers
{
    [Area("Inscripciones")]
    public class InscripcionesController : Controller
    {
        public IActionResult Inscripciones()
        {
            return View();
        }
    }
}