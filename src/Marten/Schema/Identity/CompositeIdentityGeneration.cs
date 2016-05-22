using System;
using System.Collections.Generic;
using Marten.Schema.Identity.Sequences;

namespace Marten.Schema.Identity
{
    public class CompositeIdentityGeneration<TDoc> : IIdGeneration
    {
        private readonly Func<TDoc, string> _keyGen;
        private readonly HiloSettings _hiloSettings;

        public IEnumerable<Type> KeyTypes { get; } = new[] { typeof(string) };

        public int Increment => _hiloSettings.Increment;
        public int MaxLo => _hiloSettings.MaxLo;

        public CompositeIdentityGeneration(Func<TDoc, string> keyGen, HiloSettings hiloSettings)
        {
            _keyGen = keyGen;
            _hiloSettings = hiloSettings;
        }

        public IIdGenerator<T> Build<T>(IDocumentSchema schema)
        {
            return (IIdGenerator<T>)new CompositeIdentityGenerator<TDoc>(_keyGen, schema.Sequences.Hilo(typeof(T), _hiloSettings));
        }
    }
}