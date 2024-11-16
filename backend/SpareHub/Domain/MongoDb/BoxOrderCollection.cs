namespace Domain.MongoDb;

public class BoxOrderCollection
{
        public int OrderId { get; set; }
        public List<Box> Boxes { get; set; } = []; 
}
