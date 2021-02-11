using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using JobsityChat.Core.Models;
using JobsityChat.Core.Contracts;
using JobsityChat.Infraestructure.Database;

namespace JobsityChat.Infraestructure.Services
{
    public class MessageRepository : IRepository<UserMessage>, IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageRepository(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public async Task<List<UserMessage>> GetAllAsync<TOrderBy>(int? count, Expression<Func<UserMessage, TOrderBy>> orderBy)
        {
            return count.HasValue && count > 0 ?
                  await _dbContext.Messages.Take(count.Value).OrderBy(orderBy).ToListAsync() :
                  await _dbContext.Messages.OrderBy(orderBy).ToListAsync();
        }

        public async Task<UserMessage> FirstOrDefaultAsync(Expression<Func<UserMessage, bool>> filter)
        {
            return await _dbContext.Messages.FirstOrDefaultAsync(filter);
        }

        public async Task<UserMessage> InsertAsync(UserMessage entity)
        {
            await _dbContext.Messages.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(UserMessage entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserMessage entity)
        {
            _dbContext.Messages.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserMessage>> GetLastMessagesAsync(int count)
        {
            return await GetAllAsync(count, (message) => message.CreationDate);
        }
    }
}
