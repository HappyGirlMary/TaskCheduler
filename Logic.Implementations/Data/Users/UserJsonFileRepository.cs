using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Task.Cheduler.Interfaces;
using Task.Cheduler.Interfaces.Data.Users;
using Task.Cheduler.Interfaces.Exeption;

namespace Task.Cheduler.Logic.Implementations.Data.Users
{
    internal  sealed class UserJsonFileRepository : IUserRepository
    {
        private string userFileName = "users.json";

        private readonly string _fullfilePath;
      
        public UserJsonFileRepository()
        {
            string currentDirectory = Environment.CurrentDirectory;

            _fullfilePath = Path.Combine(currentDirectory, userFileName);
            
        }
        
        public IReadOnlyList<User> Read(out bool flag)
        {
            List<User> users = ReadFile();
            if (users is null)
            {
                flag = false;
                return ArraySegment<User>.Empty;
            }
            flag = true;
            return ReadFile();
        }

        public void Create(User data)
        {
            IList<User> users = ReadFile();
            if (users is null)
            {
                users = new List<User>(1);
            }
            
            data.Id = users.Count + 1; // совпадение id, если ранее юзер был удален

            users.Add(data);

            SaveUsers(users);
        }

        public void Update(User data)
        {
            IList<User> users = ReadFile();
            if (users is null)
            {
                throw new FailedFileReadExeption();
            }
            for (int index = 0; index < users.Count; index++)
            {
                User user = users[index];

                if (user.Id == data.Id)
                {
                    users[index] = data;
                    SaveUsers(users);
                    return;
                }
            }
            Create(data);
           // throw new UpdatingNonExistentUserRepositoryExeption();
        }

        public void Delete(int id)
        {
            IList<User> users = ReadFile();
            if (users is null)
            {
                throw new FailedFileReadExeption();
            }

            for (int index = 0; index < users.Count; index++)
            {
                User user = users[index];

                if (user.Id == id)
                {
                    user.IsDeleted = true;
                    SaveUsers(users);
                }
            }
            throw new DeletingNonExistentUserRepositoryExeption();
        }

        private void SaveUsers(IList<User> users)
        {
            string jsonBody = JsonConvert.SerializeObject(users);
            File.WriteAllText(_fullfilePath, jsonBody);
        }

        private List<User> ReadFile()
        {
            if (File.Exists(_fullfilePath))
            {
                string body = File.ReadAllText(_fullfilePath);

                IList<User> users = JsonConvert.DeserializeObject<IList<User>>(body);

                List<User> finalUserList = new List<User>();

                foreach (User user in users)
                {
                    if (user.IsDeleted) continue;
                    finalUserList.Add(user);
                }

                return finalUserList;
            }
            return null;
        }
    }
}
