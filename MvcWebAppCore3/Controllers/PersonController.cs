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
        public ActionResult Create()
        {
            return View();
        }

        // Specify the type of attribute i.e.
        // it will add the record to the database

        [HttpPost]
        public async Task<ActionResult> Create(Person person)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    if(person.Discriminator == "Student")
                    {
                        person.EnrollmentDate = DateTime.Now;
                    }
                    else
                    {
                        person.HireDate = DateTime.Now;
                    }
                    client.BaseAddress = new Uri("http://localhost:44340/api/person");
                    var personString = JsonConvert.SerializeObject(person);
                    var content = new StringContent(personString);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var result = await client.PostAsync("student", content);
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("~/Views/Shared/Error.cshtml");
                    }
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
            IList<Person> staff = new List<Person>();
            using(HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44340/api/Person");
                if (response.IsSuccessStatusCode)
                {
                    var persons =  JsonConvert.DeserializeObject<List<Person>>(await response.Content.ReadAsStringAsync());
                    foreach(var person in persons)
                    {
                        staff.Add(
                            new Person { 
                                FirstName = person.FirstName,
                                LastName = person.LastName
                            });
                    }
                    return View(staff);
                }
                return NotFound();
            }
        }
    }
}
