using AutoMapper;


namespace Skalk.BLL.Services.Abstract
{
    public abstract class BaseService
    {
        private protected readonly SkalkContext _context;
        private protected readonly IMapper _mapper;

        protected BaseService(SkalkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
