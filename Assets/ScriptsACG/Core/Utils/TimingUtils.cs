using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.Core.Utils
{
    public static class TimingUtils
    {
#if UNITY_EDITOR

        #region Fields

        [Header("References")]
        private static readonly CancellationTokenSource _playModeCts;
        private static CancellationToken PlayModeToken => _playModeCts.Token;

        #endregion

        #region Constructor

        static TimingUtils()
        {
            _playModeCts = new CancellationTokenSource();
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingPlayMode)
                    _playModeCts.Cancel();
            };
        }

        #endregion

#endif

        #region Wait time

        public static async Task WaitSeconds(float seconds, bool ignoreTimeScale = false, CancellationToken cancellationToken = default)
        {
            if (seconds <= 0f)
                return;

#if UNITY_EDITOR
            using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, PlayModeToken);
            CancellationToken token = linkedCts.Token;
#else
            CancellationToken token = cancellationToken;
#endif
            float start = ignoreTimeScale ? Time.realtimeSinceStartup : Time.time;

            while (true)
            {
                if (token.IsCancellationRequested)                
                    return;
                
                float elapsed = (ignoreTimeScale ? Time.realtimeSinceStartup : Time.time) - start;
                if (elapsed >= seconds)
                    break;

                await Task.Yield();
            }
        }

        public static async Task WaitTask(Task taskToWait, CancellationToken cancellationToken = default)
        {
            if (taskToWait == null)
                return;

#if UNITY_EDITOR
            using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, PlayModeToken);
            CancellationToken token = linkedCts.Token;
#else
    CancellationToken token = cancellationToken;
#endif

            while (true)
            {
                if (token.IsCancellationRequested)
                    return;

                if (taskToWait.IsCompleted)
                {
                    await taskToWait;
                    token.ThrowIfCancellationRequested();
                    break;
                }

                await Task.Yield();
            }
        }

        public static async Task WaitAsyncOperation(AsyncOperation asyncOperation, CancellationToken cancellationToken = default)
        {
            if (asyncOperation == null)
                return;

#if UNITY_EDITOR
            using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, PlayModeToken);
            CancellationToken token = linkedCts.Token;
#else
    CancellationToken token = cancellationToken;
#endif

            while (!asyncOperation.isDone)
            {
                if (token.IsCancellationRequested)
                    return;
                
                await Task.Yield();
            }

            if (token.IsCancellationRequested)
                return;
        }

        public static async Task WaitMilliseconds(int miliseconds, bool ignoreTimeScale = false, CancellationToken cancellationToken = default)
        {
            float seconds = ToSeconds(miliseconds);
            await WaitSeconds(seconds, ignoreTimeScale, cancellationToken);
        }

        #endregion

        #region Conversions

        private static int ToMilliseconds(float seconds) => Mathf.RoundToInt(seconds * 1000);

        private static float ToSeconds(int milliseconds) => milliseconds / 1000f;

        #endregion
    }
}