﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniStoreWeb.Data.Entities
{
    public class Libro
    {
        [Key]
        public int Codigo { get; set; }

        [Display(Name ="Editorial")]
        [MaxLength(100,ErrorMessage ="El máximo numero de caractéres es de {0}")]
        [Required(ErrorMessage ="El campo {0} es obligatorio.")]
        public string Editorial { get; set; }

        [Display(Name = "Título")]
        [MaxLength(50, ErrorMessage = "El máximo numero de caractéres es de {0}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Titulo { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Descripcion { get; set; }

        [Display(Name = "Précio")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Precio { get; set; }
                
        public string Path { get; set; }
    }
}
