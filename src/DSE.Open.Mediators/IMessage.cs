// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Mediators;

/// <summary>
/// Marks a type as representing a message that can be dispatched via a <see cref="IMessageDispatcher"/>
/// to one or more registered <see cref="IMessageHandler{TMessage}"/> implementations.
/// </summary>
#pragma warning disable CA1040 // Avoid empty interfaces
public interface IMessage { }
#pragma warning restore CA1040 // Avoid empty interfaces
