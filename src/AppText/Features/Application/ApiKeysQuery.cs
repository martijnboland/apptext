using AppText.Shared.Queries;
using AppText.Storage;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    public class ApiKeysQuery : IQuery<ApiKey[]>
    {
        public string AppId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }

    public class ApiKeysQueryHandler : IQueryHandler<ApiKeysQuery, ApiKey[]>
    {
        private readonly IApplicationStore _applicationStore;

        public ApiKeysQueryHandler(IApplicationStore applicationStore)
        {
            _applicationStore = applicationStore;
        }

        public Task<ApiKey[]> Handle(ApiKeysQuery query)
        {
            return _applicationStore.GetApiKeys(query);
        }
    }
}