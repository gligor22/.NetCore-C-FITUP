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
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [Authorize]
        // GET: Clients
        public  IActionResult Index()
        {
            return View(_clientService.getAll());
        }

        [Authorize]
        // GET: Clients/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _clientService.GetDetails(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [Authorize]
        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,Email,Age,Gender,startDay,id")] Client client)
        {
            if (ModelState.IsValid)
            {
                var resulut = _clientService.findByEmail(client.Email);
                if (resulut == null)
                {
                    client.id = Guid.NewGuid();
                    _clientService.CreateNew(client);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _clientService.GetDetails(id.Value);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("FirstName,LastName,Email,Age,Gender,startDay,id")] Client client)
        {
            if (id != client.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _clientService.Update(client);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.id))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _clientService.GetDetails(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _clientService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(Guid id)
        {
            return _clientService.GetDetails(id)!=null ? true : false;
        }
    }
}
