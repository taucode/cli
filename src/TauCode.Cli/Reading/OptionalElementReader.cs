using TauCode.Cli.Exceptions;
using TauCode.Parsing.Graphs;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli.Reading
{
    public class OptionalElementReader : GroupReader
    {
        public OptionalElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            var groupMold = (GroupMold)scriptElementMold;

            var linkables = groupMold.Linkables;
            if (linkables.Count != 1)
            {
                throw new CliException("'optional' wants exactly one child.");
            }

            var linkable = linkables[0];
            var optionalEntrance = new VertexMold(groupMold, Symbol.Create("idle"))
            {
                IsEntrance = true,
            };

            var optionalExit = new VertexMold(groupMold, Symbol.Create("idle"))
            {
                IsExit = true,
            };

            optionalEntrance.AddLinkTo(linkable.GetEntranceVertexOrThrow());
            optionalEntrance.AddLinkTo(optionalExit);

            linkable.GetExitVertexOrThrow().AddLinkTo(optionalExit);

            groupMold.Add(optionalEntrance);
            groupMold.Add(optionalExit);
        }
    }
}
