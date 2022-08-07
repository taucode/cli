using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli.Reading;

internal class KeyValueElementReader : ScriptElementReaderBase
{
    internal KeyValueElementReader(IGraphScriptReader scriptReader)
        : base(scriptReader)
    {
    }

    protected override IScriptElementMold CreateScriptElementMold(IGroupMold owner, Element element)
    {
        return new GroupMold(owner, element.GetCar<Atom>());
    }

    protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
    {
        var keyValuePairGroupMold = (GroupMold)scriptElementMold;

        var keyVertexMold = new VertexMold(keyValuePairGroupMold, Symbol.Create("key"));
        keyVertexMold.IsEntrance = true;
        keyVertexMold.SetKeywordValue(":KEYS", keyValuePairGroupMold.GetKeywordValue(":KEYS"));
        keyVertexMold.SetKeywordValue(":ALIAS", keyValuePairGroupMold.GetKeywordValue(":ALIAS"));

        var valueVertexMold = new VertexMold(keyValuePairGroupMold, Symbol.Create("VALUE"));
        valueVertexMold.IsExit = true;
        valueVertexMold.SetKeywordValue(":ALIAS", keyValuePairGroupMold.GetKeywordValue(":ALIAS"));
        valueVertexMold.SetKeywordValue(":TOKEN-TYPES", keyValuePairGroupMold.GetKeywordValue(":TOKEN-TYPES"));

        keyVertexMold.AddLinkTo(valueVertexMold);

        keyValuePairGroupMold.Add(keyVertexMold);
        keyValuePairGroupMold.Add(valueVertexMold);
    }

    protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
    {
        // no content to read
    }
}