using System.Linq;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email.Send;

internal static class SendHelper
{
    internal static SendCommandDto ToSendCommandDto(this SendCommand command)
    {
        return new SendCommandDto
        {
            ExecutorName = command.ExecutorName,

            Arguments = command.Arguments
                .ToDictionary(
                    x => x.Key,
                    y => y.Value
                        .Select(x => x.ToString())
                        .ToList()),

            KeyValues = command.KeyValues
                .ToDictionary(
                    x => x.Key,
                    y => y.Value
                        .Select(x => x.ToString())
                        .ToList()),

            Switches = command.Switches
                .ToList(),

            Attachments = command.Attachments
                .Select(x => new SendCommandDto.AttachmentInfoDto
                {
                    SourceFilePath = x.SourceFilePath,
                    LocalName = x.LocalName,
                    IsInline = x.IsInline,
                })
                .ToList(),
        };
    }
}