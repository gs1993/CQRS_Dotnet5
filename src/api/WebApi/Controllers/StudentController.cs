using Logic.Students.Commands;
using Logic.Students.Dtos;
using Logic.Students.Queries;
using Logic.Studentss.Commands;
using Logic.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Utils;

namespace WebApi.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly Dispatcher _dispatcher;

        public StudentController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Get students")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Envelope<IReadOnlyList<StudentDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(Envelope))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(Envelope))]
        public async Task<IActionResult> GetList(string enrolled, int? number)
        {
            var list = await _dispatcher.Dispatch(new GetListQuery(enrolled, number));
            return Ok(list);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Register new student")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(Envelope))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(Envelope))]
        public async Task<IActionResult> Register([FromBody] NewStudentDto dto)
        {
            var result = await _dispatcher.Dispatch(new RegisterCommand(
                dto.Name, dto.Email,
                dto.Course1, dto.Course1Grade,
                dto.Course2, dto.Course2Grade));

            return FromResult(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Unregister student")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(Envelope))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(Envelope))]
        public async Task<IActionResult> Unregister(long id)
        {
            var result = await _dispatcher.Dispatch(new UnregisterCommand(id));

            return FromResult(result);
        }

        [HttpPost("{id}/enrollments")]
        [SwaggerOperation(Summary = "Enroll student to a course")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(Envelope))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(Envelope))]
        public async Task<IActionResult> Enroll(long id, [FromBody] StudentEnrollmentDto dto)
        {
            var result = await _dispatcher.Dispatch(new EnrollCommand(id, dto.Course, dto.Grade));

            return FromResult(result);
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        [SwaggerOperation(Summary = "Transfer student to a course")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(Envelope))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(Envelope))]
        public async Task<IActionResult> Transfer(long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
        {
            var result = await _dispatcher.Dispatch(new TransferCommand(id, enrollmentNumber, dto.Course, dto.Grade));

            return FromResult(result);
        }

        [HttpPost("{id}/enrollments/{enrollmentNumber}/deletion")]
        [SwaggerOperation(Summary = "Disenroll student from a course")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(Envelope))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(Envelope))]
        public async Task<IActionResult> Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
        {
            var result = await _dispatcher.Dispatch(new DisenrollCommand(id, enrollmentNumber, dto.Comment));

            return FromResult(result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Edit student data")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(Envelope))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(Envelope))]
        public async Task<IActionResult> EditPersonalInfo(long id, [FromBody] StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand(id, dto.Name, dto.Email);

            var result = await _dispatcher.Dispatch(command);

            return FromResult(result);
        }
    }
}
