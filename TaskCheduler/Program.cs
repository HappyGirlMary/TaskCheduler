using Core.Interfaces.Data.Repository;
using Core.Interfaces.Logging;
using Core.Logic.Implementation;
using System;
using System.Collections.Generic;
using Task.Cheduler.Interfaces;
using Task.Cheduler.Interfaces.Data.Users;
using Task.Cheduler.Interfaces.Data.Tasks;
using Task.Cheduler.Logic.Implementations;
using System.IO;

namespace TaskCheduler
{
    class Program
    {
        /// <summary>
        /// Реализация у самой не вызывает восторга ))) поздно приступила с д/з
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ILoggerService logger = LoggerFactory.CreateLogger();         

            

           ITaskRepository taskRepository = TaskRepositoryFactory.Create();
            
            List<UserTask> tasks = new List<UserTask>();
            bool flag;

            Show();
            ConsoleKeyInfo cki;

            //Выполняем пока не нажмем ESC          
            do
            {
                cki = Console.ReadKey();
                switch (cki.Key)
                {
                    //Вывести список задач на консоль
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        {
                            #region Вывод списка задач на консоль

                            if(CheckTaskListForNull(out IReadOnlyRepository<UserTask> taskRreadOnlyRepository))
                            { 
                                tasks = (List<UserTask>)taskRreadOnlyRepository.Read(out flag);
                                if(tasks.Count !=0)
                                    DisplayTaskListOnConsole(tasks);
                                else
                                    OutputStringConsole("Список задач пуст", null, ConsoleColor.Green);
                            }
                            
                            Show();
                            break;
                            #endregion
                        }
                    //Создать новую задачу
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:
                        {
                            #region Добавление таски
                            
                            OutputStringConsole("=== Добавление новой задачи ===", null, ConsoleColor.Magenta);                            

                            UserTask task = new UserTask() { };

                            //Дата и время создание задачи
                            task.TasDateCreated = DateTime.Now;

                            //Описание задачи
                            Console.WriteLine("Введите описание задачи: ");
                            string taskSubject = Console.ReadLine();
                            task.TaskSubject = taskSubject;

                            //Выбор исполнителя
                            task.Assignee  = Assignee();
                            
                            //Задаем статус задачи - New
                            task.TaskStatus = TaskStatus.New;

                            //Указываем приоритет задачи
                            Priority(out TaskPriority priority);
                            task.TaskPriority = priority;

                            Console.WriteLine("\nСрок выполнения задачи");

                            //устанавливаем срок сдачи задачи
                            DeadlineDataTime(out DateTime endDataTime);
                            task.TaskEndData = endDataTime;

                            taskRepository.Create(task);

                            OutputStringConsole("Задача успешно создана", null, ConsoleColor.Green);                            
                            
                            Show();
                            break;
                            #endregion
                        }
                    //Edite task
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3:
                        {
                            #region Редактирование таски
                            //реализовать
                            OutputStringConsole("Не успела реализовать, но точно доделаю ))", null, ConsoleColor.Blue);
                            Show();
                            break;
                            #endregion
                        }
                    //Delete task
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.D4:
                        {
                            #region Удаление таски
                            bool fl = true;
                            if (CheckTaskListForNull(out IReadOnlyRepository<UserTask> taskRreadOnlyRepository))
                            {
                                tasks = (List<UserTask>)taskRreadOnlyRepository.Read(out flag);

                                DisplayTaskListOnConsole(tasks);

                                OutputStringConsole("Указите номер задачи для удаления:", null, ConsoleColor.Green);

                                string strTask = Console.ReadLine();

                                EnteredStringParse(strTask, out int numTask, out flag);

                                foreach (var task in tasks)
                                {
                                    if (numTask == task.Id)
                                    {
                                        taskRepository.Delete(task.Id);

                                        OutputStringConsole("Задача номер {0} удалена", numTask, ConsoleColor.Yellow);

                                        fl = false;
                                    }
                                }
                                if (fl)
                                    Console.WriteLine("Задачи с данным номером не существует\n");
                                Show();
                            }
                            break;
                            #endregion
                        }
                    //Создать нового юзера
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.D5:
                        {
                            #region Добавление нового юзера

                            OutputStringConsole("=== Добавление нового юзера ===", null, ConsoleColor.Magenta);

                            AddNewUser();

                            OutputStringConsole("Новый юзер добавлен", null, ConsoleColor.Green);                            
                            Show();
                            break;
                            #endregion
                        }
                    default:
                        break;
                }
            }
            while (cki.Key != ConsoleKey.Escape);    
        }


