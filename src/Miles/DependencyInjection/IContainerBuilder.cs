using System;

namespace Miles.DependencyInjection
{
    public interface IContainerBuilder
    {
        IContainerBuilder AddScoped(Type service);
        IContainerBuilder AddScoped<TService>();
        IContainerBuilder AddScoped(Type service, Type implementation);
        IContainerBuilder AddScoped<TService, TImplementation>() where TImplementation : TService;
        IContainerBuilder AddScoped(Type service, Func<object> implementationFactory);
        IContainerBuilder AddScoped<TService>(Func<TService> implementationFactory);

        IContainerBuilder AddSingleton(Type service);
        IContainerBuilder AddSingleton<TService>();
        IContainerBuilder AddSingleton(Type service, Type implementation);
        IContainerBuilder AddSingleton<TService, TImplementation>() where TImplementation : TService;
        IContainerBuilder AddSingleton(Type service, Func<object> implementationFactory);
        IContainerBuilder AddSingleton<TService>(Func<TService> implementationFactory);
        IContainerBuilder AddSingleton(Type service, object instance);
        IContainerBuilder AddSingleton<TService>(TService instance);

        IContainerBuilder AddTransient(Type service);
        IContainerBuilder AddTransient<TService>();
        IContainerBuilder AddTransient(Type service, Type implementation);
        IContainerBuilder AddTransient<TService, TImplementation>() where TImplementation : TService;
        IContainerBuilder AddTransient(Type service, Func<object> implementationFactory);
        IContainerBuilder AddTransient<TService>(Func<TService> implementationFactory);

        string ContainerType { get; }

        IContainer CreateContainer();
    }
}
