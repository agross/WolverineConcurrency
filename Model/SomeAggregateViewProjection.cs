using Marten.Events.Aggregation;

namespace WolverineConcurrency.Model;

public record SomeAggregateView(Guid Id, int EventCount = 0);

public class SomeAggregateViewProjection : SingleStreamAggregation<SomeAggregateView>
{
  public static SomeAggregateView Create(SomeCreationEvent ev) => new(Id: ev.Id);
}
