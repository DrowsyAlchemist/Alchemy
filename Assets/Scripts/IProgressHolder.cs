using System;

public interface IProgressHolder
{
    public int MaxCount { get; }
    public int CurrentCount { get; }

    public event Action<int> CurrentCountChanged;
}
