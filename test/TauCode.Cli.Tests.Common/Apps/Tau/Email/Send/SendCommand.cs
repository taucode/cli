using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email.Send
{
    public class SendCommand : Command
    {
        public class AttachmentInfo
        {
            public string SourceFilePath { get; set; }
            public string LocalName { get; set; }
            public bool IsInline { get; set; }
        }

        public SendCommand(string executorName)
            : base(executorName)
        {
        }

        public List<AttachmentInfo> Attachments { get; } = new List<AttachmentInfo>();
    }
}
