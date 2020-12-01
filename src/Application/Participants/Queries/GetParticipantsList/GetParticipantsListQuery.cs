using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Participants.Queries.GetParticipantsList
{
    public class GetParticipantsListQuery : IRequest<ParticipantsListVm>
    {
        public class GetParticipantsListQueryHandler : IRequestHandler<GetParticipantsListQuery, ParticipantsListVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetParticipantsListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ParticipantsListVm> Handle(GetParticipantsListQuery request, CancellationToken cancellationToken)
            {
                var participants = await _context.Participants
                    .ProjectTo<ParticipantLookupDto>(_mapper.ConfigurationProvider)
                    .OrderBy(e => e.Name)
                    .ToListAsync(cancellationToken);

                var vm = new ParticipantsListVm
                {
                    Participants = participants
                };
                 
                return vm;
            }
        }
    }
}
