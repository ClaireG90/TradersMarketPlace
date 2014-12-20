using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Business;
using TradersMarketPlace.Models;

namespace TradersMarketPlace.Controllers
{
    public class RegistrationController : Controller
    {
        //
        // GET: /Registration/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateUser()
        {
            return View(new Models.RegistrationModel());
        }

        [HttpPost]
        public ActionResult CreateUser(RegistrationModel rm)
        {
                if (rm.Password.Length < 6)
                {
                    TempData["Message"] = "Password needs to be longer";
                    return RedirectToAction("CreateUser");
                }
                else
                {
                    Account checkAccount = new BAAccount().GetAccountByUsername(rm.UserName);
                    User checkEmail = new BAUser().GetUserByEmail(rm.Email);
                    if (checkAccount == null && checkEmail == null )
                    {
                       
                        Account acc = new Account();
                        acc.ID = Guid.NewGuid();
                        acc.Username = rm.UserName;
                        acc.Password = rm.Password;
                        new BAAccount().AddAccount(acc); 

                        Account a = new BAAccount().GetAccountByUsername(rm.UserName); 
                        //USer
                        rm.user.ID = Guid.NewGuid();
                        rm.user.Name = rm.Name;
                        rm.user.Surname = rm.Surname;
                        rm.user.Email = rm.Email;
                        rm.user.HouseNumber = rm.HouseNumber;
                        rm.user.StreetName = rm.StreetName;
                        rm.user.UserTypeID = rm.user.UserTypeID;
                        rm.user.AccountID = a.ID; 
                        new BAUser().AddUser(rm.user);


                    }
                    else
                    {
                        if (checkAccount != null)
                        {
                            TempData["Message"] = "Username already exists";
                        }
                        else if (checkEmail != null)
                        {
                            TempData["Email"] = "Email already exists";
                        }
                        

                    }
                }
                return RedirectToAction("CreateUser");
            //}
            //catch (Exception ex)
            //{
            //    TempData["CatchError"] = "An error was encountered. Please try again later";
            //    return RedirectToAction("RegisterUser");
            //}
        }

        public ActionResult UpdateUser()
        {
            return View(new Models.RegistrationModel());
        }

        [HttpPost]
        public ActionResult UpdateUser(RegistrationModel model, Guid userID)
        {
            try
            {
                Account acc = new BAAccount().GetAccountByUsername(Session["username"].ToString());
                User user = new BAUser().GetUser(userID);

                User u = new User();
                u.ID = userID;
                u.Name = user.Name;
                u.Surname = model.Surname;
                u.Email = model.Email;
                u.HouseNumber = model.HouseNumber;
                u.StreetName = model.StreetName;
                u.TownID = user.TownID;
                u.AccountID = acc.ID;
                u.UserTypeID = (Guid)user.UserTypeID;

                Account a = new Account();
                a.ID = acc.ID;
                a.Username = acc.Username;
                a.Password = model.Password;
                new BAUser().Update(u);
                new BAAccount().Update(a);
                

                //User user = new User();
                //Account acc = new Account();
                //user.Surname = model.Surname;
                //user.HouseNumber = model.HouseNumber;
                //user.StreetName = model.StreetName;
                //user.Email = model.Email;

                //acc.Password = model.Password;

                //BAUser baUser = new BAUser();
                //BAAccount baAccount = new BAAccount();

                //baUser.Update(user);
                //baAccount.Update(acc);

                return RedirectToAction("UpdateUser");
            }
            catch (Exception ex)
            {
                TempData["CatchError"] = "An error was encountered. Please try again later";
                return RedirectToAction("Index", model);
            }
        }
     
    }
}


