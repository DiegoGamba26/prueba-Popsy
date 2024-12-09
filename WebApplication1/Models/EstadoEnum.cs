using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class EstadoEnum
    {
        public EstadoEnum()
        {
            Tareas = new HashSet<Tarea>();
        }

        public int EstadoId { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Tarea> Tareas { get; set; }
    }
}
