using System;

public class ObservableProperty<T>
{
    private T _observableValue;
    private event Action<T> _onValueChange;

    public T Value 
    {
        get => _observableValue;
        set
        {
            _observableValue = value;
            _onValueChange?.Invoke(value);
        }
    }

    public void Subscribe(Action<T> observerCallback)
    {
        _onValueChange += observerCallback;
    }

    public void Unsubscribe(Action<T> observerCallback)
    {
        _onValueChange -= observerCallback;
    }
}
