using BusinessLayer.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;

namespace WebAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly IBL_Vehiculos _bl;

        public VehiculosController(IBL_Vehiculos bl)
        {
            _bl = bl;
        }

        // GET: api/<VehiculosController>
        [ProducesResponseType(typeof(List<Vehiculo>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Vehiculo> vehiculos = _bl.Get();
                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET api/<VehiculosController>/5
        [HttpGet("{matricula}")]
        public IActionResult Get(string matricula)
        {
            try
            {
                Vehiculo vehiculo = _bl.Get(matricula);

                if (vehiculo == null)
                {
                    return NotFound("Vehículo no encontrado");
                }

                return Ok(vehiculo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST api/<VehiculosController>
        [HttpPost]
        public IActionResult Post([FromBody] Vehiculo vehiculo, string documentoPersona)
        {
            try
            {
                if (vehiculo == null)
                {
                    return BadRequest("La solicitud no contiene un vehículo válido.");
                }

                _bl.Insert(vehiculo, documentoPersona);

                return CreatedAtAction(nameof(Get), new { matricula = vehiculo.Matricula }, vehiculo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT api/<VehiculosController>/5
        [HttpPut("{matricula}")]
        public IActionResult Put(string matricula, [FromBody] Vehiculo vehiculo)
        {
            try
            {
                if (vehiculo == null || vehiculo.Matricula != matricula)
                {
                    return BadRequest("La solicitud no contiene un vehículo válido.");
                }

                _bl.Update(vehiculo);

                return Ok(vehiculo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE api/<VehiculosController>/5
        [HttpDelete("{matricula}")]
        public IActionResult Delete(string matricula)
        {
            try
            {
                _bl.Delete(matricula);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
