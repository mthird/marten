namespace Marten.Schema.Identity
{
    public interface IIdGenerator<TId>
    {
        TId Assign(TId existing, out bool assigned);
    }

    public interface IIdGeneratorEx<in TDoc, TId> : IIdGenerator<TId>
    {
        TId Assign(TDoc document, TId existing, out bool assigned);
    }
}