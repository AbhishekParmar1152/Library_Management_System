using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library_Management_System.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using static Library_Management_System.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using Dapper;
using System.Web;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Identity;

namespace Library_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly INotyfService _notyf;

        public HomeController(IWebHostEnvironment webHostEnvironment, INotyfService notyf)
        {
            this.webHostEnvironment = webHostEnvironment;
            _notyf = notyf;
        }
        readonly string constr = "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=Test2;";

        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.alertmsg = TempData["Message"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel login)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/login", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var userlogin = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (userlogin.Status == "Success")
                        {
                            HttpContext.Session.SetString("Token", userlogin.Token);
                            UserRegistrationModel dTO = new UserRegistrationModel();
                            foreach (var item in (dynamic)userlogin.Data)
                            {
                                dTO = JsonConvert.DeserializeObject<UserRegistrationModel>(userlogin.Data.ToString());
                            }
                            //var claims = new List<Claim>();
                            //claims.Add(new Claim(ClaimTypes.Name, dTO.Username));
                            //var userIdentity = new ClaimsIdentity(claims, "login");
                            //ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            //Thread.CurrentPrincipal = principal;
                            //if (HttpContext.User != null)
                            //{
                            //    HttpContext.User = principal;
                            //}
                            
                            return RedirectToAction(userlogin.RedirectUrl, dTO);
                        }
                        else
                        {
                            TempData["Message"] = CommonServices.ShowAlert(Alerts.Danger, userlogin.Message);
                            return RedirectToAction("Login");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = CommonServices.ShowAlert(Alerts.Warning, Ex.Message);
            }
            TempData["Message"] = CommonServices.ShowAlert(Alerts.Danger, "Some Problem with login..");
            return RedirectToAction("Login");
        }

        #endregion

        #region Registration

        [HttpGet]
        public ActionResult RegistrationUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationUser(UserRegistrationModel registration)
        {
            try
            {
                string UserName = HttpContext.Session.GetString("Librarian");
                SqlConnection connection = new SqlConnection(constr);
                SqlCommand Command = new SqlCommand("select Library_Id from AspNetUsers where UserName = @UserName", connection);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@UserName";
                param.Value = UserName;
                Command.Parameters.Add(param);
                connection.Open();
                registration.Library_Id = (int)Command.ExecuteScalar();
                connection.Close();
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/register-user", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var userregistration = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (userregistration.Status == "Success")
                        {
                            _notyf.Success(userregistration.Message);
                            return Json(userregistration.Message);
                        }
                        else
                        {
                            _notyf.Error(userregistration.Message);
                            return Json(userregistration.Message);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return View();
        }

        [HttpGet]
        public ActionResult RegistrationAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationAdmin(UserRegistrationModel registration)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/register-admin", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var userregistration = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (userregistration.Status == "Success")
                        {
                            _notyf.Success(userregistration.Message);
                            return Json(userregistration.Message);
                        }
                        else
                        {
                            _notyf.Error(userregistration.Message);
                            return Json(userregistration.Message);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Registrationlibrarian()
        {
            try
            {
                List<UserRegistrationModel> RegisterList = null;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    BarChartLibrarian obj = new BarChartLibrarian();
                    using (SqlCommand cmd = new SqlCommand("select Library_Id, Library_Name from Library", con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        RegisterList = new List<UserRegistrationModel>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            UserRegistrationModel cobj = new UserRegistrationModel();
                            cobj.Library_Id = (int)ds.Tables[0].Rows[i]["Library_Id"];
                            cobj.Library_Name = ds.Tables[0].Rows[i]["Library_Name"].ToString();
                            RegisterList.Add(cobj);
                        }
                        con.Close();
                    }
                }
                ViewBag.List = RegisterList;
                return View(ViewBag.List);
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrationlibrarian(UserRegistrationModel registration)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/register-librarian", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var userregistration = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (userregistration.Status == "Success")
                        {
                            _notyf.Success(userregistration.Message);
                            return Json(userregistration.Message);
                        }
                        else
                        {
                            _notyf.Error(userregistration.Message);
                            return Json(userregistration.Message);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return View();
        }

        #endregion

        #region Accounts

        public async Task<IActionResult> AdminAccount(UserRegistrationModel response)
        {
            try
            {
                HttpContext.Session.SetString("Admin", response.Username);
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
                    using (var result = await httpClient.PostAsync("https://localhost:44309/api/LMS/AdminAccount", content))
                    {
                        var apiResponse = await result.Content.ReadAsStringAsync();
                        var userregistration = JsonConvert.DeserializeObject<UserRegistrationModel>(apiResponse);
                        if (userregistration != null)
                        {
                            Account account = new Account();
                            Count counter = new Count();
                            using (var connection = new SqlConnection(constr))
                            {
                                const string sql1 = @"SELECT COUNT(*) FROM AspNetUsers
                                INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId 
                                INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetRoles.Name = 'Librarian' ";
                                const string sql2 = @"SELECT COUNT(*) FROM AspNetUsers
                                INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId 
                                INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetRoles.Name = 'User' ";
                                const string sql3 = @"SELECT COUNT(*) FROM Books ";
                                const string sql4 = @"SELECT COUNT(*) FROM Library ";
                                string profilepic = @"SELECT Profile_Picture FROM AspNetUsers where UserName='" + userregistration.Username + "'";
                                var LibrarianCount = await connection.ExecuteScalarAsync<int>(sql1);
                                var StudentCount = await connection.ExecuteScalarAsync<int>(sql2);
                                var BookCount = await connection.ExecuteScalarAsync<int>(sql3);
                                var LibraryCount = await connection.ExecuteScalarAsync<int>(sql4);
                                var profile = await connection.ExecuteScalarAsync<string>(profilepic);
                                if (profile != null)
                                {
                                    HttpContext.Session.SetString("profilePicture", profile);
                                }

                                counter.LibrarianCount = LibrarianCount;
                                counter.StudentCount = StudentCount;
                                counter.BooksCount = BookCount;
                                counter.LibrariesCount = LibraryCount;
                                account.count = counter;
                            }
                            account.user = userregistration;
                            return View(account);
                        }
                        TempData["Message"] = "Something Went Wrong...!!";
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
        }
        public async Task<IActionResult> LibrarianAccount(UserRegistrationModel response)
        {
            try
            {
                HttpContext.Session.SetString("Librarian", response.Username);
                int id = (int)Librarian_Id().Value;
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
                    using (var result = await httpClient.PostAsync("https://localhost:44309/api/LMS/LibrarianAccount", content))
                    {
                        var apiResponse = await result.Content.ReadAsStringAsync();
                        var userregistration = JsonConvert.DeserializeObject<UserRegistrationModel>(apiResponse);
                        if (userregistration != null)
                        {
                            LibrarianAccount account = new LibrarianAccount();
                            CountLibrarian counter = new CountLibrarian();
                            using (var connection = new SqlConnection(constr))
                            {
                                var sql1 = @"select COUNT(*) from AspNetUsers
                                INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId
                                INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetRoles.Name = 'User' AND AspNetUsers.Library_Id ='" + id + "'";

                                var sql2 = @"SELECT COUNT(*) FROM Books where Library_Id= '" + id + "'";
                                var sql3 = @"select COUNT(*) from IssueBook where Library_Id= '" + id + "'";
                                var sql4 = @"select COUNT(*) from IssueBook where Returned='N' and Library_Id= '" + id + "'";
                                string profilepic = @"SELECT Profile_Picture FROM AspNetUsers where UserName='" + userregistration.Username + "'";
                                var StudentCount = await connection.ExecuteScalarAsync<int>(sql1);
                                var BooksCount = await connection.ExecuteScalarAsync<int>(sql2);
                                var TotalIssuedBook = await connection.ExecuteScalarAsync<int>(sql3);
                                var BookNotReturned = await connection.ExecuteScalarAsync<int>(sql4);
                                var profile = await connection.ExecuteScalarAsync<string>(profilepic);
                                if (profile != null)
                                {
                                    HttpContext.Session.SetString("profilePicture", profile);
                                }
                                counter.TotalIssuedBook = TotalIssuedBook;
                                counter.StudentCount = StudentCount;
                                counter.BooksCount = BooksCount;
                                counter.BookNotReturned = BookNotReturned;
                                account.count = counter;
                            }
                            account.user = userregistration;
                            return View(account);
                        }
                        TempData["Message"] = "Something Went Wrong...!!";
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
        }
        public async Task<IActionResult> UserAccount(UserRegistrationModel response)
        {
            try
            {
                HttpContext.Session.SetString("User", response.Username);
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
                    using (var result = await httpClient.PostAsync("https://localhost:44309/api/LMS/UserAccount", content))
                    {
                        var apiResponse = await result.Content.ReadAsStringAsync();
                        var userregistration = JsonConvert.DeserializeObject<UserRegistrationModel>(apiResponse);
                        if (userregistration != null)
                        {
                            UserAccount account = new UserAccount();
                            CountUser counter = new CountUser();
                            using (var connection = new SqlConnection(constr))
                            {
                                var sql1 = @"select COUNT(*) from IssueBook where Returned='Y' AND Student_Name='" + userregistration.Username + "'";
                                var sql2 = @"select COUNT(*) from IssueBook where Returned='N' AND Student_Name='" + userregistration.Username + "'";
                                var sql3 = @"select COUNT(*) from IssueBook where Student_Name='" + userregistration.Username + "'";
                                var sql4 = @"select SUM(Penalty) from IssueBook where Returned='N' AND Student_Name='" + userregistration.Username + "'";
                                string profilepic = @"SELECT Profile_Picture FROM AspNetUsers where UserName='" + userregistration.Username + "'";
                                var BookReturned = await connection.ExecuteScalarAsync<int>(sql1);
                                var BookNotReturned = await connection.ExecuteScalarAsync<int>(sql2);
                                var TotalIssuedBook = await connection.ExecuteScalarAsync<int>(sql3);
                                var Penalty = await connection.ExecuteScalarAsync<int>(sql4);
                                var profile = await connection.ExecuteScalarAsync<string>(profilepic);
                                if (profile != null)
                                {
                                    HttpContext.Session.SetString("profilePicture", profile);
                                }
                                counter.BookReturned = BookReturned;
                                counter.BookNotReturned = BookNotReturned;
                                counter.TotalIssuedBook = TotalIssuedBook;
                                counter.Penalty = Penalty;
                                account.count = counter;
                            }
                            account.user = userregistration;
                            return View(account);
                        }
                        TempData["Message"] = "Something Went Wrong...!!";
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
        }

        #endregion
        public IActionResult dashboard()
        {
            return PartialView();
        }
        #region DashBoards

        public async Task<PartialViewResult> DashBoardAdmin()
        {
            try
            {
                Count counter = new Count();
                using (var connection = new SqlConnection(constr))
                {
                    const string sql1 = @"SELECT COUNT(*) FROM AspNetUsers
                                INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId 
                                INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetRoles.Name = 'Librarian' ";
                    const string sql2 = @"SELECT COUNT(*) FROM AspNetUsers
                                INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId 
                                INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetRoles.Name = 'User' ";
                    const string sql3 = @"SELECT COUNT(*) FROM Books ";
                    const string sql4 = @"SELECT COUNT(*) FROM Library ";
                    var LibrarianCount = await connection.ExecuteScalarAsync<int>(sql1);
                    var StudentCount = await connection.ExecuteScalarAsync<int>(sql2);
                    var BookCount = await connection.ExecuteScalarAsync<int>(sql3);
                    var LibraryCount = await connection.ExecuteScalarAsync<int>(sql4);
                    counter.LibrarianCount = LibrarianCount;
                    counter.StudentCount = StudentCount;
                    counter.BooksCount = BookCount;
                    counter.LibrariesCount = LibraryCount;
                }
                return PartialView(counter);
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return PartialView("_dashboard");
        }
        public async Task<ActionResult> DashBoardLibrarian()
        {
            try
            {
                string Username = HttpContext.Session.GetString("User");
                int id = (int)Librarian_Id().Value;
                CountLibrarian counter = new CountLibrarian();
                using (var connection = new SqlConnection(constr))
                {
                    var sql1 = @"select COUNT(*) from AspNetUsers
                           INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId
                           INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetRoles.Name = 'User' AND AspNetUsers.Library_Id ='" + id + "'";
                    var sql2 = @"SELECT COUNT(*) FROM Books where Library_Id= '" + id + "'";
                    var sql3 = @"select COUNT(*) from IssueBook where Library_Id= '" + id + "'";
                    var sql4 = @"select COUNT(*) from IssueBook where Returned='N' and Library_Id= '" + id + "'";
                    string profilepic = @"SELECT Profile_Picture FROM AspNetUsers where UserName='" + Username + "'";
                    var StudentCount = await connection.ExecuteScalarAsync<int>(sql1);
                    var BooksCount = await connection.ExecuteScalarAsync<int>(sql2);
                    var TotalIssuedBook = await connection.ExecuteScalarAsync<int>(sql3);
                    var BookNotReturned = await connection.ExecuteScalarAsync<int>(sql4);
                    counter.StudentCount = StudentCount;
                    counter.BooksCount = BooksCount;
                    counter.TotalIssuedBook = TotalIssuedBook;
                    counter.BookNotReturned = BookNotReturned;
                }
                return View(counter);
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return View();
        }
        public async Task<ActionResult> DashBoardUser()
        {
            try
            {
                string Username = HttpContext.Session.GetString("User");
                CountUser counter = new CountUser();
                using (var connection = new SqlConnection(constr))
                {
                    var sql1 = @"select COUNT(*) from IssueBook where Returned='Y' AND Student_Name='" + Username + "'";
                    var sql2 = @"select COUNT(*) from IssueBook where Returned='N' AND Student_Name='" + Username + "'";
                    var sql3 = @"select COUNT(*) from IssueBook where Student_Name='" + Username + "'";
                    var sql4 = @"select SUM(Penalty) from IssueBook where Returned='N' AND Student_Name='" + Username + "'";
                    var BookReturned = await connection.ExecuteScalarAsync<int>(sql1);
                    var BookNotReturned = await connection.ExecuteScalarAsync<int>(sql2);
                    var TotalIssuedBook = await connection.ExecuteScalarAsync<int>(sql3);
                    var Penalty = await connection.ExecuteScalarAsync<int>(sql4);
                    counter.BookNotReturned = BookNotReturned;
                    counter.BookReturned = BookReturned;
                    counter.TotalIssuedBook = TotalIssuedBook;
                    counter.Penalty = Penalty;
                }
                return View(counter);
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return View();
        }

        #endregion

        #region Logout

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        #endregion

        #region Changepassword

        public ActionResult ChangePasswordAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAdmin(ChangePassword changePassword)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(changePassword), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/ChangePassword", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var Responce = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (Responce.Status == "Success")
                        {
                            _notyf.Success(Responce.Message);
                            return Json(Responce.Message);
                        }
                        else
                        {
                            _notyf.Error(Responce.Message);
                            return Json(Responce.Message);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return View();
        }

        [HttpGet]
        public ActionResult ChangePasswordLibrarian()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordLibrarian(ChangePassword changePassword)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(changePassword), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/ChangePassword", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var Responce = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (Responce.Status == "Success")
                        {
                            _notyf.Success(Responce.Message);
                            return Json(Responce.Message);
                        }
                        else
                        {
                            _notyf.Error(Responce.Message);
                            return Json(Responce.Message);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return View();
        }

        [HttpGet]
        public ActionResult ChangePasswordUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordUser(ChangePassword changePassword)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(changePassword), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/ChangePassword", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var Responce = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (Responce.Status == "Success")
                        {
                            _notyf.Success(Responce.Message);
                            return Json(Responce.Message);
                        }
                        else
                        {
                            _notyf.Error(Responce.Message);
                            return Json(Responce.Message);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                _notyf.Warning(Ex.Message);
            }
            return View();
        }

        #endregion

        #region Managebooks

        public async Task<IActionResult> ManageBooks()
        {
            try
            {
                int id = (int)Librarian_Id().Value;
                Books books = new Books();
                books.Library_Id = id;
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(books), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/ManageBooks", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        List<Books> RegisterList = JsonConvert.DeserializeObject<List<Books>>(apiResponse);
                        return View(RegisterList);
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public JsonResult AddBook([FromBody] List<Books> Book)
        {
            SqlConnection connection = new SqlConnection(constr);
            SqlCommand Command1 = new SqlCommand("delete from Books where isDel = 'Y'", connection);
            connection.Open();
            Command1.ExecuteNonQuery();
            connection.Close();
            try
            {
                DataTable dataTable = new DataTable("Book");
                dataTable = ConvertToDataTableBook(Book);

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_BulkUpload_Book", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Books", dataTable);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json($"Exception Occurred: {ex.Message}");
            }
            return Json(Book.Count);
        }
        [HttpGet]
        public JsonResult Librarian_Id()
        {
            var Username = HttpContext.Session.GetString("Librarian");
            SqlConnection connection = new SqlConnection(constr);
            SqlCommand Command1 = new SqlCommand("select Library_Id from AspNetUsers where UserName ='" + Username + "'", connection);
            connection.Open();
            var id = Command1.ExecuteScalar();
            return Json(id);
        }
        [HttpGet]
        public IActionResult EditBook(int id)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Books", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Book_Id", id);
                    cmd.Parameters.AddWithValue("@status", "GetbyId");
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                Books Book = new Books()
                                {
                                    Book_Id = sdr["Book_Id"].ToString(),
                                    Book_Title = sdr["Book_Title"].ToString(),
                                    Book_Publication = sdr["Book_Publication"].ToString(),
                                    Book_Author = sdr["Book_Author"].ToString(),
                                    Book_Category = sdr["Book_Category"].ToString(),
                                    No_Of_Copies_Actual = sdr["No_Of_Copies_Actual"].ToString(),
                                    No_Of_Copies_Current = sdr["No_Of_Copies_Current"].ToString(),
                                    Book_Language = sdr["Book_Language"].ToString(),
                                    Book_Added_On = sdr["Book_Added_On"].ToString()
                                };
                                ViewBag.data = Book;
                            }
                        }
                    }
                    con.Close();
                    return View(ViewBag.data);
                }
            }
        }

        [HttpPost]
        public ActionResult EditBook(Books model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = constr;
                    using (SqlCommand cmd = new SqlCommand("SP_Books", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("@Book_Id", model.Book_Id);
                        cmd.Parameters.AddWithValue("@Book_Title", model.Book_Title);
                        cmd.Parameters.AddWithValue("@Book_Publication", model.Book_Publication);
                        cmd.Parameters.AddWithValue("@Book_Author", model.Book_Author);
                        cmd.Parameters.AddWithValue("@Book_Category", model.Book_Category);
                        cmd.Parameters.AddWithValue("@No_Of_Copies_Actual", model.No_Of_Copies_Actual);
                        cmd.Parameters.AddWithValue("@No_Of_Copies_Current", model.No_Of_Copies_Current);
                        cmd.Parameters.AddWithValue("@Book_Language", model.Book_Language);
                        cmd.Parameters.AddWithValue("@Book_Added_On", model.Book_Added_On);
                        cmd.Parameters.AddWithValue("@status", "Update");
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return Json("Records Updated successfully.");
                    }
                }
            }
            catch
            {
                return Json("error.");
            }
        }

        [HttpPost]
        public JsonResult DeleteBook([FromBody] Books Book)
        {
            string constr = "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=Test2;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Books", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Book_Id", Book.Book_Id);
                    cmd.Parameters.AddWithValue("@status", "DeleteBook");

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Json("success");
        }

        #endregion

        #region Manage libraries

        public async Task<IActionResult> ManageLibraries()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);

                    using (var response = await httpClient.GetAsync("https://localhost:44309/api/LMS/ManageLibraries"))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        List<Libraries> RegisterList = JsonConvert.DeserializeObject<List<Libraries>>(apiResponse);
                        return View(RegisterList);
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult AddLibrary([FromBody] List<Libraries> Library)
        {
            try
            {
                DataTable dataTable = new DataTable("Library");
                dataTable = ConvertToDataTableLibrary(Library);

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_BulkUpload_Library", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Library", dataTable);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("BULK INSERT UPDATE Successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
            }
            return Json(Library.Count);
        }

        [HttpGet]
        public IActionResult EditLibrary(int id)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ManageLibraries", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Library_Id", id);
                    cmd.Parameters.AddWithValue("@status", "GetbyId");
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                Libraries Library = new Libraries()
                                {
                                    Library_Id = sdr["Library_Id"].ToString(),
                                    Library_Name = sdr["Library_Name"].ToString(),
                                    Library_Address = sdr["Library_Address"].ToString(),
                                    Library_City = sdr["Library_City"].ToString(),
                                    Library_Pincode = sdr["Library_Pincode"].ToString(),
                                    Library_Create_Date = sdr["Library_Create_Date"].ToString(),
                                };
                                ViewBag.data = Library;
                            }
                        }
                    }
                    con.Close();
                    return View(ViewBag.data);
                }
            }
        }

        [HttpPost]
        public ActionResult EditLibrary(Libraries model)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = constr;
                using (SqlCommand cmd = new SqlCommand("SP_ManageLibraries", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@Library_Id", model.Library_Id);
                    cmd.Parameters.AddWithValue("@Library_Name", model.Library_Name);
                    cmd.Parameters.AddWithValue("@Library_Address", model.Library_Address);
                    cmd.Parameters.AddWithValue("@Library_City", model.Library_City);
                    cmd.Parameters.AddWithValue("@Library_Pincode", model.Library_Pincode);
                    cmd.Parameters.AddWithValue("@Library_Create_Date", model.Library_Create_Date);
                    cmd.Parameters.AddWithValue("@status", "UpdateLibrary");
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return Json("Records Updated successfully.");
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteLibrary([FromBody] Libraries Library)
        {
            string constr = "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=Test2;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ManageLibraries", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Library_Id", Library.Library_Id);
                    cmd.Parameters.AddWithValue("@status", "DeleteLibrary");

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Json("success");
        }

        #endregion

        #region Manage Librarian

        public async Task<IActionResult> ManageLibrarian()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);

                    using (var response = await httpClient.GetAsync("https://localhost:44309/api/LMS/ManageLibrarian"))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        List<Users> UserList = JsonConvert.DeserializeObject<List<Users>>(apiResponse);
                        return View(UserList);
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult EditLibrarian(string id)
        {
            Users User = new Users();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Manage_Librarian", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@status", "GetbyId");
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                User.Id = sdr["Id"].ToString();
                                User.UserName = sdr["UserName"].ToString();
                                User.Email = sdr["Email"].ToString();
                                User.PhoneNumber = sdr["PhoneNumber"].ToString();
                                User.Library_Id = (int)sdr["Library_Id"];
                            }
                        }
                    }
                    SqlCommand Command1 = new SqlCommand("select Library_Name from Library where Library_Id ='" + User.Library_Id + "'", con);
                    User.Library_Name = Command1.ExecuteScalar().ToString();
                    ViewBag.data = User;
                    con.Close();
                    return View(ViewBag.data);
                }
            }
        }

        [HttpPost]
        public ActionResult EditLibrarian(Users User)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = constr;
                using (SqlCommand cmd = new SqlCommand("SP_Manage_Librarian", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@Id", User.Id);
                    cmd.Parameters.AddWithValue("@UserName", User.UserName);
                    cmd.Parameters.AddWithValue("@Email", User.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", User.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Library_Id", User.Library_Id);
                    cmd.Parameters.AddWithValue("@status", "UpdateLibrarian");
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return Json("Records Updated successfully.");
                }
            }
        }

        [HttpPost]
        public JsonResult DeleteLibrarian([FromBody] Users User)
        {
            string constr = "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=Test2;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Manage_Librarian", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", User.Id);
                    cmd.Parameters.AddWithValue("@status", "DeleteLibrarian");

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Json("success");
        }

        #endregion

        #region Manage User

        public async Task<IActionResult> ManageUser()
        {
            try
            {
                int id = (int)Librarian_Id().Value;
                Books books = new Books();
                books.Library_Id = id;
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(books), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/ManageUser", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        List<Users> UserList = JsonConvert.DeserializeObject<List<Users>>(apiResponse);
                        return View(UserList);
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Manage_User", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@status", "GetUserbyId");
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                Users User = new Users()
                                {
                                    Id = sdr["Id"].ToString(),
                                    UserName = sdr["UserName"].ToString(),
                                    Email = sdr["Email"].ToString(),
                                    PhoneNumber = sdr["PhoneNumber"].ToString(),
                                };
                                ViewBag.data = User;
                            }
                        }
                    }
                    con.Close();
                    return View(ViewBag.data);
                }
            }
        }

        [HttpPost]
        public ActionResult EditUser(Users User)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = constr;
                using (SqlCommand cmd = new SqlCommand("SP_Manage_User", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@Id", User.Id);
                    cmd.Parameters.AddWithValue("@UserName", User.UserName);
                    cmd.Parameters.AddWithValue("@Email", User.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", User.PhoneNumber);
                    cmd.Parameters.AddWithValue("@status", "UpdateUser");
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return Json("Records Updated successfully.");
                }
            }
        }

        [HttpPost]
        public JsonResult DeleteUser([FromBody] Users User)
        {
            string constr = "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=Test2;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Manage_User", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", User.Id);
                    cmd.Parameters.AddWithValue("@status", "DeleteUser");

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Json("success");
        }

        #endregion

        #region Datatables

        public DataTable ConvertToDataTableLibrary<Libraries>(List<Libraries> Library)
        {
            DataTable dataTable = new DataTable();
            PropertyInfo[] columns = null;

            if (Library == null) return dataTable;
            foreach (Libraries Record in Library)
            {
                if (columns == null)
                {
                    columns = Record.GetType().GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dataTable.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }
                DataRow dataRow = dataTable.NewRow();
                foreach (PropertyInfo pinfo in columns)
                {
                    dataRow[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
        public DataTable ConvertToDataTableBook<Books>(List<Books> Book)
        {
            DataTable dataTable = new DataTable();
            PropertyInfo[] columns = null;

            if (Book == null) return dataTable;
            foreach (Books Record in Book)
            {
                if (columns == null)
                {
                    columns = Record.GetType().GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dataTable.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }
                DataRow dataRow = dataTable.NewRow();
                foreach (PropertyInfo pinfo in columns)
                {
                    dataRow[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        #endregion

        #region Issue Book

        [HttpGet]
        public IActionResult IssueBook()
        {
            int id = (int)Librarian_Id().Value;
            List<Books> RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                Books obj = new Books();
                using (SqlCommand cmd = new SqlCommand("select * from Books where No_Of_Copies_Current > 0 and Library_id='" + id + "'", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    RegisterList = new List<Books>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Books cobj = new Books();
                        cobj.Book_Id = ds.Tables[0].Rows[i]["Book_Id"].ToString();
                        cobj.Book_Title = ds.Tables[0].Rows[i]["Book_Title"].ToString();
                        cobj.Book_Language = ds.Tables[0].Rows[i]["Book_Language"].ToString();
                        cobj.Book_Publication = ds.Tables[0].Rows[i]["Book_Publication"].ToString();
                        cobj.Book_Author = ds.Tables[0].Rows[i]["Book_Author"].ToString();
                        cobj.Book_Category = ds.Tables[0].Rows[i]["Book_Category"].ToString();
                        cobj.Book_Added_On = (ds.Tables[0].Rows[i]["Book_Added_On"].ToString());
                        cobj.No_Of_Copies_Actual = (ds.Tables[0].Rows[i]["No_Of_Copies_Actual"].ToString());
                        cobj.No_Of_Copies_Current = (ds.Tables[0].Rows[i]["No_Of_Copies_Current"].ToString());
                        RegisterList.Add(cobj);
                    }
                    con.Close();
                }
            }
            List<Users> UserList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                Users obj = new Users();
                using (SqlCommand cmd = new SqlCommand(@"select ans.* from AspNetUsers ans
                INNER JOIN AspNetUserRoles ON ans.Id = AspNetUserRoles.UserId
                INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetRoles.Name = 'User' AND ans.Library_Id ='" + id + "'", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    UserList = new List<Users>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Users cobj = new Users();
                        cobj.Id = ds.Tables[0].Rows[i]["Id"].ToString();
                        cobj.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        cobj.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        cobj.PhoneNumber = ds.Tables[0].Rows[i]["PhoneNumber"].ToString();
                        UserList.Add(cobj);
                    }
                    con.Close();
                }
            }
            var tables = new IssueBook
            {
                users = UserList,
                books = RegisterList

            };
            return View(tables);
        }

        [HttpPost]
        public IActionResult IssueBook(BookIssue data)
        {
            try
            {
                data.Library_id = (int)Librarian_Id().Value;
                string constr = "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=Test2;";
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Issue_Book", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Student_Name", data.Student_Name);
                        cmd.Parameters.AddWithValue("@Book_Name", data.Book_Name);
                        cmd.Parameters.AddWithValue("@Library_Id", data.Library_id);
                        cmd.Parameters.AddWithValue("@status", "Insert");

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        _notyf.Success("Book Issued");
                        return Json("success");
                    }
                }
            }
            catch (Exception Ex)
            {
                _notyf.Error(Ex.Message);
                return Json("error");
            }
        }

        #endregion

        #region Return Book

        [HttpGet]
        public IActionResult ReturnBook()
        {
            int id = (int)Librarian_Id().Value;
            List<BookReturn> RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from IssueBook where Returned='N' and Library_Id= '" + id + "'", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    RegisterList = new List<BookReturn>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        BookReturn cobj = new BookReturn();
                        cobj.Student_Name = ds.Tables[0].Rows[i]["Student_Name"].ToString();
                        cobj.Book_Name = ds.Tables[0].Rows[i]["Book_Name"].ToString();
                        cobj.Issue_Date = ds.Tables[0].Rows[i]["Issue_Date"].ToString();
                        cobj.Due_Date = ds.Tables[0].Rows[i]["Due_Date"].ToString();
                        cobj.Penalty = ds.Tables[0].Rows[i]["Penalty"].ToString();
                        RegisterList.Add(cobj);
                    }
                    con.Close();
                }
            }
            var tables = new IssueBook
            {
                BookReturns = RegisterList
            };
            return View(tables);
        }
        [HttpPost]
        public IActionResult ReturnBook(BookIssue data)
        {
            SqlConnection cnn = new SqlConnection(constr);
            SqlTransaction transaction;
            cnn.Open();
            transaction = cnn.BeginTransaction();

            try
            {
                SqlCommand cmd1 = new SqlCommand("SP_Issue_Book", cnn, transaction);
                SqlCommand cmd2 = new SqlCommand("SP_Issue_Book", cnn, transaction);

                cmd1.CommandType = CommandType.StoredProcedure;
                cmd2.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@Student_Name", data.Student_Name);
                cmd1.Parameters.AddWithValue("@Book_Name", data.Book_Name);
                cmd1.Parameters.AddWithValue("@Penalty", data.Penalty);
                cmd1.Parameters.AddWithValue("@status", "Delete");

                cmd2.Parameters.AddWithValue("@Student_Name", data.Student_Name);
                cmd2.Parameters.AddWithValue("@Book_Name", data.Book_Name);
                cmd2.Parameters.AddWithValue("@Penalty", data.Penalty);
                cmd2.Parameters.AddWithValue("@status", "AddPenalty");

                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();

                transaction.Commit();
                _notyf.Success("Book Returned");
                return Json("Success");
            }

            catch (SqlException Ex)
            {
                _notyf.Error(Ex.Message);
                transaction.Rollback();
                return Json("error");
            }

            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
        }

        #endregion

        #region Booklist

        public IEnumerable<BookReturn> GetBookList(string Student_Name)
        {
            List<BookReturn> RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                BookReturn obj = new BookReturn();
                using (SqlCommand cmd = new SqlCommand("SP_Issue_Book", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Student_Name", Student_Name);
                    cmd.Parameters.AddWithValue("@status", "GetBook");
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    RegisterList = new List<BookReturn>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        BookReturn cobj = new BookReturn();
                        cobj.Book_Name = ds.Tables[0].Rows[i]["Book_Name"].ToString();
                        RegisterList.Add(cobj);
                    }
                    con.Close();
                }
            }
            return RegisterList;
        }
        public IEnumerable<Libraries> LibraryList(string Student_Name)
        {
            List<Libraries> RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                BookReturn obj = new BookReturn();
                using (SqlCommand cmd = new SqlCommand("select Library_Id,Library_Name from Library", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    RegisterList = new List<Libraries>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Libraries cobj = new Libraries();
                        cobj.Library_Id = ds.Tables[0].Rows[i]["Library_Id"].ToString();
                        cobj.Library_Name = ds.Tables[0].Rows[i]["Library_Name"].ToString();
                        RegisterList.Add(cobj);
                    }
                    con.Close();
                }
            }
            return RegisterList;
        }
        #endregion

        #region Book History

        public IActionResult BookHistory()
        {
            string Student_Name = HttpContext.Session.GetString("User");
            List<BookHistory> RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                BookHistory obj = new BookHistory();
                using (SqlCommand cmd = new SqlCommand("SP_Issue_Book", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Student_Name", Student_Name);
                    cmd.Parameters.AddWithValue("@status", "BookHistory");
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    RegisterList = new List<BookHistory>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        BookHistory cobj = new BookHistory();
                        cobj.Student_Name = ds.Tables[0].Rows[i]["Student_Name"].ToString();
                        cobj.Book_Name = ds.Tables[0].Rows[i]["Book_Name"].ToString();
                        cobj.Issue_Date = ds.Tables[0].Rows[i]["Issue_Date"].ToString();
                        cobj.Due_Date = ds.Tables[0].Rows[i]["Due_Date"].ToString();
                        cobj.Return_Date = ds.Tables[0].Rows[i]["Return_Date"].ToString();
                        cobj.Penalty = ds.Tables[0].Rows[i]["Penalty"].ToString();
                        RegisterList.Add(cobj);
                    }
                    con.Close();
                }
            }
            return View(RegisterList);
        }

        #endregion

        #region Profiles

        public IActionResult UserProfile()
        {
            string Student_Name = HttpContext.Session.GetString("User");
            Profiles RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Profile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_Name", Student_Name);
                    cmd.Parameters.AddWithValue("@status", "Profile");
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                Profiles user = new Profiles()
                                {
                                    Username = sdr["Username"].ToString(),
                                    Email = sdr["Email"].ToString(),
                                    PhoneNumber = sdr["PhoneNumber"].ToString(),
                                    Profile_Picture = sdr["Profile_Picture"].ToString()
                                };
                                RegisterList = user;
                            }
                        }
                    }
                    con.Close();
                }
            }
            return View(RegisterList);
        }
        public IActionResult LibrarianProfile()
        {
            string Student_Name = HttpContext.Session.GetString("Librarian");
            Profiles RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Profile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_Name", Student_Name);
                    cmd.Parameters.AddWithValue("@status", "Profile");
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                Profiles user = new Profiles()
                                {
                                    Username = sdr["Username"].ToString(),
                                    Email = sdr["Email"].ToString(),
                                    PhoneNumber = sdr["PhoneNumber"].ToString(),
                                    Profile_Picture = sdr["Profile_Picture"].ToString()
                                };
                                RegisterList = user;
                            }
                        }
                    }
                    con.Close();
                }
            }
            return View(RegisterList);
        }
        public IActionResult AdminProfile()
        {
            string Student_Name = HttpContext.Session.GetString("Admin");
            Profiles RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Profile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_Name", Student_Name);
                    cmd.Parameters.AddWithValue("@status", "Profile");
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                Profiles user = new Profiles()
                                {
                                    Username = sdr["Username"].ToString(),
                                    Email = sdr["Email"].ToString(),
                                    PhoneNumber = sdr["PhoneNumber"].ToString(),
                                    Profile_Picture = sdr["Profile_Picture"].ToString()
                                };
                                RegisterList = user;
                            }
                        }
                    }
                    con.Close();
                }
            }
            return View(RegisterList);
        }

        #endregion

        #region Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(forgotPasswordModel), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/ForgotPassword", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var userlogin = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (userlogin.Status == "True")
                        {
                            var link = Url.Action("ResetPassword", "Home", new { forgotPasswordModel.Email }, Request.Scheme);
                            var Emailresponse = SendEmailPasswordReset(forgotPasswordModel.Email, link);
                            if (Emailresponse == true)
                            {
                                TempData["Message"] = "link successfully sended please check your email";
                                return RedirectToAction("Login");
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
            }
            TempData["Message"] = "Something went wrong ..";
            return RedirectToAction("ForgotPassword");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordModel { Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(resetPasswordModel), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync("https://localhost:44309/api/LMS/ResetPassword", content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var userlogin = JsonConvert.DeserializeObject<Response>(apiResponse);
                            if (userlogin.Status == "True")
                            {
                                TempData["Message"] = userlogin.Message;
                                return RedirectToAction("Login");
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    TempData["Message"] = Ex.Message;
                }
            }
            TempData["Message"] = "Something went wrong ..";
            return RedirectToAction("ForgotPassword");
        }

        public bool SendEmailPasswordReset(string userEmail, string link)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("abhishekm.parmar@nexuslinkservices.in");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = link;

            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("abhishekm.parmar@nexuslinkservices.in", "Nls38@#421");
            smtp.Host = "nexuslinkservices.com"; //for gmail host 
            smtp.Port = 587;
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return false;
        }

        #endregion

        #region Profile_Picture Upload
        [HttpPost]
        public IActionResult Profile_Picture(IFormFile files, string User_Name)
        {
            if (files != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Profile_Pictures");
                string filePath = Path.Combine(uploadsFolder, files.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    files.CopyTo(fileStream);
                }
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Profile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User_Name", User_Name);
                        cmd.Parameters.AddWithValue("@Profile_Picture", files.FileName);
                        cmd.Parameters.AddWithValue("@status", "Upload_Image");
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                HttpContext.Session.SetString("profilePicture", files.FileName);
                return Json("Photo Uploaded Successfully");
            }
            return Json("Something went wrong");
        }
        #endregion

        #region Chart
        public ActionResult BarChart()
        {
            string Student_Name = HttpContext.Session.GetString("User");
            List<BarChart> RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                BarChart obj = new BarChart();
                using (SqlCommand cmd = new SqlCommand("SP_Issue_Book", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Student_Name", Student_Name);
                    cmd.Parameters.AddWithValue("@status", "PenaltyChart");
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    RegisterList = new List<BarChart>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        BarChart cobj = new BarChart();
                        cobj.Due_Date = ds.Tables[0].Rows[i]["Due_Date"].ToString();
                        cobj.Penalty = ds.Tables[0].Rows[i]["Penalty"].ToString();
                        RegisterList.Add(cobj);
                    }
                    con.Close();
                }
            }
            return Json(RegisterList);
        }
        public ActionResult BarChartLibrarian()
        {
            try
            {
                int id = (int)Librarian_Id().Value;
                List<BarChartLibrarian> RegisterList = null;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    BarChartLibrarian obj = new BarChartLibrarian();
                    using (SqlCommand cmd = new SqlCommand("select No_Of_Copies_Actual,Book_Added_On from Books where Library_Id= '" + id + "'", con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        RegisterList = new List<BarChartLibrarian>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            BarChartLibrarian cobj = new BarChartLibrarian();
                            cobj.No_Of_Copies_Actual = ds.Tables[0].Rows[i]["No_Of_Copies_Actual"].ToString();
                            cobj.Book_Added_On = ds.Tables[0].Rows[i]["Book_Added_On"].ToString();
                            RegisterList.Add(cobj);
                        }
                        con.Close();
                    }
                }
                return Json(RegisterList);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        public ActionResult BarChartAdmin()
        {
            try
            {
                List<BarChartLibrarian> RegisterList = null;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    BarChartLibrarian obj = new BarChartLibrarian();
                    using (SqlCommand cmd = new SqlCommand("select No_Of_Copies_Actual,Book_Added_On from Books", con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        RegisterList = new List<BarChartLibrarian>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            BarChartLibrarian cobj = new BarChartLibrarian();
                            cobj.No_Of_Copies_Actual = ds.Tables[0].Rows[i]["No_Of_Copies_Actual"].ToString();
                            cobj.Book_Added_On = ds.Tables[0].Rows[i]["Book_Added_On"].ToString();
                            RegisterList.Add(cobj);
                        }
                        con.Close();
                    }
                }
                return Json(RegisterList);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        #endregion
    }
}