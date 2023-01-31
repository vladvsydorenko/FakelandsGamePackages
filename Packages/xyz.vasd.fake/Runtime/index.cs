using System;

namespace Xyz.Vasd.Fake
{
    public class index {}
}

namespace Xyz.Vasd.Fake
{
    public struct FakeResult<T>
    {
        public readonly T Value;
        public readonly Exception Error;

        public FakeResult(T value, Exception error = null)
        {
            Value = value;
            Error = error;
        }

        public static implicit operator T(FakeResult<T> other)
        {
            return other.Value;
        }

        public static implicit operator FakeResult<T>(T value)
        {
            return new FakeResult<T>(value);
        }
    }
}
