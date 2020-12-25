using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL;
using TimeTracker.DAL.DBModels.Auth;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;
using TimeTracker.Helper.Extensions;
using TimeTracker.Models.Period;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        protected readonly ApplicationDbContext _context;

        public ImageController(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }

        protected UserImage _GetUserImage()
        {
            int? id = User.GetUserId();
            return id == null ? null : this._context.UserImage.SingleOrDefault(x => x.User.Id == id);
        }

        protected void _CreateUserImage(string image)
        {
            int? userId = User.GetUserId();
            this._context.UserImage.Add(new UserImage((int)userId, image));
            this._context.SaveChanges();
        }

        protected void _UpdateUserImage(UserImage userImage, string image)
        {
            userImage.Image = image;
            userImage.SetUpdatedDate();
            this._context.SaveChanges();
        }

        [HttpPost]
        public IActionResult GetImage()
        {
            var result = this._GetUserImage();

            return Ok( result == null ? "" : result.Image );
        }

        [HttpPost]
        public IActionResult CreateOrUpdateImage(UserImage tmpUserImage)
        {
            string image = tmpUserImage.Image;
            var userImage = this._GetUserImage();

            if (userImage == null)
            {
                this._CreateUserImage(image);
            }
            else
            {
                this._UpdateUserImage(userImage, image);
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteImage()
        {
            var userImage = this._GetUserImage();
            if (userImage == null)
            {
                return Ok();
            }

            this._context.UserImage.Remove(userImage);
            this._context.SaveChanges();

            return Ok();
        }
    }
}
