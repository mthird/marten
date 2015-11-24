using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using FubuCore;
using Marten.Schema;
using Newtonsoft.Json;
using Npgsql;

namespace Marten.Linq
{
    public class ContainmentWhereFragment : IWhereFragment
    {
        private readonly ISerializer _serializer;
        private IDictionary<string, object> _dictionary; 

        public ContainmentWhereFragment(ISerializer serializer, BinaryExpression binary)
        {
            _serializer = serializer;


            var visitor = new FindMembers();
            visitor.Visit(binary.Left);

            var members = visitor.Members;

            if (members.Count > 1)
            {
                var dict = new Dictionary<string, object>();
                var member = members.Last();
                var value = MartenExpressionParser.Value(binary.Right);
                dict.Add(member.Name, value);

                members.Reverse().Skip(1).Each(m =>
                {
                    dict = new Dictionary<string, object> { { m.Name, dict}};
                });

                _dictionary = dict;
            }
            else
            {
                _dictionary = new Dictionary<string, object>();

                var member = members.Single();
                var value = MartenExpressionParser.Value(binary.Right);
                _dictionary.Add(member.Name, value);


            }
            
        }

        public string ToSql(NpgsqlCommand command)
        {
            var json = _serializer.ToCleanJson(_dictionary);

            return  $"data @> '{json}'";
        }
    }
}