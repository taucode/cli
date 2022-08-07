using TauCode.Cli.Reading;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli;

public class CliGraphScriptReader : GraphScriptReader
{
    #region Fields

    private readonly ExecutorElementReader _executorElementReader;
    private readonly RepeatableElementReader _repeatableElementReader;
    private readonly KeyValueElementReader _keyValueElementReader;
    private readonly KeyMultiValueElementReader _keyMultiValueElementReader;
    private readonly VertexReader _vertexElementReader;
    private readonly OptionalElementReader _optionalElementReader;
    private readonly AlternativesElementReader _alternativesGroupReader;

    #endregion

    #region ctor

    public CliGraphScriptReader()
    {
        _executorElementReader = new ExecutorElementReader(this);
        _repeatableElementReader = new RepeatableElementReader(this);
        _keyValueElementReader = new KeyValueElementReader(this);
        _keyMultiValueElementReader = new KeyMultiValueElementReader(this);
        _vertexElementReader = new VertexReader(this);
        _optionalElementReader = new OptionalElementReader(this);
        _alternativesGroupReader = new AlternativesElementReader(this);
    }

    #endregion

    #region Overridden

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
                    return _vertexElementReader;

                case "OPTIONAL":
                    return _optionalElementReader;

                case "ALTERNATIVES":
                    return _alternativesGroupReader;
            }
        }

        return base.ResolveElementReader(car);
    }

    #endregion
}