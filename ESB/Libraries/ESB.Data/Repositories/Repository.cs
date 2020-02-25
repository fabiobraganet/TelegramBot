
namespace ESB.Data.Repositories
{
    using ESB.Data.Context.Bots;
    using ESB.Domain.Entities;
    using ESB.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Repository<T> : IRepository<T> 
        where T 
        : BaseEntity
    {
        private readonly BotsContext context;

        private DbSet<T> entities;
        
        string errorMessage = string.Empty;
        
        public Repository(BotsContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        
        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }
        
        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            
            entities.Add(entity);
            
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entities.Remove(entity);
            
            context.SaveChanges();
        }
    }
}
