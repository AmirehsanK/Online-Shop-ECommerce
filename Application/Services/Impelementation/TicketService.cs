using Application.Extention;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Ticket;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.Ticket;
using Domain.ViewModel.Ticket.Admin;

namespace Application.Services.Impelementation;

public class TicketService(ITicketRepository ticketRepository) : ITicketService
{
        
    #region Ticket List
    public async Task<List<TicketListViewModel>> GetTicketListForShow(int userId)
    {
        var ticketList = await ticketRepository.GetAllCurrentTicketAsync(userId);
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

    #region Add Ticket
    public async Task AddNewTicket(AddTicketViewModel ticket, int userid)
    {
        var newTicket = new Ticket()
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
        await ticketRepository.AddTicketAsync(newTicket);
        await ticketRepository.SaveChangeAsync();

        var newTicketMessage = new TicketsMessage()
        {
            CreateDate = DateTime.Now,
            IsDeleted = false,
            SenderId = newTicket.OwnerId,
            Text = ticket.Message,
            TicketId = newTicket.Id,
        };
        if (ticket.Image != null)
        {
            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(ticket.Image.FileName);
            await ticket.Image.AddFilesToServer(fileName, PathTools.FileServerPath);
            newTicketMessage.FileName = fileName;
        }
        await ticketRepository.AddTicketMessageAsync(newTicketMessage);
        await ticketRepository.SaveChangeAsync();
    }
    #endregion

    #region Get Ticket Detail
    public async Task<TicketDetailViewModel> GetTicketDetail(int ticketid)
    {
        var ticket = await ticketRepository.GetAllAsync(ticketid);
        var ticketViewModel = new TicketViewModel()
        {
            OwnerID = ticket.OwnerId,
            TicketId = ticketid,
            Title = ticket.Title,
            Status = ticket.Status
        };
        var ticketMessage = await ticketRepository.GetMessages(ticketid);
        ticketMessage.Select(u => new TicketMessageViewModel()
        {
            CreateDate = u.CreateDate,
            Message = u.Text,
            FileName = u.FileName!,
            SenderId = u.SenderId
        }).ToList();
        var ticketDetail = new TicketDetailViewModel()
        {
            Messages = ticketMessage,
            Ticket = ticketViewModel
        };
        return ticketDetail;
    }
    #endregion

    #region Add Message to Current Ticket
    public async Task AddMessageToCurrentTicket(TicketDetailViewModel model, int ticketid, int userId)
    {
        var newMessage = new TicketsMessage
        {
            CreateDate = DateTime.Now
        };
        if (model.FileName != null)
        {
            newMessage.FileName = model.FileName;
        }
        newMessage.IsDeleted = false;
        newMessage.SenderId = userId;
        newMessage.Text = model.Message;
        newMessage.TicketId = ticketid;
        await ticketRepository.AddTicketMessageAsync(newMessage);

        var ticket = await ticketRepository.GetTicketAsync(ticketid);
        ticket.Status = TicketsEnum.Status.InProgress;
        ticket.UpdateDate = DateTime.Now;
        ticketRepository.UpdateTicketAsync(ticket);
        await ticketRepository.SaveChangeAsync();
    }
    #endregion

    #region Get All Ticket List for Admin
    public async Task<List<ShowAllTicketList>> GetAllTicketListForAdmin()
    {
        var allTicket = await ticketRepository.GetAllTicketForAdminAsync();
        return allTicket.Select(u => new ShowAllTicketList()
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
    }

    public async Task<List<TicketAdminPanelViewModel>> GetAllTicketForAdminPanel()
    {
        var allTicket = await ticketRepository.GetAllTicketForAdminAsync();
        return allTicket.Select(u => new TicketAdminPanelViewModel()
        {
            Ticketid = u.Id,
            Title = u.Title,
            CreateDate = u.CreateDate
        }).Take(3).ToList();
    }
    #endregion

    #region Add Message to Current Ticket from Admin
    public async Task AddMessageToCurrentTicketFromAdmin(TicketDetailViewModel model, int ticketid, int userId)
    {
        var newMessage = new TicketsMessage
        {
            CreateDate = DateTime.Now
        };
        if (model.FileName != null)
        {
            newMessage.FileName = model.FileName;
        }
        newMessage.IsDeleted = false;
        newMessage.SenderId = userId;
        newMessage.Text = model.Message;
        newMessage.TicketId = ticketid;
        await ticketRepository.AddTicketMessageAsync(newMessage);

        var ticket = await ticketRepository.GetTicketAsync(ticketid);
        ticket.Status = TicketsEnum.Status.IsAnswered;
        ticket.UpdateDate = DateTime.Now;
        ticketRepository.UpdateTicketAsync(ticket);
        await ticketRepository.SaveChangeAsync();
    }
    #endregion

    #region Close Ticket
    public async Task CloseTicket(int ticketid)
    {
        var ticket = await ticketRepository.GetTicketAsync(ticketid);
        ticket.Status = TicketsEnum.Status.IsClosed;
        ticketRepository.UpdateTicketAsync(ticket);
        await ticketRepository.SaveChangeAsync();
    }
    #endregion
        
}