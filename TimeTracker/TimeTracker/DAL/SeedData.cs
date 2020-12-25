using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTracker.DAL.DBModels.Auth;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.DAL.Models;

namespace TimeTracker.DAL
{
    public static class SeedData
    {
        public static void Init(ModelBuilder modelBuilder)
        {
            var admin = new User("admin@auth.com", "test")
            {
                Id = 1,
                //Guid = Guid.NewGuid(),
                Name = "Admin",
                AccountStatus = AccountStatus.Approved
            };

            var user = new User("user@auth.com", "test")
            {
                Id = 2,
                //Guid = Guid.NewGuid(),
                Name = "User",
                AccountStatus = AccountStatus.Approved
            };

            AddUserAndRole(modelBuilder, admin, user);

            AddParameter(modelBuilder);

            var taskType = AddTaskType(modelBuilder);

            var taskSource = AddTaskSource(modelBuilder);

            AddTask(modelBuilder, admin, user, taskType, taskSource);
        }

        /// <summary>
        /// add admin, user
        /// add role
        /// add role of user
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void AddUserAndRole(ModelBuilder modelBuilder, User admin, User user)
        {
            // add admin, user
            modelBuilder
                .Entity<User>()
                .HasData(admin);

            modelBuilder
                .Entity<User>()
                .HasData(user);

            modelBuilder
                .Entity<UserImage>()
                .HasData(new UserImage(1, SeedDataImg.GetPickachu()));

            modelBuilder
                .Entity<UserImage>()
                .HasData(new UserImage(2, SeedDataImg.GetTotoro()));

            modelBuilder
                .Entity<User>()
                .HasData(new User("test@auth.com", "test")
                {
                    Id = 3,
                    Name = "Test",
                    AccountStatus = AccountStatus.Uncheck
                });

            modelBuilder
                .Entity<User>()
                .HasData(new User("test2@auth.com", "test")
                {
                    Id = 4,
                    Name = "Test2",
                    AccountStatus = AccountStatus.Suspend
                });

            // add role
            modelBuilder
                .Entity<UserRole>()
                .HasData(new UserRole
                {
                    Id = (int)Models.UserRoles.Admin,
                    CodeName = Models.UserRoles.Admin.ToString(),
                    DisplayName = Models.UserRoles.Admin.ToString(),
                });

            modelBuilder
                .Entity<UserRole>()
                .HasData(new UserRole
                {
                    Id = (int)Models.UserRoles.User,
                    CodeName = Models.UserRoles.User.ToString(),
                    DisplayName = Models.UserRoles.User.ToString()
                });

            // add role of user
            modelBuilder
                .Entity<MapUserRole>()
                .HasData(new MapUserRole
                {
                    UserId = 1,
                    UserRolesId = (int)Models.UserRoles.Admin
                });

            for (int i = 2; i <= 4; i++)
            {
                modelBuilder
                    .Entity<MapUserRole>()
                    .HasData(new MapUserRole
                    {
                        UserId = i,
                        UserRolesId = (int)Models.UserRoles.User
                    });
            }
        }

        /// <summary>
        /// add DayWorkLimitTime
        /// add Period
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void AddParameter(ModelBuilder modelBuilder)
        {
            // add DayWorkLimitTime
            modelBuilder
                .Entity<DayWorkLimitTime>()
                .HasData(new DayWorkLimitTime()
                {
                    Guid = Guid.NewGuid(),
                    LimitWorkTime = 7.5,
                });

            // add Period
            modelBuilder
                .Entity<Period>()
                .HasData(new Period()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Test1",
                    StartDate = new DateTime(2020, 12, 10),
                    EndDate = new DateTime(2020, 12, 16)
                });
        }

