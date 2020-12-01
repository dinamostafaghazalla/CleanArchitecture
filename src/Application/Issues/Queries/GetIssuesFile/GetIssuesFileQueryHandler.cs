using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Issues.Queries.GetIssuesFile
{
    public class GetIssuesFileQueryHandler : IRequestHandler<GetIssuesFileQuery, IssuesFileVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICsvFileBuilder _fileBuilder;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public GetIssuesFileQueryHandler(IApplicationDbContext context, ICsvFileBuilder fileBuilder, IMapper mapper, IDateTime dateTime)
        {
            _context = context;
            _fileBuilder = fileBuilder;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public async Task<IssuesFileVm> Handle(GetIssuesFileQuery request, CancellationToken cancellationToken)
        {
            var records = await _context.Issues
                .ProjectTo<IssueRecordDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            //var fileContent = _fileBuilder.BuildTodoItemsFile(records);

            var vm = new IssuesFileVm
            {
                Content = null,
                ContentType = "text/csv",
                FileName = $"{_dateTime.Now:yyyy-MM-dd}-Issues.csv"
            };

            return vm;
        }
    }
}
