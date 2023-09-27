using System;
using System.Collections.Generic;

/// <summary>
/// �I�u�U�[�o�[�p�^�[�����g�p����ۂ̍w�ǉ����p�N���X�ł��B
/// </summary>
/// <typeparam name="T">IObserver��T�ƈ�v�����Ă�������</typeparam>
/// �Q�l�T�C�g
/// https://qiita.com/yutorisan/items/6e960426da71b7e02af7
/// �T�C�g�Ɣ�r����ƃW�F�l���b�N��p���ėl�X�ȏꍇ�ň�����悤�Ɋg�����Ă��܂�

public class Unsubscriber<T> : IDisposable
{
    //���s�惊�X�g
    List<IObserver<T>> _observers;

    //Dispose���ꂽ�Ƃ���Remove����IObserver<T>
    IObserver<T> _observer;

    public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
    {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose()
    {
        //Dispose���ꂽ�甭�s�惊�X�g����Ώۂ̔��s����폜����
        _observers.Remove(_observer);
    }
}
