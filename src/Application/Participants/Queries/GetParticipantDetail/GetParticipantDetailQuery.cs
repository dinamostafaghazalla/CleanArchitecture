using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Participants.Queries.GetParticipantDetail
{
    public class GetParticipantDetailQuery : IRequest<ParticipantDetailVm>
    {
        public int Id { get; set; }

        public class GetParticipantDetailQueryHandler : IRequestHandler<GetParticipantDetailQuery, ParticipantDetailVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetParticipantDetailQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ParticipantDetailVm> Handle(GetParticipantDetailQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.Participants
                    .Where(e => e.Id == request.Id)
                    .ProjectTo<ParticipantDetailVm>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);

                return vm;
            }
        }
    }
}
