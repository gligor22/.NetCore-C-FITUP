using FitUp.Domain.Models;
using FitUp.Repository.Interface;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitUp.Service.Jobs
{
    public class WeekJob : IJob
    {
        private readonly IClientRepository _clientRepository;
        private readonly IRepository<Coach> _coachRepository;

        public WeekJob(IClientRepository clientRepository, IRepository<Coach> coachRepository)
        {
            _clientRepository = clientRepository;
            _coachRepository = coachRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            List<Client> clients = _clientRepository.GetAll().ToList();
            foreach (Client client in clients)
            {
                    client.enteries = 0;
                    _clientRepository.Update(client);
            }
            List<Coach> coaches = _coachRepository.GetAll().ToList();
            foreach (Coach coach in coaches)
            {
                coach.Trainings_Weekly = 0;
                _coachRepository.Update(coach);
            }
            return Task.CompletedTask;
        }
    }
}
