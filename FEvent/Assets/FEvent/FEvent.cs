﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using FEvent.Internal;
namespace FEvent
{
    public static partial class FEvents
    {
        public static IEventPublisher Publisher { get; private set; } = new FEventPublisher();


       
        
    }
}
