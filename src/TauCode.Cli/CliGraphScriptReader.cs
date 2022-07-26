using System;
using System.Linq;
using TauCode.Cli.Reading;
using TauCode.Data.Graphs;
using TauCode.Parsing;
using TauCode.Parsing.Graphs.Building;
using TauCode.Parsing.Graphs.Building.Impl;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli
{
    // todo regions
    public class CliGraphScriptReader : GraphScriptReader
    {
        private readonly ExecutorElementReader _executorElementReader;
        private readonly RepeatableElementReader _repeatableElementReader;
        private readonly KeyValueElementReader _keyValueElementReader;
        private readonly KeyMultiValueElementReader _keyMultiValueElementReader;
        private readonly VertexReader _vertexElementReader;
        private readonly OptionalElementReader _optionalElementReader;
        private readonly AlternativesElementReader _alternativesGroupReader;

        private readonly IGraphBuilder _graphBuilder;

        public CliGraphScriptReader(IVertexFactory vertexFactory)
        {
            _executorElementReader = new ExecutorElementReader(this);
            _repeatableElementReader = new RepeatableElementReader(this);
            _keyValueElementReader = new KeyValueElementReader(this);
            _keyMultiValueElementReader = new KeyMultiValueElementReader(this);
            _vertexElementReader = new VertexReader(this);
            _optionalElementReader = new OptionalElementReader(this);
            _alternativesGroupReader = new AlternativesElementReader(this);

            _graphBuilder = new GraphBuilder(vertexFactory);
        }

        public override IScriptElementReader ResolveElementReader(Atom car)
        {
            if (car is Symbol symbol)
            {
                switch (symbol.Name)
                {
                    case "EXECUTOR":
                        return _executorElementReader;

                    case "REPEATABLE":
                        return _repeatableElementReader;

                    case "KEY-VALUE":
                        return _keyValueElementReader;

                    case "KEY-MULTI-VALUE":
                        return _keyMultiValueElementReader;

                    case "ARGUMENT":
                    case "SWITCH":
                    case "END":
                    case "KEY":
                    case "SOME-TEXT":
                    case "FALLBACK":
                    case "BOOLEAN":
                    case "CUSTOM-KEY":
                    case "ANY":
                        return _vertexElementReader;

                    case "OPTIONAL":
                        return _optionalElementReader;

                    case "ALTERNATIVES":
                        return _alternativesGroupReader;
                }
            }

            return base.ResolveElementReader(car);
        }

        public IGraph BuildGraph(string script)
        {
            // todo checks

            var groupMold = this.ReadScript(script.AsMemory());
            var graph = _graphBuilder.Build(groupMold);

            return graph;

        }

        public IParsingNode ResolveParsingNode(IGraph graph)
        {
            var node = (IParsingNode)graph.Single(x => x.Name == "parsing-top");
            return node;
        }

        public IParsingNode BuildNode(string script)
        {
            // todo checks
            var graph = this.BuildGraph(script);
            var node = this.ResolveParsingNode(graph);
            return node;
        }
    }
}

