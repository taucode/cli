using TauCode.Parsing.Graphs;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli.Reading
{
    internal class RepeatableElementReader : GroupReader
    {
        internal RepeatableElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            var groupMold = (GroupMold)scriptElementMold;

            var splitterNode = new VertexMold(groupMold, Symbol.Create("idle"))
            {
                IsEntrance = true,
            };

            var jointNode = new VertexMold(groupMold, Symbol.Create("idle"))
            {
                IsExit = true,
            };

            foreach (var linkable in groupMold.Linkables)
            {
                var linkableEntrance = linkable.GetEntranceVertexOrThrow();
                var linkableExit = linkable.GetExitVertexOrThrow();

                splitterNode.AddLinkTo(linkableEntrance);
                linkableExit.AddLinkTo(splitterNode);
                linkableExit.AddLinkTo(jointNode);
            }

            splitterNode.AddLinkTo(jointNode);

            groupMold.Add(splitterNode);
            groupMold.Add(jointNode);
        }
    }
}
