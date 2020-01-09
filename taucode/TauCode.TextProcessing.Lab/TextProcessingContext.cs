﻿using System;
using System.Collections.Generic;

namespace TauCode.TextProcessing.Lab
{
    public class TextProcessingContext
    {
        #region Nested

        private class Generation
        {
            private readonly TextProcessingContext _holder;

            public Generation(TextProcessingContext holder)
            {
                _holder = holder;


                this.StartingIndex = holder.GetAbsoluteIndex();
                this.LocalIndex = 0;
                this.CurrentLine = holder.GetCurrentLine();
                this.CurrentColumn = holder.GetCurrentColumn();
            }

            public int StartingIndex { get; private set; }
            public int LocalIndex { get; private set; }
            public int CurrentLine { get; private set; }
            public int CurrentColumn { get; private set; }

            public int GetAbsoluteIndex() => this.StartingIndex + this.LocalIndex;

            public void Advance(int indexShift, int lineShift, int currentColumn)
            {
                this.LocalIndex += indexShift;
                this.CurrentLine += lineShift;
                this.CurrentColumn = currentColumn;

                _holder._version++;
            }
        }

        #endregion

        #region Fields

        private readonly Stack<Generation> _generations;
        private int _version;

        #endregion

        #region Constructor

        public TextProcessingContext(string text)
        {
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
            _generations = new Stack<Generation>();
            var rootGeneration = new Generation(this);
            _generations.Push(rootGeneration);
            _version = 1;
        }

        #endregion

        #region Private

        private Generation GetLastGeneration()
        {
            if (_generations.Count == 0)
            {
                return null;
            }

            return _generations.Peek();
        }

        #endregion

        #region Public

        public string Text { get; }

        public void RequestGeneration()
        {
            var generation = new Generation(this);
            _generations.Push(generation);
        }

        public void ReleaseGeneration()
        {
            // todo checks
            _generations.Pop();
        }

        public int Depth => _generations.Count;
        public int Version => _version;

        public int GetCurrentLine() => this.GetLastGeneration()?.CurrentLine ?? 0;

        public int GetAbsoluteIndex() => this.GetLastGeneration()?.GetAbsoluteIndex() ?? 0;

        public int GetCurrentColumn() => this.GetLastGeneration()?.CurrentColumn ?? 0;

        public int GetStartingIndex() => this.GetLastGeneration()?.StartingIndex ?? 0;

        public int GetLocalIndex()
        {
            var lastGeneration = _generations.Peek();
            var localIndex = lastGeneration.LocalIndex;

            return localIndex;
        }

        public bool IsEnd()
        {
            // todo checks
            var lastGeneration = _generations.Peek();
            var absoluteIndex = lastGeneration.StartingIndex + lastGeneration.LocalIndex;
            if (absoluteIndex > this.Text.Length)
            {
                throw new NotImplementedException();
            }

            return absoluteIndex == this.Text.Length;
        }

        public void Advance(int indexShift, int lineShift, int currentColumn)
        {
            // todo temp wtf
            if (lineShift > 0)
            {
                throw new NotImplementedException();
            }

            this.GetLastGeneration().Advance(indexShift, lineShift, currentColumn);
        }

        public char GetCurrentChar()
        {
            // todo checks
            var absoluteIndex = this.GetAbsoluteIndex();
            return this.Text[absoluteIndex];
        }

        public void AdvanceByChar()
        {
            // todo checks
            this.Advance(1, 0, this.GetCurrentColumn() + 1);
        }

        public char? GetPreviousAbsoluteChar()
        {
            // todo: checks
            var absoluteIndex = this.GetAbsoluteIndex();
            if (absoluteIndex == 0)
            {
                return null;
            }

            return this.Text[absoluteIndex - 1];
        }

        #endregion
    }

    public static class TextProcessingContextExtensions
    {
        public static void ReleaseGenerationAndGetMetrics(
            this TextProcessingContext context,
            out int indexShift,
            out int lineShift,
            out int currentColumn)
        {
            // todo checks on context's generations.

            var newIndex = context.GetAbsoluteIndex();
            var newLine = context.GetCurrentLine();
            currentColumn = context.GetCurrentColumn();

            context.ReleaseGeneration();

            var oldIndex = context.GetAbsoluteIndex();
            var oldLine = context.GetCurrentLine();

            indexShift = newIndex - oldIndex;
            lineShift = newLine - oldLine;
        }
    }
}
