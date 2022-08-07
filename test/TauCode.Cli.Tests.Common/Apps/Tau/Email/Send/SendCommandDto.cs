using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email.Send;

public class SendCommandDto : TestCommandDto
{
    public class AttachmentInfoDto
    {
        public string? SourceFilePath { get; set; }
        public string? LocalName { get; set; }
        public bool IsInline { get; set; }
    }

    public List<AttachmentInfoDto> Attachments { get; set; } = new();
}