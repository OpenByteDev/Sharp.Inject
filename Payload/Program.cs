﻿using System;
using Sharp.Inject.Bootstrap;

namespace Payload {
    public static class Program {
        [EntryPoint]
        public static unsafe void EntryPoint(string str) {
            Console.WriteLine(str);
        }
    }
}