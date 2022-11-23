using FitUp.Domain.DTO;
using FitUp.Domain.Models;
using FitUp.Repository.Interface;
using FitUp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitUp.Service.Implementations
{
    public class CoachService : ICoachService
    {
        private readonly IRepository<Coach> _coachRepository;

        public CoachService(IRepository<Coach> coachRepository)
        {
            _coachRepository= coachRepository;
        }

        public void CreateNew(Coach c)
        {
            _coachRepository.Insert(c);
        }

        public void Delete(Guid id)
        {
            var c = _coachRepository.Get(id);
            if (c != null)
                _coachRepository.Delete(c);
        }

        public void discardSession(Coach c)
        {
            c.All_training_num -= 1;
            c.Trainings_Weekly -= 1;
            c.Trainings_Monthly -= 1;
            c.Trainings_Yearly -= 1;
            _coachRepository.Update(c);
        }

        public List<Coach> getAll()
        {
            return _coachRepository.GetAll().ToList();
        }

        public Coach GetDetails(Guid? id)
        {
            return _coachRepository.Get(id);
        }

        public void newSession(Coach c)
        {
            c.All_training_num += 1;
            c.Trainings_Weekly += 1;
            c.Trainings_Monthly += 1;
            c.Trainings_Yearly += 1;
            _coachRepository.Update(c);
        }

        public void rateCoach(rateDTO rateDTO)
        {
            var num = rateDTO.number;
            Coach c = _coachRepository.Get(rateDTO.CoachID);
            var s1 = c.Rate * c.num_raters;
            var s2 = s1 + num;
            var s3 = s2 / (c.num_raters + 1);
            c.Rate = s3;
            c.num_raters += 1;
            _coachRepository.Update(c);
        }

        public void Update(Coach c)
        {
            _coachRepository.Update(c);
        }
    }
}

