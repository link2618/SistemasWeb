using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using SistemasWeb.Areas.Cursos.Models;
using SistemasWeb.Data;
using SistemasWeb.Models;

namespace SistemasWeb.Library
{
    public class LCursos
    {
        private Uploadimage _image; 
        private ApplicationDbContext context;
        private IWebHostEnvironment environment;

        public LCursos(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
            _image = new Uploadimage();
        }

        public async Task<IdentityError> RegistrarCursoAsync(DataPaginador<TCursos> model)
        {
            IdentityError identityError;
            try
            {
                if (model.Input.CursoID.Equals(0)) //vamos agregar un curso
                {
                    var imageByte = await _image.ByteAvatarImageAsync(model.AvatarImage, environment);
                    var curso = new TCursos
                    {
                        Curso = model.Input.Curso,
                        Informacion = model.Input.Informacion,
                        Horas = model.Input.Horas,
                        Costo = model.Input.Costo,
                        Estado = model.Input.Estado,
                        Image = imageByte,
                        CategoriaID = model.Input.CategoriaID
                    };
                    context.Add(curso);
                    context.SaveChanges();
                }
                else // vamos a actualizar un curso
                {
                    byte[] imageByte;
                    if (model.AvatarImage != null) //  actualizar la imagen del curso si no es la dada por defecto
                    {
                        imageByte = await _image.ByteAvatarImageAsync(model.AvatarImage, environment);
                    }
                    else // Para no actualizar la imagen y dejar la misma
                    {
                        imageByte = model.Input.Image;
                    }
                    var curso = new TCursos
                    {
                        CursoID = model.Input.CursoID,
                        Curso = model.Input.Curso,
                        Informacion = model.Input.Informacion,
                        Horas = model.Input.Horas,
                        Costo = model.Input.Costo,
                        Estado = model.Input.Estado,
                        Image = imageByte,
                        CategoriaID = model.Input.CategoriaID
                    };
                    context.Update(curso);
                    context.SaveChanges();
                }

                identityError = new IdentityError { Code = "Done" };
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

        internal List<TCursos> getTCursos(string search)
        {
            List<TCursos> listCursos;
            if (search == null)
            {
                listCursos = context._TCursos.ToList();
            }
            else
            {
                listCursos = context._TCursos.Where(c => c.Curso.StartsWith(search)).ToList();
            }
            return listCursos;
        }

        internal IdentityError UpdateEstado(int id)
        {
            IdentityError identityError;

            try
            {
                var curso = context._TCursos.Where(c => c.CursoID.Equals(id)).ToList().ElementAt(0);
                curso.Estado = curso.Estado ? false : true;

                context.Update(curso);
                context.SaveChanges();

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

        internal IdentityError DeleteCurso(int cursoID)
        {
            IdentityError identityError;
            try
            {
                var curso = new TCursos
                {
                    CursoID = cursoID
                };
                context.Remove(curso);
                context.SaveChanges();
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
    }
}
