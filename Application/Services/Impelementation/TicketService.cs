using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Extention;
using Application.Services.Interfaces;
using Application.Tools;
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
        #endregion

        #region Addticket

        public async Task AddNewTicket(AddTicketViewModel ticket, int userid)
        {
            if (ticket.Image == null)
            {
                var imagename = "nothing";
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
                await _ticketRepository.SaveChangeAsync();
                var newtikcetmessage = new TicketsMessage()
                {
                    CreateDate = DateTime.Now,
                    IsDeleted = false,
                    SenderId = newticket.OwnerId,
                    Text = ticket.Message,
                    TicketId = newticket.Id,
                    FileName = imagename

                };
                await _ticketRepository.AddTicketMessageAsync(newtikcetmessage);
                await _ticketRepository.SaveChangeAsync();
            }
            else
            {
                var imagename = Guid.NewGuid().ToString("N") + Path.GetExtension(ticket.Image.FileName);
                ticket.Image.AddImageToServer(imagename, PathTools.ProductImageServerPath, 100, 70,
                    PathTools.ProductThumbImageServerPath);
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
                await _ticketRepository.SaveChangeAsync();
                var newtikcetmessage = new TicketsMessage()
                {
                    CreateDate = DateTime.Now,
                    IsDeleted = false,
                    SenderId = newticket.OwnerId,
                    Text = ticket.Message,
                    TicketId = newticket.Id,
                    FileName = imagename

                };
                await _ticketRepository.AddTicketMessageAsync(newtikcetmessage);
                await _ticketRepository.SaveChangeAsync();
            }



        }

        public async Task<TicketDetailViewModel> GetTicketDetail(int ticketid)
        {
            var ticket = await _ticketRepository.GetAllAsync(ticketid);
            var ticketDetail = new TicketDetailViewModel()
            {
                Ticket = ticket,
                Messages = ticket.TicketsMessages

            };
            return ticketDetail;




        }

        public async Task AddMessageToCurrentTicket(TicketDetailViewModel model, int ticketid, int userId)
        {

            var Newmessage = new TicketsMessage();

            Newmessage.CreateDate = DateTime.Now;
            if (model.FileName!=null)
            {
                Newmessage.FileName = model.FileName;
            }
            Newmessage.IsDeleted = false;
            Newmessage.SenderId = userId;
            Newmessage.Text = model.Message;
            Newmessage.TicketId = ticketid;
            
            
            await _ticketRepository.AddTicketMessageAsync(Newmessage);
            //Chnage ticket UpdateDate

            var Ticket = await _ticketRepository.GetTicketAsync(ticketid);
            Ticket.UpdateDate = DateTime.Now;
            _ticketRepository.UpdateTicketAsync(Ticket);

            await _ticketRepository.SaveChangeAsync();

        }

        #endregion


    }
}
