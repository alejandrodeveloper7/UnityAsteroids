using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public static class CoroutineExtensions 
{
    public static Task RunCoroutineAsTask(this MonoBehaviour mb, IEnumerator coroutine)
    {
        var tcs = new TaskCompletionSource<bool>();
        mb.StartCoroutine(Run(coroutine, tcs));
        return tcs.Task;
    }

    private static IEnumerator Run(IEnumerator coroutine, TaskCompletionSource<bool> tcs)
    {
        yield return coroutine;
        tcs.SetResult(true);
    }
}
