﻿using System;

namespace Typezor.SourceGenerator.Logger
{
    public interface ILogger
    {
        void Warn(string message);
        void Error(string message);
        void Info(string message);
        IDisposable Performance(string measureName);
    }
}