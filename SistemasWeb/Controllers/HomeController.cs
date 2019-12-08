﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SistemasWeb.Areas.Cursos.Models;
using SistemasWeb.Data;
using SistemasWeb.Library;
using SistemasWeb.Models;

namespace SistemasWeb.Controllers
{
    public class HomeController : Controller
    {
        private LCursos _curso;
        private static DataPaginador<TCursos> models;

        //private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        //public HomeController(ILogger<HomeController> logger)
        //{
            //_logger = logger;
        //}

        //IServiceProvider _serviceProvider;
        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager, IServiceProvider serviceProvider)
        {
            //_serviceProvider = serviceProvider;
            _signInManager = signInManager;
            _curso = new LCursos(context, null);
        }

        public IActionResult Index(int id, string Search)
        {
            Object[] objects = new Object[3];
            var data = _curso.getTCursos(Search);

            if (0 < data.Count)
            {
                var url = Request.Scheme + "://" + Request.Host.Value;
                objects = new LPaginador<TCursos>().paginador(data, id, 10, "", "Home", "Index", url);
            }
            else
            {
                objects[0] = "No hay datos que mostrar";
                objects[1] = "No hay datos que mostrar";
                objects[2] = new List<TCursos>();
            }

            models = new DataPaginador<TCursos>
            {
                List = (List<TCursos>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TCursos(),
            };

            //await CreateRolesAsync(_serviceProvider);
            return View(models);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task CreateRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //Creacion de roles
            String[] rolesName = { "Admin", "Student" };
            foreach (var item in rolesName)
            {
                var roleExist = await roleManager.RoleExistsAsync(item);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
                }
            }

            //Le asignamos un rol a un usuario ya existente
            var user = await userManager.FindByIdAsync("02ab999a-4940-454c-b5f4-ca910539624b");
            await userManager.AddToRoleAsync(user, "Admin");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
