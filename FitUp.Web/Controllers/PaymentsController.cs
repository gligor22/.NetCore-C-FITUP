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
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace FitUp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IPaymentService _paymentService;

        public PaymentsController( IClientService clientService, IPaymentService paymentService)
        {
            _clientService = clientService;
            _paymentService = paymentService;
        }

        // GET: Payments
       
        public IActionResult Index(int id)
        {
            if (id == 0)
                id = DateTime.UtcNow.Month;
            var applicationDbContext = _paymentService.getAll(id);
            return View(applicationDbContext);
        }

        // GET: Payments/Details/5
       
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = _paymentService.GetDetails(id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        //GET: Payments/CreateInvoice
        
        public FileContentResult CreateInvoice(int id)
        {
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var InvoiceName = "InvoiceMonth-" + id + ".xlsx";

            using(var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Payments - Month-"+id);
                worksheet.Cell(1, 1).Value = "Client Name";
                worksheet.Cell(1, 2).Value = "Client Surname";
                worksheet.Cell(1, 3).Value = "Payment Day";
                worksheet.Cell(1, 4).Value = "Payed Price";

                var payemnts = _paymentService.getAll(id);

                for(int i = 0; i < payemnts.Count; i++)
                {
                    var payment = payemnts[i];
                    worksheet.Cell(i+2, 1).Value = payment.Client.FirstName;
                    worksheet.Cell(i+2, 2).Value = payment.Client.LastName;
                    worksheet.Cell(i+2, 3).Value = payment.payment_date;
                    worksheet.Cell(i+2, 4).Value = payment.Payed_Price;
                }


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,contentType,InvoiceName);
                }
            }
        }

        // GET: Payments/Create
       
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("Payed_Price,payment_date,ClientID")] PaymentDTO paymentDTO)
        {
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
                            payment_date = paymentDTO.payment_date,
                            Client = client
                        };
                        _paymentService.CreateNew(payment);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", paymentDTO.ClientID);
                    ModelState.AddModelError("message", ex.Message);
                    return View(paymentDTO);
                }
            }
            ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", paymentDTO.ClientID);
            return View(paymentDTO);
        }

        // GET: Payments/Edit/5
        
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = _paymentService.GetDetails(id.Value);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", payment.ClientID);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public IActionResult Edit(Guid id, [Bind("Payed_Price,week_number_trainings,payment_date,ClientID,id")] Payment payment)
        {
            if (id != payment.id)
            {
                return NotFound();
            }
            payment.Client = _clientService.GetDetails(payment.ClientID);
            if (payment.Payed_Price!=null && payment.payment_date!=null && payment.Client!=null)
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
                catch(Exception ex)
                {
                    ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", payment.ClientID);
                    ModelState.AddModelError("message", ex.Message);
                    return View(payment);
                }
            }
            ViewData["ClientID"] = new SelectList(_clientService.getAll(), "id", "Email", payment.ClientID);
            return View(payment);
        }

        // GET: Payments/Delete/5
       
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = _paymentService.GetDetails(id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public IActionResult DeleteConfirmed(Guid id)
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
