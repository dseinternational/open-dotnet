// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;

namespace DSE.Open.ViewModels;

public class UserInterfaceModelTests
{
    [Fact]
    public void Defaults_AreFalseForFlagsAndCurrentCulturesForCultures()
    {
        var model = new TestModel();
        Assert.False(model.IsBusy);
        Assert.False(model.IsReadOnly);
        Assert.False(model.IsInitialized);
        Assert.Equal(CultureInfo.CurrentCulture, model.FormatCulture);
        Assert.Equal(CultureInfo.CurrentUICulture, model.PresentationCulture);
    }

    [Fact]
    public void FormatCulture_Setter_UpdatesValueAndRaisesEvents()
    {
        var model = new TestModel();
        var fr = CultureInfo.GetCultureInfo("fr-FR");

        var changing = new List<string?>();
        var changed = new List<string?>();
        model.PropertyChanging += (_, e) => changing.Add(e.PropertyName);
        model.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        model.FormatCulture = fr;

        Assert.Equal(fr, model.FormatCulture);
        Assert.Contains(nameof(UserInterfaceModel.FormatCulture), changing);
        Assert.Contains(nameof(UserInterfaceModel.FormatCulture), changed);
    }

    [Fact]
    public void FormatCulture_Setter_NullThrows()
    {
        var model = new TestModel();
        _ = Assert.Throws<ArgumentNullException>(() => model.FormatCulture = null!);
    }

    [Fact]
    public void PresentationCulture_Setter_UpdatesValueAndRaisesEvents()
    {
        var model = new TestModel();
        var de = CultureInfo.GetCultureInfo("de-DE");

        var changing = new List<string?>();
        var changed = new List<string?>();
        model.PropertyChanging += (_, e) => changing.Add(e.PropertyName);
        model.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        model.PresentationCulture = de;

        Assert.Equal(de, model.PresentationCulture);
        Assert.Contains(nameof(UserInterfaceModel.PresentationCulture), changing);
        Assert.Contains(nameof(UserInterfaceModel.PresentationCulture), changed);
    }

    [Fact]
    public void PresentationCulture_Setter_NullThrows()
    {
        var model = new TestModel();
        _ = Assert.Throws<ArgumentNullException>(() => model.PresentationCulture = null!);
    }

    [Fact]
    public void SettingSameCulture_DoesNotRaiseEvent()
    {
        var model = new TestModel();
        var current = model.FormatCulture;

        var changed = 0;
        model.PropertyChanged += (_, _) => changed++;

        model.FormatCulture = current;

        Assert.Equal(0, changed);
    }

    [Fact]
    public void IsBusy_Setter_RaisesEvents()
    {
        var model = new TestModel();
        var changed = new List<string?>();
        model.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        model.SetIsBusy(true);

        Assert.True(model.IsBusy);
        Assert.Contains(nameof(UserInterfaceModel.IsBusy), changed);
    }

    [Fact]
    public void IsReadOnly_Setter_RaisesEvents()
    {
        var model = new TestModel();
        var changed = new List<string?>();
        model.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        model.SetIsReadOnly(true);

        Assert.True(model.IsReadOnly);
        Assert.Contains(nameof(UserInterfaceModel.IsReadOnly), changed);
    }

    [Fact]
    public async Task InitializeAsync_CallsCoreAndSetsInitialized()
    {
        var model = new TestModel();
        await model.InitializeAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(model.IsInitialized);
        Assert.Equal(1, model.InitializeCoreCount);
    }

    [Fact]
    public async Task InitializeAsync_PassesStateToCore()
    {
        var model = new TestModel();
        var payload = new object();

        await model.InitializeAsync(payload, TestContext.Current.CancellationToken);

        Assert.Same(payload, model.LastState);
    }

    [Fact]
    public async Task InitializeAsync_PassesCancellationTokenToCore()
    {
        using var cts = new CancellationTokenSource();
        var model = new TestModel();

        await model.InitializeAsync(cancellationToken: cts.Token);

        Assert.Equal(cts.Token, model.LastCancellationToken);
    }

    [Fact]
    public async Task InitializeAsync_SecondCallThrows()
    {
        var model = new TestModel();
        await model.InitializeAsync(cancellationToken: TestContext.Current.CancellationToken);

        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await model.InitializeAsync(cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task InitializeAsync_WhenCoreThrows_DoesNotMarkInitialized()
    {
        var model = new ThrowingModel();

        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await model.InitializeAsync(cancellationToken: TestContext.Current.CancellationToken));

        Assert.False(model.IsInitialized);
    }

    [Fact]
    public async Task InitializeAsync_AfterFailure_CanBeRetried()
    {
        var model = new FailThenSucceedModel();

        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await model.InitializeAsync(cancellationToken: TestContext.Current.CancellationToken));
        Assert.False(model.IsInitialized);

        await model.InitializeAsync(cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(model.IsInitialized);
    }

    [Fact]
    public async Task InitializeAsync_RaisesPropertyChangedForIsInitialized()
    {
        var model = new TestModel();
        var changed = new List<string?>();
        model.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        await model.InitializeAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Contains(nameof(UserInterfaceModel.IsInitialized), changed);
    }

    [Fact]
    public void Model_ImplementsINotifyPropertyChangedAndChanging()
    {
        var model = new TestModel();
        Assert.IsAssignableFrom<INotifyPropertyChanged>(model);
        Assert.IsAssignableFrom<INotifyPropertyChanging>(model);
        Assert.IsAssignableFrom<IUserInterfaceModel>(model);
    }

    private sealed class TestModel : UserInterfaceModel
    {
        public int InitializeCoreCount { get; private set; }
        public object? LastState { get; private set; }
        public CancellationToken LastCancellationToken { get; private set; }

        public void SetIsBusy(bool value) => IsBusy = value;
        public void SetIsReadOnly(bool value) => IsReadOnly = value;

        protected override ValueTask InitializeCoreAsync(object? state, CancellationToken cancellationToken)
        {
            InitializeCoreCount++;
            LastState = state;
            LastCancellationToken = cancellationToken;
            return ValueTask.CompletedTask;
        }
    }

    private sealed class ThrowingModel : UserInterfaceModel
    {
        protected override ValueTask InitializeCoreAsync(object? state, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("core failure");
        }
    }

    private sealed class FailThenSucceedModel : UserInterfaceModel
    {
        private int _attempts;

        protected override ValueTask InitializeCoreAsync(object? state, CancellationToken cancellationToken)
        {
            _attempts++;
            if (_attempts == 1)
            {
                throw new InvalidOperationException("first attempt fails");
            }
            return ValueTask.CompletedTask;
        }
    }
}
