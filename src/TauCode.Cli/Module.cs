using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public abstract class Module : IModule
    {
        #region Fields

        private readonly Dictionary<string, IExecutor> _executors;
        private IExecutor _namelessExecutor;
        private IExecutionContext _currentExecutionContext;

        #endregion

        #region ctor

        protected Module(string name, ILexer lexer)
        {
            if (name != null)
            {
                if (!CliHelper.IsValidTerm(name))
                {
                    throw new ArgumentException($"'{name}' is not a valid module name.", nameof(name));
                }
            }

            this.Name = name;
            this.Lexer = lexer ?? throw new ArgumentNullException(nameof(lexer));

            _executors = new Dictionary<string, IExecutor>();
        }

        #endregion

        #region IModule Members

        public string Name { get; }

        public ILexer Lexer { get; }

        public IReadOnlyList<IExecutor> Executors
        {
            get
            {
                var list = _executors.Values.ToList();
                if (_namelessExecutor != null)
                {
                    list.Add(_namelessExecutor);
                }

                return list;
            }
        }

        public void AddExecutor(IExecutor executor)
        {
            if (executor == null)
            {
                throw new ArgumentNullException(nameof(executor));
            }

            if (!(executor is ExecutorBase))
            {
                throw new ArgumentException(
                    $"'{nameof(executor)}' must be an instance of '{typeof(ExecutorBase).FullName}'.",
                    nameof(executor));
            }

            if (executor.Name == null)
            {
                if (_namelessExecutor == null)
                {
                    if (_executors.Count > 0)
                    {
                        throw new InvalidOperationException("Cannot add nameless executor if a named executor was added previously.");
                    }

                    _namelessExecutor = executor;
                }
                else
                {
                    throw new InvalidOperationException("Nameless executor already added.");
                }
            }
            else
            {
                if (_namelessExecutor != null)
                {
                    throw new InvalidOperationException("Cannot add named executor if a nameless executor was added previously.");
                }

                if (_executors.ContainsKey(executor.Name))
                {
                    throw new ArgumentException($"Executor with name '{executor.Name}' already added.", nameof(executor));
                }

                _executors.Add(executor.Name, executor);
            }
        }

        public IExecutor GetExecutor(string executorName)
        {
            return executorName == null ? _namelessExecutor : _executors.GetValueOrDefault(executorName);
        }

        public abstract IExecutionContext CreateExecutionContext(ILogger logger, TextReader input, TextWriter output);

        public IExecutionContext CurrentExecutionContext
        {
            get => _currentExecutionContext;
            set
            {
                _currentExecutionContext = value;
                this.CurrentExecutionContextChanged?.Invoke(this);
            }
        }

        public event Action<IModule> CurrentExecutionContextChanged;

        #endregion
    }
}
