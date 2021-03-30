using System;
using Core.Interfaces.Data;

namespace Task.Cheduler.Interfaces
{
    public sealed class UserTask : BaseUniqueId<int>
    {
        private string taskSubject;

        private TaskStatus taskStatus;

        private TaskPriority taskPriority;

        private DateTime tasDateCreated;

        private DateTime taskStartData;

        private DateTime taskEndData;

        private User assignee;

        
        /// <summary>
        /// Краткое описани задачи
        /// </summary>
        public string TaskSubject
        {
            get { return taskSubject; }
            set { taskSubject = value; }
        }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public TaskStatus TaskStatus
        {
            get { return taskStatus; }
            set { taskStatus = value; }
        }

        /// <summary>
        /// Приоритет задачи
        /// </summary>
        public TaskPriority TaskPriority
        {
            get { return taskPriority; }
            set { taskPriority = value; }
        }

        /// <summary>
        /// Дата и время создания задачи
        /// </summary>
        public DateTime TasDateCreated
        {
            get { return tasDateCreated; }
            set { tasDateCreated = value; }
        }

        /// <summary>
        /// Старт задачи
        /// </summary>
        public DateTime TaskStartData
        {
            get { return taskStartData; }
            set { taskStartData = value; }
        }
        
        /// <summary>
        /// Deadline
        /// </summary>
        public DateTime TaskEndData
        {
            get { return taskEndData; }
            set { taskEndData = value; }
        }

        /// <summary>
        /// Исполняющий
        /// </summary>
        public User Assignee
        {
            get { return assignee; }
            set { assignee = value; }
        }

    }

    ///<summary>
    /// Приоритеты задач
    /// </summary>
    public enum TaskPriority
    {
        None = 0,
        High = 1,
        Normal = 2,
        Low = 3
    }

    /// <summary>
    /// Текущий статус задачи
    /// </summary>
    public enum TaskStatus
    {
        None = 0,
        New = 1,
        InProgress = 2,
        Deferred = 3,
        Passed = 4,
        Close = 5
    }   
}
