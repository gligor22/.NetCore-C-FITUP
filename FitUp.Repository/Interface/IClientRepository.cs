using FitUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Repository.Interface
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAll();
        Client Get(Guid? id);
        void Insert(Client entity);
        void Update(Client entity);
        void Delete(Client entity);

        Client findByEmail(string email);
    }
}
