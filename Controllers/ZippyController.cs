using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zippy.Controllers.Contract;
using Zippy.Models;
using Zippy.Services.Contract;
using Zippy.Utils;

namespace Zippy.Controllers
{
    [Route("zippy")]
    public class ZippyController : Controller
    {
        private readonly ILogger<ZippyController> logger;
        private readonly IZCoreService coreService;

        public ZippyController(IZCoreService locationService, ILoggerFactory factory)
        {
            this.coreService = locationService;
            this.logger = factory.CreateLogger<ZippyController>();
        }

        [HttpGet]
        [Route("index")]
        [Route("/")]
        [Route("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        [Route("persons")]
        public async Task<IActionResult> FindPersons([FromQuery] string zip)
        {
            logger.LogInformation($"FindPersons({zip}) request received  ...");
            var persons = await coreService.FindPersons(zip);
            var response = new ZResponse(persons);
            return Json(response, Helpers.DefaultJsonSettings);
        }

        [HttpGet]
        [Route("locate")]
        public async Task<IActionResult> Locate([FromQuery] string name, [FromQuery] string address)
        {
            logger.LogInformation($"Locate({name}, {address}) request received  ...");
            var person = await LocatePerson(name, address);
            var response = new ZResponse(person);
            return Json(response, Helpers.DefaultJsonSettings);
        }

        [HttpGet]
        [Route("person/{name}")]
        public async Task<IActionResult> FindPerson(string name)
        {
            logger.LogInformation($"Locate({name}) request received  ...");
            var person = await coreService.FindPerson(name);
            Throw.IfNull(person, $"Person not found: {name}");
            return Json(new ZResponse(person), Helpers.DefaultJsonSettings);
        }

        [HttpPost]
        [Route("locate")]
        public async Task<IActionResult> Locate([FromBody] IList<LocateRequest> requests)
        {
            logger.LogInformation($"Received (POST) LocateUser requests ({requests.Count}) ...");

            var payloads = new List<object>();

            for (int index = 0; index < requests.Count; ++index)
            {
                var request = requests[index];

                if (request == null)
                {
                    logger.LogInformation($"Skip processing null request @ {index}");
                    continue;
                }

                try
                {
                    logger.LogInformation($"Processing request@{index}: ({request})");

                    var person = await LocatePerson(request?.Name, request?.Address);

                    payloads.Add(new
                    {
                        Name = request.Name,
                        Person = person
                    });
                }
                catch (Exception ex)
                {
                    payloads.Add(new
                    {
                        Name = request.Name,
                        Person = (Person)null,
                        Error = ex.Message // Ideally, this message should be a user-facing message. Throw/handle custom exceptions!
                    });
                }
            }

            return Json(
               new ZResponse(payloads),
               Helpers.DefaultJsonSettings
            );
        }

        private async Task<Person> LocatePerson(string name, string address)
        {
            var person = await coreService.LocatePerson(name, address);
            Throw.IfNull(person, $"Could not locate address ({name} => {address})");
            return person;
        }
    }
}
