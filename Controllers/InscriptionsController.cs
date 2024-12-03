using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PAEL_V2.Data;
using PAEL_V2.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;



public class InscriptionsController : Controller
{
    private readonly string _connectionString;

    public InscriptionsController(PAEL_V2Context context, IConfiguration configuration)
    {
       // _context = context;
        _connectionString = configuration.GetConnectionString("PAEL_V2Context");
    }


    // GET: Inscriptions
    public async Task<IActionResult> Index()
    {
        var inscriptions = new List<Inscription>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("GetAllInscriptions", connection) { CommandType = CommandType.StoredProcedure })
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    inscriptions.Add(new Inscription
                    {
                        InscriptionId = (int)reader["InscriptionId"],
                        FormationNom = reader["FormationNom"].ToString(), 
                        EtudiantNom = reader["EtudiantNom"].ToString(),   
                        DateInscription = (DateTime)reader["DateInscription"]
                    });
                }
            }
        }

        return View(inscriptions);
    }


    // GET: Inscriptions/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Inscription inscription = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("GetInscriptionById", connection) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        inscription = new Inscription
                        {
                            InscriptionId = (int)reader["InscriptionId"],
                            // Assurez-vous que les noms des colonnes correspondent à ceux de la base de données
                            FormationNom = reader["FormationNom"] != DBNull.Value ? reader["FormationNom"].ToString() : string.Empty,
                            EtudiantNom = reader["EtudiantNom"] != DBNull.Value ? reader["EtudiantNom"].ToString() : string.Empty,
                            DateInscription = (DateTime)reader["DateInscription"]
                        };
                    }
                }
            }
        }

        if (inscription == null)
        {
            return NotFound();
        }

        return View(inscription);
    }


    // GET: Inscriptions/Create
    public IActionResult Create()
    {
        ViewData["EtudiantId"] = GetEtudiants();
        ViewData["FormationId"] = GetFormations();
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("InscriptionId,FormationId,EtudiantId,DateInscription")] Inscription inscription)
    {
        //if (ModelState.IsValid)
        //{
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Retrieve FormationNom based on FormationId
                string formationNom = string.Empty;
                using (var formationCommand = new SqlCommand("GetFormationNom", connection) { CommandType = CommandType.StoredProcedure })
                {
                    formationCommand.Parameters.AddWithValue("@FormationId", inscription.FormationId);
                    var result = await formationCommand.ExecuteScalarAsync();
                    if (result != null)
                    {
                        formationNom = result.ToString();
                    }
                }

                // Retrieve EtudiantNom and EtudiantPrenom based on EtudiantId
                string etudiantNom = string.Empty;
                string etudiantPrenom = string.Empty;
                using (var etudiantCommand = new SqlCommand("GetEtudiantNom", connection) { CommandType = CommandType.StoredProcedure })
                {
                    etudiantCommand.Parameters.AddWithValue("@EtudiantId", inscription.EtudiantId);
                    using (var reader = await etudiantCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            etudiantNom = reader["Nom"].ToString();
                            etudiantPrenom = reader["Prenom"].ToString();
                        }
                    }
                }

                using (var command = new SqlCommand("AddInscription", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@FormationId", inscription.FormationId);
                    command.Parameters.AddWithValue("@EtudiantId", inscription.EtudiantId);
                    command.Parameters.AddWithValue("@DateInscription", inscription.DateInscription);
                    command.Parameters.AddWithValue("@FormationNom", formationNom);
                    command.Parameters.AddWithValue("@EtudiantNom", etudiantNom);
                    command.Parameters.AddWithValue("@EtudiantPrenom", etudiantPrenom);

                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            //}
        }

        ViewData["EtudiantId"] = GetEtudiants();
        ViewData["FormationId"] = GetFormations();
        return View(inscription);
    }





    // GET: Inscriptions/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Inscription inscription = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("GetInscriptionById", connection) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        inscription = new Inscription
                        {
                            InscriptionId = (int)reader["InscriptionId"],
                            FormationId = (int)reader["FormationId"],
                            EtudiantId = (int)reader["EtudiantId"],
                            DateInscription = (DateTime)reader["DateInscription"]
                        };
                    }
                }
            }
        }

        if (inscription == null)
        {
            return NotFound();
        }

        ViewData["EtudiantId"] = GetEtudiants();
        ViewData["FormationId"] = GetFormations();
        return View(inscription);
    }

    // POST: Inscriptions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("InscriptionId,FormationId,EtudiantId,DateInscription")] Inscription inscription)
    {
        if (id != inscription.InscriptionId)
        {
            return NotFound();
        }

        //if (ModelState.IsValid)
        //{
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("UpdateInscription", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@InscriptionId", inscription.InscriptionId);
                    command.Parameters.AddWithValue("@FormationId", inscription.FormationId);
                    command.Parameters.AddWithValue("@EtudiantId", inscription.EtudiantId);
                    command.Parameters.AddWithValue("@DateInscription", inscription.DateInscription);
                    await command.ExecuteNonQueryAsync();
                }
            //}
            return RedirectToAction(nameof(Index));
        }

        ViewData["EtudiantId"] = GetEtudiants();
        ViewData["FormationId"] = GetFormations();
        return View(inscription);
    }

    // GET: Inscriptions/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Inscription inscription = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("GetInscriptionById", connection) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        inscription = new Inscription
                        {
                            InscriptionId = (int)reader["InscriptionId"],
                            FormationNom = reader["FormationNom"].ToString(),
                            EtudiantNom = reader["EtudiantNom"].ToString(),
                            DateInscription = (DateTime)reader["DateInscription"]
                        };
                    }
                }
            }
        }

        if (inscription == null)
        {
            return NotFound();
        }

        return View(inscription);
    }

    // POST: Inscriptions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("DeleteInscription", connection) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@id", id);
                await command.ExecuteNonQueryAsync();
            }
        }

        return RedirectToAction(nameof(Index));
    }

    private bool InscriptionExists(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("GetInscriptionById", connection) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@id", id);
                return command.ExecuteScalar() != null;
            }
        }
    }

    private IEnumerable<SelectListItem> GetEtudiants()
    {
        var etudiants = new List<SelectListItem>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("GetEtudiants", connection) { CommandType = CommandType.StoredProcedure })
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    etudiants.Add(new SelectListItem
                    {
                        Value = reader["EtudiantId"].ToString(),
                        Text = reader["Nom"].ToString() + " " + reader["Prenom"].ToString()
                    });
                }
            }
        }

        return etudiants;
    }

    private IEnumerable<SelectListItem> GetFormations()
    {
        var formations = new List<SelectListItem>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("GetFormations", connection) { CommandType = CommandType.StoredProcedure })
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    formations.Add(new SelectListItem
                    {
                        Value = reader["FormationId"].ToString(),
                        Text = reader["Nom"].ToString()
                    });
                }
            }
        }

        return formations;
    }

}
