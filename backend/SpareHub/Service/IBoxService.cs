using Domain;
using Shared;

namespace Service;

public interface IBoxService
{
    public Task CreateBox(BoxRequest boxRequest, int orderId);
    public Task<List<OrderBoxCollection>> GetBoxes(int orderId);
    public Task<OrderBoxCollection> UpdateBoxes (BoxRequest boxRequest, int orderId);
    public void DeleteBox(int orderId);
    
}
