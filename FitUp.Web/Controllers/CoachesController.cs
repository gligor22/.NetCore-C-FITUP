using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitUp.Domain.Models;
using FitUp.Repository.Data;
using FitUp.Domain.DTO;
using FitUp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FitUp.Web.Controllers
{
    public class CoachesController : Controller
    {
        private readonly ICoachService _coachService;

        public CoachesController(ICoachService coachService)
        {
            _coachService = coachService;
        }

        // GET: Coaches
        public IActionResult Index()
        {
            return View(_coachService.getAll());
        }
        // GET: Coaches/Rate
        [Authorize]
        public IActionResult Rate()
        {
            ViewData["CoachesID"] = new SelectList(_coachService.getAll(), "id", "Email");
            return View();
        }
        // POST: Coaches/Rate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Rate([Bind("CoachID,number")] rateDTO rateDTO)
        {
            if (ModelState.IsValid)
            {
                _coachService.rateCoach(rateDTO);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoachesID"] = new SelectList(_coachService.getAll(), "id", "Email");
            return View(rateDTO);
        }

        // GET: Coaches/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = _coachService.GetDetails(id);   
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // GET: Coaches/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("FirstName,LastName,Email,Age,Gender")] CoachDTO coachDTO)
        {
            
            if (ModelState.IsValid)
            {
                Coach coach = new Coach
                {
                    FirstName = coachDTO.FirstName,
                    LastName = coachDTO.LastName,
                    Email = coachDTO.Email,
                    Age = coachDTO.Age,
                    Gender = coachDTO.Gender,
                    entryDay= DateTime.UtcNow,
                    Rate=1,
                    num_raters=0,
                    Trainings_Weekly=0,
                    Trainings_Monthly=0,
                    Trainings_Yearly=0,
                    All_training_num=0
                };
                coach.id = Guid.NewGuid();
                _coachService.CreateNew(coach);
                return RedirectToAction(nameof(Index));
            }
            return View(coachDTO);
        }

        // GET: Coaches/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = _coachService.GetDetails(id);
            if (coach == null)
            {
                return NotFound();
            }
            CoachDTO dTO = new CoachDTO
            {
                ID = coach.id,
                FirstName = coach.FirstName,
                LastName= coach.LastName,
                Email= coach.Email,
                Gender = coach.Gender,
                Age = coach.Age,
            };
            return View(dTO);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, [Bind("FirstName,LastName,Email,Age,Gender,ID")] CoachDTO coachDTO)
        {
            if (id != coachDTO.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Coach old = _coachService.GetDetails(coachDTO.ID);
                    old.FirstName = coachDTO.FirstName;
                    old.LastName = coachDTO.LastName;
                    old.Age= coachDTO.Age;
                    old.Email= coachDTO.Email;
                    old.Gender= coachDTO.Gender;
                    _coachService.Update(old);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoachExists(coachDTO.ID))
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
            return View(coachDTO);
        }

        // GET: Coaches/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = _coachService.GetDetails(id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _coachService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        private bool CoachExists(Guid id)
        {
            return _coachService.GetDetails(id) != null ? true : false;
        }
    }
}
