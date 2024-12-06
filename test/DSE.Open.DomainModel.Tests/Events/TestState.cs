// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;

namespace DSE.Open.DomainModel.Tests.Events;

public class TestState : ConcurrentDictionary<string, object>;
