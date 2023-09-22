using DataAccessLayer.EFModels;
using DataAccessLayer.IDALs;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.DALs
{
    public class DAL_Vehiculos_EF : IDAL_Vehiculos
    {
        private readonly DBContextCore _dbContext;

        public DAL_Vehiculos_EF(DBContextCore dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<Vehiculo> Get()
        {
            return _dbContext.Vehiculos
                .Select(v => MapToVehiculo(v))
                .ToList();
        }

        public Vehiculo Get(string matricula)
        {
            var vehiculo = _dbContext.Vehiculos.FirstOrDefault(v => v.Matricula == matricula);
            return vehiculo != null ? MapToVehiculo(vehiculo) : null;
        }

        public void Insert(Vehiculo vehiculo, string documentoPersona)
        {
            var persona = _dbContext.Personas.FirstOrDefault(p => p.Documento == documentoPersona);
            if (persona == null)
            {
                throw new InvalidOperationException("La persona con el documento especificado no existe.");
            }

            var nuevoVehiculo = new Vehiculos
            {
                Matricula = vehiculo.Matricula,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Persona = persona
            };

            _dbContext.Vehiculos.Add(nuevoVehiculo);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Manejar excepciones de Entity Framework Core aquí (puedes personalizar mensajes de error)
                throw new InvalidOperationException("Error al insertar el vehículo.", ex);
            }
        }

        public void Update(Vehiculo vehiculo)
        {
            var vehiculoExistente = _dbContext.Vehiculos.FirstOrDefault(v => v.Matricula == vehiculo.Matricula);
            if (vehiculoExistente != null)
            {
                vehiculoExistente.Marca = vehiculo.Marca;
                vehiculoExistente.Modelo = vehiculo.Modelo;

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Manejar excepciones de Entity Framework Core aquí (puedes personalizar mensajes de error)
                    throw new InvalidOperationException("Error al actualizar el vehículo.", ex);
                }
            }
        }

        public void Delete(string matricula)
        {
            var vehiculo = _dbContext.Vehiculos.FirstOrDefault(v => v.Matricula == matricula);
            if (vehiculo != null)
            {
                _dbContext.Vehiculos.Remove(vehiculo);

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Manejar excepciones de Entity Framework Core aquí (puedes personalizar mensajes de error)
                    throw new InvalidOperationException("Error al eliminar el vehículo.", ex);
                }
            }
        }

        private static Vehiculo MapToVehiculo(Vehiculos vehiculoEntity)
        {
            return new Vehiculo
            {
                Matricula = vehiculoEntity.Matricula,
                Marca = vehiculoEntity.Marca,
                Modelo = vehiculoEntity.Modelo
            };
        }
    }
}
