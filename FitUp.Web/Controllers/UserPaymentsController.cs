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
using FitUp.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FitUp.Domain.DTO;

namespace FitUp.Web.Controllers
{
    
    public class UserPaymentsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<FitUpApplicationUser> _userManager;

        public UserPaymentsController(IClientService clientService, IPaymentService paymentService, UserManager<FitUpApplicationUser> userManager)
        {
            _clientService = clientService;
            _paymentService = paymentService;
            _userManager = userManager; 
        }

        // GET: UserPayments
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            Client client = _clientService.findByEmail(email);
            var applicationDbContext = _paymentService.getAllbyClient(client);
            return View(applicationDbContext);
        }

        // GET: UserPayments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            Client client = _clientService.findByEmail(email);

            var payment = _paymentService.GetDetails(id);
            if (payment == null || payment.Client!=client)
            {
                return NotFound();
            }

            return View(payment);
        }
        
        // GET: UserPayments/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            var client = _clientService.findByEmail(email);
            List<Client> clients = new List<Client>();
            clients.Add(client);
            ViewData["ClientID"] = new SelectList(clients, "id", "Email");
            return View();
        }

        // POST: UserPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Payed_Price,ClientID")] PaymentDTO2 paymentDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            var clientt = _clientService.findByEmail(email);
            List<Client> clients = new List<Client>();
            clients.Add(clientt);
            if (ModelState.IsValid)
            {
                try
                {
                    Client client = _clientService.GetDetails(paymentDTO.ClientID);
                    if (client != null)
                    {
                        Payment payment = new Payment
                        {
                            id = new Guid(),
                            ClientID = paymentDTO.ClientID,
                            Payed_Price = paymentDTO.Payed_Price,
                            week_number_trainings = 0,
                            payment_date = DateTime.UtcNow,
                            Client = client
                        };
                        _paymentService.CreateNew(payment);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ViewData["ClientID"] = new SelectList(clients, "id", "Email",paymentDTO.ClientID);
                    ModelState.AddModelError("message", ex.Message);
                    return View(paymentDTO);
                }
            }
            ViewData["ClientID"] = new SelectList(clients, "id", "Email", paymentDTO.ClientID);
            return View(paymentDTO);
        }

        
        // GET: UserPayments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            Client client = _clientService.findByEmail(email);

            var payment = _paymentService.GetDetails(id);
            if (payment == null || payment.Client != client)
            {
                return NotFound();
            }
            List<Client> clients = new List<Client>();
            clients.Add(client);
            ViewData["ClientID"] = new SelectList(clients, "id", "Email", payment.ClientID);
            return View(payment);
        }

        // POST: UserPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Payed_Price,week_number_trainings,payment_date,ClientID,id")] Payment payment)
        {

            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            Client client = _clientService.findByEmail(email);
            List<Client> clients = new List<Client>();
            clients.Add(client);


            if (id != payment.id)
            {
                return NotFound();
            }
            payment.Client = _clientService.GetDetails(payment.ClientID);
            if (payment.payment_date != null)
            {
                try
                {
                    _paymentService.Update(payment);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ViewData["ClientID"] = new SelectList(clients, "id", "Email", payment.ClientID);
                    ModelState.AddModelError("message", ex.Message);
                    return View(payment);
                }
            }
            ViewData["ClientID"] = new SelectList(clients, "id", "Email", payment.ClientID);
            return View(payment);
        }

        // GET: UserPayments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            Client client = _clientService.findByEmail(email);

            var payment = _paymentService.GetDetails(id);
            if (payment == null || payment.Client != client)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: UserPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _paymentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(Guid id)
        {
            return _paymentService.GetDetails(id) != null ? true : false;
        }
    }
}
