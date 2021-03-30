using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task.Cheduler.Interfaces;
using Task.Cheduler.Interfaces.Data.Tasks;
using Task.Cheduler.Interfaces.Exeption;

namespace Task.Cheduler.Logic.Implementations.Data.Tasks
{
    internal sealed class TaskJsonFileRepository : ITaskRepository
    {
        private string taskFileName = "task.json";
        private readonly string _fullfilePath;
        
        public TaskJsonFileRepository()
        {
            string currentDirectory = Environment.CurrentDirectory;

            _fullfilePath = Path.Combine(currentDirectory, taskFileName);

        }

        public IReadOnlyList<UserTask> Read(out bool flag)
        {
            List<UserTask> users = ReadFile();
            if (users is null)
            {
                flag = false;
                return ArraySegment<UserTask>.Empty;
            }
            flag = true;
            return ReadFile();
        }

        public void Create(UserTask data)
        {
            IList<UserTask> tasks = ReadFile();
            if (tasks is null)
            {
                tasks = new List<UserTask>(1);
            }

            if (tasks.Count == 0)
                data.Id = tasks.Count + 1;
            else 
            {
                //ищем максимальное значение id 
                int maxIdTask = tasks.Max(task => task.Id);
                data.Id = maxIdTask + 1; // увеличиваем на 1
            }              

            tasks.Add(data);
            SaveUsers(tasks);
        }

        public void Update(UserTask data)
        {
            IList<UserTask> tasks = ReadFile();
            if (tasks is null)
            {
                throw new FailedFileReadExeption();
            }
            for (int index = 0; index < tasks.Count; index++)
            {
                UserTask user = tasks[index];

                if (user.Id == data.Id)
                {
                    tasks[index] = data;
                    SaveUsers(tasks);
                    return;
                }
            }
            Create(data);
            // throw new UpdatingNonExistentUserRepositoryExeption();
        }

        public void Delete(int id)
        {
            IList<UserTask> tasks = ReadFile();
            if (tasks is null)
            {
                throw new FailedFileReadExeption();
            }

            for (int index = 0; index < tasks.Count; index++)
            {
                UserTask task = tasks[index];

                if (task.Id == id)
                {
                    task.IsDeleted = true;
                    SaveUsers(tasks);                    
                }
            }
          //  if(flag)
             //   throw new DeletingNonExistentUserRepositoryExeption();
        }

        private void SaveUsers(IList<UserTask> tasks)
        {
            string jsonBody = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(_fullfilePath, jsonBody);
        }

        private List<UserTask> ReadFile()
        {
            if (File.Exists(_fullfilePath))
            {
                string body = File.ReadAllText(_fullfilePath);

                IList<UserTask> tasks = JsonConvert.DeserializeObject<IList<UserTask>>(body);

                List<UserTask> finalUserList = new List<UserTask>();

                foreach (UserTask task in tasks)
                {
                    if (task.IsDeleted) continue;
                    finalUserList.Add(task);
                }

                return finalUserList;
            }
            return null;
        }       
    }
}
