using Serilog;
using System.Data;
using System.IO;
using System.Text;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db
{
    public class DbExecutionContext : TestExecutionContextBase
    {
        public DbExecutionContext(
            ILogger logger,
            TextReader input,
            TextWriter output,
            IDbConnection connection)
            : base(logger, input, output)
        {
            this.Connection = connection;
        }

        public IDbConnection Connection { get; set; }

        protected override void DisposeImpl()
        {
            this.Connection?.Dispose();
        }

        public override string GetDescription()
        {
            var sb = new StringBuilder();

            if (this.Connection == null)
            {
                sb.Append("Connection is null.");
            }
            else
            {
                sb.AppendLine("Connection info");
                sb.AppendLine($"Type     : {this.Connection.GetType().FullName}");
                sb.AppendLine($"String   : {this.Connection.ConnectionString}");
                sb.AppendLine($"Database : {this.Connection.Database}");
                sb.AppendLine($"State    : {this.Connection.State}");
            }

            return sb.ToString();
        }
    }
}
