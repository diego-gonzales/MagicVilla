using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories.IRepositories;

namespace MagicVilla_API.Repositories;

public class VillaRepository : Repository<Villa>, IVillaRepository
{
    private readonly ApplicationDbContext _dbContext;

    // el : base le indicamos esa configuración al constructor de la clase base 'Repository'.
    public VillaRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Villa> Update(Villa villa)
    {
        villa.UpdateDate = DateTime.Now;
        _dbContext.Villas.Update(villa);
        await _dbContext.SaveChangesAsync();
        return villa;
    }
}
