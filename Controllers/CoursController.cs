using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PAEL_V2.Data;
using PAEL_V2.Models;
using Microsoft.Extensions.Configuration;

namespace PAEL_V2.Controllers
{
    public class CoursController : Controller
    {
        private readonly PAEL_V2Context _context;
        private readonly IConfiguration _configuration;

        public CoursController(PAEL_V2Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Cours
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cours.ToListAsync());
        }

        // GET: Cours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cours = await _context.Cours.FirstOrDefaultAsync(m => m.CoursId == id);
            if (cours == null)
            {
                return NotFound();
            }

            return View(cours);
        }

        // GET: Cours/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoursId,Title,Description,StartDate,EndDate")] Cours cours)
        {
           // if (ModelState.IsValid)
            //{
                _context.Add(cours);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            return View(cours);
        }

        // GET: Cours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cours = await _context.Cours.FindAsync(id);
            if (cours == null)
            {
                return NotFound();
            }
            return View(cours);
        }


        // POST: Cours/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CoursId,Title,Description,StartDate,EndDate")] Cours cours)
        {
            if (id != cours.CoursId)
            {
                return NotFound();
            }

           //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(cours);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoursExists(cours.CoursId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
              // }
                return RedirectToAction(nameof(Index));
            }
            return View(cours);
        }

        // GET: Cours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cours = await _context.Cours.FirstOrDefaultAsync(m => m.CoursId == id);
            if (cours == null)
            {
                return NotFound();
            }

            return View(cours);
        }

        // POST: Cours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cours = await _context.Cours.FindAsync(id);
            if (cours != null)
            {
                _context.Cours.Remove(cours);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult SearchCours(string searchTerm)
        {
            List<Cours> results = new List<Cours>();

            if (string.IsNullOrEmpty(searchTerm))
            {
                return View("SearchCours", results);
            }

            // Retrieve the connection string from appsettings.json
            string connectionString = _configuration.GetConnectionString("PAEL_V2Context");

            // Use the connection string to establish a database connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Create the command to execute the stored procedure
                using (SqlCommand cmd = new SqlCommand("SearchCours", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@searchTerm", searchTerm));

                    conn.Open();

                    // Execute the command and retrieve the results
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cours cours = new Cours
                            {
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                CoursId = reader.GetInt32(reader.GetOrdinal("CoursId"))
                            };
                            results.Add(cours);
                        }
                    }
                }
            }

            return View("SearchCours", results);
        }

        private bool CoursExists(int id)
        {
            return _context.Cours.Any(e => e.CoursId == id);
        }
    }
}
