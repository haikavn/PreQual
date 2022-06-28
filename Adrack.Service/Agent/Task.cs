// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="Task.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Agent;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Configuration;
using Adrack.Service.Audit;
using Adrack.Service.Infrastructure;
using Autofac;
using System;

namespace Adrack.Service.Agent
{
    /// <summary>
    /// Represents a Task
    /// </summary>
    public partial class Task
    {
        #region Fields

        /// <summary>
        /// Gets or sets the task to execute.
        /// </summary>
        /// <value>The task to execute.</value>
        public ITask TaskToExecute { get; set; }

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Create Task
        /// </summary>
        /// <param name="scope">Lifetime Scope</param>
        /// <returns>Task Item</returns>
        private ITask CreateTask(ILifetimeScope scope)
        {
            ITask task = null;

            if (this.Enabled)
            {
                var serviceType = System.Type.GetType(this.ServiceType);

                if (serviceType != null)
                {
                    object instance;

                    if (!AppEngineContext.Current.ContainerManager.TryResolve(serviceType, scope, out instance))
                    {
                        instance = AppEngineContext.Current.ContainerManager.ResolveUnregistered(serviceType, scope);
                    }

                    task = instance as ITask;
                }
            }

            return task;
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Task
        /// </summary>
        private Task()
        {
            this.Enabled = true;
        }

        /// <summary>
        /// Task
        /// </summary>
        /// <param name="task">Task</param>
        public Task(ScheduleTask task)
        {
            this.Name = task.Name;
            this.ServiceType = task.ServiceType;
            this.Enabled = task.Enabled;
            this.StopOnError = task.StopOnError;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Executes the task
        /// </summary>
        /// <param name="throwException">A value indicating whether exception should be thrown if some error happens</param>
        /// <param name="dispose">A value indicating whether all instances hsould be disposed after task run</param>
        /// <param name="ensureRunOnOneWebFarmInstance">A value indicating whether we should ensure this task is run on one farm node at a time</param>
        /// <exception cref="Exception">Machine name cannot be detected. You cannot run in web farm.</exception>
        public void Execute(bool throwException = false, bool dispose = true, bool ensureRunOnOneWebFarmInstance = true)
        {
            var scope = AppEngineContext.Current.ContainerManager.Scope();

            var scheduleTaskService = AppEngineContext.Current.ContainerManager.Resolve<IScheduleTaskService>("", scope);

            var scheduleTask = scheduleTaskService.GetScheduleTaskByServiceType(this.ServiceType);

            try
            {
                if (ensureRunOnOneWebFarmInstance)
                {
                    var appConfiguration = AppEngineContext.Current.ContainerManager.Resolve<AppConfiguration>("", scope);

                    if (appConfiguration.WebFarmMultipleInstanceEnabled)
                    {
                        var currentMachineName = AppEngineContext.Current.ContainerManager.Resolve<IMachineName>("", scope);

                        var machineName = currentMachineName.GetMachineName();

                        if (String.IsNullOrEmpty(machineName))
                        {
                            throw new Exception("Machine name cannot be detected. You cannot run in web farm.");
                        }

                        //if (scheduleTask.MachineDuration.HasValue && scheduleTask.MachineDuration.Value >= DateTime.UtcNow && scheduleTask.MachineName != machineName)
                          //  return;

                        scheduleTask.MachineName = machineName;
                        scheduleTask.MachineDuration = DateTime.UtcNow.AddMinutes(30);
                        scheduleTaskService.UpdateScheduleTask(scheduleTask);
                    }
                }

                var task = this.CreateTask(scope);

                if (task != null)
                {
                    this.LastStart = DateTime.UtcNow;

                    if (scheduleTask != null)
                    {
                        scheduleTask.LastStart = this.LastStart;
                        scheduleTaskService.UpdateScheduleTask(scheduleTask);
                    }

                    task.Execute();

                    this.LastEnd = this.LastSuccess = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                this.Enabled = !this.StopOnError;
                this.LastEnd = DateTime.UtcNow;

                // Log Error
                var logService = AppEngineContext.Current.ContainerManager.Resolve<ILogService>("", scope);

                logService.Error(string.Format("Error while running the '{0}' schedule task. {1}", this.Name, ex.Message), ex);

                if (throwException)
                    throw;
            }

            if (scheduleTask != null)
            {
                scheduleTask.LastEnd = this.LastEnd;
                scheduleTask.LastSuccess = this.LastSuccess;
                scheduleTaskService.UpdateScheduleTask(scheduleTask);
            }

            if (dispose)
            {
                scope.Dispose();
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        /// <value>The type of the service.</value>
        public string ServiceType { get; private set; }

        /// <summary>
        /// Gets or Sets the Enabled
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Gets or Sets the Stop On Error
        /// </summary>
        /// <value><c>true</c> if [stop on error]; otherwise, <c>false</c>.</value>
        public bool StopOnError { get; private set; }

        /// <summary>
        /// Gets or Sets the Last Start
        /// </summary>
        /// <value>The last start.</value>
        public DateTime? LastStart { get; private set; }

        /// <summary>
        /// Gets or Sets the Last End
        /// </summary>
        /// <value>The last end.</value>
        public DateTime? LastEnd { get; private set; }

        /// <summary>
        /// Gets or Sets the Last Success
        /// </summary>
        /// <value>The last success.</value>
        public DateTime? LastSuccess { get; private set; }

        #endregion Properties
    }
}