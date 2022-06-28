// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="TaskManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Accounting;
using Adrack.Service.Audit;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Infrastructure.ApplicationEvent;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Adrack.Service.Agent
{
    /// <summary>
    /// Class SheduleRegistry.
    /// Implements the <see cref="FluentScheduler.Registry" />
    /// </summary>
    /// <seealso cref="FluentScheduler.Registry" />
    public class SheduleRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SheduleRegistry" /> class.
        /// </summary>
        public SheduleRegistry()
        {
            Schedule<WeeklyTask>().ToRunEvery(10).Minutes();
        }
    }

    /// <summary>
    /// Class WeeklyTask.
    /// Implements the <see cref="FluentScheduler.IJob" />
    /// </summary>
    /// <seealso cref="FluentScheduler.IJob" />
    public class WeeklyTask : IJob
    {
        /// <summary>
        /// Executes the job.
        /// </summary>
        public void Execute()
        {
            
            // call the method to run weekly here
            
        }
    }

    /// <summary>
    /// Represents a Task Manager
    /// </summary>
    public partial class TaskManager
    {
        #region Fields

        /// <summary>
        /// The task manager
        /// </summary>
        private static readonly TaskManager _taskManager = new TaskManager();

        /// <summary>
        /// The task manager post
        /// </summary>
        private static readonly TaskManager _taskManagerPost = new TaskManager();

        /// <summary>
        /// The task threads
        /// </summary>
        private readonly List<TaskThread> _taskThreads = new List<TaskThread>();

        /// <summary>
        /// The not run tasks interval
        /// </summary>
        private int _notRunTasksInterval = 60 * 30; //30 minutes

        private bool IsInitialized { get; set; }

        private bool IsStarted { get; set; }


        #endregion Fields

        #region Constructor

        /// <summary>
        /// Task Manager
        /// </summary>
        private TaskManager()
        {
            IsInitialized = false;
            IsStarted = false;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Initializes the task manager with the property values specified in the configuration file.
        /// </summary>
        public void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;

            this._taskThreads.Clear();

            var taskService = AppEngineContext.Current.Resolve<IScheduleTaskService>();

            var scheduleTasks = taskService
                .GetAllScheduleTasks()
                .OrderBy(x => x.Seconds)
                .ToList();

            //group by threads with the same seconds
            foreach (var scheduleTaskGrouped in scheduleTasks.GroupBy(x => x.Seconds))
            {
                //create a thread
                var taskThread = new TaskThread()
                {
                    Seconds = scheduleTaskGrouped.Key
                };

                foreach (var scheduleTask in scheduleTaskGrouped)
                {
                    var task = new Task(scheduleTask);
                    taskThread.AddTask(task);

                    if (scheduleTask.FirstStartTime.HasValue)
                    {
                        if (scheduleTask.FirstStartTime > DateTime.UtcNow)
                            taskThread.FirstStartSeconds = (int)((DateTime)scheduleTask.FirstStartTime - DateTime.UtcNow).TotalSeconds;
                        else
                            taskThread.FirstStartSeconds = 1;
                    }
                    else
                    {
                        taskThread.FirstStartSeconds = scheduleTask.Seconds;
                    }

                }

                this._taskThreads.Add(taskThread);
            }

            //sometimes a task period could be set to several hours (or even days).
            //in this case a probability that it'll be run is quite small (an application could be restarted)
            //we should manually run the tasks which weren't run for a long time
            var notRunTasks = scheduleTasks
                .Where(x => x.Seconds >= _notRunTasksInterval)
                .Where(x => !x.LastStart.HasValue || x.LastStart.Value.AddSeconds(_notRunTasksInterval) < DateTime.UtcNow)
                .ToList();

            //create a thread for the tasks which weren't run for a long time
            if (notRunTasks.Count > 0)
            {
                var taskThread = new TaskThread()
                {
                    RunOnlyOnce = true,
                    Seconds = 60 * 5 //let's run such tasks in 5 minutes after application start
                };

                foreach (var scheduleTask in notRunTasks)
                {
                    var task = new Task(scheduleTask);

                    taskThread.AddTask(task);
                }

                this._taskThreads.Add(taskThread);
            }

            JobManager.Initialize(new SheduleRegistry());
        }

        /// <summary>
        /// Starts the task manager
        /// </summary>
        public void Start()
        {
            if (IsStarted) return;
            IsStarted = true;

            foreach (var taskThread in this._taskThreads)
            {
                taskThread.InitTimer();
            }
        }

        /// <summary>
        /// Stops the task manager
        /// </summary>
        public void Stop()
        {
            foreach (var taskThread in this._taskThreads)
            {
                taskThread.Dispose();
            }
        }

        /// <summary>
        /// Adds the task thread.
        /// </summary>
        /// <param name="taskThread">The task thread.</param>
        public void AddTaskThread(TaskThread taskThread)
        {
            _taskThreads.Add(taskThread);
            taskThread.InitTimer();
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets the task mamanger instance
        /// </summary>
        /// <value>The instance.</value>
        public static TaskManager Instance
        {
            get
            {
                return _taskManager;
            }
        }

        /// <summary>
        /// Gets the post instance.
        /// </summary>
        /// <value>The post instance.</value>
        public static TaskManager PostInstance
        {
            get
            {
                return _taskManagerPost;
            }
        }

        /// <summary>
        /// Gets a list of task threads of this task manager
        /// </summary>
        /// <value>The task threads.</value>
        public IList<TaskThread> TaskThreads
        {
            get
            {
                return new ReadOnlyCollection<TaskThread>(this._taskThreads);
            }
        }

        #endregion Properties
    }
}