using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemasWeb.Areas.Categorias.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemasWeb.Models
{
    public class DataPaginador<T>
    {
        public List<T> List { get; set; }
        public string Pagi_info { get; set; }
        public string Pagi_navegacion { get; set; }
        public T Input { get; set; }
        public int Registros { get; set; }
        public string search { get; set; }
        public IFormFile AvatarImage { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }
}
}
