using System;
using UnityEngine.Events;

public interface IInfiniteScrollPrefab
{
    int getKey();
    InfiniteScrollerEvent GetClicked();
}

public interface InfiniteScrollPrefabInterface<T> : IInfiniteScrollPrefab
{
    //void setData(int key, object data);
    //Type DataType();
    void SetData(int key, T data);

    //void setKey(int key);

}
