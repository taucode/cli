using TauCode.Data.Text;

namespace TauCode.Cli.Extractors;

public class WinCmdStringExtractor : TextDataExtractorBase<string>
{
    private const int MaxPathLength = 260;

    public WinCmdStringExtractor()
        : base(MaxPathLength, CliHelper.IsCliWhiteSpace)
    {
    }

    protected override TextDataExtractionResult TryExtractImpl(ReadOnlySpan<char> input, out string? value)
    {
        var pos = 0;
        value = default;

        if (input.Length == 0)
        {
            return new TextDataExtractionResult(0, TextDataExtractionErrorCodes.UnexpectedEnd);
        }

        while (true)
        {
            if (pos == input.Length)
            {
                return new TextDataExtractionResult(pos, TextDataExtractionErrorCodes.UnclosedString);
            }

            var c = input[pos];

            if (pos == 0)
            {
                if (c != '"')
                {
                    return new TextDataExtractionResult(0, TextDataExtractionErrorCodes.UnexpectedCharacter);
                }
            }
            else
            {
                if (c == '"')
                {
                    pos++;
                    break;
                }
            }

            pos++;

            if (this.IsOutOfCapacity(pos))
            {
                return new TextDataExtractionResult(pos, TextDataExtractionErrorCodes.InputIsTooLong);
            }
        }

        if (pos == 0)
        {
            return new TextDataExtractionResult(0, TextDataExtractionErrorCodes.UnexpectedEnd);
        }

        value = input[..pos].ToString();
        return new TextDataExtractionResult(pos, null);
    }
}
