using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Components;

public class SectionContent : IComponent, IDisposable
{
    private SectionRegistry _registry;

    [Parameter] public string Name { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    public void Attach(RenderHandle renderHandle)
    {
        _registry = SectionRegistry.GetRegistry(renderHandle);
    }

    public Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        _registry.SetContent(Name, ChildContent);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (!string.IsNullOrEmpty(Name))
        {
            // This relies on the assumption that the old SectionContent gets disposed before the
            // new one is added to the output. This won't be the case in all possible scenarios.
            // We should have the registry keep track of which SectionContent is the most recent
            // one to supply new content, and disregard updates from ones that were superseded.
            _registry.SetContent(Name, null);
        }
    }
}

public class SectionOutlet : IComponent, IDisposable
{
    private static RenderFragment EmptyRenderFragment = builder => { };
    private string _subscribedName;
    private SectionRegistry _registry;
    private Action<RenderFragment> _onChangeCallback;

    public void Attach(RenderHandle renderHandle)
    {
        _onChangeCallback = content => renderHandle.Render(content ?? EmptyRenderFragment);
        _registry = SectionRegistry.GetRegistry(renderHandle);
    }

    public Task SetParametersAsync(ParameterView parameters)
    {
        var suppliedName = parameters.GetValueOrDefault<string>("Name");

        if (suppliedName != _subscribedName)
        {
            _registry.Unsubscribe(_subscribedName, _onChangeCallback);
            _registry.Subscribe(suppliedName, _onChangeCallback);
            _subscribedName = suppliedName;
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _registry?.Unsubscribe(_subscribedName, _onChangeCallback);
    }
}

internal class SectionRegistry
{
    private static ConditionalWeakTable<Dispatcher, SectionRegistry> _registries
        = new ConditionalWeakTable<Dispatcher, SectionRegistry>();

    private Dictionary<string, List<Action<RenderFragment>>> _subscriptions
        = new Dictionary<string, List<Action<RenderFragment>>>();

    public static SectionRegistry GetRegistry(RenderHandle renderHandle)
    {
        return _registries.GetOrCreateValue(renderHandle.Dispatcher);
    }

    public void Subscribe(string name, Action<RenderFragment> callback)
    {
        if (!_subscriptions.TryGetValue(name, out var existingList))
        {
            existingList = new List<Action<RenderFragment>>();
            _subscriptions.Add(name, existingList);
        }

        existingList.Add(callback);
    }

    public void Unsubscribe(string name, Action<RenderFragment> callback)
    {
        if (name != null && _subscriptions.TryGetValue(name, out var existingList))
        {
            existingList.Remove(callback);
        }
    }

    public void SetContent(string name, RenderFragment content)
    {
        if (_subscriptions.TryGetValue(name, out var existingList))
        {
            foreach (var callback in existingList)
            {
                callback(content);
            }
        }
    }
}