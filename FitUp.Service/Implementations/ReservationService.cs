using FitUp.Domain.Exceptions;
using FitUp.Domain.Models;
using FitUp.Repository.Interface;
using FitUp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitUp.Service.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        public readonly IRepository<EmailMessage> _emailMessageRepository;
        private readonly IClientService _clientService;
        private readonly ITrainingRepository _trainingRepository;

        public ReservationService(
            IRepository<EmailMessage> emailRepository,IReservationRepository reservationRepository, IClientService clientService, ITrainingRepository trainingRepository)
        {
            _reservationRepository = reservationRepository;
            _clientService = clientService;
            _trainingRepository = trainingRepository;
            _emailMessageRepository = emailRepository;
        }

        public void CreateNew(Reservation c)
        {
            if(c!=null)
            {
                var Client = _clientService.GetDetails(c.ClientID);
                var Training = _trainingRepository.Get(c.TrainingID);
                if(Client.active==false)
                {
                    throw new DeactiveClient(Client.Email);
                }
                else if(Client.enteries==Client.allowed_times)
                {
                    throw new AllowedClientEnteries(Client.Email);
                }
                else if (Training.capacity == Training.Reservations.Count())
                {
                    throw new SessionFull();
                }
                var c_reserv = Client.Reservations.ToList();
                for (int i = 0; i < c_reserv.Count; i++)
                {
                    var training = _trainingRepository.Get(c_reserv[i].TrainingID);
                    if (Training.dateTime.ToString().Split(' ')[0].Equals(training.dateTime.ToString().Split(' ')[0]))
                    {
                        throw new ReservedForTheDay(Client.Email);
                    }
                }

                EmailMessage emailMessage = new EmailMessage();
                emailMessage.MailTo = Client.Email;
                emailMessage.Subject = "Reservation Confirmed";
                emailMessage.Status = false;
                emailMessage.Content = "Your Reservation is confirmed. We expect to see you at out gym: "+ Training.ToString()+ 
                    ". Please remove your reservation at least 1 day before, if you are not comming to the session.";

                _emailMessageRepository.Insert(emailMessage);
                _clientService.clientReserve(Client);
                _reservationRepository.Insert(c);
            }
            
        }

        public void Delete(Guid id)
        {
            var c = _reservationRepository.Get(id);
            if (c != null)
                _clientService.clientRemoveReservation(c.Client);
                _reservationRepository.Delete(c);
        }

        public List<Reservation> getAll()
        {
            return _reservationRepository.GetAll().ToList();
        }

        public Reservation GetDetails(Guid? id)
        {
            return _reservationRepository.Get(id);
        }

        public void Update(Reservation c)
        {
            var Client = _clientService.GetDetails(c.ClientID);
            var Training = _trainingRepository.Get(c.TrainingID);
            if (Training.capacity == Training.Reservations.Count())
            {
                throw new SessionFull();
            }
            var c_reserv = Client.Reservations.ToList();
            for (int i = 0; i < c_reserv.Count; i++)
            {
                if (Training.dateTime.ToString().Split(' ')[0].Equals(_trainingRepository.Get(c_reserv[i].TrainingID).dateTime.ToString().Split(' ')[0]) && 
                    c_reserv[i].id!=c.id)
                {
                    throw new ReservedForTheDay(Client.Email);
                }
            }
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.MailTo = Client.Email;
            emailMessage.Subject = "Reservation Updated";
            emailMessage.Status = false;
            emailMessage.Content = "Your Reservation is updated. We expect to see you at out gym: " + Training.ToString() +
                ". Please remove your reservation at least 1 day before, if you are not comming to the session.";

            _emailMessageRepository.Insert(emailMessage);
            _reservationRepository.Update(c);
        }

        public List<Reservation> GetAllByUser(String email)
        {
            return _reservationRepository.GetAllbyUser(email).ToList();
        }
    }
}
