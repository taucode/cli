﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TauCode.Cli.Exceptions;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;

namespace TauCode.Cli
{
    public abstract class CliAddInBase : CliFunctionalityProviderBase, ICliAddIn
    {
        #region Fields

        private readonly INodeFamily _nodeFamily;
        private readonly List<ICliWorker> _workers;

        #endregion

        #region Constructor

        protected CliAddInBase(string name, string version, bool supportsHelp)
            : base(name, version, supportsHelp)
        {
            if (name == null)
            {
                if (version != null)
                {
                    throw new ArgumentException("Nameless add-in cannot have version.", nameof(version)); // todo ut
                }

                if (supportsHelp)
                {
                    throw new ArgumentException("Nameless add-in cannot have version.", nameof(version)); // todo ut
                }
            }

            _nodeFamily = new NodeFamily($"Add-in node family: {this.Name ?? string.Empty}. Add-in type is '{this.GetType().FullName}'.");
            _workers = new List<ICliWorker>();
        }

        protected CliAddInBase()
            : this(null, null, false)
        {
        }

        #endregion

        #region Overridden

        public override TextWriter Output
        {
            get => this.Host.Output;
            set => throw new NotSupportedException($"Use host's '{nameof(Output)}'.");
        }

        public override TextReader Input
        {
            get => this.Host.Input;
            set => throw new NotSupportedException($"Use host's '{nameof(Output)}'.");
        }

        protected override string GetHelpImpl()
        {
            var dummy = this.Node;

            if (this.Name != null)
            {
                return "todo: write about myself.";
            }

            if (_workers[0].Name == null)
            {
                // single unnamed worker
                try
                {
                    return _workers.Single().GetHelp();
                }
                catch
                {
                    return "Help is not supported.";
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine(this.Name);

            foreach (var worker in _workers)
            {
                sb.Append(worker.Name);
                var workerDescription = "";
                try
                {
                    workerDescription = worker.Descriptor.Description;
                }
                catch
                {
                    // dismiss
                }

                sb.Append($" {workerDescription}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        protected override INode CreateNodeTree()
        {
            INode addInNode;

            if (this.Name == null)
            {
                addInNode = new IdleNode(
                    _nodeFamily,
                    $"Root node of nameless '{this.GetType().FullName}' add-in");
            }
            else
            {
                addInNode = new ExactTextNode(
                    this.Name,
                    TermTextClass.Instance,
                    true,
                    this.ProcessAddInName,
                    _nodeFamily,
                    $"Root node of '{this.Name}' add-in");

                addInNode.Properties["add-in-name"] = this.Name;
            }

            var workers = this.CreateWorkers();

            if (workers == null)
            {
                throw new CliException($"'{nameof(CreateWorkers)}' must not return null.");
            }

            if (workers.Count == 0)
            {
                throw new CliException($"'{nameof(CreateWorkers)}' must not return empty collection.");
            }

            var validTypes = workers.All(x => x is CliWorkerBase);
            if (!validTypes)
            {
                throw new CliException($"'{nameof(CreateWorkers)}' must return instances of type '{typeof(CliWorkerBase).FullName}'.");
            }

            if (workers.Any(x => x.Name == null) && workers.Count > 1)
            {
                throw new CliException($"'{nameof(CreateWorkers)}' must return either all workers having non-null name, or exactly one worker with null name.");
            }

            foreach (var worker in workers)
            {
                ((CliWorkerBase)worker).AddIn = this;
            }

            _workers.AddRange(workers);

            foreach (var worker in workers)
            {
                addInNode.EstablishLink(worker.Node);
            }

            return addInNode;
        }

        protected override void OnNodeCreated()
        {
            if (this.Version != null)
            {
                this.AddVersion();
            }

            if (this.SupportsHelp)
            {
                this.AddHelp();
            }
        }

        #endregion

        #region Private

        private void ProcessAddInName(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            resultAccumulator.AddAddInCommand(node.Properties["add-in-name"]);
        }

        #endregion

        #region Protected

        protected abstract IReadOnlyList<ICliWorker> CreateWorkers();

        #endregion

        #region ICliAddIn Members

        public ICliHost Host { get; internal set; }

        public IReadOnlyList<ICliWorker> GetWorkers()
        {
            if (_workers.Count == 0)
            {
                var dummy = this.Node;
            }

            return _workers;
        }

        #endregion
    }
}
