using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Threading;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using HanumanInstitute.MvvmDialogs.Avalonia.Navigation;

namespace MvvmDialogs.Avalonia.Ursa;

public static class UrsaUiExtensions
{
    /// <summary>
    /// Creates a UrsaWindowViewWrapper around specified view.
    /// </summary>
    /// <param name="view">The Window to get a wrapper for.</param>
    /// <param name="windowFactory"></param>
    /// <returns>A UrsaWindowViewWrapper referencing the view.</returns>
    public static UrsaWindowViewWrapper? AsWindowWrapper(this ContentControl? view, IWindowFactory? windowFactory = null)
    {
        if (view == null)
        {
            return null;
        }
        var wrapper = new UrsaWindowViewWrapper(windowFactory);
        wrapper.InitializeExisting((INotifyPropertyChanged)view.DataContext!, view);
        return wrapper;
    }

    /// <summary>
    /// Returns the <see cref="IView"/> RefObj property as an Avalonia ContentControl.
    /// </summary>
    /// <param name="view">The IView to get the Ref property for.</param>
    /// <returns>The ContentControl held within the IView.</returns>
    public static ContentControl? GetRef(this IView? view)
    {
        return view switch
        {
            UrsaWindowViewWrapper uwv=>uwv.Ref,
            ViewWrapper v => v.Ref,
            ViewNavigationWrapper nav => nav.Ref,
            _ => null
        };
    }

    // /// <summary>
    // /// Converts an IView into a ViewWrapper.
    // /// </summary>
    // /// <param name="window">The IWindow to convert.</param>
    // /// <returns>A ViewWrapper referencing the window.</returns>
    // [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull("window")]
    // public static ViewWrapper? AsWrapper(this IView? window) =>
    //     (ViewWrapper?)window;

    /// <summary>
    /// Runs a synchronous action asynchronously on the UI thread.
    /// </summary>
    /// <param name="action">The action to run asynchronously.</param>
    /// <typeparam name="T">The return type of the action.</typeparam>
    /// <returns>The result of the action.</returns>
    public static Task<T> RunUiAsync<T>(Func<T> action)
    {
        TaskCompletionSource<T> completion = new();
        Dispatcher.UIThread.Post(new Action(() => completion.SetResult(action())));
        return completion.Task;
    }
}