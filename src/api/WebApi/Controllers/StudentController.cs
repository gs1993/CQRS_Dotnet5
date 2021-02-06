using Logic.AppServices;
using Logic.Commands;
using Logic.Dtos;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetListAsync(string enrolled, int? number)
        {
            var list = await _dispatcher.Dispatch(new GetListQuery(enrolled, number));
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] NewStudentDto dto)
        {
            var result = await _dispatcher.Dispatch(new RegisterCommand(
                dto.Name, dto.Email,
                dto.Course1, dto.Course1Grade,
                dto.Course2, dto.Course2Grade));

            return FromResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> UnregisterAsync(long id)
        {
            var result = await _dispatcher.Dispatch(new UnregisterCommand(id));

            return FromResult(result);
        }

        [HttpPost("{id}/enrollments")]
        public async Task<IActionResult> EnrollAsync(long id, [FromBody] StudentEnrollmentDto dto)
        {
            var result = await _dispatcher.Dispatch(new EnrollCommand(id, dto.Course, dto.Grade));

            return FromResult(result);
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        public async Task<IActionResult> TransferAsync(long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
        {
            var result = await _dispatcher.Dispatch(new TransferCommand(id, enrollmentNumber, dto.Course, dto.Grade));

            return FromResult(result);
        }

        [HttpPost("{id}/enrollments/{enrollmentNumber}/deletion")]
        public async Task<IActionResult> DisenrollAsync(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
        {
            var result = await _dispatcher.Dispatch(new DisenrollCommand(id, enrollmentNumber, dto.Comment));

            return FromResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPersonalInfoAsync(long id, [FromBody] StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand(id, dto.Name, dto.Email);

            var result = await _dispatcher.Dispatch(command);

            return FromResult(result);
        }
    }
}
