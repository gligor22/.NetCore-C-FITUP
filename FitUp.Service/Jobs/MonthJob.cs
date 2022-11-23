﻿using FitUp.Domain.Models;
using FitUp.Repository.Interface;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitUp.Service.Jobs
{
    public class MonthJob : IJob
    {
        private readonly IRepository<Coach> _repo;

        public MonthJob(IRepository<Coach> repo)
        {
            _repo=repo;
        }

        public Task Execute(IJobExecutionContext context)
        {
            List<Coach> coaches = _repo.GetAll().ToList();
            foreach (Coach coach in coaches)
            {
                coach.Trainings_Monthly = 0;
                _repo.Update(coach);
            }
            return Task.CompletedTask;
        }
    }
}
