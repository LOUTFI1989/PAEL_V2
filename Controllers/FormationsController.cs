using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAEL_V2.Data;
using PAEL_V2.Models;

namespace PAEL_V2.Controllers
{
    public class FormationsController : Controller
    {
        private readonly PAEL_V2Context _context;

        public FormationsController(PAEL_V2Context context)
        {
            _context = context;
        }

        // GET: Formations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Formation.ToListAsync());
        }

        // GET: Formations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formation = await _context.Formation
                .FirstOrDefaultAsync(m => m.FormationId == id);
            if (formation == null)
            {
                return NotFound();
            }

            return View(formation);
        }

        // GET: Formations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Formations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FormationId,Nom,Description,DateDebut,DateFin")] Formation formation)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(formation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            return View(formation);
        }

        // GET: Formations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formation = await _context.Formation.FindAsync(id);
            if (formation == null)
            {
                return NotFound();
            }
            return View(formation);
        }

        // POST: Formations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FormationId,Nom,Description,DateDebut,DateFin")] Formation formation)
        {
            if (id != formation.FormationId)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(formation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormationExists(formation.FormationId))
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
            return View(formation);
        }

        // GET: Formations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formation = await _context.Formation
                .FirstOrDefaultAsync(m => m.FormationId == id);
            if (formation == null)
            {
                return NotFound();
            }

            return View(formation);
        }

        // POST: Formations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formation = await _context.Formation.FindAsync(id);
            if (formation != null)
            {
                _context.Formation.Remove(formation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Méthode de recherche d'étudiant par nom, prénom ou e-mail utilisant une procédure stockée
        [HttpGet]
        public IActionResult SearchFormation(string searchTerm)
        {
            // Vérifiez si le terme de recherche est vide
            if (string.IsNullOrEmpty(searchTerm))
            {
                return View("SearchFormation", new List<Formation>());
            }

            // Appel de la procédure stockée avec le paramètre de recherche
            var results = _context.Formation
                .FromSqlRaw("EXEC SearchFormation @searchTerm = {0}", searchTerm)
                .ToList();

            return View("SearchFormation", results);
        }


        private bool FormationExists(int id)
        {
            return _context.Formation.Any(e => e.FormationId == id);
        }
    }
}
