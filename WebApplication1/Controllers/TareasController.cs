using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models; // Reemplaza con el nombre correcto del espacio de nombres
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DTOs;

namespace WEBAPI.Controllers // Reemplaza con el nombre correcto del espacio de nombres
{
    [Route("api/tareas/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public TareaController(MyDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost]
        [Route("createTarea")]

        public IActionResult CreateReservation([FromBody] TareaDTO tareaDTO)
        {
            if (tareaDTO == null)
            {
                return BadRequest("Tarea data is required.");
            }
            if (tareaDTO.titulo == null || tareaDTO.titulo == "" || tareaDTO.fechaVencimiento <= DateTime.Today)
            {
                return Ok(new { message = "Error creating tarea, please verify the fechaVencimiento or the titulo."});
            }
            var tarea = new Tarea
            {
                Titulo = tareaDTO.titulo,
                Descripcion = tareaDTO.descripcion,
                FechaCreacion = tareaDTO.fechaCreacion,
                FechaVencimiento = tareaDTO.fechaVencimiento,
                 EstadoId = tareaDTO.estadoEnum
            };

            try
            {
                _dbContext.Tareas.Add(tarea);
                _dbContext.SaveChanges();
                return Ok(new { message = "Tarea created successfully.", tarea = tarea.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error creating tarea", error = ex.Message });
            }
        }
        [HttpGet]
        [Route("getTarea")]
        public IActionResult GetReservations()
        {
            try
            {
                var query = _dbContext.Tareas.AsQueryable();
                var tareas = query.ToList();

                return Ok(new { message = "Tareas retrieved successfully", data = tareas });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving tarea", error = ex.Message });
            }
        }

        [HttpPut]
        [Route("updateTarea/{id}")]
        public IActionResult UpdateTarea(int id, [FromBody] TareaDTO tareaDTO)
        {
            if (tareaDTO == null)
            {
                return BadRequest("Reservation data is required.");
            }
            if (tareaDTO.titulo == null || tareaDTO.titulo == "" || tareaDTO.fechaVencimiento <= DateTime.Today)
            {
                return Ok(new { message = "Error creating tarea, please verify the fechaVencimiento or the titulo." });
            }
            try
            {
                var existingReservation = _dbContext.Tareas.Find(id);
                if (existingReservation == null)
                {
                    return NotFound("Tarea not found.");
                }


                existingReservation.Titulo = tareaDTO.titulo;
                existingReservation.Descripcion = tareaDTO.descripcion;
                existingReservation.FechaVencimiento = tareaDTO.fechaVencimiento;
                existingReservation.FechaCreacion = tareaDTO.fechaCreacion;
                existingReservation.EstadoId = tareaDTO.estadoEnum;

                _dbContext.Tareas.Update(existingReservation);
                _dbContext.SaveChanges();

                return Ok(new { message = "Tarea updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating tarea", error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("deleteTarea/{id}")]
        public IActionResult DeleteTarea(int id)
        {
            try
            {
                var tareas = _dbContext.Tareas.Find(id);
                if (tareas == null)
                {
                    return NotFound("Tarea not found.");
                }


                _dbContext.Tareas.Remove(tareas);

                _dbContext.SaveChanges();
                return Ok(new { message = "Tarea deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error deleting tarea", error = ex.Message });
            }
        }
        [HttpGet]
        [Route("tareasById")]
        public IActionResult GetTareasById(int? tareaId)
        {
            // Consultar las reservas asociadas al usuario
            var tareas = _dbContext.Tareas
                .Where(ur => ur.Id == tareaId)
                .ToList();

            if (tareas == null || !tareas.Any())
            {
                return NotFound("No tareas found .");
            }

            return Ok(tareas[0]);
        }

    }
}
