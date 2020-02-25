
namespace ESB.Domain.Interfaces
{
    using ESB.Domain.Entities;
    using System;
    using System.Collections.Generic;

    public interface IRepository<T> 
        where T 
        : BaseEntity
    {
        IEnumerable<T> GetAll();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
