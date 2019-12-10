using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemasWeb.Data;
using SistemasWeb.Library;
using SistemasWeb.Models;

namespace SistemasWeb.Areas.Inscripciones.Controllers
{
    [Area("Inscripciones")]
    public class InscripcionesController : Controller
    {
        private LCursos _curso;
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private static DataPaginador<DataCurso> models;
        private static IdentityError identityError;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public InscripcionesController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _signInManager = signInManager;
            _userManager = userManager;
            _curso = new LCursos(context, null);
        }

        public async Task<IActionResult> Inscripciones(int id, String search, int Registros)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var user = await _userManager.GetUserAsync(User);
                var idUser = await _userManager.GetUserIdAsync(user);
                var data = _curso.Inscripciones(idUser, search);

                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<DataCurso>().paginador(data
                     , id, Registros, "Inscripciones", "Inscripciones", "Inscripciones", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<DataCurso>();
                }

                models = new DataPaginador<DataCurso>
                {
                    List = (List<DataCurso>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new DataCurso()
                };

                return View(models);

            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        public IActionResult Detalles(int id)
        {
            var model = _curso.getTCurso(id);
            return View(model);
        }

        public async Task<IActionResult> Export()
        {
            var list = new List<String[]>();

            if (!models.List.Equals(0))
            {
                foreach (var item in models.List)
                {
                    String[] listData ={
                        item.CursoID.ToString(),
                        item.Curso,
                        item.Informacion,
                        item.Horas.ToString(),
                        String.Format("${0:#,###,###,##0.00####}", item.Costo),
                        item.Estado.ToString(),
                        item.Categoria,
                    };
                    list.Add(listData);
                }
            }
            String[] titles = { "CursoID", "Curso", "Informacion", "Horas", "Costo", "Estado", "Categoria" };
            var export = new ExportData(_hostingEnvironment, list, titles, "Inscripciones.xlsx", "Inscripciones");
            return await export.ExportExcelAsync();
        }

    }
}