using Avalonia.Controls;

namespace MvvmDialogs.Avalonia.Ursa;

/// <summary>
/// Create a Window to wrapper view.
/// </summary>
public interface IWindowFactory
{
    /// <summary>
    /// Create a new Window.
    /// </summary>
    /// <returns></returns>
    public Window CreateWindow();
}