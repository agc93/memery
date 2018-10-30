using MediatR;
using Memery.Configuration;
using Memery.Services;
using Microsoft.Extensions.Options;

namespace Memery.Messaging
{
    public class AppMetadataRequestHandler : IRequestHandler<AppMetadataRequest, object>
    {
        private readonly IOptions<MemeryOptions> _options;
        private readonly MetadataService _metadata;

        public AppMetadataRequestHandler(IOptions<Configuration.MemeryOptions> options, MetadataService metadata)
        {
            _options = options;
            _metadata = metadata;
        }
        public object Handle(AppMetadataRequest message)
        {
            return new
            {
                version = _metadata.Version,
                runtime = _metadata.RuntimeVersion,
                api = new {
                    name = Services.MetadataService.APIName,
                    spec = $"/api/{Services.MetadataService.APIName}",
                    info = "/help"
                },
                options = _options
            };
        }
    }
}