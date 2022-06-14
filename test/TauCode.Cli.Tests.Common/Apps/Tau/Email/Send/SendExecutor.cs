using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Nodes;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email.Send
{
    public class SendExecutor : TestExecutorBase
    {
        public SendExecutor()
            : base(
                "send",
                TauHelper.BuildParsingGraph($".{nameof(SendExecutor)}.lisp"))
        {
            ILexicalTokenConverter tokenConverter = new SendTokenConverter();

            var attachmentKeyNode = (CustomKeyNode)this.Graph.Single(x => x.Name == "attachment-key");
            attachmentKeyNode.Action = AttachmentKeyAction;

            var attachmentFilePathNode = (ActionNode)this.Graph.Single(x => x.Name == "attachment-file-path");
            attachmentFilePathNode.Action = AttachmentFilePathAction;

            var attachmentFileLocalNameNode = (TextNode)this.Graph.Single(x => x.Name == "attachment-file-local-name");
            attachmentFileLocalNameNode.Action = AttachmentFileLocalNameAction;
            attachmentFileLocalNameNode.TokenConverter = tokenConverter;

            var attachmentFileIsInlineNode = (BooleanNode)this.Graph.Single(x => x.Name == "attachment-file-is-inline");
            attachmentFileIsInlineNode.Action = AttachmentFileIsInlineNodeAction;
            attachmentFileIsInlineNode.TokenConverter = tokenConverter;
        }

        private static void AttachmentKeyAction(ActionNode node, ParsingContext parsingContext)
        {
            var sendCommand = (SendCommand)parsingContext.ParsingResult;
            sendCommand.Attachments.Add(new SendCommand.AttachmentInfo());

            sendCommand.IncreaseVersion();
        }

        private static void AttachmentFilePathAction(ActionNode node, ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();
            var parsingResult = parsingContext.ParsingResult;

            var textToken = (TextTokenBase)token;

            var sendCommand = (SendCommand)parsingResult;
            var attachment = sendCommand.Attachments.Last();
            attachment.SourceFilePath = textToken.Text;

            parsingResult.IncreaseVersion();
        }

        private static void AttachmentFileLocalNameAction(ActionNode node, ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();
            var parsingResult = parsingContext.ParsingResult;

            var textToken = (TextTokenBase)token;

            var sendCommand = (SendCommand)parsingResult;
            var attachment = sendCommand.Attachments.Last();
            attachment.LocalName = textToken.Text;

            parsingResult.IncreaseVersion();
        }

        private static void AttachmentFileIsInlineNodeAction(ActionNode node, ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();
            var parsingResult = parsingContext.ParsingResult;

            var booleanToken = (BooleanToken)token;

            var sendCommand = (SendCommand)parsingResult;
            var attachment = sendCommand.Attachments.Last();
            attachment.IsInline = booleanToken.Value;

            parsingResult.IncreaseVersion();
        }

        protected override Command CreateCommand() => new SendCommand(this.Name);

        protected override void ExecuteImpl(Command command, IExecutionContext executionContext)
        {
            var dto = ((SendCommand)command).ToSendCommandDto();
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            executionContext.Output.WriteLine("Running sync");
            executionContext.Output.WriteLine(json);
        }

        protected override async Task ExecuteImplAsync(
            Command command,
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            var dto = ((SendCommand)command).ToSendCommandDto();
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            await executionContext.Output.WriteLineAsync("Running async");
            await executionContext.Output.WriteLineAsync(json.AsMemory(), cancellationToken);
        }
    }
}
