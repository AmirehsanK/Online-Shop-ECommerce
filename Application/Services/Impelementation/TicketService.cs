using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Entities.Ticket;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.Ticket;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.Services.Impelementation
{
    public class TicketService : ITicketService
    {
        #region Ctor

        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        #endregion

        #region ticketList

        public async Task<List<TicketListViewModel>> GetTicketListForShow(int userId)
        {
            var ticketList = await _ticketRepository.GetAllCurrentTicketAsync(userId);
            return ticketList.Select(u => new TicketListViewModel()
            {
                Section = u.Section,
                CreateDate = u.CreateDate,
                Status = u.Status,
                UpdateDate = DateTime.Now,
                TicketId = u.Id,
                Title = u.Title

            }).ToList();


        }

        public async Task AddNewTicket(AddTicketViewModel ticket, int userid)
        {
            var newticket = new Ticket()
            {
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Section = ticket.Section,
                IsClosed = false,
                OwnerId = userid,
                Title = ticket.Title,
                Status = TicketsEnum.Status.InProgress,
                Priority = ticket.Priority,
                UpdateDate = DateTime.Now,
            };
            await _ticketRepository.AddTicketAsync(newticket);
            var newtikcetmessage = new TicketsMessage()
            {
                CreateDate = DateTime.Now,
                IsDeleted = false,
                SenderId = newticket.OwnerId,
                Text = ticket.Message,
                TicketId = newticket.Id,
                FileName = Guid.NewGuid().ToString("N"),

            };
            await _ticketRepository.AddTicketMessageAsync(newtikcetmessage);
            await _ticketRepository.SaveChangeAsync();

        }

        #endregion



    }
}
