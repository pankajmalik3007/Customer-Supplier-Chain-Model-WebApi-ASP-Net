using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RepositoryAndServices.Services.CustomServices.UserTypeServices;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : Controller
    {
        #region Private Variables and Constructor
        private readonly IUserTypeService _serviceUserType;
        public UserTypeController(IUserTypeService serviceUserType)
        {
            _serviceUserType = serviceUserType;
        }
        #endregion

        #region UserType Section

        [Route("GetAllUserType")]
        [HttpGet]
        public async Task<ActionResult<UserTypeViewModel>> GetAllUserType()
        {
            var result = await _serviceUserType.GetAll();

            if(result == null)
                return BadRequest("No Records Found, Please Try Again After Adding them...!");

            return Ok(result);
        }


        [Route("GetUserType")]
        [HttpGet]
        public async Task<ActionResult<UserTypeViewModel>> GetUserType(Guid Id)
        {
            if(Id != Guid.Empty)
            { 
                var result = await _serviceUserType.GetAll();

                if (result == null)
                    return BadRequest("No Records Found, Please Try Again After Adding them...!");

                return Ok(result);
            }
            else
                return NotFound("Invalid UserType ID, Please Entering a Valid One...!");
        }

        [Route("InsertUserType")]
        [HttpPost]
        public async Task<IActionResult> InsertUserType(UserTypeInsertModel userTypeInsertModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceUserType.Insert(userTypeInsertModel);
                if (result == true)
                    return Ok("UserType Inserted Successfully...!");
                else
                    return BadRequest("Something Went Wrong, UserType Is Not Inserted, Please Try After Sometime...!");
            }
            else
                return BadRequest("Invalid UserType Information, Please Provide Correct Details for UserType...!");
        }

        [Route("UpdateUserType")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserType(UserTypeUpdateModel userTypeModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceUserType.Update(userTypeModel);
                if (result == true)
                    return Ok(userTypeModel);
                else
                    return BadRequest("Something went wrong, Please Try again later...!");
            }
            else
                return BadRequest("Invalid UserType Information, Please Provide Correct Details for UserType...!");
        }

        [Route("DeleteUserType")]
        [HttpDelete]

        public async Task<IActionResult> DeleteUserType(Guid Id)
        {
            var result = await _serviceUserType.Delete(Id);
            if (result == true)
                return Ok("UserType Deleted Successfully...!");
            else
                return BadRequest("UserType is not deleted, Please Try again later...!");

        }
        #endregion
    }
}
