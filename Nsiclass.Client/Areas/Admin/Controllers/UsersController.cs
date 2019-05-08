using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Nsiclass.Client.Areas.Admin.Models.Users;

using Nsiclass.Data.Models;
using Nsiclass.Services;
using Nsiclass.Services.Models;
using static Nsiclass.Common.DataConstants;

namespace Nsiclass.Client.Areas.Admin.Controllers
{
    //[Authorize(Roles = AdministratorRole)]
    [Authorize(Roles = "Администратор, Програмист")]
    public class UsersController : BaseController
    {
        private readonly IAdminUserService users;
        private readonly IAdminClassService classification;
        private readonly IDepartmentsService _departments;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;


        public UsersController(UserManager<User> userManager,
            SignInManager<User> _signInManager,
            RoleManager<IdentityRole> roleManager,
            IAdminUserService users,
            IDepartmentsService departments,
            IAdminClassService classification)
        {
            this.classification = classification;
            this._departments = departments;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.users = users;
            this.signInManager = _signInManager;
        }


        [Authorize(Roles = DeveloperRole)]
        public async Task<IActionResult> ImportUsers()
        {
            var sourcePath = Path.Combine(
               Directory.GetCurrentDirectory(),
               "wwwroot/xls", "users.xls");

            using (var fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {

                HSSFWorkbook hssfwb = new HSSFWorkbook(fs); //чете xls
                ISheet sheet = hssfwb.GetSheetAt(0);

                IRow headerRow = sheet.GetRow(0);
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    var currentUser = new User();

                    currentUser.UserName = row.GetCell(1).ToString();
                    currentUser.Name = row.GetCell(2).ToString();
                    var pass = row.GetCell(3).ToString();
                    var workPlace = row.GetCell(4).ToString();
                    if (workPlace.Contains("НСИ"))
                    {
                        currentUser.DepartmentId = this._departments.GetAllDepartments().Where(d => d.Id == 3).Select(d => d.Id).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Благоевград"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 12).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Бургас"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 13).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Варна"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 14).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Търново"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 15).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Видин"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 16).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Враца"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 17).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Габрово"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 18).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Добрич"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 19).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Кърджали"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 20).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Кюстендил"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 21).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Ловеч"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 22).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Монтана"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 23).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Пазарджик"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 24).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Перник"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 25).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Плевен"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 26).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Пловдив"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 27).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Разград"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 28).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Русе"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 29).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Силистра"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 30).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Сливен"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 31).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Смолян"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 32).FirstOrDefault();
                    }
                    else if (workPlace.Contains("София-град"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 33).FirstOrDefault();
                    }
                    else if (workPlace.Contains("София-област"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 34).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Стара Загора"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 35).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Търговище"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 37).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Хасково"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 38).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Шумен"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 39).FirstOrDefault();
                    }
                    else if (workPlace.Contains("Ямбол"))
                    {
                        currentUser.Department = this._departments.GetAllDepartments().Where(d => d.Id == 40).FirstOrDefault();
                    }
                    else
                    {
                        currentUser.DepartmentId = this._departments.GetAllDepartments().Where(d => d.Id == 3).Select(d => d.Id).FirstOrDefault();
                    }



                    currentUser.isDeleted = false;
                    bool isFullAccess = row.GetCell(6).ToString() == "1";
                    var roleId = row.GetCell(7).ToString();
                    var role = roleManager.Roles.Where(r => r.Id == roleId).Select(r => r.Name).FirstOrDefault();
                    //currentUser.DateCreated = DateTime.ParseExact(row.GetCell(8).ToString(), "dd-MM-yyyy HH:mm:ss,fff",
                    //System.Globalization.CultureInfo.InvariantCulture);
                    var date = row.GetCell(8).ToString();
                    var createDate = new DateTime();
                    var isDateSuccessfull = DateTime.TryParse(date, out createDate);
                    if (isDateSuccessfull)
                    {
                        currentUser.DateCreated = createDate;
                    }
                    else
                    {
                        currentUser.DateCreated = DateTime.UtcNow.Date;
                    }

                    currentUser.Phone = row.GetCell(9).ToString();
                    currentUser.Email = row.GetCell(10).ToString();
                    currentUser.CreateUser = await this.userManager.GetUserAsync(User);

                    var existingUser = await userManager.FindByNameAsync(currentUser.UserName);
                    if (existingUser == null)
                    {
                        var createResult = await userManager.CreateAsync(currentUser, pass);
                        if (!createResult.Succeeded)
                        {
                            var exceptionText = createResult.Errors.Aggregate("User Creation Failed - Identity Exception. Errors were: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                            throw new Exception(exceptionText);
                        }

                    }
                    else
                    {
                        currentUser = existingUser;
                    }

                    var addToRoleResult = await userManager.AddToRoleAsync(currentUser, role);
                    if (!addToRoleResult.Succeeded)
                    {
                        var exceptionText = addToRoleResult.Errors.Aggregate("Add to role Failed - Identity Exception. Errors were: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                        // throw new Exception(exceptionText);
                    }

                    if (isFullAccess && role != AdministratorRole)
                    {
                        try
                        {
                            addToRoleResult = await userManager.AddToRoleAsync(currentUser, AdministratorRole);
                        }
                        catch (Exception)
                        {
                        }

                    }

                }
            }


            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [Authorize(Roles = "Администратор, Програмист")]
        public IActionResult AdminTasks()
        {
            return View();
        }

        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> AllUsers(string sortOrder, string role, string searchString)
        {
            try
            {
                var result = new List<AdminUserServiceModel>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    result = await this.users.GetSearchedUsers(searchString);
                    ViewData["collection"] = $"Потребители съдържащи \"{searchString}\" в името или потребителското си име";
                    return View(result.OrderBy(u => u.Name));
                }

                if (String.IsNullOrEmpty(role))
                {
                    result = await this.users.GetAllUsers();
                    ViewData["collection"] = "Всички потребители";
                }
                else
                {
                    result = await this.users.GetAllUsers(role);
                    ViewData["role"] = role;
                    if (role == AdministratorRole)
                    {
                        ViewData["collection"] = "Администратори";
                    }
                    else if (role == MethodologistsExRole)
                    {
                        ViewData["collection"] = "Външни методолози";
                    }
                    else if (role == MethodologistsInRole)
                    {
                        ViewData["collection"] = "Вътрешни методолози";
                    }
                    else if (role == TsbRole)
                    {
                        ViewData["collection"] = "ТСБ потребители";
                    }
                    else if (role == NsiUser)
                    {
                        ViewData["collection"] = "НСИ потребители";
                    }
                    else
                    {
                        TempData[ErrorMessageKey] = "Error! Непознат вид потребители";
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }


                }

                if (sortOrder == "Date")
                {
                    return View(result.OrderByDescending(u => u.DateCreated));
                }
                else if (sortOrder == "Department")
                {
                    return View(result.OrderBy(u => u.DepartmentName));
                }
                else if (sortOrder == "Parent")
                {
                    return View(result.OrderBy(u => u.ParentUser));
                }
                else
                {
                    return View(result.OrderBy(u => u.Name));
                }



            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Неуспешна подготовка на изходните данни";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

        }

        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> DeleteUser(string userId, string name)
        {
            try
            {
                if (userId == null)
                {
                    TempData[ErrorMessageKey] = "Невалиден потребителски номер";
                    return RedirectToAction(nameof(AllUsers));
                }

                var userToDelete = await this.userManager.FindByIdAsync(userId);
                if (userToDelete == null)
                {
                    TempData[ErrorMessageKey] = "Невалиден потребител за изтриване";
                    return RedirectToAction(nameof(AllUsers));
                }

                return View(new DeleteUserViewModel
                {
                    UserId = userId,
                    Name = name
                });

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Възникна грешка при подготовката на данните на потребителя";
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> DestroyUser(string userId)
        {
            try
            {
                if (!await ValidateAdminRights())
                {
                    TempData[ErrorMessageKey] = "Нямате права за изтриване на потребители";
                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                if (userId == null)
                {
                    TempData[ErrorMessageKey] = "Невалиден потребителски номер";
                    return RedirectToAction(nameof(AllUsers));
                }

                if (await this.users.DeleteUserAsync(userId))
                {
                    TempData[SuccessMessageKey] = "Потребителя е изтрит";
                }

                return RedirectToAction(nameof(AllUsers));

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Възникна грешка при изтриването на потребителя";
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> RestoreUser(string userId, string name)
        {
            try
            {
                if (userId == null)
                {
                    TempData[ErrorMessageKey] = "Невалиден потребителски номер";
                    return RedirectToAction(nameof(AllUsers));
                }

                var userToRestore = await this.userManager.FindByIdAsync(userId);
                if (userToRestore == null)
                {
                    TempData[ErrorMessageKey] = "Невалиден потребител";
                    return RedirectToAction(nameof(AllUsers));
                }

                return View(new RestoreUserViewModel
                {
                    UserId = userId,
                    Name = name
                });
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Възникна грешка при подготовката за възстановяване на потребителя";
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> UndeleteUser(string userId)
        {
            try
            {
                if (!await ValidateAdminRights())
                {
                    TempData[ErrorMessageKey] = "Нямате права за възстановяване на потребители";
                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                if (userId == null)
                {
                    TempData[ErrorMessageKey] = "Невалиден потребителски номер";
                    return RedirectToAction(nameof(AllUsers));
                }

                if (await this.users.RestoreUserAsync(userId))
                {
                    TempData[SuccessMessageKey] = "Потребителя е възстановен";
                }

                return RedirectToAction(nameof(AllUsers));

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Възникна грешка при wyzstanowqwaneto на потребителя";
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }



        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> EditUser(string userId)
        {
            try
            {
                var userToEdit = await this.userManager.FindByIdAsync(userId);
                if (userToEdit == null)
                {
                    TempData[ErrorMessageKey] = "Няма такъв потребител";
                    return RedirectToAction(nameof(AllUsers), new { sortOrder = "Name" });
                }

                var editableUser = new EditUserViewModel
                {
                    Name = userToEdit.Name,
                    Id = userToEdit.Id,
                    Email = userToEdit.Email,
                    Username = userToEdit.UserName,
                    Phone = userToEdit.Phone,
                    PhoneNumber = userToEdit.PhoneNumber
                };

                editableUser.Departments = this._departments.GetAllDepartments()
                           .Select(a => new SelectListItem
                           {
                               Text = a.Name,
                               Value = a.Id.ToString(),
                               Selected = (a.Id == userToEdit.DepartmentId ? true : false)
                           })
                           .ToList();

                //var userRoles = await this.userManager.GetRolesAsync(userToEdit);


                var roles = this.roleManager.Roles.Where(r => r.Name != DeveloperRole).OrderBy(r => r.Name).ToList();

                editableUser.Roles = roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = (this.userManager.IsInRoleAsync(userToEdit, r.Name).Result ? "1" : "0"),
                    Selected = (this.userManager.IsInRoleAsync(userToEdit, r.Name).Result ? true : false)

                }).ToList();


                return View(editableUser);
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Неуспешна подготовка на изходните данни";
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> EditUser(EditUserViewModel model, int departmentName, bool role0, bool role1, bool role2, bool role3, bool role4, string input0, string input1, string input2, string input3, string input4)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if (!await ValidateAdminRights())
                {
                    TempData[ErrorMessageKey] = "Нямате права за промяна на данните на потребителя";
                    return RedirectToAction(nameof(AllUsers));
                }

                var roles = new List<string>();
                if (role0)
                {
                    roles.Add(input0);
                }
                if (role1)
                {
                    roles.Add(input1);
                }
                if (role2)
                {
                    roles.Add(input2);
                }
                if (role3)
                {
                    roles.Add(input3);
                }
                if (role4)
                {
                    roles.Add(input4);
                }


                var result = await this.users.UpdateUserData(model.Id, model.Name, model.Phone, model.PhoneNumber, model.Email, departmentName, roles);

                if (!result)
                {
                    TempData[ErrorMessageKey] = "Записа на промените е неуспешен";
                    return RedirectToAction(nameof(AllUsers));
                }
                else
                {
                    TempData[SuccessMessageKey] = "Промените са записани успешно";
                    return RedirectToAction(nameof(AllUsers));
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Основна грешка";
                return RedirectToAction(nameof(AllUsers));
            }
        }

        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> ChangePassword(string userId)
        {
            try
            {
                var userToEdit = await this.userManager.FindByIdAsync(userId);
                if (userToEdit == null)
                {
                    TempData[ErrorMessageKey] = $"Няма потребител с номер: {userId}";
                    return RedirectToAction(nameof(AllUsers));
                }
                return View(new ChangeUserPasswordViewModel
                {
                    Username = userToEdit.UserName,
                    Id = userToEdit.Id
                });
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Неуспешна подготовка на изходните данни";
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }


        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if (!await ValidateAdminRights())
                {
                    TempData[ErrorMessageKey] = "Insufficient rights";
                    return RedirectToAction(nameof(AllUsers));
                }

                Regex regex = new Regex(@"^(?=.*[\p{Ll}])(?=.*[\p{Lu}])(?=.*\d)(?=.*[$@$!%*?&])[\p{Ll}\p{Lu}\d$@$!%*?&]{8,}");
                Match match = regex.Match(model.Password);
                if (match.Success)
                {
                    var userToEdit = await this.userManager.FindByIdAsync(model.Id);

                    string code = await this.userManager.GeneratePasswordResetTokenAsync(userToEdit);

                    IdentityResult result = await this.userManager.ResetPasswordAsync(userToEdit, code, model.Password);
                    if (result.Succeeded)
                    {
                        TempData[SuccessMessageKey] = $"Успешно сменена парола на потребител: {model.Username} ";
                        await this.userManager.ResetAuthenticatorKeyAsync(userToEdit);

                        var currentUser = await this.userManager.FindByNameAsync(User.Identity.Name);

                        if (currentUser.UserName == userToEdit.UserName)
                        {
                            await signInManager.SignOutAsync();
                            TempData.Remove("SuccessMessageKey");
                        }

                    }
                    else
                    {
                        TempData[ErrorMessageKey] = $"Паролата не е сменена. Програмна грешка";
                    }

                }
                else
                {
                    TempData[ErrorMessageKey] = $"Паролата не отговаря на ISO 27000";
                }

                return RedirectToAction(nameof(AllUsers));

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Основна грешка";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

        }

        [Authorize(Roles = "Администратор, Програмист")]
        public IActionResult CreateNewUser()
        {
            try
            {
                var editableUser = new CreateUserViewModel
                {
                    Departments = this._departments.GetAllDepartments()
                   .Select(a => new SelectListItem
                   {
                       Text = a.Name,
                       Value = a.Id.ToString(),
                   })
                   .ToList()
                };

                var roles = this.roleManager.Roles.Where(r => r.Name != DeveloperRole).OrderBy(r => r.Name).ToList();

                editableUser.Roles = roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = "0",
                    Selected = false
                }).ToList();

                return View(editableUser);
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = $"Error! Грешка при подготовка на данните за нов потребител";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

        }

        //[Authorize(Roles = "Администратор, Програмист")]
        //public IActionResult SearchUsers(string searchString)
        //{
        //    try
        //    {
        //        var result = new List<AdminUserServiceModel>();
        //        result = this.users.SearchAsync(searchString);

        //        return View(result);



        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        public async Task<IActionResult> CreateNewUser(CreateUserViewModel model, int departmentName, bool role0, bool role1, bool role2, bool role3, bool role4, string input0, string input1, string input2, string input3, string input4)
        {
            if (!ModelState.IsValid)
            {
                model = CreateUserModelPrepare(model);

                return View(model);
            }

            try
            {
                if (!await ValidateAdminRights())
                {
                    TempData[ErrorMessageKey] = "Нямате права за промяна на данните на потребителя";
                    return RedirectToAction(nameof(AllUsers));
                }

                Regex regex = new Regex(@"^(?=.*[\p{Ll}])(?=.*[\p{Lu}])(?=.*\d)(?=.*[$@$!%*?&])[\p{Ll}\p{Lu}\d$@$!%*?&]{8,}");
                Match match = regex.Match(model.Password);
                if (match.Success)
                {


                    var adminUser = await this.userManager.GetUserAsync(User);
                    var roles = new List<string>();
                    if (role0)
                    {
                        roles.Add(input0);
                    }
                    if (role1)
                    {
                        roles.Add(input1);
                    }
                    if (role2)
                    {
                        roles.Add(input2);
                    }
                    if (role3)
                    {
                        roles.Add(input3);
                    }
                    if (role4)
                    {
                        roles.Add(input4);
                    }

                    var parentUser = await this.userManager.GetUserAsync(User);
                    var result = await this.users.CreateNewUser(model.Username, model.Name, model.Phone, model.PhoneNumber, model.Email, model.Password, departmentName, roles, parentUser.Id);

                    if (result == "success")
                    {
                        TempData[SuccessMessageKey] = "Промените са записани успешно";
                        return RedirectToAction(nameof(AllUsers), new { sortOrder = "Date" });
                    }
                    else
                    {
                        TempData[ErrorMessageKey] = result;
                        model = CreateUserModelPrepare(model);
                        return View(model);
                    }
                }
                else
                {
                    TempData[ErrorMessageKey] = $"Паролата не отговаря на ISO 27000";
                    model = CreateUserModelPrepare(model);
                    return View(model);
                }

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Error! Основна грешка при създаване на потребител";
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        private CreateUserViewModel CreateUserModelPrepare(CreateUserViewModel model)
        {
            var roles = this.roleManager.Roles.Where(r => r.Name != DeveloperRole).OrderBy(r => r.Name).ToList();

            var incompleteUser = new CreateUserViewModel()
            {
                Username = model.Username,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Departments = this._departments.GetAllDepartments()
               .Select(a => new SelectListItem
               {
                   Text = a.Name,
                   Value = a.Id.ToString(),
               })
               .ToList(),
                Roles = roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = "0",
                    Selected = false
                }).ToList()
            };
            return incompleteUser;
        }

        private async Task<bool> ValidateAdminRights()
        {
            var currentUser = await this.userManager.GetUserAsync(User);

            var adminRights = await this.userManager.IsInRoleAsync(currentUser, AdministratorRole);
            var developerRights = await this.userManager.IsInRoleAsync(currentUser, DeveloperRole);
            if (adminRights || developerRights)
            {
                return true;
            }
            return false;
        }



    }
}