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
    public class EtudiantsController : Controller
    {
        private readonly PAEL_V2Context _context;

        public EtudiantsController(PAEL_V2Context context)
        {
            _context = context;
        }

        // GET: Etudiants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Etudiant.ToListAsync());
        }

        // GET: Etudiants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etudiant = await _context.Etudiant
                .FirstOrDefaultAsync(m => m.EtudiantId == id);
            if (etudiant == null)
            {
                return NotFound();
            }

            return View(etudiant);
        }

        // GET: Etudiants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Etudiants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EtudiantId,Nom,Prenom,Email,DateInscription")] Etudiant etudiant)
        {
            // Vérifie si le modèle est valide
           // if (ModelState.IsValid)
            //{
                // Ajoute l'étudiant au contexte
                _context.Add(etudiant);
                // Sauvegarde les changements dans la base de données
                await _context.SaveChangesAsync();
                // Redirige vers la liste des étudiants
                return RedirectToAction(nameof(Index));
            //}

            // Si le modèle n'est pas valide, retourne la vue avec les données saisies
            return View(etudiant);
        }


        // GET: Etudiants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etudiant = await _context.Etudiant.FindAsync(id);
            if (etudiant == null)
            {
                return NotFound();
            }
            return View(etudiant);
        }

        // POST: Etudiants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Etudiants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EtudiantId,Nom,Prenom,Email,DateInscription")] Etudiant etudiant)
        {
            if (id != etudiant.EtudiantId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid) // Vérifiez si le modèle est valide ici
            //{
                try
                {
                    _context.Update(etudiant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)); // Redirection vers Index après enregistrement réussi
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EtudiantExists(etudiant.EtudiantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
               // }
            }

            return View(etudiant); // Retourner à la vue si le modèle n'est pas valide
        }

        // GET: Etudiants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etudiant = await _context.Etudiant
                .FirstOrDefaultAsync(m => m.EtudiantId == id);
            if (etudiant == null)
            {
                return NotFound();
            }

            return View(etudiant);
        }

        // POST: Etudiants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var etudiant = await _context.Etudiant.FindAsync(id);
            if (etudiant != null)
            {
                _context.Etudiant.Remove(etudiant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Méthode de recherche d'étudiant par nom, prénom ou e-mail
        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            // Vérifiez si le terme de recherche est vide
            if (string.IsNullOrEmpty(searchTerm))
            {
                return View("Search", new List<Etudiant>());
            }

            // Effectuer la recherche par nom, prénom ou email
            var results = _context.Etudiant
                .Where(e => e.Nom.Contains(searchTerm) ||
                            e.Prenom.Contains(searchTerm) ||
                            e.Email.Contains(searchTerm))
                .ToList();

            return View("Search", results);
        }


        private bool EtudiantExists(int id)
        {
            return _context.Etudiant.Any(e => e.EtudiantId == id);
        }
    }
}
