﻿#pragma warning disable 67

namespace Typezor.Tests.CodeModel.Support
{
    public class EventInfo
    {
        /// <summary>
        /// summary
        /// </summary>
        [AttributeInfo]
        public event Delegate DelegateEvent;
        public event GenericDelegate<string> GenericDelegateEvent;
    }
}
