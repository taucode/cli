﻿using TauCode.Parsing;

namespace TauCode.TextProcessing.Lab
{
    public interface ITextProcessor<out TProduct>
    {
        bool AcceptsFirstChar(char c);

        TextProcessingResult Process(TextProcessingContext context);

        TProduct Produce(string text, int absoluteIndex, int consumedLength, Position position);
    }
}
