using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemasWeb.Areas.Categorias.Models;
using SistemasWeb.Data;

namespace SistemasWeb.Library
{
    public class LCategorias
    {
        private ApplicationDbContext context;

        public LCategorias(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IdentityError RegistrarCategoria(TCategoria categoria) 
        {
            IdentityError identityError;

            try
            {
                if (categoria.CategoriaID.Equals(0))// agregar categoria
                {
                    this.context.Add(categoria);
                }else //actualizar categoria
                {
                    this.context.Update(categoria);
                }

                this.context.SaveChanges();

                identityError = new IdentityError { Code = "Done"};
            }
            catch (Exception e)
            {
                identityError = new IdentityError
                {
                    Code = "Error",
                    Description = e.Message
                };
            }

            return identityError;
        }

        public List<TCategoria> getTCategoria(string valor) 
        {
            List<TCategoria> listCategoria;

            if(valor == null)
            {
                listCategoria = this.context._TCategoria.ToList();
            }else
            {
                listCategoria = this.context._TCategoria.Where(c => c.Categoria.StartsWith(valor)).ToList();
            }

            return listCategoria;
        }

        internal IdentityError UpdateEstado2(int id)
        {
            IdentityError identityError;

            try
            {
                var categoria = this.context._TCategoria.Where(c => c.CategoriaID.Equals(id)).ToList().ElementAt(0);
                categoria.Estado = categoria.Estado ? false : true;
                //int data = Convert.ToInt16("a");
                this.context.Update(categoria);
                this.context.SaveChanges();
                identityError = new IdentityError { Description = "Done" };
            }
            catch (Exception e)
            {
                identityError = new IdentityError
                {
                    Code = "Error",
                    Description = e.Message
                };
            }

            return identityError;
        }

        internal IdentityError DeleteCategoria(int categoriaID)
        {
            IdentityError identityError;
            try
            {
                var categoria = new TCategoria
                {
                    CategoriaID = categoriaID
                };

                this.context.Remove(categoria);
                this.context.SaveChanges();
                identityError = new IdentityError { Description = "Done" };
            }
            catch (Exception e)
            {
                identityError = new IdentityError
                {
                    Code = "Error",
                    Description = e.Message
                };
            }

            return identityError;
        }

        public List<SelectListItem> getCategoria()
        {
            var _selectList = new List<SelectListItem>();
            var categorias = this.context._TCategoria.Where(c => c.Estado.Equals(true)).ToList();

            foreach (var item in categorias)
            {
                _selectList.Add(new SelectListItem {
                    Text = item.Categoria,
                    Value = item.CategoriaID.ToString()
                });
            }

            return _selectList;
        }
    }
}
