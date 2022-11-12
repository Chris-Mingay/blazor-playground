using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Blazor.App.Components.Forms;

public abstract class ExInputBase<T> : InputBase<T>
{
    
    [Parameter, EditorRequired] 
    public Expression<Func<string>> ValidationFor { get; set; } = default!;
    
    [Parameter]
    public bool Required { get; set; }
    
    [Parameter]
    public string? Label { get; set; }
    
    [Parameter]
    public string? Id { get; set; }
    
}