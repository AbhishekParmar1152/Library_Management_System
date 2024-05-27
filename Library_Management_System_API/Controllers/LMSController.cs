using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System_API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static Library_Management_System.Models.DTO;

namespace Library_Management_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LMSController : ControllerBase
    {
        readonly string constr = "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=Test2;";
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;


        public LMSController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<Response> Login([FromBody] UserLoginModel model)
        {
            Response response = new Response();
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user == null)
            {
                response.Status = "False";
                response.Message = "Incorrect UserName";
                return response;
            }
            var sign = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
            if (sign.IsLockedOut)
            {
                response.Message = "Account Locked try again after 5 minutes";
                response.Status = "False";
                return response;
            }
            if (user != null && sign.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var role = "";
                var newUrl = "";

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    role = userRole;
                }

                var token = GetToken(authClaims);
                var a = (new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
                var claim = new Claim(ClaimTypes.Name, user.UserName);
                var result = await _userManager.AddClaimAsync(user, claim);
                //await _signInManager.SignInAsync(user, false, null);
                var Claimsoutput = await _userManager.GetClaimsAsync(user);

                if (role == "Admin")
                {
                    newUrl = "AdminAccount";
                }
                else if (role == "User")
                {
                    newUrl = "UserAccount";
                }
                else if (role == "Librarian")
                {
                    newUrl = "LibrarianAccount";
                }
                response.Status = "Success";
                response.Message = "Login successfully!";
                response.Token = a.token;
                response.Data = user;
                response.RedirectUrl = newUrl;
                return response;
            }
            response.Status = "False";
            response.Message = "Incorrect Password";
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("register-user")]
        public async Task<Response> Register([FromBody] UserRegistrationModel model)
        {
            Response response = new Response();
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                {
                    response.Status = "Error";
                    response.Message = "User already exists!";
                    return response;
                }

                var user = new IdentityUser
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    response.Status = "Error";
                    response.Message = "User creation failed! Please check user details and try again.";
                    return response;
                }

                if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }

                if (await _roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                    SqlConnection connection = new SqlConnection(constr);
                    SqlCommand Command1 = new SqlCommand("update AspNetUsers set IsActive=0 where SecurityStamp = @SecurityStamp", connection);
                    SqlCommand Command2 = new SqlCommand("update AspNetUsers set Library_Id=@Library_Id where SecurityStamp = @SecurityStamp", connection);
                    SqlParameter param1 = new SqlParameter();
                    SqlParameter param2 = new SqlParameter();
                    SqlParameter param3 = new SqlParameter();
                    param1.ParameterName = "@SecurityStamp";
                    param1.Value = user.SecurityStamp;
                    param2.ParameterName = "@Library_Id";
                    param2.Value = model.Library_Id;
                    param3.ParameterName = "@SecurityStamp";
                    param3.Value = user.SecurityStamp;
                    Command1.Parameters.Add(param1);
                    Command2.Parameters.Add(param2);
                    Command2.Parameters.Add(param3);
                    connection.Open();
                    Command1.ExecuteNonQuery();
                    Command2.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = ex.ToString();
                return response;
            }
            response.Status = "Success";
            response.Message = "User created successfully!";
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("register-admin")]
        public async Task<Response> RegisterAdmin([FromBody] UserRegistrationModel model)
        {
            Response response = new Response();
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                {
                    response.Status = "Error";
                    response.Message = "User already exists!";
                    return response;
                }

                IdentityUser user = new IdentityUser
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    response.Status = "Error";
                    response.Message = "User creation failed! Please check user details and try again.";
                    return response;
                }

                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }

                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {

                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                    SqlConnection connection = new SqlConnection(constr);
                    SqlCommand Command1 = new SqlCommand("update AspNetUsers set IsActive=0 where SecurityStamp = @SecurityStamp", connection);
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@SecurityStamp";
                    param.Value = user.SecurityStamp;
                    Command1.Parameters.Add(param);
                    connection.Open();
                    Command1.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = ex.ToString();
                return response;
            }
            response.Status = "Success";
            response.Message = "User created successfully!";
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("register-librarian")]
        public async Task<Response> Registerlibrarian([FromBody] UserRegistrationModel model)
        {
            Response response = new Response();
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                {
                    response.Status = "Error";
                    response.Message = "User already exists!";
                    return response;
                }

                IdentityUser user = new IdentityUser
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    response.Status = "Error";
                    response.Message = "User creation failed! Please check user details and try again.";
                    return response;
                }

                if (!await _roleManager.RoleExistsAsync(UserRoles.Librarian))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Librarian));
                }

                if (await _roleManager.RoleExistsAsync(UserRoles.Librarian))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Librarian);

                    SqlConnection connection = new SqlConnection(constr);
                    SqlCommand Command1 = new SqlCommand("update AspNetUsers set IsActive=0 where SecurityStamp = @SecurityStamp", connection);
                    SqlCommand Command2 = new SqlCommand("update AspNetUsers set Library_Id=@Library_Id where SecurityStamp = @SecurityStamp", connection);
                    SqlParameter param1 = new SqlParameter();
                    SqlParameter param2 = new SqlParameter();
                    SqlParameter param3 = new SqlParameter();
                    param1.ParameterName = "@SecurityStamp";
                    param1.Value = user.SecurityStamp;
                    param2.ParameterName = "@Library_Id";
                    param2.Value = model.Library_Id;
                    param3.ParameterName = "@SecurityStamp";
                    param3.Value = user.SecurityStamp;
                    Command1.Parameters.Add(param1);
                    Command2.Parameters.Add(param2);
                    Command2.Parameters.Add(param3);
                    connection.Open();
                    Command1.ExecuteNonQuery();
                    Command2.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = ex.ToString();
                return response;
            }
            response.Status = "Success";
            response.Message = "User created successfully!";
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("AdminAccount")]
        public async Task<IActionResult> AdminAccount([FromBody] UserRegistrationModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return Ok(model);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Something went wrong!" });
        }

        [Authorize]
        [HttpPost]
        [Route("LibrarianAccount")]
        public async Task<IActionResult> LibrarianAccount([FromBody] UserRegistrationModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return Ok(model);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Something went wrong!" });
        }

        [Authorize]
        [HttpPost]
        [Route("UserAccount")]
        public async Task<IActionResult> UserAccount([FromBody] UserRegistrationModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return Ok(model);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Something went wrong!" });
        }

        [Authorize]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<Response> ChangePassword(ChangePassword Obj)
        {
            Response response = new Response();
            try
            {
                var user = await _userManager.FindByNameAsync(Obj.Username);

                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, Obj.Password, Obj.NewPassword);

                    if (!result.Succeeded)
                    {
                        response.Status = "Error";
                        response.Message = "Please Enter Correct Password";
                        return response;
                    }
                    else
                    {
                        response.Status = "Success";
                        response.Message = "Password Changed Successfully...!!";
                        return response;
                    }
                }
            }
            catch (Exception Ex)
            {
                response.Status = "Error";
                response.Message = Ex.Message.ToString();
                return response;
            }
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("ManageLibraries")]
        public List<DTO.Libraries> ManageLibraries()
        {
            List<Libraries> RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                Libraries obj = new Libraries();
                using (SqlCommand cmd = new SqlCommand("SP_ManageLibraries", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "Display");
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
                        cobj.Library_Address = ds.Tables[0].Rows[i]["Library_Address"].ToString();
                        cobj.Library_Create_Date = ds.Tables[0].Rows[i]["Library_Create_Date"].ToString();
                        cobj.Library_City = ds.Tables[0].Rows[i]["Library_City"].ToString();
                        cobj.Library_Pincode = ds.Tables[0].Rows[i]["Library_Pincode"].ToString();
                        RegisterList.Add(cobj);
                    }
                    con.Close();
                }
            }
            return RegisterList;
        }

        [Authorize]
        [HttpPost]
        [Route("ManageBooks")]
        public List<DTO.Books> ManageBooks(Books books)
        {
            int id = books.Library_Id;
            List<Books> RegisterList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                Books obj = new Books();
                using (SqlCommand cmd = new SqlCommand("SP_Books", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "Display");
                    cmd.Parameters.AddWithValue("@Library_Id", id);
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
            return RegisterList;
        }

        [Authorize]
        [HttpGet]
        [Route("ManageLibrarian")]
        public List<DTO.Users> ManageLibrarian()
        {
            List<Users> UserList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                Users obj = new Users();
                using (SqlCommand cmd = new SqlCommand("SP_Manage_Librarian", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "Display");
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
                        int Lib_id = (int)ds.Tables[0].Rows[i]["Library_Id"];
                        SqlCommand Command1 = new SqlCommand("select Library_Name from Library where Library_Id ='" + Lib_id + "'", con);
                        cobj.Library_Name = (string)Command1.ExecuteScalar();
                        UserList.Add(cobj);
                    }
                    con.Close();
                }
            }
            return UserList;
        }

        [Authorize]
        [HttpPost]
        [Route("ManageUser")]
        public List<DTO.Users> ManageUser(Books books)
        {
            int id = books.Library_Id;
            List<Users> UserList = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                Users obj = new Users();
                using (SqlCommand cmd = new SqlCommand("SP_Manage_User", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "Display");
                    cmd.Parameters.AddWithValue("@Library_Id", id);
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
                        int Lib_id = (int)ds.Tables[0].Rows[i]["Library_Id"];
                        SqlCommand Command1 = new SqlCommand("select Library_Name from Library where Library_Id ='" + Lib_id + "'", con);
                        cobj.Library_Name = (string)Command1.ExecuteScalar();
                        UserList.Add(cobj);
                    }
                    con.Close();
                }
            }
            return UserList;
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<Response> ForgotPassword(DTO.ForgotPasswordModel forgotPasswordModel)
        {
            Response response = new Response();
            if (!ModelState.IsValid)
                response.RedirectUrl = "ForgotPassword";
            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user != null)
            {
                response.Status = "True";
            }
            return response;
        }
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<Response> ResetPassword(ResetPasswordModel resetPassword)
        {
            Response response = new Response();
            if (resetPassword.Password == resetPassword.ConfirmPassword)
            {
                var user1 = await _userManager.FindByEmailAsync(resetPassword.Email);
                if (user1 == null)
                {
                    response.Status = "False";
                    return response;
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user1);
                try
                {
                    var resetPassResult = await _userManager.ResetPasswordAsync(user1, token, resetPassword.ConfirmPassword);
                    if (!resetPassResult.Succeeded)
                    {
                        response.Status = "False";
                        foreach (var error in resetPassResult.Errors)
                        {
                            response.Message = error.Description;
                        }
                    }
                    else
                    {
                        response.Status = "True";
                        response.Message = "Password has been reset. Please Login.";
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                }

            }
            response.Message = "Some Problem..Please Check New Password and Confirm Password must be same!!";
            return response;
        }

        //---------- Token Generate ----------//
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }

}
