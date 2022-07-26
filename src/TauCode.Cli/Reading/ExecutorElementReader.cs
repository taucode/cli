using TauCode.Parsing.Graphs;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli.Reading
{
    internal class ExecutorElementReader : GroupReader
    {
        internal ExecutorElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            var groupMold = (GroupMold)scriptElementMold;

            var entrance = new VertexMold(groupMold, Symbol.Create("idle"))
            {
                Name = "parsing-top", // todo store in keyword values
                IsEntrance = true,
            };

            var exit = new VertexMold(groupMold, Symbol.Create("idle"))
            {
                IsExit = true,
            };

            var linkables = groupMold.Linkables;

            for (var i = 0; i < linkables.Count; i++)
            {
                var linkable = linkables[i];
                if (i < linkables.Count - 1)
                {
                    var next = linkables[i + 1];

                    linkable.GetExitVertexOrThrow().AddLinkTo(next.GetEntranceVertexOrThrow());
                }
            }

            entrance.AddLinkTo(linkables[0].GetEntranceVertexOrThrow());
            linkables[^1].GetExitVertexOrThrow().AddLinkTo(exit);


            groupMold.Add(entrance);
            groupMold.Add(exit);
        }
    }
}