        /// <summary>
        /// add TaskType
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static List<TaskType> AddTaskType(ModelBuilder modelBuilder)
        {
            string[] taskTypes = { "Coding", "Shopping", "Play Baseball", "Wash Floors", "Reading", "Play PC Game" };

            var taskTypeList = new List<TaskType>();

            taskTypeList.Add(new TaskType()
            {
                Guid = Guid.NewGuid(),
                CodeName = "default",
                DisplayName = ""
            });

            foreach (var taskType in taskTypes)
            {
                taskTypeList.Add(new TaskType()
                    {
                        Guid = Guid.NewGuid(),
                        CodeName = taskType,
                        DisplayName = taskType
                    });
            }

            foreach (var taskType in taskTypeList)
            {
                modelBuilder
                .Entity<TaskType>()
                .HasData(taskType);
            }

            return taskTypeList;
        }

        /// <summary>
        /// add TaskSource
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static List<TaskSource> AddTaskSource(ModelBuilder modelBuilder)
        {
            string[] taskSources = { "Boss", "Girl friend", "Father", "Mother" };

            var taskSourcesList = new List<TaskSource>();

            taskSourcesList.Add(new TaskSource()
            {
                Guid = Guid.NewGuid(),
                CodeName = "default",
                DisplayName = ""
            });

            foreach (var taskSource in taskSources)
            {
                taskSourcesList.Add(new TaskSource()
                    {
                        Guid = Guid.NewGuid(),
                        CodeName = taskSource,
                        DisplayName = taskSource
                    });
            }

            foreach (var taskSource in taskSourcesList)
            {
                modelBuilder
                    .Entity<TaskSource>()
                    .HasData(taskSource);
            }

            return taskSourcesList;
        }

