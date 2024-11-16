using Domain.MySql;

namespace Domain.MongoDb;

public class BoxOrderCollection
{
        public int OrderId { get; set; }
        public List<BoxEntity> Boxes { get; set; } = []; 
}
