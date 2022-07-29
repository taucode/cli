using TauCode.Cli.Nodes;
using TauCode.Data.Graphs;
using TauCode.Parsing;
using TauCode.Parsing.Graphs;
using TauCode.Parsing.Graphs.Building;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli;

public class CliVertexFactory : IVertexFactory
{
    public CliVertexFactory(ILexicalTokenTypeResolver tokenTypeResolver) // todo: Func might be enough.
    {
        this.TokenTypeResolver = tokenTypeResolver;
    }

    public ILexicalTokenTypeResolver TokenTypeResolver { get; }

    public virtual IVertex Create(IVertexMold vertexMold)
    {
        IParsingNode node;
        if (vertexMold.Car is Symbol symbol)
        {
            switch (symbol.Name)
            {
                case "CUSTOM-KEY":
                    node = new CustomKeyNode(
                        vertexMold.GetKeywordValue<List<string>>(":KEYS"));
                    break;

                case "KEY":
                    node = new KeyNode(
                        vertexMold.GetKeywordValue<string>(":ALIAS", null),
                        vertexMold.GetKeywordValue<List<string>>(":KEYS"));
                    break;

                case "VALUE":
                    node = new KeyValueNode(
                        vertexMold.GetKeywordValue<string>(":ALIAS"),
                        vertexMold
                            .GetKeywordValue<List<string>>(":TOKEN-TYPES")
                            .Select(x => this.TokenTypeResolver.Resolve(x)));
                    break;

                case "BOOLEAN":
                    node = new BooleanNode();
                    break;

                case "ARGUMENT":
                    node = new ArgumentNode(
                        vertexMold.GetKeywordValue<string>(":ALIAS"),
                        vertexMold
                            .GetKeywordValue<List<string>>(":TOKEN-TYPES")
                            .Select(x => this.TokenTypeResolver.Resolve(x)));
                    break;

                case "SOME-TEXT":
                    node = new TextNode(vertexMold
                        .GetKeywordValue<List<string>>(":TOKEN-TYPES")
                        .Select(x => this.TokenTypeResolver.Resolve(x)));
                    break;

                case "SWITCH":
                    node = new SwitchNode(
                        vertexMold.GetKeywordValue<string>(":ALIAS"),
                        vertexMold.GetKeywordValue<List<string>>(":KEYS"));
                    break;

                case "IDLE":
                    node = new IdleNode();
                    break;

                case "END":
                    node = new EndNode();
                    break;

                case "FALLBACK":
                    node = new FallbackNode();
                    break;

                default:
                    throw new NotImplementedException($"error: unknown car for creation: '{symbol.Name}'.");
            }
        }
        else
        {
            throw new NotImplementedException("error");
        }

        node.Name = vertexMold.Name;
        return node;
    }
}