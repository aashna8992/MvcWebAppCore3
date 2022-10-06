using Microsoft.AspNetCore.Mvc;
using MvcWebAppCore3.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MvcWebAppCore3.Controllers
{
    public class PersonController : Controller
    {
        private readonly HttpClient _personClient;
        private const string Person = "person";
        public PersonController(IHttpClientFactory httpClientFactory)
        {
            _personClient = httpClientFactory.CreateClient("PersonClient");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Person person)
        {
            if (ModelState.IsValid)
            {
                // Make Discrimator enum.
                if (person.Discriminator.ToLower().Equals("Student".ToLower()))
                {
                    person.EnrollmentDate = DateTime.Now;
                }
                else
                {
                    person.HireDate = DateTime.Now;
                }

                var personString = JsonConvert.SerializeObject(person);
                var content = new StringContent(personString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await _personClient.PostAsync(Person, content);
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            else
            {
                return BadRequest("Invalid Model");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _personClient.GetAsync(Person);
            if (response.IsSuccessStatusCode)
            {
                var staff = JsonConvert.DeserializeObject<List<Person>>(await response.Content.ReadAsStringAsync());
                return View(staff);
            }
            return NotFound();
        }
    }
}
