using Microsoft.EntityFrameworkCore;
using SagaCoordinator.Application.Interfaces;
using SagaCoordinator.Domain.Entities;
using SagaCoordinator.Domain.Enums;
using SagaCoordinator.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Infrastructure.Repositories
{
    public class SagaRepository(SagaContext context) : ISagaRepository
    {
        private readonly SagaContext _context = context;


        public async Task<object> AddNewSaga(Guid correlationId, TypeSaga typeSaga, string? message)
        {
            var saga = new SagaStatus(correlationId, typeSaga, StatusSaga.Pending, message);
            await _context.SagaStatuses.AddAsync(saga);
            return saga;
        }

        public async Task<StatusSaga?> GetSagaStatus(Guid correlationId, TypeSaga typeSaga)
        {
            var saga = await _context.SagaStatuses
                .Where(s => s.CorrelationId == correlationId && s.TypeSaga == typeSaga)
                .Select(s => s.Status)
                .FirstOrDefaultAsync();
            return saga;
        }

        public async Task<bool> SagaExists(Guid correlationId, TypeSaga typeSaga)
        {
            return await _context.SagaStatuses
                .AnyAsync(s => s.CorrelationId == correlationId && s.TypeSaga == typeSaga);
        }

        public async Task<bool> UpdateSagaStatus(Guid correlationId, TypeSaga typeSaga, StatusSaga status, string? message)
        {
            var saga = await _context.SagaStatuses
                .Where(s => s.CorrelationId == correlationId && s.TypeSaga == typeSaga)
                .FirstOrDefaultAsync();
            if (saga == null) return false;
            saga.UpdateStatus(status);
            saga.Message = message;
            return true;
        }
    }
}
