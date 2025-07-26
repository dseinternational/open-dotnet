// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Memory;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="JsonConverterFactory"/> for serializing and deserializing sequences of bytes
/// to JSON number arrays (rather than the default base 64 string).
/// </summary>
/// <remarks>
/// Supported types include:
/// <list type="bullet">
/// <item><see cref="ReadOnlyMemory{Byte}"/></item>
/// <item><see cref="Memory{Byte}"/></item>
/// <item><see cref="byte"/> array</item>
/// <item><see cref="IEnumerable{Byte}"/></item>
/// </list>
/// </remarks>
public sealed class ByteSequenceJsonConverter : JsonConverterFactory
{
    private static readonly FrozenSet<Type> s_supportedTypes = FrozenSet.Create(
        typeof(ReadOnlyMemory<byte>),
        typeof(Memory<byte>),
        typeof(byte[]),
        typeof(IEnumerable<byte>)
    );

    public override bool CanConvert(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        return s_supportedTypes.Contains(typeToConvert)
            || typeToConvert.IsAssignableTo(typeof(IEnumerable<byte>));
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        if (typeToConvert == typeof(ReadOnlyMemory<byte>))
        {
            return new ByteReadOnlyMemoryConverter();
        }

        if (typeToConvert == typeof(Memory<byte>))
        {
            return new ByteMemoryConverter();
        }

        if (typeToConvert == typeof(byte[]))
        {
            return new ByteArrayConverter();
        }

        if (typeToConvert == typeof(IEnumerable<byte>) || typeToConvert.IsAssignableTo(typeof(IEnumerable<byte>)))
        {
            return new ByteEnumerableConverter();
        }

        throw new NotSupportedException($"Type '{typeToConvert.FullName}' is not supported by {nameof(ByteSequenceJsonConverter)}.");
    }

#pragma warning disable DSEOPEN001 // ArrayBuilder

    private abstract class ByteMemoryJsonConverterBase<T> : JsonConverter<T>
    {
        protected abstract ReadOnlyMemory<byte> GetMemory(T value);

        protected abstract T GetValue(Memory<byte> memory);

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected start of array.");
            }

            using var builder = new ArrayBuilder<byte>(2048, false);

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType != JsonTokenType.Number)
                {
                    throw new JsonException("Expected number in byte array.");
                }

                if (!reader.TryGetByte(out var value))
                {
                    throw new JsonException("Invalid byte value.");
                }

                builder.Add(value);
            }

            return GetValue(builder.ToMemory());
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);

            var memory = GetMemory(value);

            writer.WriteStartArray();

            foreach (var b in memory)
            {
                writer.WriteNumberValue(b);
            }

            writer.WriteEndArray();
        }
    }

    private sealed class ByteMemoryConverter : ByteMemoryJsonConverterBase<Memory<byte>>
    {
        protected override ReadOnlyMemory<byte> GetMemory(Memory<byte> value)
        {
            return value;
        }

        protected override Memory<byte> GetValue(Memory<byte> memory)
        {
            return memory;
        }
    }

    private sealed class ByteReadOnlyMemoryConverter : ByteMemoryJsonConverterBase<ReadOnlyMemory<byte>>
    {
        protected override ReadOnlyMemory<byte> GetMemory(ReadOnlyMemory<byte> value)
        {
            return value;
        }

        protected override ReadOnlyMemory<byte> GetValue(Memory<byte> memory)
        {
            return memory;
        }
    }

    private sealed class ByteArrayConverter : ByteMemoryJsonConverterBase<byte[]>
    {
        protected override ReadOnlyMemory<byte> GetMemory(byte[] value)
        {
            return value.AsMemory();
        }

        protected override byte[] GetValue(Memory<byte> memory)
        {
            return memory.ToArray();
        }
    }

    private sealed class ByteEnumerableConverter : ByteMemoryJsonConverterBase<IEnumerable<byte>>
    {
        protected override ReadOnlyMemory<byte> GetMemory(IEnumerable<byte> value)
        {
            throw new NotImplementedException("This method should not be called for IEnumerable<byte>.");
        }

        protected override IEnumerable<byte> GetValue(Memory<byte> memory)
        {
            return memory.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<byte> value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.WriteStartArray();

            foreach (var b in value)
            {
                writer.WriteNumberValue(b);
            }

            writer.WriteEndArray();
        }
    }
}
