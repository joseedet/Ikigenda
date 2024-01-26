using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ikigenda.Models;

namespace Ikigenda.Data
{
    public class SeedDb
    {
        private readonly AgendaDbContext _dbContext;

        public SeedDb(AgendaDbContext dbContext)
        {

            _dbContext=dbContext;
            


        }

        public async Task SeedAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            //await InsertUserAsync();
            await InsertTipoServiciosAsync();
            await InsertTipoDocumentosAsync();
            await InsertRolsAsync();
            await InsertUsuariosAsync();
            await InsertPlaninsAsync();
            await InsertClientesAsync();
            await InsertHistorialsAsync();
        }

        private async Task InsertHistorialsAsync()
        {
            var historial=new Historial{
                PlaninId=1,
                ClienteId=1,
                TipoServicioId=1,
                Descripcion="Constitución de una empresa en Santander",
                Observaciones="Es una SL cuya actividad es de venta de aparatos electrónicos"

            };
            _dbContext.Add(historial);
            await _dbContext.SaveChangesAsync();
            //throw new NotImplementedException();
        }

        private async Task InsertClientesAsync()
        {
            var cliente = new Cliente
            {
                TipoDocumentoId = 1,
                Documento = "X0999769M",
                Nombre = "José",
                Apellidos = "Edet Yake",
                Telefono = "692444968"
            };
            _dbContext.Add(cliente);
            await _dbContext.SaveChangesAsync();
            //throw new NotImplementedException();
        }

        private async Task InsertUsuariosAsync()
        {
            var usuario = new Usuario
            {
                Nombre="José",
                Correo="fulanito@gmail.com",
                NombreUsuario="fulanito@gmail.com",
                Contrasenya="pepito.123",
                Activo=1,
                RolId=1
            };
            _dbContext.Add(usuario);
            await _dbContext.SaveChangesAsync();
           
        }

        private async Task InsertRolsAsync()
        {
             var tipoDocumento=new TipoServicio{
                Nombre="Administrador"
            };
                _dbContext.Add(tipoDocumento);
                 await _dbContext.SaveChangesAsync();
           
        }

        private async Task InsertTipoDocumentosAsync()
        {

            var tipoDocumento=new TipoServicio{
                    Nombre="DNI"
            };
                _dbContext.Add(tipoDocumento);
                await _dbContext.SaveChangesAsync();
           
        }

        private async Task InsertTipoServiciosAsync()
        {

            var tipoServicio=new TipoServicio{
                    Nombre="Constitución Empresas"
            };
            _dbContext.Add(tipoServicio);
            await _dbContext.SaveChangesAsync();
            //throw new NotImplementedException();
        }

        private async Task InsertPlaninsAsync()
        {
            if (!_dbContext.Planins.Any())
            {
                var initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                var finalDate = initialDate.AddYears(1);
                while (initialDate < finalDate)
                {
                    if (initialDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var finalDate2 = initialDate.AddHours(10);
                        while (initialDate < finalDate2)
                        {
                            _dbContext.Planins.Add(new Planin
                            {
                                FechaLocal = initialDate,
                                Disponible = 1
                            });

                            initialDate = initialDate.AddMinutes(30);
                        }

                        initialDate = initialDate.AddHours(14);
                    }
                    else
                    {
                        initialDate = initialDate.AddDays(1);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}