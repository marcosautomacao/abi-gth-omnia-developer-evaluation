using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public abstract class Entity
    {
        private List<DomainEvent> _domainEvents;

        public Guid Id { get; protected set; }
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
            _domainEvents = new List<DomainEvent>();
        }

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents ??= new List<DomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
} 