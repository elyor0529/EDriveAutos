using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Membership.Providers;

namespace Edrive.Membership.Helpers
{
   public sealed class MembershipHelper
    {
		private const string CurrentUserDetailsSession = "CurrentUserDetailsSession";

        public static HttpContextBase Context
        {
            get { return new HttpContextWrapper(HttpContext.Current); }
        }

        public static HttpRequestBase Request
        {
            get { return Context.Request; }
        }

        public static HttpResponseBase Response
        {
            get { return Context.Response; }
        }

        public static System.Security.Principal.IPrincipal User
        {
            get { return Context.User; }
        }

        public static bool IsAuthenticated
        {
            get { return User.Identity.IsAuthenticated; }
        }

        public static MembershipCreateStatus Register(string username, string password, string email, bool isApproved, string firstName, string lastName, string role)
        {
            MembershipCreateStatus createStatus;
            System.Web.Security.Membership.CreateUser(username, password, email, null, null, isApproved, null, out createStatus);

            if (createStatus == MembershipCreateStatus.Success)
            {
                using (var context = new DataContext())
                {
                    User user = context.Users.FirstOrDefault(usr => usr.Username == username);
                    if (user != null)
                    {
                        user.FirstName = firstName;
                        user.LastName = lastName;
                    	//user.IsActive = isApproved;
                    }
                    context.SaveChanges();
                }

				Roles.AddUserToRole(username, role.ToString());

//                if (isApproved)
//                {
//                    FormsAuthentication.SetAuthCookie(username, false);
//                }
            }

            return createStatus;
        }

        public enum MembershipLoginStatus
        {
            Success, Failure
        }

        public static MembershipLoginStatus Login(string username, string password, bool rememberMe)
        {
            if (System.Web.Security.Membership.ValidateUser(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, rememberMe);
                return MembershipLoginStatus.Success;
            }
            else
            {
                return MembershipLoginStatus.Failure;
            }
        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }
		
    	public static User CurrentUser
    	{
			get
			{
				User user = null;
				
				if(IsAuthenticated)
				{
					if(HttpContext.Current.Session[CurrentUserDetailsSession] == null)
					{
						GetUser(HttpContext.Current.User.Identity.Name); //init user session

						if(HttpContext.Current.Session[CurrentUserDetailsSession] == null)
							Response.Redirect("~/Account/LogOff");
					}

					if (HttpContext.Current.Session[CurrentUserDetailsSession] != null)
					{
						user = (User) HttpContext.Current.Session[CurrentUserDetailsSession];
					}
				}

				return user;
			}
			set { HttpContext.Current.Session[CurrentUserDetailsSession] = value; }
    	}

		public static MembershipUser GetUser(string username)
        {
            return System.Web.Security.Membership.GetUser(username);
        }

        public static bool ChangePassword(string oldPassword, string newPassword)
        {
            MembershipUser currentUser = System.Web.Security.Membership.GetUser(User.Identity.Name);
            return currentUser != null && currentUser.ChangePassword(oldPassword, newPassword);
        }

        public static bool DeleteUser(string username)
        {
            return System.Web.Security.Membership.DeleteUser(username);
        }

        public static List<MembershipUser> FindUsersByEmail(string email, int pageIndex, int pageSize)
        {
            int totalRecords;
            return System.Web.Security.Membership.FindUsersByEmail(email, pageIndex, pageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> FindUsersByName(string username, int pageIndex, int pageSize)
        {
            int totalRecords;
            return System.Web.Security.Membership.FindUsersByName(username, pageIndex, pageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> GetAllUsers(int pageIndex, int pageSize)
        {
            int totalRecords;
            return System.Web.Security.Membership.GetAllUsers(pageIndex, pageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }
    }
}
