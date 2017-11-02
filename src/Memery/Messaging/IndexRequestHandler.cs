using System.Collections.Generic;
using MediatR;
using Memery.Services;

namespace Memery.Messaging
{
    public class IndexRequestHandler : IRequestHandler<IndexRequest, Dictionary<string, ImageReference>>
    {
        public IndexRequestHandler(IImageIndexService service) {
            IndexService = service;
        }

        public IImageIndexService IndexService { get; }

        public Dictionary<string, ImageReference> Handle(IndexRequest message) => IndexService.GetIndex();
    }


}