using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class Wait : MonoBehaviour
{
    private static Wait wait;

    public static Wait Instance
    {
        get
        {
            if (!wait)
            {
                wait = FindObjectOfType(typeof(Wait)) as Wait;

                if (!wait)
                {
                    Debug.LogError("There needs to be one active Wait script on a GameObject in your scene.");
                }
            }

            return wait;
        }
    }

    public static Task<bool> ForIEnumerator(YieldInstruction enumerator)
    {
        return Instance._ForIEnumerator(enumerator);
    }

    private Task<bool> _ForIEnumerator(YieldInstruction enumerator)
    {
        TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
        StartCoroutine(_ForIEnumeratorCoRoutine(enumerator, taskCompletionSource));
        return taskCompletionSource.Task;
    }
    private IEnumerator _ForIEnumeratorCoRoutine(YieldInstruction enumerator, TaskCompletionSource<bool> taskCompletionSource)
    {
        yield return enumerator;
        taskCompletionSource.SetResult(true);
    }
}
