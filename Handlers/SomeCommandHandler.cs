using Marten;

using WolverineConcurrency.Model;

namespace WolverineConcurrency.Handlers;

public record SomeCommand;
public record TheReturnValue(Guid Id);

public class SomeCommandHandler
{
  static readonly Guid TheId = Guid.NewGuid();

  public async Task<TheReturnValue> Handle(SomeCommand command, IDocumentSession session)
  {
    var agg = await session.Events.FetchForWriting<SomeAggregate>(id: TheId);

    agg.AppendOne(new SomeCreationEvent(Id: TheId));

    return new TheReturnValue(Id: TheId);
  }
}
