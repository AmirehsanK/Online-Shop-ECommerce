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
using Domain.ViewModel.Ticket.Admin;
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
            var newticket = new Ticket()
            {
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Section = ticket.Section,
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


            };
            if (ticket.Image != null)
            {
                var FileName = Guid.NewGuid().ToString("N") + Path.GetExtension(ticket.Image.FileName);
                await ticket.Image.AddFilesToServer(FileName, PathTools.FileServerPath);
                newtikcetmessage.FileName = FileName;
            }
            await _ticketRepository.AddTicketMessageAsync(newtikcetmessage);
            await _ticketRepository.SaveChangeAsync();


        }


        #endregion

        #region GetTicketDetail

        public async Task<TicketDetailViewModel> GetTicketDetail(int ticketid)
        {
            var ticket = await _ticketRepository.GetAllAsync(ticketid);
            var TicketViewModel = new TicketViewModel()
            {
                OwnerID = ticket.OwnerId,
                TicketId = ticketid,
                Title = ticket.Title,
                Status = ticket.Status
            };
            var ticketmessagge = await _ticketRepository.GetMessages(ticketid);
            ticketmessagge.Select(u => new TicketMessageViewModel()
            {
                CreateDate = u.CreateDate,
                Message = u.Text,
                FileName = u.FileName,
                SenderId = u.SenderId

            }).ToList();



            var ticketDetail = new TicketDetailViewModel()
            {
                Messages = ticketmessagge,
                Ticket = TicketViewModel

            };
            return ticketDetail;




        }

        #endregion

        #region AddMessageToCurrentTicket

        public async Task AddMessageToCurrentTicket(TicketDetailViewModel model, int ticketid, int userId)
        {

            var Newmessage = new TicketsMessage();

            Newmessage.CreateDate = DateTime.Now;
            if (model.FileName != null)
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
            Ticket.Status = TicketsEnum.Status.InProgress;
            Ticket.UpdateDate = DateTime.Now;
            _ticketRepository.UpdateTicketAsync(Ticket);

            await _ticketRepository.SaveChangeAsync();

        }

        #endregion

        #region GetAllTicketListForAdmin

        public async Task<List<ShowAllTicketList>> GetAllTicketListForAdmin()
        {
            var allticket = await _ticketRepository.GetAllTicketForAdminAsync();
            return allticket.Select(u => new ShowAllTicketList()
            {
                Ticketid = u.Id,
                Section = u.Section,
                Priority = u.Priority,
                CreateDate = u.CreateDate,
                OwnerId = u.OwnerId,
                Status = u.Status,
                Title = u.Title,
                UpdateDate = u.UpdateDate
            }).ToList();
        }public async Task<List<TicketAdminPanelViewModel>> GetAllTicketForAdminPanel()
        {
            var allticket = await _ticketRepository.GetAllTicketForAdminAsync();
            return allticket.Select(u => new TicketAdminPanelViewModel()
            {
                Ticketid = u.Id,
                Title = u.Title,
                CreateDate = u.CreateDate
            }).Take(3).ToList();
        }

        #endregion

        #region AddMessageToCurrentTicketFromAdmin

        public async Task AddMessageToCurrentTicketFromAdmin(TicketDetailViewModel model, int ticketid, int userId)
        {
            var Newmessage = new TicketsMessage();


            Newmessage.CreateDate = DateTime.Now;
            if (model.FileName != null)
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
            Ticket.Status = TicketsEnum.Status.IsAnswered;
            Ticket.UpdateDate = DateTime.Now;
            _ticketRepository.UpdateTicketAsync(Ticket);

            await _ticketRepository.SaveChangeAsync();
        }

        #endregion

        #region CloseTicket

        public async Task CloseTicket(int ticketid)
        {
            var ticket = await _ticketRepository.GetTicketAsync(ticketid);
            ticket.Status = TicketsEnum.Status.IsClosed;
            _ticketRepository.UpdateTicketAsync(ticket);
            await _ticketRepository.SaveChangeAsync();
        }


        #endregion





    }
}