        /// <summary>
        /// add taskday
        /// add task
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void AddTask(ModelBuilder modelBuilder, User admin, User user, List<TaskType> taskTypes, List<TaskSource> taskSources)
        {
            // add taskday
            var taskDays = new List<TaskDay>();

            taskDays.Add(new TaskDay(new DateTime(2020, 12, 10), admin)); // 0
            taskDays.Add(new TaskDay(new DateTime(2020, 12, 11), admin)); // 1
            taskDays.Add(new TaskDay(new DateTime(2020, 12, 12), admin)); // 2
            taskDays.Add(new TaskDay(new DateTime(2020, 12, 13), admin) { IsLeave = true });
            taskDays.Add(new TaskDay(new DateTime(2020, 12, 14), admin)); // 4
            taskDays.Add(new TaskDay(new DateTime(2020, 12, 16), admin) { IsLeave = true });

            taskDays.Add(new TaskDay(new DateTime(2020, 12, 10), user)); // 6
            taskDays.Add(new TaskDay(new DateTime(2020, 12, 11), user)); // 7
            taskDays.Add(new TaskDay(new DateTime(2020, 12, 12), user)); // 8

            foreach (var taskDay in taskDays)
            {
                modelBuilder
                .Entity<TaskDay>()
                .HasData(new {
                    Guid = taskDay.Guid,
                    Date = taskDay.Date,
                    UserId = taskDay.User.Id,
                    IsLeave = taskDay.IsLeave
                });
            }            
            
            // add task
            var adminTasks = new List<Task>();
            var userTasks = new List<Task>();

            // admin 2020-12-10
            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[0],
                Date = taskDays[0].Date,
                DisplayOrder = 0,

                ConsumeTime = 4,
                TaskType = taskTypes[1],
                TaskSource = taskSources[1],
                TaskName = "Track Time",
                TaskContent = "Architecture design"
            });

            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[0],
                Date = taskDays[0].Date,
                DisplayOrder = 1,

                ConsumeTime = 3,
                TaskType = taskTypes[6],
                TaskSource = taskSources[0],
                TaskName = "FEZ",
                TaskContent = ""
            });

            // admin 2020-12-11
            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[1],
                Date = taskDays[1].Date,
                DisplayOrder = 0,                

                ConsumeTime = 3,
                TaskType = taskTypes[0],
                TaskSource = taskSources[0],
                TaskName = "",
                TaskContent = "AAAAA"
            });

            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[1],
                Date = taskDays[1].Date,
                DisplayOrder = 1,

                ConsumeTime = 0,
                TaskType = taskTypes[0],
                TaskSource = taskSources[0],
                TaskName = "BBBBBB",
                TaskContent = ""
            });

            // admin 2020-12-12
            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[2],
                Date = taskDays[2].Date,
                DisplayOrder = 0,

                ConsumeTime = 2,
                TaskType = taskTypes[3],
                TaskSource = taskSources[3],
                TaskName = "Yeah",
                TaskContent = "hooooooooooo"
            });

            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[2],
                Date = taskDays[2].Date,
                DisplayOrder = 1,

                ConsumeTime = 4.5,
                TaskType = taskTypes[0],
                TaskSource = taskSources[1],
                TaskName = "",
                TaskContent = ""
            });

            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[2],
                Date = taskDays[2].Date,
                DisplayOrder = 2,

                ConsumeTime = 4,
                TaskType = taskTypes[6],
                TaskSource = taskSources[3],
                TaskName = "FEZ",
                TaskContent = "dadadadadada"
            });

            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[2],
                Date = taskDays[2].Date,
                DisplayOrder = 3,

                ConsumeTime = 1.5,
                TaskType = taskTypes[0],
                TaskSource = taskSources[0],
                TaskName = "CCCCCCCCCC",
                TaskContent = "cccCcccCcccC"
            });

            // admin 2020-12-13
            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[3],
                Date = taskDays[3].Date,
                DisplayOrder = 0,

                ConsumeTime = 7,
                TaskType = taskTypes[0],
                TaskSource = taskSources[0],
                TaskName = "",
                TaskContent = ""
            });

            // admin 2020-12-14
            adminTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[4],
                Date = taskDays[4].Date,
                DisplayOrder = 0,

                ConsumeTime = 4.5,
                TaskType = taskTypes[0],
                TaskSource = taskSources[0],
                TaskName = "",
                TaskContent = ""
            });

            // user 2020-12-10
            userTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[6],
                Date = taskDays[6].Date,
                DisplayOrder = 0,

                ConsumeTime = 3,
                TaskType = taskTypes[0],
                TaskSource = taskSources[0],
                TaskName = "",
                TaskContent = ""
            });

            // user 2020-12-11
            userTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[7],
                Date = taskDays[7].Date,
                DisplayOrder = 0,

                ConsumeTime = 2,
                TaskType = taskTypes[0],
                TaskSource = taskSources[0],
                TaskName = "",
                TaskContent = ""
            });

            userTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[7],
                Date = taskDays[7].Date,
                DisplayOrder = 1,

                ConsumeTime = 3,
                TaskType = taskTypes[0],
                TaskSource = taskSources[0],
                TaskName = "",
                TaskContent = ""
            });

            // user 2020-12-12
            userTasks.Add(new Task()
            {
                Guid = Guid.NewGuid(),
                TaskDay = taskDays[8],
                Date = taskDays[8].Date,
                DisplayOrder = 0,

                ConsumeTime = 5,
                TaskType = taskTypes[0],
                TaskSource = taskSources[0],
                TaskName = "",
                TaskContent = ""
            });

            var tasks = new List<Task>();
            tasks.AddRange(adminTasks);
            tasks.AddRange(userTasks);

            foreach (var task in tasks)
            {
                modelBuilder
                    .Entity<Task>()
                    .HasData(new {
                        Guid = task.Guid,
                        TaskDayGuid = task.TaskDay.Guid,
                        Date = task.Date,
                        DisplayOrder = task.DisplayOrder,

                        ConsumeTime = task.ConsumeTime,
                        TaskTypeGuid = task.TaskType.Guid,
                        TaskSourceGuid = task.TaskSource.Guid,
                        TaskName = task.TaskName,
                        TaskContent = task.TaskContent,
                    });
            }
        }
    }
}
