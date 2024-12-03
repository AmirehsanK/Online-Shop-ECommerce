using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Domain.Interface;
using Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IOC.DiContainer;

public static class DiContainer
{
    public static void IOcContainer(this IServiceCollection services)
    {
        #region Repositories

        services.AddScoped<IUserRepository, UserRepository>();

        #endregion

        #region Services

        services.AddScoped<IUserService, UserService>();

        #endregion
    }
}