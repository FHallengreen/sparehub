using Domain.Models;
using Neo4j.Driver;
using Repository.Interfaces;

namespace Repository.Neo4J;

public class BoxNeo4JRepository(IDriver driver) : IBoxRepository
{
    public async Task<Box> CreateBoxAsync(Box box)
    {
        var query = @"
            MATCH (o:Order {id: $orderId})
            CREATE (b:Box {id: $id, length: $length, width: $width, height: $height, weight: $weight})
            CREATE (o)-[:CONTAINS]->(b)
            RETURN b";

        var session = driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query, new
            {
                id = box.Id,
                orderId = box.OrderId,
                length = box.Length,
                width = box.Width,
                height = box.Height,
                weight = box.Weight
            });

            var record = await result.SingleAsync();
            var boxNode = record["b"].As<INode>();

            return new Box
            {
                Id = boxNode["id"].As<string>(),
                Length = boxNode["length"].As<int>(),
                Width = boxNode["width"].As<int>(),
                Height = boxNode["height"].As<int>(),
                Weight = boxNode["weight"].As<double>(),
                OrderId = box.OrderId
            };
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task<List<Box>> GetBoxesByOrderIdAsync(string orderId)
    {
        const string query = "MATCH (o:Order {id: $orderId})<-[:BELONGS_TO]-(b:Box) RETURN b";

        var session = driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query, new { orderId });
            var boxes = new List<Box>();

            await result.ForEachAsync(record =>
            {
                var boxNode = record["b"].As<INode>();
                boxes.Add(new Box
                {
                    Id = boxNode["id"].As<string>(),
                    Length = boxNode["length"].As<int>(),
                    Width = boxNode["width"].As<int>(),
                    Height = boxNode["height"].As<int>(),
                    Weight = boxNode["weight"].As<double>(),
                    OrderId = orderId
                });
            });

            return boxes;
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public Task UpdateBoxesAsync(string orderId, List<Box> boxRequests)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBoxAsync(string orderId, string boxId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBoxAsync(string orderId, Box box)
    {
        throw new NotImplementedException();
    }
}