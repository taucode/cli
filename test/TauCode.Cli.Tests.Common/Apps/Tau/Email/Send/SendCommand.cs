using System;
using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email.Send;

public class SendCommand : TestCommand
{
    public class AttachmentInfo
    {
        public string SourceFilePath { get; set; } = null!;
        public string LocalName { get; set; } = null!;
        public bool IsInline { get; set; }
    }

    public SendCommand(string? executorName, ReadOnlyMemory<char> input)
        : base(executorName, input)
    {
    }

    public List<AttachmentInfo> Attachments { get; } = new();
}