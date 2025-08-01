﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ISagaRepository? SagaRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task CommitAsync();
        void BeginTransaction();
        Task BeginTransactionAsync();
        Task RollbackAsync();
    }
}
