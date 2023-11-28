using Domain.Models;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using RepositoryAndServices.Services.CustomServices.ItemServices;
using RepositoryAndServices.Services.CustomServices.CategoryServices;
using RepositoryAndServices.Services.CustomServices.SupplierServices;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ItemController : ControllerBase
    {
        #region Private Variables and Constructor
        private readonly ILogger _logger;
        private readonly IItemService _serviceItem;
        private readonly ISupplierService _serviceUser;
        private readonly IWebHostEnvironment _environment;
        private readonly ICategoryService _serviceCategory;
        public ItemController(ILogger<ItemController> logger, IItemService serviceItem, ISupplierService serviceUser, ICategoryService serviceCategory, IWebHostEnvironment environment)
        {
            _logger = logger;
            _serviceItem = serviceItem;
            _serviceUser = serviceUser;
            _environment = environment;
            _serviceCategory = serviceCategory;
        }
        #endregion

        #region "Image Upload section"

        [HttpGet(nameof(GetAllItemBySupplier))]
        public async Task<ActionResult<ItemViewModel>> GetAllItemBySupplier(Guid id)
        {
            ICollection<ItemViewModel> items = await _serviceItem.GetAllItemByUser(id);
            if (items == null)
                return BadRequest("No Records Found, Please Try Again After Adding them...!");
            else
                return Ok(items);

        }
        [HttpGet(nameof(GetAllItemByCustomer))]
        public async Task<ActionResult<ItemViewModel>> GetAllItemByCustomer(Guid id)
        {
            ICollection<ItemViewModel> items = await _serviceItem.GetAllItemByUser(id);
            if (items.ToList().Count == 0)
                return BadRequest("Customer is not valid, Please Enter Valid Customer Details...!");
            else
                return Ok(items);
        }

        [HttpGet(nameof(GetItem))]
        public async Task<ActionResult<ItemViewModel>> GetItem(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var result = await _serviceItem.Get(Id);
                if (result == null)
                {
                    return BadRequest("No Records Found, Please Try Again After Adding them...!");
                }
                return Ok(result);
            }
            else
                return NotFound("Invalid Item Id, Please Entering a Valid One...!");

        }

        [HttpPost(nameof(AddExistingItemToSupplier))]
        public async Task<IActionResult> AddExistingItemToSupplier([FromForm] ExistingItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User supplier = await _serviceUser.Find(x => x.Id == itemModel.UserId);
                if (supplier != null)
                {
                    var CheckItem = await _serviceItem.Find(x => x.Id == itemModel.Id);

                    if (CheckItem == null)
                        return BadRequest("Item not Exist with Id :" + itemModel.Id + ", Please Try Again After Sometime...!");
                    else
                    {

                        var CheckUser = await _serviceUser.Find(x => x.Id == itemModel.UserId);
                        if (CheckUser != null)
                        {
                            _logger.LogInformation("Starting Item Insert to Supplier Item...!");
                            var result = await _serviceItem.InsertExistingItem(itemModel);
                            if (result == true)
                                return Ok("Succesfully Inserted Item to Supplier Item...!");
                            else
                                return BadRequest("Invalid Supplier Item Information, Please Entering a Valid One...!");

                        }
                        else
                        {
                            _logger.LogWarning("User not Exist with Id :" + itemModel.UserId + ", Please Try Again After Sometime...!");
                            return BadRequest("User not Exist with Id :" + itemModel.UserId + ", Please Try Again After Sometime...!");
                        }
                    }
                }
                else
                    return BadRequest("Unauthorized User, Please Provide Valid Credentials and Try Again Later...!");
            }
            else
                return BadRequest("Invalid Supplier Item Information, Please Entering a Valid One...!");
        }

        [HttpPost(nameof(AddExistingItemToCustomer))]
        public async Task<IActionResult> AddExistingItemToCustomer([FromForm] ExistingItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User supplier = await _serviceUser.Find(x => x.Id == itemModel.UserId);
                if (supplier != null)
                {
                    var CheckItem = await _serviceItem.Find(x => x.Id == itemModel.Id);

                    if (CheckItem == null)
                        return BadRequest("Item not Exist with Id :" + itemModel.Id + ", Please Try Again After Sometime...!");
                    else
                    {

                        var CheckUser = await _serviceUser.Find(x => x.Id == itemModel.UserId);
                        if (CheckUser != null)
                        {
                            var result = await _serviceItem.InsertExistingItem(itemModel);
                            if (result == true)
                                return Ok("Succesfully Inserted Item to Customer Item...!");
                            else
                                return BadRequest("Invalid Customer Item Information, Please Entering a Valid One...!");
                        }
                        else
                            return BadRequest("User not Exist with Id :" + itemModel.UserId + ", Please Try Again After Sometime...!");
                    }
                }
                else
                    return BadRequest("Unauthorized User, Please Provide Valid Credentials and Try Again Later...!");
            }
            else
                return BadRequest("Invalid Customer Item Information, Please Entering a Valid One...!");


        }

        [HttpPost(nameof(AddSupplierItem))]
        public async Task<IActionResult> AddSupplierItem([FromForm] ItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User supplier = await _serviceUser.Find(x => x.Id == itemModel.UserId);
                if (supplier != null)
                {
                    var CheckItem = await _serviceItem.Find(x => x.ItemCode == itemModel.ItemCode);
                    if (CheckItem != null)
                        return BadRequest("Item already Exist with Id :" + itemModel.ItemCode + ", Please Try Again After Sometime...!");
                    else
                    {
                        var CheckItemName = await _serviceItem.Find(x => x.ItemName == itemModel.ItemName);
                        if (CheckItemName != null)
                            return BadRequest(" ItemName :" + itemModel.ItemName + " already Exist...!");
                    }
                    var category = await _serviceCategory.Get(itemModel.CategoryId);
                    if (category == null)
                        return NotFound("Category Not Found, Please Provide Valid Category...!");

                    var photo = await UploadPhoto(itemModel.ItemImage, itemModel.ItemName);
                    var result = await _serviceItem.Insert(itemModel, photo);
                    if (result == true)
                        return Ok("Succesfully Item inserted...!");
                    else
                        return BadRequest("Something Went Wrong, Please Try Again After Sometime...!");
                }
                else
                    return BadRequest("Unauthorized Supplier, Please Provide Valid Credentials and Try Again Later...!");
            }
            else
            {
                _logger.LogWarning("Invalid Customer Information, Please Entering a Valid One...!");
                return BadRequest("Invalid Customer Information, Please Entering a Valid One...!");
            }

        }


        [HttpPost(nameof(AddCustomerItem))]
        public async Task<IActionResult> AddCustomerItem([FromForm] ItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User customer = await _serviceUser.Find(x => x.Id == itemModel.UserId);
                if (customer != null)
                {
                    var CheckItem = await _serviceItem.Find(x => x.ItemCode == itemModel.ItemCode);

                    if (CheckItem != null)
                        return BadRequest("Item already Exist with Id :" + itemModel.ItemCode + ", Please Try Again After Sometime...!");
                    else
                    {
                        var CheckItemName = await _serviceItem.Find(x => x.ItemName == itemModel.ItemName);
                        if (CheckItemName != null)
                        {
                            return BadRequest(" ItemName :" + itemModel.ItemName + " already Exist...!");
                        }
                    }
                    var category = await _serviceCategory.Get(itemModel.CategoryId);
                    if (category == null)
                        return NotFound("Category Not Found, Please Provide Valid Category...!");

                    var photo = await UploadPhoto(itemModel.ItemImage, itemModel.ItemName);
                    var result = await _serviceItem.Insert(itemModel, photo);
                    if (result == true)
                        return Ok("Succesfully Item inserted...!");
                    else
                        return BadRequest("Something Went Wrong, Please Try Again After Sometime...!");

                }
                else
                    return BadRequest("Unauthorized Customer, Please Provide Valid Credentials and Try Again Later...!");
            }
            else
            {
                return BadRequest("Invalid Customer Information, Please Entering a Valid One...!");
            }
        }

        [HttpPut(nameof(EditItem))]
        public async Task<IActionResult> EditItem([FromForm] ItemUpdateModel itemModel)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Finding for Customer Update id" + itemModel.Id);
                ItemViewModel item = await _serviceItem.Get(itemModel.Id);
                if (item.ItemId == itemModel.Id)
                {
                    if (item != null)
                    {
                        var CheckItem = await _serviceItem.Find(x => x.ItemCode == itemModel.ItemCode && x.Id != itemModel.Id);

                        if (CheckItem != null)
                        {
                            return BadRequest("Item Code : " + itemModel.ItemCode + " already Exist...!");
                        }
                        else
                        {
                            var CheckItemname = await _serviceItem.Find(x => x.ItemName == itemModel.ItemName);
                            if (CheckItemname != null)
                            {
                                return BadRequest(" ItemName :" + itemModel.ItemName + " already Exist...!");
                            }
                        }

                        if (itemModel.ItemImage == null)
                        {
                            var result = await _serviceItem.Update(itemModel, null);
                            if (result == true)
                                return Ok("Succesfully Updated Customer...!");
                            else
                                return BadRequest("Something Went Wrong, Please Try Again After Sometime...!");
                        }
                        else
                        {
                            var photo = await UploadPhoto(itemModel.ItemImage, itemModel.ItemName);
                            var result = await _serviceItem.Update(itemModel, photo);
                            if (result == true)
                                return Ok("Item Information Updated...!");
                            else
                                return BadRequest("Something Went Wrong, Please Try Again After Sometime...!");
                        }
                    }
                    else
                        return NotFound("Item Not Found. id :" + itemModel.Id + ", Please Provide Valid Details and Try Again...!");
                }
                else
                    return NotFound("Invalid Item Id, Please Provide Valid Details and Try Again...!");
            }
            else
                return BadRequest("Invalid Item Information, Please Entering a Valid One...!");

        }


        [HttpDelete(nameof(DeleteItem))]
        public async Task<IActionResult> DeleteItem(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var result = await _serviceItem.Delete(Id);
                if (result == true)
                    return Ok("Item Deleted SUccessfully...!");
                else
                    return BadRequest("Something Went Wrong, Item Not Deleted, Please Try Again After Sometime...!");
            }
            else
                return BadRequest("Invalid Item Id, Please Provide Valid Details and Try Again...!");

        }



        #region "Image Upload section"
        [HttpPost("FileUpload")]

        private async Task<string> UploadPhoto(IFormFile file, string Id)
        {
            string fileName;
            try
            {
                Console.WriteLine(Id);
                _logger.LogInformation("Starting uploading Item Image...!");
                //string wwwPath = this._environment.WebRootPath;
                string contentPath = this._environment.ContentRootPath;
                var extension = "." + file.FileName.Split('.')[^1];
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    fileName = Id.ToLower() + extension; //Create a new Name for the file due to security reasons.
                    string outputFileName = Regex.Replace(fileName, @"[^0-9a-zA-Z._]+", "");
                    var pathBuilt = Path.Combine(contentPath, "Images\\Item");

                    if (!Directory.Exists(pathBuilt))
                    {
                        Directory.CreateDirectory(pathBuilt);
                    }
                    var path = Path.Combine(contentPath, "Images\\Item", outputFileName);

                    Console.WriteLine(path);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    _logger.LogInformation("Successfully uploaded Item Image with Name : " + outputFileName);
                    return outputFileName;
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Error while uploading Item Image with Name : " + ex.StackTrace);

            }

            return "";
        }
        #endregion "Image Upload section"

        #endregion
    }
}
