using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.Interface
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}