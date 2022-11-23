using FitUp.Domain.Models;
using FitUp.Repository.Interface;
using FitUp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitUp.Service.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void clientRemoveReservation(Client c)
        {
            c.enteries = c.enteries - 1;
            _clientRepository.Update(c);
        }

        public void clientReserve(Client c)
        {
            c.enteries = c.enteries + 1;
            _clientRepository.Update(c);
        }

        public void CreateNew(Client c)
        {
            _clientRepository.Insert(c);
        }

        public void deactive(Client c)
        {
            c.active = false;
            _clientRepository.Update(c);
        }

        public void Delete(Guid id)
        {
            var c = _clientRepository.Get(id);
            if(c != null)
                _clientRepository.Delete(c);
        }

        public List<Client> getAll()
        {
             return _clientRepository.GetAll().ToList();
        }

        public Client GetDetails(Guid? id)
        {
            return _clientRepository.Get(id);
        }

        public void newPayment(Client c,DateTime date,int allowed)
        {
            c.day_last_payment = date;
            c.allowed_times = allowed;
            c.active = true;
            _clientRepository.Update(c);
        }

        public void Update(Client c)
        {
            _clientRepository.Update(c);
        }

        public Client findByEmail(string email)
        {
            return _clientRepository.findByEmail(email);
        }
    }
}
