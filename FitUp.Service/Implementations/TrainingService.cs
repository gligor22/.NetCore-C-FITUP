using FitUp.Domain.Models;
using FitUp.Repository.Interface;
using FitUp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitUp.Service.Implementations
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;

        public TrainingService(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }

        public void CreateNew(Training c)
        {
            _trainingRepository.Insert(c);
        }

        public void Delete(Guid id)
        {
            var c = _trainingRepository.Get(id);
            if (c != null)
                _trainingRepository.Delete(c);
        }

        public List<Training> getAll()
        {
            return _trainingRepository.GetAll().ToList();
        }

        public Training GetDetails(Guid? id)
        {
            return _trainingRepository.Get(id);
        }

        public void Update(Training c)
        {
            _trainingRepository.Update(c);
        }
    }
}
