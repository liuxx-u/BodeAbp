﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.Extensions
{
    public static class TaskExtensions
    {
        public static TResult WaitResult<TResult>(this Task<TResult> task, int timeoutMillis)
        {
            if (task.Wait(timeoutMillis))
            {
                return task.Result;
            }
            return default(TResult);
        }
        public static async Task TimeoutAfter(this Task task, int millisecondsDelay)
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(millisecondsDelay, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
            }
            else
            {
                throw new TimeoutException("The operation has timed out.");
            }
        }
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, int millisecondsDelay)
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(millisecondsDelay, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                return task.Result;
            }
            else
            {
                throw new TimeoutException("The operation has timed out.");
            }
        }

        public static Task StartDelayedTask(this TaskFactory factory, int millisecondsDelay, Action action)
        {
            // Validate arguments
            if (factory == null) throw new ArgumentNullException("factory");
            if (millisecondsDelay < 0) throw new ArgumentOutOfRangeException("millisecondsDelay");
            if (action == null) throw new ArgumentNullException("action");

            // Check for a pre-canceled token
            if (factory.CancellationToken.IsCancellationRequested)
            {
                return new Task(() => { }, factory.CancellationToken);
            }

            // Create the timed task
            var tcs = new TaskCompletionSource<object>(factory.CreationOptions);
            var ctr = default(CancellationTokenRegistration);

            // Create the timer but don't start it yet.  If we start it now,
            // it might fire before ctr has been set to the right registration.
            var timer = new Timer(self =>
            {
                // Clean up both the cancellation token and the timer, and try to transition to completed
                ctr.Dispose();
                ((Timer)self).Dispose();
                tcs.TrySetResult(null);
            });

            // Register with the cancellation token.
            if (factory.CancellationToken.CanBeCanceled)
            {
                // When cancellation occurs, cancel the timer and try to transition to canceled.
                // There could be a race, but it's benign.
                ctr = factory.CancellationToken.Register(() =>
                {
                    timer.Dispose();
                    tcs.TrySetCanceled();
                });
            }

            // Start the timer and hand back the task...
            try { timer.Change(millisecondsDelay, Timeout.Infinite); }
            catch (ObjectDisposedException) { } // in case there's a race with cancellation; this is benign

            return tcs.Task.ContinueWith(_ => action(), factory.CancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, factory.Scheduler ?? TaskScheduler.Current);
        }
    }
}
