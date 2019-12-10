using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using SistemasWeb.Areas.Cursos.Models;
using SistemasWeb.Areas.Inscripciones.Models;
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

        public DataCurso getTCurso(int id)
        {
            DataCurso dataCurso = null;
            var query = context._TCategoria.Join(context._TCursos,
                c => c.CategoriaID, //Categorias
                t => t.CategoriaID, //Cursos
                (c, t) => new {
                    c.CategoriaID,
                    c.Categoria,
                    t.CursoID,
                    t.Curso,
                    t.Informacion,
                    t.Horas,
                    t.Costo,
                    t.Estado,
                    t.Image
                }).Where(d => d.CursoID.Equals(id)).ToList();

            if (!query.Count.Equals(0))
            {
                var data = query.Last(); //se obtiene el ultimo elemento
                dataCurso = new DataCurso
                {
                    CursoID = data.CursoID,
                    Curso = data.Curso,
                    Informacion = data.Informacion,
                    Horas = data.Horas,
                    Costo = data.Costo,
                    Estado = data.Estado,
                    Image = data.Image,
                    Categoria = data.Categoria,
                };
            }

            return dataCurso;
        }

        public IdentityError Inscripcion(string idUser, int cursoID)
        {
            IdentityError identityError;
            try
            {
                //Miramos si esta suscrito en el curso
                var cursoInscripcion = context._TInscripcion.Where(c => c.CursoID.Equals(cursoID) && c.EstudianteID.Equals(idUser)).ToList();
                //Si no hace parte del curso se inscribe
                if (cursoInscripcion.Count.Equals(0))
                {
                    var curso = getTCurso(cursoID);
                    var inscripcion = new Inscripcion
                    {
                        EstudianteID = idUser,
                        Fecha = DateTime.Now,
                        Pago = curso.Costo,
                        CursoID = curso.CursoID
                    };
                    context.Add(inscripcion);
                    context.SaveChanges();
                    identityError = new IdentityError { Description = "Done" };
                }
                else
                {
                    identityError = new IdentityError { Description = "Ya está suscrito en el curso" };
                }
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

        public List<DataCurso> Inscripciones(string idUser, String search)
        {
            List<DataCurso> cursos = new List<DataCurso>();
            var inscripciones = context._TInscripcion.Where(c => c.EstudianteID.Equals(idUser)).ToList();

            if (!inscripciones.Count.Equals(0))
            {
                inscripciones.ForEach(c =>
                {
                    if (search == null || search.Equals(""))
                    {
                        var query = context._TCategoria.Join(context._TCursos,
                            c => c.CategoriaID,
                            t => t.CategoriaID,
                            (c, t) => new
                            {
                                c.CategoriaID,
                                c.Categoria,
                                t.CursoID,
                                t.Curso,
                                t.Informacion,
                                t.Horas,
                                t.Costo,
                                t.Estado,
                                t.Image

                            }).Where(d => d.CursoID.Equals(c.CursoID)).ToList();

                        if (!query.Count.Equals(0))
                        {
                            var data = query.Last();
                            cursos.Add(new DataCurso
                            {
                                CursoID = data.CursoID,
                                Curso = data.Curso,
                                Informacion = data.Informacion,
                                Horas = data.Horas,
                                Costo = data.Costo,
                                Estado = data.Estado,
                                Image = data.Image,
                                Categoria = data.Categoria,
                            });
                        }

                    }
                    else
                    {
                        var query = context._TCategoria.Join(context._TCursos,
                            c => c.CategoriaID,
                            t => t.CategoriaID,
                            (c, t) => new
                            {
                                c.CategoriaID,
                                c.Categoria,
                                t.CursoID,
                                t.Curso,
                                t.Informacion,
                                t.Horas,
                                t.Costo,
                                t.Estado,
                                t.Image

                            }).Where(d => d.CursoID.Equals(c.CursoID) && d.Curso.StartsWith(search)).ToList();

                        if (!query.Count.Equals(0))
                        {
                            var data = query.Last();
                            cursos.Add(new DataCurso
                            {
                                CursoID = data.CursoID,
                                Curso = data.Curso,
                                Informacion = data.Informacion,
                                Horas = data.Horas,
                                Costo = data.Costo,
                                Estado = data.Estado,
                                Image = data.Image,
                                Categoria = data.Categoria,
                            });
                        }
                    }
                });
            }

            return cursos;
        }

    }
}
