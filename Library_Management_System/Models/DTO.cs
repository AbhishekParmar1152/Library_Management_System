using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_System.Models
{
    public class DTO
    {
        public enum Alerts
        {
            Success,
            Danger,
            Info,
            Warning
        }
        public class Account
        {
            public Count count { get; set; }
            public UserRegistrationModel user { get; set; }
        }
        public class UserAccount
        {
            public CountUser count { get; set; }
            public UserRegistrationModel user { get; set; }
        }
        public class LibrarianAccount
        {
            public CountLibrarian count { get; set; }
            public UserRegistrationModel user { get; set; }
        }
        public class IssueBook
        {
            public IEnumerable<Users> users { get; set; }
            public IEnumerable<Books> books { get; set; }
            public IEnumerable<BookReturn> BookReturns { get; set; }
        }
        public class UserLoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public class UserRegistrationModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string PhoneNumber { get; set; }
            public string Library_Name { get; set; }
            public int Library_Id { get; set; }
        }
        public class Response
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public string RedirectUrl { get; set; }

            public string Token { get; set; }
            public object Data { get; set; }
        }
        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string Librarian = "Librarian";
        }
        public class ChangePassword
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string NewPassword { get; set; }
        }
        public class Books
        {
            [Key]
            public string Book_Id { get; set; }
            public string Book_Title { get; set; }
            public string Book_Publication{ get; set; }
            public string Book_Author { get; set; }
            public string Book_Category { get; set; }
            public string No_Of_Copies_Actual { get; set; }
            public string No_Of_Copies_Current { get; set; }
            public string Book_Language { get; set; }
            public string Book_Added_On { get; set; }
            public int Library_Id { get; set; }
        }
        public class Libraries
        {
            [Key]
            public string Library_Id { get; set; }
            public string Library_Name { get; set; }
            public string Library_Address { get; set; }
            public string Library_City { get; set; }
            public string Library_Pincode { get; set; }
            public string Library_Create_Date { get; set; }
            public bool IsActive { get; set; }
        }
        public class Users
        {
            [Key]
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Library_Name { get; set; }
            public int Library_Id { get; set; }
        }
        public class Count
        {
            public int LibrariesCount { get; set; }
            public int LibrarianCount { get; set; }
            public int BooksCount { get; set; }
            public int StudentCount { get; set; }
        }
        public class CountUser
        {
            public int BookReturned { get; set; }
            public int BookNotReturned { get; set; }
            public int TotalIssuedBook { get; set; }
            public int Penalty { get; set; }
        }
        public class CountLibrarian
        {
            public int BookNotReturned { get; set; }
            public int TotalIssuedBook { get; set; }
            public int BooksCount { get; set; }
            public int StudentCount { get; set; }
        }
        public class BookIssue
        {
            public string Student_Name { get; set; }
            public string Book_Name { get; set; }
            public string Penalty { get; set; }
            public int Library_id { get; set; }
        }
        public class BookReturn
        {
            public string Student_Name { get; set; }
            public string Book_Name { get; set; }
            public string Issue_Date { get; set; }
            public string Due_Date { get; set; }
            public string Penalty { get; set; }
        }
        public class BookHistory
        {
            public string Student_Name { get; set; }
            public string Book_Name { get; set; }
            public string Issue_Date { get; set; }
            public string Due_Date { get; set; }
            public string Penalty { get; set; }
            public string Return_Date { get; set; }
        }
        public class ResetPasswordModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string Email { get; set; }
            public string Token { get; set; }
        }
        //public class Message
        //{
        //    public List<MailboxAddress> To { get; set; }
        //    public string Subject { get; set; }
        //    public string Content { get; set; }

        //    public IFormFileCollection Attachments { get; set; }
           
        //    public Message(IEnumerable<string> to, string subject, string content, IFormFileCollection attachments)
        //    {

        //        To = new List<MailboxAddress>();

        //        To.AddRange(to.Select(x => new MailboxAddress(x,x)));

        //        Subject = subject;
        //        Content = content;
        //        Attachments = attachments;
        //    }
        //}
        public class ForgotPasswordModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        public class Profiles
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Profile_Picture { get; set; }
            public string PhoneNumber { get; set; }
        }
        public class BarChart
        {
            public string Penalty { get; set; }
            public string Due_Date { get; set; }
        }
        public class BarChartLibrarian
        {
            public string No_Of_Copies_Actual { get; set; }
            public string Book_Added_On { get; set; }
        }
    }
}
