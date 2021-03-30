using System;
using System.Collections.Generic;
using Core.Interfaces.Data;

namespace Task.Cheduler.Interfaces
{
    public sealed class User : BaseUniqueId <int>
    {
        private string userFirstName;

        private string userLastName;

        private UserStatus userStatus;

        
        /// <summary>
        /// Имя
        /// </summary>
        public string UserFirstName
        {
            get { return userFirstName; }
            set { userFirstName = value; }
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string UserLastName
        {
            get { return userLastName; }
            set { userLastName = value; }
        }

        /// <summary>
        /// Статус сотрудника
        /// </summary>
        public UserStatus UserStatus
        {
            get { return userStatus; }
            set { userStatus = value; }
        }
    }

    public enum UserStatus
    {
        Administrator = 0,
        Manager = 1,
        Employee = 2
    }
   
}
