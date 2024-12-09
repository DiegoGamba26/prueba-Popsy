using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Tarea
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int EstadoId { get; set; }

        public virtual EstadoEnum Estado { get; set; } = null!;
    }
}
