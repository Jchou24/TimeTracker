using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TimeTracker.DAL.Attributes;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.DAL.DBModelsBase;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;

namespace TimeTracker.DAL.DBModels.Auth
{
    public class UserImage : EntityDateInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        /// <summary>
        /// base 64 string
        /// </summary>
        public string Image { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public UserImage(){}

        public UserImage(int userId, string image)
        {
            this.Guid = Guid.NewGuid();

            this.Image = image;

            this.UserId = userId;
        }
    }
}
