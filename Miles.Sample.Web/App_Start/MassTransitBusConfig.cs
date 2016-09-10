﻿using MassTransit;
using Miles.Sample.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MassTransitBusConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(MassTransitBusConfig), "Stop")]

namespace Miles.Sample.Web.App_Start
{
    public static class MassTransitBusConfig
    {
        private static IBusControl bus;

        public static IBusControl GetBus()
        {
            if (bus != null)
                return bus;

            bus = Bus.Factory.CreateUsingRabbitMq(c =>
            {
                var host = c.Host(new Uri("rabbitmq://localhost"), cfg =>
                {
                    cfg.Username("guest");
                    cfg.Password("guest");
                });
            });

            return bus;
        }

        public static void Start()
        {
            GetBus().Start();
        }

        public static void Stop()
        {
            if (bus != null)
            {
                bus.Stop();
                bus = null;
            }
        }
    }
}