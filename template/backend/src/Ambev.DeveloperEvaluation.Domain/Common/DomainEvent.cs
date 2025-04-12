using System;

namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public abstract class DomainEvent
    {
        public DateTime DateOccurred { get; protected set; }

        protected DomainEvent()
        {
            DateOccurred = DateTime.UtcNow;
        }
    }
} 