// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="TaskThread.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace Adrack.Service.Agent
{
    /// <summary>
    /// Represents a Task Thread
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public partial class TaskThread : IDisposable
    {
        #region Fields

        /// <summary>
        /// The timer
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The tasks
        /// </summary>
        private readonly Dictionary<string, Task> _tasks;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Runs this instance.
        /// </summary>
        private void Run()
        {
            if (Seconds <= 0)
                return;

            this.StartedUtc = DateTime.UtcNow;
            this.IsRunning = true;
            foreach (Task task in this._tasks.Values)
            {
                task.Execute();
            }
            this.IsRunning = false;
        }

        /// <summary>
        /// Timers the handler.
        /// </summary>
        /// <param name="state">The state.</param>
        private void TimerHandler(object state)
        {
            this._timer.Change(-1, -1);
            this.Run();
            if (this.RunOnlyOnce)
            {
                this.Dispose();
            }
            else
            {
                this._timer.Change(this.Interval, this.Interval);
            }
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Task Thread
        /// </summary>
        internal TaskThread()
        {
            this._tasks = new Dictionary<string, Task>();
            this.Seconds = 10 * 60;
            this.FirstStartSeconds = 10 * 60;

        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if ((this._timer != null) && !this._disposed)
                {
                    lock (this)
                    {
                        this._timer.Dispose();
                        this._timer = null;
                        this._disposed = true;
                    }
                }
        }

        /// <summary>
        /// Disposes the instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Inits a timer
        /// </summary>
        public void InitTimer()
        {
            if (this._timer == null)
            {
                this._timer = new Timer(new TimerCallback(this.TimerHandler), null, this.FirstStartInterval, this.Interval);
            }
        }

        /// <summary>
        /// Adds a task to the thread
        /// </summary>
        /// <param name="task">The task to be added</param>
        public void AddTask(Task task)
        {
            if (!this._tasks.ContainsKey(task.Name))
            {
                this._tasks.Add(task.Name, task);
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the interval in seconds at which to run the tasks
        /// </summary>
        /// <value>The seconds.</value>
        public int FirstStartSeconds { get; set; }

        /// <summary>
        /// Gets or sets the interval in seconds at which to run the tasks
        /// </summary>
        /// <value>The seconds.</value>
        public int Seconds { get; set; }

        /// <summary>
        /// Get or sets a datetime when thread has been started
        /// </summary>
        /// <value>The started UTC.</value>
        public DateTime StartedUtc { get; private set; }

        /// <summary>
        /// Get or sets a value indicating whether thread is running
        /// </summary>
        /// <value><c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Get a list of tasks
        /// </summary>
        /// <value>The tasks.</value>
        public IList<Task> Tasks
        {
            get
            {
                var list = new List<Task>();

                foreach (var task in this._tasks.Values)
                {
                    list.Add(task);
                }
                return new ReadOnlyCollection<Task>(list);
            }
        }

        /// <summary>
        /// Gets the FirtStartInterval at which to run the tasks
        /// </summary>
        /// <value>The FirtStartInterval.</value>
        public int FirstStartInterval
        {
            get
            {
                return this.FirstStartSeconds * 1000;
            }
        }

        /// <summary>
        /// Gets the interval at which to run the tasks
        /// </summary>
        /// <value>The interval.</value>
        public int Interval
        {
            get
            {
                return this.Seconds * 1000;
            }
        }

        /// <summary>
        /// Gets or Sets the value indicating whether the thread whould be run only once (per appliction start)
        /// </summary>
        /// <value><c>true</c> if [run only once]; otherwise, <c>false</c>.</value>
        public bool RunOnlyOnce { get; set; }

        #endregion Properties
    }
}
