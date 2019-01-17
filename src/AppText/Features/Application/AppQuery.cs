using AppText.Shared.Queries;
using AppText.Storage;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    public class AppQuery : IQuery<App[]>
    {
        public string Id { get; set; }
    }

    public class AppQueryHandler : IQueryHandler<AppQuery, App[]>
    {
        public IApplicationStore _store;

        public AppQueryHandler(IApplicationStore store)
        {
            _store = store;
        }

        public Task<App[]> Handle(AppQuery query)
        {
            return _store.GetApps(query);
        }
    }
}
