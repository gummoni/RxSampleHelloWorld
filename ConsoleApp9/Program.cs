
// See https://aka.ms/new-console-template for more information
using System.Reactive.Linq;

Console.WriteLine("Hello, World!");

var observable = new MyObservable();

using (observable.Where(_ => !string.IsNullOrWhiteSpace(_)).Subscribe(new MyObserver()))
{
    observable.Notify("Hello");
    observable.Notify("   ");
    observable.Notify("World");
}

observable.Notify("okwwww");


class Disposable<T> : IDisposable
{
    readonly List<IObserver<T>> Collection = new();
    readonly IObserver<T> Item;

    public Disposable(List<IObserver<T>> parent, IObserver<T> item)
    {
        Collection = parent;
        Item = item;
    }

    public void Dispose()
    {
        lock(Collection)
        {
            Collection.Remove(Item);
        }
    }
}

class MyObservable : IObservable<string>
{
    readonly List<IObserver<string>> Collection = new();

    public IDisposable Subscribe(IObserver<string> observer)
    {
        Collection.Add(observer);
        return new Disposable<string>(Collection, observer);
    }

    public void Notify(string value)
    {
        Collection.ForEach(_ => _.OnNext(value));
    }
}

class MyObserver : IObserver<string>
{
    public void OnCompleted()
    {
        Console.WriteLine("Complete");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine($"Error:{error.Message}");
    }

    public void OnNext(string value)
    {
        Console.WriteLine($"Next:{value}");
    }
}



