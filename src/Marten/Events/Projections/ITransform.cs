namespace Marten.Events.Projections
{
    // SAMPLE: ITransform
    public interface ITransform<TEvent, TView>
    {
        TView Transform(Event<TEvent> input);
    }
    // ENDSAMPLE
}