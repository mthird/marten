using System;
using Marten.Schema.Identity.Sequences;

namespace Marten.Schema.Identity
{
    public class CompositeIdentityGenerator<TDoc> : IIdGeneratorEx<TDoc, string>
    {
        public Func<TDoc, string> KeyGenerator { get; set; }
        public ISequence Sequence { get; }

        public CompositeIdentityGenerator(Func<TDoc, string> keyGen, ISequence sequence)
        {
            KeyGenerator = keyGen;
            Sequence = sequence;
        }

        public string Assign(string existing, out bool assigned)
        {
            throw new NotImplementedException("IIdGenerator<TId>.Assign should not be called");
        }

        public string Assign(TDoc document, out bool assigned)
        {
            if (KeyGenerator == null)
            {
                throw new Exception("keyGen function is not set");
            }

            var id = KeyGenerator.Invoke(document) ?? String.Empty;

            // get the next sequence number if the existing id ends with /
            if (String.IsNullOrEmpty(id) || id.EndsWith("/"))
            {
                // use hilogenerator
                assigned = true;

                return id + Sequence.NextLong();
            }

            assigned = false;
            return id;
        }
    }
}