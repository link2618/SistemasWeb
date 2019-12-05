using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemasWeb.Areas.Categorias.Models;
using SistemasWeb.Controllers;
using SistemasWeb.Data;
using SistemasWeb.Library;
using SistemasWeb.Models;

namespace SistemasWeb.Areas.Categorias.Controllers
{
    [Area("Categorias")]
    [Authorize(Roles = "Admin")]
    public class CategoriasController : Controller
    {
        private TCategoria _categoria;
        private LCategorias _lcategoria;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<TCategoria> models;
        private static IdentityError identityError = null;
        public CategoriasController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _lcategoria = new LCategorias(context);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Categoria(int id, string Search, int registros)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _lcategoria.getTCategoria(Search);

                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<TCategoria>().paginador(_lcategoria.getTCategoria(Search), id, registros, "Categorias", "Categorias", "Categoria", url);

                }
                else
                {
                    objects[0] = "No hay datos para mostrar.";
                    objects[1] = "No hay datos para mostrar.";
                    objects[2] = new List<TCategoria>();
                }

                models = new DataPaginador<TCategoria>
                {
                    List = (List<TCategoria>)objects[2],
                    Pagi_info = (string)objects[0],
                    Pagi_navegacion = (string)objects[1], 
                    Input = new TCategoria()
                };

                if (identityError != null)
                {
                    models.Pagi_info = identityError.Description;
                    identityError = null;
                }
                return View(models);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public String GetCategorias(DataPaginador<TCategoria> model) 
        {
            if (model.Input.Nombre != null && model.Input.Descripcion != null) 
            {
                var data = _lcategoria.RegistrarCategoria(model.Input);
                return JsonConvert.SerializeObject(data);
            }
            else 
            {
                return "Llene los campos que son obligatorios.";
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateEstado(int id)
        {
            identityError = _lcategoria.UpdateEstado2(id);
            return Redirect("/Categorias/Categoria?area=Categorias");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public string EliminarCategoria(int CategoriaID)
        {
            identityError = _lcategoria.DeleteCategoria(CategoriaID);
            return JsonConvert.SerializeObject(identityError);
        }
    }
}