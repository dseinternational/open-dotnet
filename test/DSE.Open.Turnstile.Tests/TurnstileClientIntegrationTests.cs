// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Turnstile;

public sealed class TurnstileClientIntegrationTests
{
    [Fact]
    public async Task CanGetPassesValidationResponse()
    {
        using var httpClient = new HttpClient();

        var client = new TurnstileClient(httpClient, new TurnstileClientOptions
        {
            Endpoint = new Uri("https://challenges.cloudflare.com/turnstile/v0/siteverify"),
            SecretKey = TestSecretKeys.Passes
        });

        var response = await client.ValidateAsync("ABCDEFGHIJKLMNOP");

        Assert.NotNull(response);
        Assert.True(response.Success);
    }

    [Fact]
    public async Task CanGetFailsValidationResponse()
    {
        using var httpClient = new HttpClient();

        var client = new TurnstileClient(httpClient, new TurnstileClientOptions
        {
            Endpoint = new Uri("https://challenges.cloudflare.com/turnstile/v0/siteverify"),
            SecretKey = TestSecretKeys.Fails
        });

        var response = await client.ValidateAsync("ABCDEFGHIJKLMNOP");

        Assert.NotNull(response);
        Assert.False(response.Success);
    }
}
