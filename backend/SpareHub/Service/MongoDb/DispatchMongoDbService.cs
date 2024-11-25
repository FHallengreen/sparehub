using AutoMapper;
using Domain.Models;
using Repository.MongoDb;
using Service.Interfaces;
using Shared.DTOs.Dispatch;

namespace Service.MongoDb;

public class DispatchMongoDbService : IDispatchService
{
    private readonly DispatchMongoDbRepository dispatchRepository;
    private readonly IMapper mapper;

    public DispatchMongoDbService(DispatchMongoDbRepository dispatchRepository, IMapper mapper)
    {
        this.dispatchRepository = dispatchRepository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<DispatchResponse>> GetDispatches()
    {
        var dispatches = await dispatchRepository.GetDispatchesAsync();
        return mapper.Map<IEnumerable<DispatchResponse>>(dispatches);
    }

    public async Task<DispatchResponse?> GetDispatchById(string dispatchId)
    {
        var dispatch = await dispatchRepository.GetDispatchByIdAsync(dispatchId);
        return mapper.Map<DispatchResponse?>(dispatch);
    }

    public Task<DispatchResponse> CreateDispatch(DispatchRequest dispatchRequest, string orderId)
    {
        throw new NotImplementedException();
    }

    public Task<DispatchResponse> UpdateDispatch(string dispatchId, DispatchRequest dispatchRequest)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDispatch(string dispatchId)
    {
        throw new NotImplementedException();
    }
}