        private static bool CheckTaskListForNull(out IReadOnlyRepository<UserTask> taskRreadOnlyRepository)
        {
            taskRreadOnlyRepository = TaskRepositoryFactory.Create();
            taskRreadOnlyRepository.Read(out bool listNotEmpty);
            if (!listNotEmpty)
            {
                OutputStringConsole("Список задач пуст", null, ConsoleColor.Green);
                return listNotEmpty;
            }
            else
            {
                return true;
            }            
        }

        /// <summary>
        /// Добавление нового юзера
        /// </summary>
        /// <returns></returns>
        private static User AddNewUser()
        {
            IUserRepository userRepository = UserRepositoryFactory.Create();
            User user = new User() { };

            //Имя
            Console.WriteLine("Введите имя юзера: ");
            string firstName = Console.ReadLine();
            user.UserFirstName = firstName;

            //Фамилия
            Console.WriteLine("Введите фамилию юзера: ");
            string lastName = Console.ReadLine();
            user.UserLastName = lastName;

            //Указываем приоритет задачи
            Status(out UserStatus status);
            user.UserStatus = status;

            userRepository.Create(user);
            return user;
        }

        /// <summary>
        /// Вывод окрашенной строки на экран
        /// </summary>
        /// <param name="outputString">строка</param>
        /// <param name="arg">параметр</param>
        /// <param name="color">установить цвет</param>
        private static void OutputStringConsole(string outputString, object arg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            if(arg!= null) 
                Console.WriteLine(outputString, arg);
            else
                Console.WriteLine("{0}\n", outputString);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Формирование даты дедлайна
        /// </summary>
        /// <param name="endDataTime"></param>
        private static void DeadlineDataTime(out DateTime endDataTime)
        {
            CheckingValidityEnteredDate("\nВведите год: ", DateTime.Now.Year, MaxInputDate.Year, true, out int dateYearTask);

            bool validDate;

            if (dateYearTask == DateTime.Now.Year)
                validDate = true;
            else
                validDate = false;
            CheckingValidityEnteredDate("\nВведите месяц: ", DateTime.Now.Month, MaxInputDate.Month, validDate, out int dateMonthTask);


            if (dateMonthTask == DateTime.Now.Month)
                validDate = true;
            else
                validDate = false;
            CheckingValidityEnteredDate("\nВведите день: ", DateTime.Now.Day, MaxInputDate.Day, validDate, out int dateDayTask);
           

            endDataTime = new DateTime(dateYearTask, dateMonthTask, dateDayTask);
        }

        /// <summary>
        /// Проверка валидности вводимых данных
        /// </summary>
        /// <param name="consoleString"></param>
        /// <param name="nowDate">текущаа дата</param>
        /// <param name="maxData">максимально допустимая дата</param>
        /// <param name="flag"></param>
        /// <param name="dateTask">дата дла записи в список</param>
        private static void CheckingValidityEnteredDate(string consoleString, int nowDate, MaxInputDate maxData, bool flag, out int dateTask)
        {
            dateTask = 1;
            bool dateTrue = true;
            while (dateTrue)
            {
                Console.WriteLine(consoleString);
                string enterString = Console.ReadLine();
                EnteredStringParse(enterString, out int number, out bool enteredNumber);
                if ((flag && number < nowDate)| number == 0)
                {
                    OutputStringConsole("Введенная вами дата неактаульна\nУкажите дату не ниже: {0}", nowDate, ConsoleColor.Yellow);                    
                }
                else if (number > (int)maxData)
                {
                    OutputStringConsole("Введенная вами дата невалидна", null, ConsoleColor.Yellow);                                  
                }
                else
                {
                   dateTask = number;
                   dateTrue = false;                    
                }
            }           

        }

        /// <summary>
        /// Устанавливаем статус usera
        /// </summary>
        /// <param name="status"></param>
        private static void Status(out UserStatus status)
        {
            bool bl = false;
            status = UserStatus.Administrator;
            while (!bl)
            {
                Console.WriteLine("Укажите приоритет задачи:\n 1 - Manager\n 2 - Employee\n");
                string enteredString = Console.ReadLine();

                EnteredStringParse(enteredString, out int num, out bool flag);
                if (num < 3)
                {
                    switch (num)
                    {
                        case 0:
                            {
                                bl = true;
                                status = UserStatus.Administrator;
                                break;
                            }
                        case 1:
                            {
                                bl = true;
                                status = UserStatus.Manager;
                                break;
                            }
                        case 2:
                            {
                                bl = true;
                                status = UserStatus.Employee;
                                break;
                            }                        
                    }
                }
                else
                {
                    OutputStringConsole("Введите номер статуса из списка", null, ConsoleColor.Yellow);                    
                }
            }
        }

        /// <summary>
        /// Вывод полей списка задач
        /// </summary>
        /// <param name="userTasks"></param>
        private static void DisplayTaskListOnConsole(List<UserTask> userTasks)
        {
            foreach (var task in userTasks)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("======= Задача номер: {0} =======", task.Id);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Дата создания:   {0}", task.TasDateCreated);
                Console.WriteLine("Описание задачи: {0}", task.TaskSubject);

                if(task.Assignee == null)
                {
                    Console.WriteLine("Исполнитель: ");
                }
                else
                    Console.WriteLine("Исполнитель:     {0} {1}", task.Assignee.UserFirstName, task.Assignee.UserLastName);

                Console.WriteLine("Статус:          {0}", task.TaskStatus);
                Console.WriteLine("Приоритет:       {0}", task.TaskPriority);
                Console.WriteLine("Срок сдачи:      {0}", task.TaskEndData);
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Отображаем подобие меню (если это так можно назвать) программы, 
        /// </summary>
        private static void Show()
        {
            OutputStringConsole("=== Планировщик задач ===", null, ConsoleColor.Magenta);
            Console.WriteLine("Выберете действия:\n 1 - Просмотреть текущие задачи\n 2 - Добавить новую задачу\n 3 - Внести изменения в задачу\n 4 - Удалить задачу\n 5 - Добавить нового user\n Esc - Выйти из программы\n");
        }

        /// <summary>
        /// Парсим введеную строку
        /// </summary>
        /// <param name="value">вводимая строка</param>
        /// <param name="number"></param>
        /// <param name="enteredNumber">флак указывающий что введенная строка является числом</param>
        private static void EnteredStringParse(string value, out int number, out bool enteredNumber)
        {
            bool result = Int32.TryParse(value, out int num);
            if (result)
            {
                number = num;
                enteredNumber = true;
            }
            else
            {
                number = 0;
                enteredNumber = false;

                OutputStringConsole("\nНеобходимо ввести число", null, ConsoleColor.Yellow);                
            }            
        }

        /// <summary>
        /// Выбираем исполнителя
        /// </summary>
        /// <param name="users">список изеров</param>
        /// <param name="assignee">назначенный исполнитель</param>
        private static User Assignee()
        {
            IReadOnlyRepository<User> userRreadOnlyRepository = UserRepositoryFactory.Create();
            
            userRreadOnlyRepository.Read(out bool flag);
            if (flag == false)
            {
                OutputStringConsole("Список исполнителе пуст\nДобавте нового исполнителя:", null, ConsoleColor.Green);
                return AddNewUser();
            }

            List<User> users = (List<User>)userRreadOnlyRepository.Read(out bool f);
            //Флаг для выхода из цикла 
            bool exitLoop = false;
           
            while (!exitLoop)
            {
                Console.WriteLine("\nСписок исполнителей: ");
                foreach (var us in users)
                {
                    Console.WriteLine("{0} - {1} {2}", us.Id, us.UserFirstName, us.UserLastName);
                }

                Console.WriteLine("\nВведите номер исполнителя: ");

                string id = Console.ReadLine();

                EnteredStringParse(id, out int num, out bool flg);
                //Если пользователь ввел валидный номер из списка 
                //то пробегаемся по нашему списку 
                if (flg)
                {
                    foreach (var user in users)
                    {
                        if (num == user.Id)
                        {
                            exitLoop = true;
                            return user;
                        }                        
                    }
                }
                //Если введенный номер не из списка выводим сообщение
                OutputStringConsole("Введите номер исполнителя из списка", null, ConsoleColor.Yellow);
            }
            return null;
        }

        /// <summary>
        /// Устанавливаем приоритет
        /// </summary>
        /// <param name="priority"></param>
        private static void Priority(out TaskPriority priority)
        {
            bool bl = false;
            priority = TaskPriority.None;
            while (!bl)
            {
                Console.WriteLine("\nУкажите приоритет задачи:\n 1 - High\n 2 - Normal\n 3 - Low");
                string status = Console.ReadLine();

                EnteredStringParse(status, out int num, out bool flag);
                if (num < 4)
                {
                    switch (num)
                    {
                        case 0:
                            {
                                bl = true;
                                priority = TaskPriority.None;
                                break;
                            }
                        case 1:
                            {
                                bl = true;
                                priority = TaskPriority.High;
                                break;
                            }
                        case 2:
                            {
                                bl = true;
                                priority = TaskPriority.Normal;
                                break;
                            }
                        case 3:
                            {
                                bl = true;
                                priority = TaskPriority.Low;
                                break;
                            }
                    }
                }
                else
                {
                    OutputStringConsole("Введите номер приоритета из списка", null, ConsoleColor.Yellow);                    
                }
            }            
        }
    }
    public enum DateStatus
    {
        Year = 0,
        Month = 1,
        Day = 2
    }

    public enum MaxInputDate
    {
        Year = 9999,
        Month = 12,
        Day = 30
    }
}
