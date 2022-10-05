using EFSchool;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Person>> Get()
        {
            using (var context = new SchoolContext())
            {
                var persons = context.Person.ToList();
                if (persons == null)
                {
                    return NotFound();
                }

                return Ok(persons);

            }
        }

        [HttpGet("{discriminator}")]
        public ActionResult<List<Person>> Get(string discriminator)
        {
            using (var context = new SchoolContext())
            {
                var persons = context.Person.Where(s => s.Discriminator == discriminator).ToList();
                if (persons == null)
                {
                    return NotFound();
                }

                return Ok(persons);

            }
        }

        [HttpPost]
        public ActionResult<List<Person>> Create(Person person)
        {
            try
            {
                // To open a connection to the database
                using (var context = new SchoolContext())
                {
                    // Add data to the particular table
                    context.Person.Add(person);

                    // save the changes
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an issue in saving the person details " + ex.Message);
            }


            return Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            using (var context = new SchoolContext())
            {
                var products = await context.Person.FindAsync(id);

                if (products == null)
                {
                    return NotFound();
                }

                return Ok(products);
            }
        }
    }
}
