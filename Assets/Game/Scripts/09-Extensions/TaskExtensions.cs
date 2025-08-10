using System;
using System.Collections;
using System.Threading.Tasks;

public static class TaskExtensions
{
    public static IEnumerator AsCoroutine(this Task task)
    {
        while (!task.IsCompleted)
            yield return null;

        if (task.IsFaulted)
            throw task.Exception;
    }

    public static IEnumerator AsCoroutine<T>(this Task<T> task, Action<T> onCompleted)
    {
        while (!task.IsCompleted)
            yield return null;

        if (task.IsFaulted)
            throw task.Exception;

        onCompleted?.Invoke(task.Result);
    }
}
