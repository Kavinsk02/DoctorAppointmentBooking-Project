using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;
using System.Configuration;

namespace WebApplication13.Controllers
{
    public class ChnagepasswordController : Controller
    {
        string conString = ConfigurationManager.ConnectionStrings["DBconnect"].ToString();
        Changepassword changepassword=new Changepassword();
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(Changepassword cpmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(cpmodel);
            }

            if (!IsOldPasswordValid(User.Identity.Name, cpmodel.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "The old password is incorrect.");
                return View(cpmodel);
            }

            bool passwordChanged = changepassword.ChangePassword(User.Identity.Name, cpmodel.NewPassword);
            if (passwordChanged)
            {
                return RedirectToAction("ChangePasswordSuccess");
            }
            else
            {
                ModelState.AddModelError("", "Failed to change the password. Please try again.");
                return View(cpmodel);
            }
        }


        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private bool IsOldPasswordValid(string username, string oldPassword)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                string query = "SELECT Password FROM Signup WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    string storedPassword = command.ExecuteScalar()?.ToString();


                    bool passwordIsValid = (oldPassword == storedPassword);
                    return passwordIsValid;
                }
            }
        }
    }
}
