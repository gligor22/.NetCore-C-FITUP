using FitUp.Domain.DTO;
using FitUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Service.Interfaces
{
    public interface ICoachService
    {
        List<Coach> getAll();
        Coach GetDetails(Guid? id);
        void CreateNew(Coach c);
        void Update(Coach c);
        void Delete(Guid id);
        void rateCoach(rateDTO rateDTO);
        void newSession(Coach c);
        void discardSession(Coach c);

    }
}
