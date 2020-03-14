using eStore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Infrastructure.Persistence.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
