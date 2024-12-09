using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class TareaDTO
    {
        [StringLength(100)]
        public string titulo { get; set; } = null!;

        [StringLength(255)]
        public string descripcion { get; set; } = null!;
      
        public DateTime fechaCreacion { get; set; }

        public DateTime fechaVencimiento { get; set; }
        public int estadoEnum { get; set; }

    }
}
