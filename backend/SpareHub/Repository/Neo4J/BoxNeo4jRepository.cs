using Domain.Models;
using Neo4j.Driver;
using Repository.Interfaces;

namespace Repository.Neo4J;

public class BoxNeo4JRepository(IDriver driver) : IBoxRepository
{
    public async Task<Box> CreateBoxAsync(Box box)
    {
        const string query = @"
          MATCH (o:Order {id: $orderId})
          CREATE (b:Box {id: $id, length: $length, width: $width, height: $height, weight: $weight})
          CREATE (b)-[:BELONGS_TO]->(o)
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

    public async Task UpdateBoxesAsync(string orderId, List<Box> boxRequests)
    {
        const string queryTemplate = @"
        MATCH (o:Order {id: $orderId})<-[:BELONGS_TO]-(b:Box {id: $boxId})
        SET b.length = $length,
            b.width = $width,
            b.height = $height,
            b.weight = $weight
        RETURN b";

        var session = driver.AsyncSession();
        try
        {
            var tasks = boxRequests.Select(boxRequest =>
            {
                return session.RunAsync(queryTemplate, new
                {
                    orderId,
                    boxId = boxRequest.Id,
                    length = boxRequest.Length,
                    width = boxRequest.Width,
                    height = boxRequest.Height,
                    weight = boxRequest.Weight
                });
            });

            await Task.WhenAll(tasks);
        }
        finally
        {
            await session.CloseAsync();
        }
    }


    public async Task DeleteBoxAsync(string boxId)
    {
        const string query = @"
        MATCH (o:Order {id: $orderId})<-[:BELONGS_TO]-(b:Box {id: $boxId})
        DELETE b";

        var session = driver.AsyncSession();
        try
        {
            await session.RunAsync(query, boxId);
        }
        finally
        {
            await session.CloseAsync();
        }
    }


    public async Task UpdateBoxAsync(string orderId, Box box)
    {
        const string query = @"
        MATCH (o:Order {id: $orderId})<-[:BELONGS_TO]-(b:Box {id: $boxId})
        SET b.length = $length,
            b.width = $width,
            b.height = $height,
            b.weight = $weight
        RETURN b";

        var session = driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query, new
            {
                orderId,
                boxId = box.Id,
                length = box.Length,
                width = box.Width,
                height = box.Height,
                weight = box.Weight
            });

            var record = await result.SingleAsync();
            var updatedBoxNode = record["b"].As<INode>();
        }
        finally
        {
            await session.CloseAsync();
        }
    }
}