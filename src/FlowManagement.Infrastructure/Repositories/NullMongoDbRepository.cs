using FlowManagement.Infrastructure.Repositories;

public class NullMongoDbRepository<T> : MongoDbRepository<T> where T : class
{
  public NullMongoDbRepository() : base(null!, null!) { }

}
