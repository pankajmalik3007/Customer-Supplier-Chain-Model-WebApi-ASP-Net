using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using RepositoryAndServices.Services.CustomServices.CustomerServices;
using RepositoryAndServices.Services.CustomServices.SupplierServices;
using RepositoryAndServices.Services.CustomServices.UserTypeServices;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        #region Private Variables and Constructor
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly ISupplierService _serviceSupplier;
        private readonly ICustomerService _serviceCustomer;
        private readonly IUserTypeService _serviceUserType;
        public UserController(ILogger<UserController> logger, ISupplierService serviceSupplier, ICustomerService serviceCustomer, IUserTypeService serviceUserType, IWebHostEnvironment environment)
        {
            _logger = logger;
            _serviceSupplier = serviceSupplier;
            _serviceUserType = serviceUserType;
            _serviceCustomer = serviceCustomer;
            _environment = environment;
        }
        #endregion


        #region Supplier Section

        [HttpGet(nameof(GetAllSupplier))]
        public async Task<ActionResult<UserViewModel>> GetAllSupplier()
        {
            var result = await _serviceSupplier.GetAll();
            if (result == null)
                return BadRequest("No Records Found, Please Try Again After Adding them...!");
            return Ok(result);

        }

        [HttpGet(nameof(GetSupplier))]

        public async Task<IActionResult> GetSupplier(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var result = await _serviceSupplier.Get(Id);
                if (result == null)
                    return BadRequest("No Records Found, Please Try Again After Adding them...!");
                return Ok(result);
            }
            else
                return NotFound("Invalid Supplier Id, Please Entering a Valid One...!");
        }


        [HttpPut(nameof(EditSupplier))]
        public async Task<IActionResult> EditSupplier([FromForm] UserUpdateModel customerModel)
        {
            if (ModelState.IsValid)
            {
                var CheckUser = await _serviceSupplier.Find(x => x.UserID == customerModel.UserID && x.Id != customerModel.Id);

                if (CheckUser != null)
                    return BadRequest("User ID : " + customerModel.UserID + " already Exist...!");
                else
                {
                    var CheckUsername = await _serviceSupplier.Find(x => x.UserName == customerModel.UserName && x.Id != customerModel.Id);
                    if (CheckUsername != null)
                        return BadRequest(" UserName :" + customerModel.UserName + " already Exist...!");
                }
                if (customerModel.ProfilePhoto != null)
                {
                    var photo = await UploadPhoto(customerModel.ProfilePhoto, customerModel.UserName, DateTime.Now.ToString("dd/MM/yyyy"));
                    if (string.IsNullOrEmpty(photo))
                        return BadRequest("Error While Uploading Supplier Profile Photo, Please Try again Later...!");
                    var result = await _serviceSupplier.Update(customerModel, photo);
                    if (result == true)
                        return Ok("Supplier Updated Succesfully...!");
                    else
                        return BadRequest("Something Went Wrong, Supplier Is Not Updated, Please Try After Sometime...!");
                }
                else
                {
                    var result = await _serviceSupplier.Update(customerModel, " ");
                    if (result == true)
                        return Ok("Supplier Updated Successfully...!");
                    else
                        return BadRequest("Something Went Wrong, Supplier Is Not Updated, Please Try After Sometime...!");
                }
            }
            else
                return NotFound("Supplier Not Found with id :" + customerModel.Id + ", Please Try Again After Sometime...!");
        }

        [HttpDelete(nameof(DeleteSupplier))]

        public async Task<IActionResult> DeleteSupplier(Guid Id)
        {
            var result = await _serviceSupplier.Delete(Id);
            if (result == true)
                return Ok("Supplier Deleted Successfully...!");
            else
                return BadRequest("Something Went Wrong, Supplier Is Not Deleted, Please Try After Sometime...!");
        }

        #endregion Supplier Section

        #region Customer Section

        [HttpGet(nameof(GetAllCustomer))]
        public async Task<ActionResult<UserViewModel>> GetAllCustomer()
        {
            var result = await _serviceCustomer.GetAll();
            if (result == null)
                return BadRequest("No Records Found, Please Try Again After Adding them...!");
            return Ok(result);
        }

        [HttpGet(nameof(GetCustomer))]
        public async Task<ActionResult<UserViewModel>> GetCustomer(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var result = await _serviceCustomer.Get(Id);
                if (result == null)
                    return BadRequest("No Records Found, Please Try Again After Adding them...!");
                return Ok(result);
            }
            else
                return NotFound("Invalid Customer Id, Please Entering a Valid One...!");
        }


        [HttpPut(nameof(EditCustomer))]
        public async Task<IActionResult> EditCustomer([FromForm] UserUpdateModel customerModel)
        {
            if (ModelState.IsValid)
            {
                var CheckUser = await _serviceCustomer.Find(x => x.UserID == customerModel.UserID && x.Id != customerModel.Id);

                if (CheckUser != null)
                    return BadRequest("User ID : " + customerModel.UserID + " already Exist...!");
                else
                {
                    var CheckUsername = await _serviceCustomer.Find(x => x.UserName == customerModel.UserName && x.Id != customerModel.Id);
                    if (CheckUsername != null)
                        return BadRequest(" UserName :" + customerModel.UserName + " already Exist...!");
                }
                if (customerModel.ProfilePhoto != null)
                {
                    var photo = await UploadPhoto(customerModel.ProfilePhoto, customerModel.UserName, DateTime.Now.ToString("dd/MM/yyyy"));
                    if (string.IsNullOrEmpty(photo))
                        return BadRequest("Error While Uploading Customer Profile Photo, Please Try again Later...!");
                    var result = await _serviceCustomer.Update(customerModel, photo);
                    if (result == true)
                        return Ok("Customer Updated Successfully...!");
                    else
                        return BadRequest("Something Went Wrong, Customer Is Not Updated, Please Try After Sometime...!");
                }
                else
                {
                    var result = await _serviceCustomer.Update(customerModel, " ");
                    if (result == true)
                        return Ok("Customer Updated Successfully...!");
                    else
                        return BadRequest("Something Went Wrong, Customer Is Not Updated, Please Try After Sometime...!");
                }
            }
            else
                return NotFound("Customer Not Found with id :" + customerModel.Id + ", Please Try Again After Sometime...!");
        }

        [HttpDelete(nameof(DeleteCustomer))]
        public async Task<IActionResult> DeleteCustomer(Guid Id)
        {
            var result = await _serviceCustomer.Delete(Id);
            if (result == true)
                return Ok("customer Deleted SUccessfully...!");
            else
                return BadRequest("Something Went Wrong, Customer Is Not Deleted, Please Try After Sometime...!");
        }

        #endregion Customer Section

        #region "Image Upload section"
        private async Task<string> UploadPhoto(IFormFile file, string Id, string date)
        {
            string fileName;
            Console.WriteLine(Id);
            _logger.LogInformation("Started uploading User Profile photo...!");
            //string wwwPath = this._environment.WebRootPath;
            string contentPath = this._environment.ContentRootPath;
            var extension = "." + file.FileName.Split('.')[^1];
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                fileName = Id.ToLower() + "-" + date + extension; //Create a new Name for the file due to security reasons.
                string outputFileName = Regex.Replace(fileName, @"[^0-9a-zA-Z.]+", "");
                var pathBuilt = Path.Combine(contentPath, "Images\\User");

                if (!Directory.Exists(pathBuilt))
                    Directory.CreateDirectory(pathBuilt);
                var path = Path.Combine(contentPath, "Images\\User", outputFileName);

                Console.WriteLine(path);

                using (var stream = new FileStream(path, FileMode.Create))
                    await file.CopyToAsync(stream);
                _logger.LogInformation("Successfully uploaded User photo with the file Name : " + outputFileName);
                return outputFileName;
            }
            else
                return "";
        }
        #endregion "Image Upload section"

    }
}
