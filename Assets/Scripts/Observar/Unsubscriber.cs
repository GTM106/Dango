using System;
using System.Collections.Generic;

/// <summary>
/// オブザーバーパターンを使用する際の購読解除用クラスです。
/// </summary>
/// <typeparam name="T">IObserverのTと一致させてください</typeparam>
/// 参考サイト
/// https://qiita.com/yutorisan/items/6e960426da71b7e02af7
/// サイトと比較するとジェネリックを用いて様々な場合で扱えるように拡張しています

public class Unsubscriber<T> : IDisposable
{
    //発行先リスト
    List<IObserver<T>> _observers;

    //DisposeされたときにRemoveするIObserver<T>
    IObserver<T> _observer;

    public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
    {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose()
    {
        //Disposeされたら発行先リストから対象の発行先を削除する
        _observers.Remove(_observer);
    }
}
