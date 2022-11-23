using FitUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Repository.Interface
{
    public interface ITrainingRepository
    {
        IEnumerable<Training> GetAll();
        Training Get(Guid? id);
        void Insert(Training entity);
        void Update(Training entity);
        void Delete(Training entity);
    }
}
