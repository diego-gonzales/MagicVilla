using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories.IRepositories;

namespace MagicVilla_API.Repositories;

public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
{
    private readonly ApplicationDbContext _dbContext;

    // el : base le indicamos esa configuración al constructor de la clase base 'Repository'.
    public VillaNumberRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<VillaNumber> Update(VillaNumber villaNumber)
    {
        villaNumber.UpdateDate = DateTime.Now;
        _dbContext.VillaNumbers.Update(villaNumber);
        await _dbContext.SaveChangesAsync();
        return villaNumber;
    }
}
