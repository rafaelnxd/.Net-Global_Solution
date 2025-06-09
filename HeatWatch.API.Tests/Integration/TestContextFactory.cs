// HeatWatch.API.Tests.Integration.TestContextFactory.cs
using HeatWatch.API.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace HeatWatch.API.Tests.Integration
{
    public static class TestContextFactory
    {
        public static HeatWatchContext Create()
        {
            var options = new DbContextOptionsBuilder<HeatWatchContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new HeatWatchContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
