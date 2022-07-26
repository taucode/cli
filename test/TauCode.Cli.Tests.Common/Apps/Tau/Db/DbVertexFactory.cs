using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db
{
    public class DbVertexFactory : CliVertexFactory
    {
        public DbVertexFactory()
            : base(new DbTokenTypeResolver())
        {
        }

        public override IVertex Create(IVertexMold vertexMold)
        {
            if (vertexMold.Car is Symbol symbol)
            {
                switch (symbol.Name)
                {
                    case "ANY":
                        return new AnyTokenNode
                        {
                            Name = vertexMold.Name,
                        };
                }
            }

            return base.Create(vertexMold);
        }
    }
}
