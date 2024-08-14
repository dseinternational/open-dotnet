// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using MessagePack;
using MessagePack.Formatters;

namespace DSE.Open.Values.Text.MessagePack.Serialization;

public sealed class MessagePackUInt64ValueConverter<TValue> : IMessagePackFormatter<TValue>
    where TValue : struct, IValue<TValue, ulong>
{
    public void Serialize(ref MessagePackWriter writer, TValue value, MessagePackSerializerOptions options)
    {
        writer.WriteUInt64(value);
    }

    public TValue Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var value = reader.ReadUInt64();
        return TValue.FromValue(value);
    }
}
