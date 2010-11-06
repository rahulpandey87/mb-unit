using System;
using System.Collections.Generic;
using Gallio.Common.Concurrency;
using Gallio.Runtime.ProgressMonitoring;
using Gallio.UI.Common.Policies;
using Gallio.UI.Events;

namespace Gallio.UI.ProgressMonitoring
{
    /// <inheritdoc />
    public class TaskRunner : ITaskRunner
    {
        private readonly ITaskQueue taskQueue;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnhandledExceptionPolicy unhandledExceptionPolicy;
        private readonly IDictionary<string, ThreadTask> currentWorkerTasks;

        ///<summary>
        /// Ctor.
        ///</summary>
        ///<param name="taskQueue">The task queue to use.</param>
        ///<param name="eventAggregator">An event aggregator.</param>
        ///<param name="unhandledExceptionPolicy">An exception policy.</param>
        public TaskRunner(ITaskQueue taskQueue, IEventAggregator eventAggregator,
            IUnhandledExceptionPolicy unhandledExceptionPolicy)
        {
            this.taskQueue = taskQueue;
            this.eventAggregator = eventAggregator;
            this.unhandledExceptionPolicy = unhandledExceptionPolicy;
            currentWorkerTasks = new Dictionary<string, ThreadTask>();
        }

        /// <inheritdoc />
        public void RunTask(string queueId)
        {
            if (currentWorkerTasks.ContainsKey(queueId))
                return;

            var command = taskQueue.GetNextTask(queueId);

            if (command == null)
                return;

            BeginNextTask(queueId, command);
        }

        private void BeginNextTask(string queueId, ICommand command)
        {
            var workerTask = new ThreadTask(queueId, () =>
            {
                var progressMonitor = new ObservableProgressMonitor();
                eventAggregator.Send(new TaskStarted(queueId, progressMonitor));
                command.Execute(progressMonitor);
            });

            currentWorkerTasks.Add(queueId, workerTask);

            workerTask.Terminated += delegate
            {
                TaskTerminated(workerTask, queueId);
            };

            workerTask.Start();
        }

        private void TaskTerminated(Task workerTask, string queueId)
        {
            if (!workerTask.IsAborted && !workerTask.Result.HasValue)
            {
                ProcessFailure(workerTask.Result.Exception, queueId);
            }

            lock (this)
            {
                currentWorkerTasks.Remove(queueId);
            }

            eventAggregator.Send(new TaskCompleted(queueId));

            RunTask(queueId);
        }

        private void ProcessFailure(Exception exception, string queueId)
        {
            if (exception is OperationCanceledException)
            {
                eventAggregator.Send(new TaskCancelled(queueId));
            }
            else
            {
                unhandledExceptionPolicy.Report("An exception occurred while running a task.",
                    exception);
            }
        }
    }
}
