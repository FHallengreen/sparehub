using Domain;
using Shared;

namespace Service;

public interface IBoxService
{
    public Task CreateBox(BoxRequest boxRequest, int orderId);
    public Task<List<OrderBoxCollection>> GetBoxes(int orderId);
    public Task UpdateOrderBoxes (int orderId, List<BoxRequest> boxes);
    public void DeleteBox(int orderId);
    
}
