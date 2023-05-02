using AuthApi.Models;
using AuthApi.Models.Domain;
using AuthApi.Models.DTO;
using AuthApi.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly DatabaseContext _context;

        public UserController(IUserService userService, DatabaseContext context)
        {
            _userService = userService;
            this._context = context;
        }

        [HttpGet]
        public IActionResult GetData()
        {
            string msg = "Data from User controller";
            return Ok(new Status(200, "Success", msg));
        }

        [HttpGet]
        public async Task<IActionResult> userDetails()
        {
            var ab = User.Identities.ToList();
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await _userService.GetSingleUser(userEmail);
            if (result == null)
            {
                return Ok(new Status(404, "NO Record Found", null));
            }
            var userinfo = result;
            if (userinfo.ProfileImage != null)
            {
                string img = string.Format("{0}/Resources/ProfileImages/{1}", Models.Url.apiUrl,userinfo.ProfileImage);
                userinfo.ProfileImage = img;
            }
            return Ok(new Status(200, "Success", userinfo));
        }

        [HttpPost]
        public async Task<IActionResult> AddImageAsync([FromForm] UploadImage model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Status(400, "Please pass all the Fields", null));
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = _userService.AddImage(model.formFile, userEmail);
            if (result is null)
            {
                var allowedExtentions = new string[] { ".jpg", ".png", ".jpeg", ".PNG", ".JPG", ".JPEG" };
                string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtentions));
                return Ok(new Status(400, msg, null));
            }
            var userImageInstance = new UserImages
            {
                UserEmail = userEmail,
                UserImage = result.Result,
            };
            await _context.UserImages.AddAsync(userImageInstance);
            await _context.SaveChangesAsync();
            return Ok(new Status(200, "Image Added Successfully", null));
        }

        [HttpGet]
        public async Task<ActionResult<List<UserImages>>> ShowImages()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.AllUsers.SingleAsync(u => u.Email == userEmail);
            int userID = user.Id;
            var result = _userService.ShowImage(userEmail);
            if (result is null)
            {
                return Ok(new Status(400, "No image Found", null));
            }
            List<UserImageDto> resultList = new List<UserImageDto>();
            foreach (var res in result)
            {
                UserImageDto tempVar = new UserImageDto();
                tempVar.UserImage = res.UserImage;
                tempVar.Id = res.Id;

                resultList.Add(tempVar);
            }
            foreach (var res in resultList)
            {
                res.UserImage = string.Format("{0}/Resources/{1}/{2}",Models.Url.apiUrl, userID.ToString(), res.UserImage);
            }

            return Ok(new Status(200, "User Images", resultList));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImageAsync(JustId model)
        {
            if (model.id == null)
            {
                return Ok(new Status(400, "Please pass an id of image", null));
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = _userService.DeleteImage(model.id, userEmail);
            if (result.Result is null)
            {
                return Ok(new Status(400, "Error while deleting image (Image Not Found)", null));
            }
            return Ok(new Status(200, result.Result, null));
        }
    }
}
