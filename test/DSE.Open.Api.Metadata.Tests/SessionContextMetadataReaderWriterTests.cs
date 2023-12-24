// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Requests;
using DSE.Open.Results;
using DSE.Open.Sessions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DSE.Open.Api.Metadata.Tests;

public class SessionContextMetadataReaderWriterTests
{
    [Fact]
    public async Task ReadRequestMetadataAsync_WithSession_ShouldWriteSessionToRequestAndResult()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var sessionContext = new SessionContext();
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();
        context.Data.TryAdd(SessionContextMetadataKeys.SessionContext, SessionContextSerializer.SerializeToBase64Utf8Json(sessionContext));

        // Act
        await readerWriter.ReadRequestMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        var requestSessionContext = requestMetadata.Properties[SessionContextMetadataKeys.SessionContext] as SessionContext;
        Assert.Equivalent(sessionContext, requestSessionContext);

        var resultSessionContext = resultMetadata.Properties[SessionContextMetadataKeys.SessionContext] as SessionContext;
        Assert.Equivalent(sessionContext, resultSessionContext);

        Assert.Same(requestSessionContext, resultSessionContext);
    }

    [Fact]
    public async Task ReadRequestMetadataAsync_WithInvalidSessionJson_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();
        context.Data.TryAdd(SessionContextMetadataKeys.SessionContext, "invalid json");

        // Act
        async Task Act() => await readerWriter.ReadRequestMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(Act);
    }

    [Fact]
    public async Task ReadRequestMetadataAsync_WithNoSession_ShouldCreateSession()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();

        // Act
        await readerWriter.ReadRequestMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        var requestSessionContext = requestMetadata.Properties[SessionContextMetadataKeys.SessionContext] as SessionContext;
        Assert.NotNull(requestSessionContext);
    }

    [Fact]
    public async Task ReadRequestMetadataAsync_WithStorageContexts_ShouldOverwrite()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var originalSessionContext = new SessionContext();
        var newSessionContext = new SessionContext();

        var requestMetadata = new RequestMetadata
        {
            Properties =
            {
                [SessionContextMetadataKeys.SessionContext] = originalSessionContext
            }
        };

        var resultMetadata = new ResultMetadata
        {
            Properties =
            {
                [SessionContextMetadataKeys.SessionContext] = originalSessionContext
            }
        };

        var context = new MetadataStorageContext();
        context.Data.TryAdd(SessionContextMetadataKeys.SessionContext, SessionContextSerializer.SerializeToBase64Utf8Json(newSessionContext));

        // Act
        await readerWriter.ReadRequestMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        var requestSessionContext = requestMetadata.Properties[SessionContextMetadataKeys.SessionContext] as SessionContext;
        Assert.Equivalent(newSessionContext, requestSessionContext);

        var resultSessionContext = resultMetadata.Properties[SessionContextMetadataKeys.SessionContext] as SessionContext;
        Assert.Equivalent(newSessionContext, resultSessionContext);

        Assert.Same(requestSessionContext, resultSessionContext);
    }

    [Fact]
    public async Task ReadResultMetadataAsync_WithSession_ShouldWriteSessionToResult()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var sessionContext = new SessionContext();
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();
        context.Data.TryAdd(SessionContextMetadataKeys.SessionContext, SessionContextSerializer.SerializeToBase64Utf8Json(sessionContext));

        // Act
        await readerWriter.ReadResultMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        Assert.Equivalent(sessionContext, resultMetadata.Properties[SessionContextMetadataKeys.SessionContext]);
        Assert.False(requestMetadata.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
    }

    [Fact]
    public async Task ReadResultMetadataAsync_WithSessionContextInResult_ShouldOverwrite()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var originalSessionContext = new SessionContext();
        var newSessionContext = new SessionContext();

        var requestMetadata = new RequestMetadata();

        var resultMetadata = new ResultMetadata
        {
            Properties =
            {
                [SessionContextMetadataKeys.SessionContext] = originalSessionContext
            }
        };

        var context = new MetadataStorageContext();
        context.Data.TryAdd(SessionContextMetadataKeys.SessionContext, SessionContextSerializer.SerializeToBase64Utf8Json(newSessionContext));

        // Act
        await readerWriter.ReadResultMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        Assert.Equivalent(newSessionContext, resultMetadata.Properties[SessionContextMetadataKeys.SessionContext]);
        Assert.False(requestMetadata.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
    }

    [Fact]
    public async Task ReadResultMetadata_WithInvalidSession_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();
        context.Data.TryAdd(SessionContextMetadataKeys.SessionContext, "invalid json");

        // Act
        async Task Act() => await readerWriter.ReadResultMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(Act);
        Assert.False(resultMetadata.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
        Assert.False(requestMetadata.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
    }

    [Fact]
    public async Task ReadResultMetadataAsync_WithNoSessionContext_ShouldNotAdd()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();

        // Act
        await readerWriter.ReadResultMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        Assert.False(resultMetadata.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
        Assert.False(requestMetadata.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
    }

    [Fact]
    public async Task WriteRequestMetadataAsync_WithNoSessionContext_ShouldNotWrite()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();

        // Act
        await readerWriter.WriteRequestMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        Assert.False(context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
    }

    [Fact]
    public async Task WriteRequestMetadataAsync_WithNonSessionContext_ShouldNotWriteSessionContext()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var requestMetadata = new RequestMetadata
        {
            Properties =
            {
                [SessionContextMetadataKeys.SessionContext] = new object()
            }
        };
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();

        // Act
        await readerWriter.WriteRequestMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        Assert.False(context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
    }

    [Fact]
    public async Task WriteRequestMetadataAsync_WithSessionContext_ShouldWriteSessionContext()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var sessionContext = new SessionContext();
        var requestMetadata = new RequestMetadata
        {
            Properties =
            {
                [SessionContextMetadataKeys.SessionContext] = sessionContext
            }
        };
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();

        // Act
        await readerWriter.WriteRequestMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        Assert.True(context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextValue));
        Assert.Equivalent(sessionContext, SessionContextSerializer.DeserializeFromBase64Utf8Json(sessionContextValue));
    }

    [Fact]
    public async Task WriteResultMetadataAsync_WithNoSessionContext_ShouldNotWrite()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata();

        var context = new MetadataStorageContext();

        // Act
        await readerWriter.WriteResultMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        Assert.False(context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
    }

    [Fact]
    public async Task WriteResultMetadataAsync_WithNonSessionContext_ShouldNotWrite()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata
        {
            Properties =
            {
               [SessionContextMetadataKeys.SessionContext] = new object()
            }
        };

        var context = new MetadataStorageContext();

        // Act
        await readerWriter.WriteResultMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        Assert.False(context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out _));
    }

    [Fact]
    public async Task WriteResultMetadataAsync_WithSessionContext_ShouldWriteSessionContext()
    {
        // Arrange
        var logger = new Mock<ILogger<SessionContextMetadataReaderWriter>>();
        var readerWriter = new SessionContextMetadataReaderWriter(logger.Object);
        var sessionContext = new SessionContext();
        var requestMetadata = new RequestMetadata();
        var resultMetadata = new ResultMetadata
        {
            Properties =
            {
                [SessionContextMetadataKeys.SessionContext] = sessionContext
            }
        };

        var context = new MetadataStorageContext();

        // Act
        await readerWriter.WriteResultMetadataAsync(
            requestMetadata,
            resultMetadata,
            context,
            CancellationToken.None);

        // Assert
        Assert.True(context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextValue));
        Assert.Equivalent(sessionContext, SessionContextSerializer.DeserializeFromBase64Utf8Json(sessionContextValue));
    }
}
