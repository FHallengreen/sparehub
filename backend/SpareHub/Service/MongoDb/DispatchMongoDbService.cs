using AutoMapper;
using Domain.Models;
using Repository.MongoDb;
using Service.Interfaces;
using Shared.DTOs.Dispatch;

namespace Service.MongoDb;

public class DispatchMongoDbService : IDispatchService
{
    private readonly DispatchMongoDbRepository _dispatchRepository;
    private readonly IMapper _mapper;

    public DispatchMongoDbService(DispatchMongoDbRepository dispatchRepository, IMapper mapper)
    {
        _dispatchRepository = dispatchRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DispatchResponse>> GetDispatches()
    {
        var dispatches = await _dispatchRepository.GetDispatchesAsync();
        return _mapper.Map<IEnumerable<DispatchResponse>>(dispatches);
    }

    public async Task<DispatchResponse?> GetDispatchById(string dispatchId)
    {
        var dispatch = await _dispatchRepository.GetDispatchByIdAsync(dispatchId);
        return _mapper.Map<DispatchResponse?>(dispatch);
    }

    public async Task<DispatchResponse> CreateDispatch(DispatchRequest dispatchRequest, string orderId)
    {
        var dispatch = _mapper.Map<Dispatch>(dispatchRequest);
        var createdDispatch = await _dispatchRepository.CreateDispatchAsync(dispatch);
        return _mapper.Map<DispatchResponse>(createdDispatch);
    }

    public async Task<DispatchResponse> UpdateDispatch(string dispatchId, DispatchRequest dispatchRequest)
    {
        var existingDispatch = await _dispatchRepository.GetDispatchByIdAsync(dispatchId);

        if (existingDispatch == null)
        {
            throw new Exception($"Dispatch with ID {dispatchId} not found.");
        }

        var updatedDispatch = _mapper.Map(dispatchRequest, existingDispatch);
        updatedDispatch.Id = dispatchId;

        var savedDispatch = await _dispatchRepository.UpdateDispatchAsync(updatedDispatch);
        return _mapper.Map<DispatchResponse>(savedDispatch);
    }

    public async Task DeleteDispatch(string dispatchId)
    {
        var existingDispatch = await _dispatchRepository.GetDispatchByIdAsync(dispatchId);

        if (existingDispatch == null)
        {
            throw new Exception($"Dispatch with ID {dispatchId} not found.");
        }

        await _dispatchRepository.DeleteDispatchAsync(dispatchId);
    }
}
