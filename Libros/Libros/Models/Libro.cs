//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Libros.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Libro
    {
        public Libro()
        {
            this.AutorLibro = new HashSet<AutorLibro>();
        }
    
        public int IdLibro { get; set; }
        public string Titulo { get; set; }

        public System.DateTime FechaEdicion { get; set; }

        public int Autores { get; set; }
    
        public virtual ICollection<AutorLibro> AutorLibro { get; set; }
    }
}
