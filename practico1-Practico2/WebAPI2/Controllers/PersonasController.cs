using BusinessLayer.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;

namespace WebAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly IBL_Personas _bl;

        public PersonasController(IBL_Personas bl)
        {
            _bl = bl;
        }

        // GET: api/<PersonasController>
        [ProducesResponseType(typeof(List<Persona>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Persona> personas = _bl.Get();
                return Ok(personas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET api/<PersonasController>/5
        [HttpGet("{documento}")]
        public IActionResult Get(string documento)
        {
            try
            {
                Persona persona = _bl.Get(documento);

                if (persona == null)
                {
                    return NotFound("Persona no encontrada");
                }

                return Ok(persona);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST api/<PersonasController>
        [HttpPost]
        public IActionResult Post([FromBody] Persona persona)
        {
            try
            {
                if (persona == null)
                {
                    return BadRequest("La solicitud no contiene una persona válida.");
                }

                _bl.Insert(persona);

                return CreatedAtAction(nameof(Get), new { documento = persona.Documento }, persona);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT api/<PersonasController>/5
        [HttpPut("{documento}")]
        public IActionResult Put(string documento, [FromBody] Persona persona)
        {
            try
            {
                if (persona == null || persona.Documento != documento)
                {
                    return BadRequest("La solicitud no contiene una persona válida.");
                }

                _bl.Update(persona);

                return Ok(persona);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE api/<PersonasController>/5
        [HttpDelete("{documento}")]
        public IActionResult Delete(string documento)
        {
            try
            {
                _bl.Delete(documento);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
