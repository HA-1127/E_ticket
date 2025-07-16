using E_ticket.data;
using E_ticket.Models;
using E_ticket.Repostoris.IRepository;

namespace E_ticket.Repostoris
{
    public class CategoryRepository : Repository<Category>, ICategotyRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
