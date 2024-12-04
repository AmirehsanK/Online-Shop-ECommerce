using Application.Security;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Domain.Interface;
using Infra.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace IOC.DiContainer;

public static class DiContainer
{
    public static void IOcContainer(this IServiceCollection services)
    {
        #region Repositories

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        #endregion

        #region Services

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddSingleton<IAuthorizationHandler, AdminHandler>();
        #endregion

        services.AddSingleton<HtmlEncoder>(
            HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                UnicodeRanges.Arabic }));
    }
}