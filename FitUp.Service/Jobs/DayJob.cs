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
    public class DayJob : IJob
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITrainingRepository _trainingRepository;

        public DayJob(IClientRepository clientRepository, ITrainingRepository trainingRepository)
        {
            _clientRepository = clientRepository;
            _trainingRepository = trainingRepository;
        }
        public Task Execute(IJobExecutionContext context)
        {
        List<Client> clients = _clientRepository.GetAll().ToList();
            DateTime now = DateTime.UtcNow;
            foreach (Client client in clients)
            {
                int days = now.Subtract(client.day_last_payment).Days;
                if (days > 35)
                {
                    client.active = false;
                    _clientRepository.Update(client);
                }
            }
            List<Training> trainings = _trainingRepository.GetAll().ToList();
            foreach (Training training in trainings)
            {
                int days = now.Subtract(training.dateTime).Days;
                if(days > 2)
                {
                    _trainingRepository.Delete(training);
                }
            }
            return Task.CompletedTask;
        }
    }
}
