﻿using System;
using EasyNetQ;
using EasyNetQ.ConnectionString;
using EasyNetQ.DI;
using EasyNetQ.DI.LightInject;

// ReSharper disable once CheckNamespace
namespace LightInject
{
    public static class EasyNetQServiceContainerExtensions
    {
        public static IServiceContainer RegisterEasyNetQ(this IServiceContainer serviceContainer, Func<IServiceResolver, ConnectionConfiguration> connectionConfigurationFactory, Action<IServiceRegister> registerServices)
        {
            return serviceContainer.RegisterEasyNetQ(connectionConfigurationFactory, (r, _) => registerServices(r));
        }

        public static IServiceContainer RegisterEasyNetQ(this IServiceContainer serviceContainer, Func<IServiceResolver, ConnectionConfiguration> connectionConfigurationFactory, Action<IServiceRegister, ICollectionServiceRegister> registerServices) 
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException(nameof(serviceContainer));
            }
            
            var serviceRegister = new LightInjectAdapter(serviceContainer);
            RabbitHutch.RegisterBus(serviceRegister, connectionConfigurationFactory, registerServices);
            return serviceContainer;
        }
        
        public static IServiceContainer RegisterEasyNetQ(this IServiceContainer serviceContainer, Func<IServiceResolver, ConnectionConfiguration> connectionConfigurationFactory)
        {
            return serviceContainer.RegisterEasyNetQ(connectionConfigurationFactory, c => { });
        }

        public static IServiceContainer RegisterEasyNetQ(this IServiceContainer serviceContainer, string connectionString, Action<IServiceRegister> registerServices)
        {
            return serviceContainer.RegisterEasyNetQ(connectionString, (r, _) => registerServices(r));
        }

        public static IServiceContainer RegisterEasyNetQ(this IServiceContainer serviceContainer, string connectionString, Action<IServiceRegister, ICollectionServiceRegister> registerServices)
        {
            return serviceContainer.RegisterEasyNetQ(c => c.Resolve<IConnectionStringParser>().Parse(connectionString), registerServices);
        }
        
        public static IServiceContainer RegisterEasyNetQ(this IServiceContainer serviceContainer, string connectionString)
        {
            return serviceContainer.RegisterEasyNetQ(c => c.Resolve<IConnectionStringParser>().Parse(connectionString));
        }
    }
}
