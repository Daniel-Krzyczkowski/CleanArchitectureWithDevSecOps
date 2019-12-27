using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IAsyncIdentityRepository<T> where T : BaseEntity
    {
        Task<T> GetProfile(Guid id);
        Task UpdateProfile(T entity);
        Task DeleteProfile(Guid id);
        Task<IReadOnlyList<T>> GetProfiles(IList<Guid> identifiers);
    }
}
