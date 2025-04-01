using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.Repository;

public class BoosterRepository : GenericRepository<Booster>, IBoosterRepository
{
    private readonly ApplicationDbContext _context;
    public BoosterRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
