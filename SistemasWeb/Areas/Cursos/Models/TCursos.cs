using SistemasWeb.Areas.Categorias.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemasWeb.Areas.Cursos.Models
{
    public class TCursos
    {
        [Key]
        public int CursoID { get; set; }
        [Required(ErrorMessage = "El campo nombre es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo descripcion es obligatorio.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo horas es obligatorio.")]
        public byte Horas { get; set; }
        [Required(ErrorMessage = "El campo costo es obligatorio.")]
        [RegularExpression(@"^[0-9]+([.][0-9]+)?$", ErrorMessage = "El precio no es correcto.")]
        public decimal Costo { get; set; }
        public Boolean Estado { get; set; }
        [Required(ErrorMessage = "Seleccione una Categoria.")]
        public int CategoriaID { get; set; }
        public byte[] Image { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public TCategoria Categoria { get; set; }
    }
}
