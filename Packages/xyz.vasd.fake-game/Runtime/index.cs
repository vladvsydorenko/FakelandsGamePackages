using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyz.Vasd.FakeGame
{
    public class index {}
    
    public struct Result<T>
    {
        public readonly T Value;
        public readonly Exception Error;

        public Result(T value, Exception error = null)
        {
            Value = value;
            Error = error;
        }

        public static implicit operator T(Result<T> other) 
        {
            return other.Value;
        }

        public static implicit operator Result<T>(T value) 
        {
            return new Result<T>(value);
        }
    }

}
