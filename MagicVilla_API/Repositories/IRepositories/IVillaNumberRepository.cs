using MagicVilla_API.Models;

namespace MagicVilla_API.Repositories.IRepositories;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    Task<VillaNumber> Update(VillaNumber villaNumber);
}
