using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli
{
    public class AppHost : IAppHost
    {
        #region ctor

        public AppHost(IApp app)
        {
            this.App = app ?? throw new ArgumentNullException(nameof(app));
        }

        #endregion

        #region Private

        private void RunModule(
            IModule module,
            string[] args)
        {
            var namelessExecutor = module.GetExecutor(null);

            if (namelessExecutor == null)
            {
                if (args.Length == 0)
                {
                    throw new NotImplementedException("error");
                }

                var executorName = args[0];
                var executor = module.GetExecutor(executorName);

                if (executor == null)
                {
                    throw new NotImplementedException("error");
                }

                this.RunExecutor(module, executor, args.Skip(1).ToArray());
            }
            else
            {
                this.RunExecutor(module, namelessExecutor, args);
            }
        }

        private void RunExecutor(
            IModule module,
            IExecutor executor,
            string[] args)
        {
            var input = string.Join(" ", args);

            var executionContext = module.CreateExecutionContext(this.Logger, this.Input, this.Output);
            module.CurrentExecutionContext = executionContext;

            var tokens = module.Lexer.Tokenize(input.AsMemory());

            executor.Execute(tokens, executionContext);
        }

        private async Task RunModuleAsync(
            IModule module,
            string[] args,
            CancellationToken cancellationToken)
        {
            var namelessExecutor = module.GetExecutor(null);

            if (namelessExecutor == null)
            {
                if (args.Length == 0)
                {
                    throw new NotImplementedException("error");
                }

                var executorName = args[0];
                var executor = module.GetExecutor(executorName);

                if (executor == null)
                {
                    throw new NotImplementedException("error");
                }

                await this.RunExecutorAsync(module, executor, args.Skip(1).ToArray(), cancellationToken);
            }
            else
            {
                await this.RunExecutorAsync(module, namelessExecutor, args, cancellationToken);
            }
        }

        private async Task RunExecutorAsync(
            IModule module,
            IExecutor executor,
            string[] args,
            CancellationToken cancellationToken)
        {
            var input = string.Join(" ", args);

            var executionContext = module.CreateExecutionContext(this.Logger, this.Input, this.Output);
            module.CurrentExecutionContext = executionContext;

            var tokens = module.Lexer.Tokenize(input.AsMemory());

            await executor.ExecuteAsync(tokens, executionContext, cancellationToken);
        }

        #endregion

        #region IAppHost Members

        public IApp App { get; }

        public ILogger Logger { get; set; }

        public TextReader Input { get; set; }

        public TextWriter Output { get; set; }

        public virtual void Run(string[] args)
        {
            var namelessModule = this.App.GetModule(null);

            if (namelessModule == null)
            {
                if (args.Length == 0)
                {
                    throw new NotImplementedException("error");
                }

                var moduleName = args[0];
                var module = this.App.GetModule(moduleName);
                if (module == null)
                {
                    throw new NotImplementedException("error");
                }

                this.RunModule(module, args.Skip(1).ToArray());
            }
            else
            {
                this.RunModule(namelessModule, args);
            }
        }

        public virtual async Task RunAsync(string[] args, CancellationToken cancellationToken = default)
        {
            var namelessModule = this.App.GetModule(null);

            if (namelessModule == null)
            {
                if (args.Length == 0)
                {
                    throw new NotImplementedException("error");
                }

                var moduleName = args[0];
                var module = this.App.GetModule(moduleName);
                if (module == null)
                {
                    throw new NotImplementedException("error");
                }

                await this.RunModuleAsync(module, args.Skip(1).ToArray(), cancellationToken);
            }
            else
            {
                await this.RunModuleAsync(namelessModule, args, cancellationToken);
            }
        }

        #endregion
    }
}
