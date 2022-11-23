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

namespace FitUp.Web.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly ITrainingService _trainingService; 
        private readonly ICoachService _coachService;   

        public TrainingsController(ITrainingService trainingService, ICoachService coachService)
        {
            _trainingService = trainingService;
            _coachService = coachService;
        }

        // GET: Trainings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _trainingService.getAll();
            return View( applicationDbContext);
        }

        // GET: Trainings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = _trainingService.GetDetails(id);
                
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // GET: Trainings/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CoachID"] = new SelectList(_coachService.getAll(), "id", "Email");
            return View();
        }

        // POST: Trainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("type,duration,dateTime,capacity,CoachID,id")] Training training)
        {
            if (ModelState.IsValid)
            {
                _trainingService.CreateNew(training);   
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoachID"] = new SelectList(_coachService.getAll(), "id", "Email", training.CoachID);
            return View(training);
        }

        // GET: Trainings/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = _trainingService.GetDetails(id);
            if (training == null)
            {
                return NotFound();
            }
            ViewData["CoachID"] = new SelectList(_coachService.getAll(), "id", "Email", training.CoachID);
            return View(training);
        }

        // POST: Trainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("type,duration,dateTime,capacity,CoachID,id")] Training training)
        {
            if (id != training.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _trainingService.Update(training);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoachID"] = new SelectList(_coachService.getAll(), "id", "Email", training.CoachID);
            return View(training);
        }

        // GET: Trainings/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training =_trainingService.GetDetails(id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _trainingService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool TrainingExists(Guid id)
        {
            return _trainingService.GetDetails(id) != null ? true : false;
        }
    }
}
