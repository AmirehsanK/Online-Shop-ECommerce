using System.Text.Encodings.Web;
using System.Text.Unicode;
using Application.Security;
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
        services.AddScoped<IContactUsRepository, ContactUsRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IFileHandleRepository, FileHandleRepository>();

        #endregion

        #region Services

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IContactUsService, ContactUsService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IFileHandleService, FileHandleService>();
        services.AddSingleton<IAuthorizationHandler, AdminHandler>();

        #endregion

        services.AddSingleton<HtmlEncoder>(
            HtmlEncoder.Create(new[]
            {
                UnicodeRanges.BasicLatin,
                UnicodeRanges.Arabic
            }));
    }
}