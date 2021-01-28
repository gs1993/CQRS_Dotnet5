using Logic.AppServices;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class StudentController : Controller
    {
        private readonly Messages _messages;

        public StudentController(Messages messages)
        {
            _messages = messages;
        }


        public async Task<IActionResult> Index(string enrolled, int? number)
        {
            var list = await _messages.DispatchAsync(new GetListQuery(enrolled, number));
            return View(list);
        }
    }
}
