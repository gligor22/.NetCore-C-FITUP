using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitUp.Domain.Models;
using FitUp.Repository.Data;
using FitUp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FitUp.Domain.Identity;
using System.Linq;

namespace FitUp.Web.Controllers
{
   [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITrainingService _trainingService;
        private readonly IClientService _clientService;
        private readonly UserManager<FitUpApplicationUser> _userManager;

        public ReservationsController(UserManager<FitUpApplicationUser> userManager,IReservationService reservationService, ITrainingService trainingService, IClientService clientService)
        {
                _reservationService = reservationService;
                _trainingService = trainingService;
                _clientService = clientService;
                _userManager = userManager;
        }

        // GET: Reservations
       
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            bool inrole = User.IsInRole("Admin");
            if(inrole)
            {
                var applicationDbContext = _reservationService.getAll();
                return View(applicationDbContext);
            }
            else
            {
                var applicationDbContext = _reservationService.GetAllByUser(email);
                return View(applicationDbContext);
            }
        }

        // GET: Reservations/Details/5
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetDetails(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }


        // GET: Reservations/Create
        
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            List<Client> clients = new List<Client>();
            clients.Add(_clientService.findByEmail(email));
            bool inrole = User.IsInRole("Admin");
            if (inrole == true)
            {
                ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email");
            }
            else
            {
                ViewData["ClientID"] = new SelectList(clients, "id", "Email");
            }
            ViewData["TrainingID"] = new SelectList(_trainingService.getAll(), "id", "type");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("TrainingID,ClientID,id")] Reservation reservation)
        {
            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            List<Client> clients = new List<Client>();
            clients.Add(_clientService.findByEmail(email));
            bool inrole = User.IsInRole("Admin");

            if (ModelState.IsValid)
            {
                reservation.id = Guid.NewGuid();
                try {
                    _reservationService.CreateNew(reservation);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e){
                    if(inrole == true)
                    {
                        ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", reservation.ClientID);
                    }
                    else
                    {
                        ViewData["ClientID"] = new SelectList(clients, "id", "Email", reservation.ClientID);
                    }
                    ViewData["TrainingID"] = new SelectList(_trainingService.getAll(), "id", "type", reservation.TrainingID);
                    ModelState.AddModelError("message", e.Message);
                    return View(reservation);
                }
            }
            if (inrole == true)
            {
                ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", reservation.ClientID);
            }
            else
            {
                ViewData["ClientID"] = new SelectList(clients, "id", "Email", reservation.ClientID);
            }
            ViewData["TrainingID"] = new SelectList(_trainingService.getAll(), "id", "type", reservation.TrainingID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
       
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetDetails(id);
            if (reservation == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            List<Client> clients = new List<Client>();
            clients.Add(_clientService.findByEmail(email));
            bool inrole = User.IsInRole("Admin");

            if (inrole == true)
            {
                ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", reservation.ClientID);
            }
            else
            {
                ViewData["ClientID"] = new SelectList(clients, "id", "Email", reservation.ClientID);
            }
            ViewData["TrainingID"] = new SelectList(_trainingService.getAll(), "id", "type", reservation.TrainingID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(Guid id, [Bind("TrainingID,ClientID,id")] Reservation reservation)
        {
            if (id != reservation.id)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            List<Client> clients = new List<Client>();
            clients.Add(_clientService.findByEmail(email));
            bool inrole = User.IsInRole("Admin");

            if (ModelState.IsValid)
            {
                try
                {
                    _reservationService.Update(reservation);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch(Exception ex)
                {
                    if (inrole == true)
                    {
                        ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", reservation.ClientID);
                    }
                    else
                    {
                        ViewData["ClientID"] = new SelectList(clients, "id", "Email", reservation.ClientID);
                    }
                    ViewData["TrainingID"] = new SelectList(_trainingService.getAll(), "id", "type", reservation.TrainingID);
                    ModelState.AddModelError("message", ex.Message);
                    return View(reservation);
                }
            }
            if (inrole == true)
            {
                ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", reservation.ClientID);
            }
            else
            {
                ViewData["ClientID"] = new SelectList(clients, "id", "Email", reservation.ClientID);
            }
            ViewData["TrainingID"] = new SelectList(_trainingService.getAll(), "id", "type", reservation.TrainingID);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
       
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetDetails(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
           _reservationService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        
        private bool ReservationExists(Guid id)
        {
            return _reservationService.GetDetails(id) != null ? true : false;
        }
    }
}
