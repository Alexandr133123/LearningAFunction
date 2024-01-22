using DocSearch.Docs.DataAccess.Data;
using DocSearch.Docs.DataAccess.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DocSearch.Docs.API.Functions
{
    public class DoctorsApi
    {
        private readonly DoctorsDbContext _dbContext;
        private readonly ILogger<DoctorsApi> _logger;

        public DoctorsApi(DoctorsDbContext dbContext, ILogger<DoctorsApi> logger) 
        {
            _dbContext = dbContext;
            _logger = logger; 
        }
        

        [FunctionName("GetDoctors")]
        [OpenApiOperation(operationId: "GetDoctors", tags: new[] { "Doctors" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(List<Doctor>), Description = "The List of Doctor Models")]
        public async Task<IActionResult> GetDoctors(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request)
        {

            var doctors = await _dbContext.Doctors.ToListAsync();

            JsonConvert.SerializeObject(doctors, Formatting.Indented);

            return new OkObjectResult(doctors);
        }

        [FunctionName("GetDoctorById")]
        [OpenApiOperation(operationId: "GetDoctorById", tags: new[] { "Doctors" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Doctor), Description = "The Doctor Model")]
        public async Task<IActionResult> GetDoctorById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetDoctorById/{id:int}")] HttpRequest request,
            int id)
        {
            var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
            {
                _logger.LogError($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - Doctor with id ({id}) not found.");

                return new BadRequestResult();
            }

            return new OkObjectResult(doctor);
        }

        [FunctionName("CreateDoctor")]
        [OpenApiOperation(operationId: "CreateDoctor", tags: new[] { "Doctors" })]
        [OpenApiRequestBody("text/plain", typeof(Doctor), Required = true, Example = typeof(Doctor))]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK, Description = "Doctor Successfully Created")]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "Failed to Create The Doctor")]
        public async Task<IActionResult> CreateDoctor(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request)
        {

            Doctor doctor = JsonConvert.DeserializeObject<Doctor>(await new StreamReader(request.Body).ReadToEndAsync());

            _dbContext.Add(doctor);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - New doctor with name: ({doctor.Name}) was added.");

            return new OkResult();
        }

        [FunctionName("DeleteDoctorById")]
        [OpenApiOperation(operationId: "DeleteDoctorById", tags: new[] { "Doctors" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK, Description = "Doctor Successfully Deleted")]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "Failed to Delete The Doctor")]
        public async Task<IActionResult> DeleteDoctorById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteDoctorById/{id:int}")] HttpRequest request,
            int id)
        {
            var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
            {
                _logger.LogError($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - Doctor wasn't deleted. Doctor with id ({id}) not found.");

                return new BadRequestResult();
            }

            _dbContext.Remove(doctor);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - Doctor with name ({doctor.Name}) was deleted");

            return new OkResult();
        }

        [FunctionName("UpdateDoctor")]
        [OpenApiOperation(operationId: "UpdateDoctor", tags: new[] { "Doctors" })]
        [OpenApiRequestBody("text/plain", typeof(Doctor), Required = true, Example = typeof(Doctor))]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK, Description = "Doctor Successfully Updated")]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "Failed to Update The Doctor")]
        public async Task<IActionResult> UpdateDoctor(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request)
        {
            var updatedDoctor = JsonConvert.DeserializeObject<Doctor>(await new StreamReader(request.Body).ReadToEndAsync());

            _dbContext.Attach(updatedDoctor);
            _dbContext.Entry(updatedDoctor).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - Doctor with id ({updatedDoctor.Id}) was updated");

            return new OkResult();
        }        

    }
}
