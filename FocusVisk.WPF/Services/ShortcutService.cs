namespace FocusVisk.Services;

public class ShortcutService
{
    private bool _pendingNewTask;

    public event Action? OnNewTaskRequested;

    public void RequestNewTask()
    {
        _pendingNewTask = true;
        OnNewTaskRequested?.Invoke();
    }

    public bool ConsumePendingNewTask()
    {
        if (!_pendingNewTask) return false;
        _pendingNewTask = false;
        return true;
    }
}