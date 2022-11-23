using FitUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Service.Interfaces
{
    public interface ITrainingService
    {
        List<Training> getAll();
        Training GetDetails(Guid? id);
        void CreateNew(Training c);
        void Update(Training c);
        void Delete(Guid id);
    }
}
