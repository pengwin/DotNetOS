using System;
using System.Collections.Generic;

namespace Common
{
    public struct Option<T>
    {
        public bool IsNone { get; }

        public T Value { get; }

        private Option(T value)
        {
            IsNone = false || value == null;
            Value = value;
        }

        public static Option<T> None => new Option<T>(default(T));

        public static Option<T> New(T value)
        {
            if (!EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                return new Option<T>(value);
            }

            throw new ArgumentException($"Could not create Option from {value}. If you need empty value use Option<T>.None", nameof(value));
        }
    }
}
