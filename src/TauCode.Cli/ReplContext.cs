using System;
using TauCode.Data.Text.TextDataExtractors;

namespace TauCode.Cli
{
    public class ReplContext
    {
        private readonly KeyExtractor _keyExtractor;
        private readonly TermExtractor _termExtractor;

        private ReadOnlyMemory<char> _input;

        public ReplContext()
        {
            _keyExtractor = new KeyExtractor(CliHelper.IsCliWhiteSpace);
            _termExtractor = new TermExtractor(CliHelper.IsCliWhiteSpace);
        }

        public int Position;

        public IApp App { get; set; }
        public IModule Module { get; set; }
        public IExecutor Executor { get; set; }
        public IExecutionContext ExecutionContext { get; set; }

        public void Reset(string input)
        {
            _input = input.AsMemory();
            this.Position = 0;

            this.App = null;
            this.Module = null;
            this.Executor = null;
            this.ExecutionContext = null;
        }

        public void ResetPosition()
        {
            this.Position = 0;
        }

        public void SkipWhiteSpace()
        {
            var span = _input.Span[this.Position..];
            if (span.Length == 0)
            {
                return;
            }

            var pos = 0;

            while (CliHelper.IsCliWhiteSpace(span, pos))
            {
                pos++;
            }

            this.Position += pos;
        }

        public string TryExtractTerm()
        {
            this.SkipWhiteSpace();
            var span = _input.Span[this.Position..];

            var result = _termExtractor.TryExtract(span, out var term);
            if (result.ErrorCode.HasValue)
            {
                return null;
            }

            this.Position += result.CharsConsumed;

            this.SkipWhiteSpace();

            return term;
        }

        public string TryExtractKey()
        {
            this.SkipWhiteSpace();
            var span = _input.Span[this.Position..];

            var result = _keyExtractor.TryExtract(span, out var key);
            if (result.ErrorCode.HasValue)
            {
                return null;
            }

            this.Position += result.CharsConsumed;

            this.SkipWhiteSpace();

            return key;
        }

        public ReadOnlyMemory<char> RemainingInput => _input[this.Position..];
    }
}
