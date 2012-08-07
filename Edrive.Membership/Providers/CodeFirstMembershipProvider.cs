using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using Edrive.Core.Model;
	using Edrive.Data;
	using Edrive.Membership.Helpers;
	
namespace Edrive.Membership.Providers
{
	public class CodeFirstMembershipProvider : MembershipProvider
    {
        #region Properties
        // ReSharper disable FunctionRecursiveOnAllPaths
        public override string ApplicationName
        {
            get
            {
                return GetType().Assembly.GetName().Name;
            }
            set
            {
                ApplicationName = GetType().Assembly.GetName().Name;
            }
        }

        // ReSharper restore FunctionRecursiveOnAllPaths
        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 0; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return String.Empty; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        #endregion

        #region Functions

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (string.IsNullOrEmpty(username))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }
            if (string.IsNullOrEmpty(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (string.IsNullOrEmpty(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            string hashedPassword = Crypto.HashPassword(password);
            if (hashedPassword.Length > 128)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            using (var context = new DataContext())
            {
                if (context.Users.Any(usr => usr.Username == username))
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                    return null;
                }

                if (context.Users.Any(usr => usr.Email == email))
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                    return null;
                }

                User newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Username = username,
                    Password = password,
                    IsApproved = isApproved,
                    //IsActive = isApproved,
                    //Percentage = 0,
					FirstName = username,
					LastName = username,
                    //Address = "address",
                    //PhoneNumber = "12345",
                    //MobilePhone = "12345",
                    Email = email,
                    CreateDate = DateTime.UtcNow,
                    LastPasswordChangedDate = DateTime.UtcNow,
                    PasswordFailuresSinceLastSuccess = 0,
                    LastLoginDate = DateTime.UtcNow,
                    LastActivityDate = DateTime.UtcNow,
                    LastLockoutDate = DateTime.UtcNow,
                    IsLockedOut = false,
                    LastPasswordFailureDate = DateTime.UtcNow,
                    //StartMinute = 0,
                    //EndMinute = 0,
                    //StartWork = new TimeSpan(8, 0, 0),
                    //FinishWork = new TimeSpan(20, 0, 0),
					Created = DateTime.Now.ToString(),
					//UnlimitedAccess = false
                };

                context.Users.Add(newUser);
                context.SaveChanges();
                status = MembershipCreateStatus.Success;
                if (newUser.CreateDate != null && newUser.LastLoginDate != null && newUser.LastActivityDate != null &&
                    newUser.LastPasswordChangedDate != null && newUser.LastLockoutDate != null)
                    return new MembershipUser(System.Web.Security.Membership.Provider.Name, newUser.Username, newUser.UserId,
                                              newUser.Email, null, null, newUser.IsApproved,
                                              newUser.IsLockedOut, newUser.CreateDate.Value,
                                              newUser.LastLoginDate.Value, newUser.LastActivityDate.Value,
                                              newUser.LastPasswordChangedDate.Value,
                                              newUser.LastLockoutDate.Value);
            }
            return null;
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            using (var context = new DataContext())
            {
                User user = context.Users.FirstOrDefault(usr => usr.Username == username);
                if (user == null)
                {
                    return false;
                }
                if (!user.IsApproved)
                {
                    return false;
                }
                if (user.IsLockedOut)
                {
                    return false;
                }
                var hashedPassword = user.Password;
                var verificationSucceeded = (hashedPassword != null && password == hashedPassword);
                if (verificationSucceeded)
                {
                    user.PasswordFailuresSinceLastSuccess = 0;
                    user.LastLoginDate = DateTime.UtcNow;
                    user.LastActivityDate = DateTime.UtcNow;
                }
                else
                {
                    int failures = user.PasswordFailuresSinceLastSuccess;
                    if (failures < MaxInvalidPasswordAttempts)
                    {
                        user.PasswordFailuresSinceLastSuccess += 1;
                        user.LastPasswordFailureDate = DateTime.UtcNow;
                    }
                    else if (failures >= MaxInvalidPasswordAttempts)
                    {
                        user.LastPasswordFailureDate = DateTime.UtcNow;
                        user.LastLockoutDate = DateTime.UtcNow;
                        user.IsLockedOut = true;
                    }
                }
                context.SaveChanges();
                if (verificationSucceeded)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            using (var context = new DataContext())
            {
                var user = context.Users.FirstOrDefault(usr => usr.Username == username);
                if (user != null)
                {
					MembershipHelper.CurrentUser = user;

                    if (userIsOnline)
                    {
                        user.LastActivityDate = DateTime.UtcNow;
                        context.SaveChanges();
                    }

                    return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.GetValueOrDefault(), user.LastLoginDate.GetValueOrDefault(), user.LastActivityDate.GetValueOrDefault(), user.LastPasswordChangedDate.GetValueOrDefault(), user.LastLockoutDate.GetValueOrDefault());
                }
                return null;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey is Guid) { }
            else
            {
                return null;
            }

            using (var context = new DataContext())
            {
                User user = null;
                user = context.Users.Find(providerUserKey);
                if (user != null)
                {
                    if (userIsOnline)
                    {
                        user.LastActivityDate = DateTime.UtcNow;
                        context.SaveChanges();
                    }

                    return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value);
                }
                return null;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(oldPassword))
            {
                return false;
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return false;
            }
            using (var context = new DataContext())
            {
                User user = null;
                user = context.Users.FirstOrDefault(usr => usr.Username == username);
                if (user == null)
                {
                    return false;
                }
                var hashedPassword = user.Password;
                var verificationSucceeded = (hashedPassword != null && hashedPassword == oldPassword);
                if (verificationSucceeded)
                {
                    user.PasswordFailuresSinceLastSuccess = 0;
                }
                else
                {
                    int failures = user.PasswordFailuresSinceLastSuccess;
                    if (failures < MaxInvalidPasswordAttempts)
                    {
                        user.PasswordFailuresSinceLastSuccess += 1;
                        user.LastPasswordFailureDate = DateTime.UtcNow;
                    }
                    else if (failures >= MaxInvalidPasswordAttempts)
                    {
                        user.LastPasswordFailureDate = DateTime.UtcNow;
                        user.LastLockoutDate = DateTime.UtcNow;
                        user.IsLockedOut = true;
                    }
                    context.SaveChanges();
                    return false;
                }
                var newHashedPassword = newPassword;
                if (newHashedPassword.Length > 128)
                {
                    return false;
                }
                user.Password = newHashedPassword;
                user.LastPasswordChangedDate = DateTime.UtcNow;
                context.SaveChanges();
                return true;
            }
        }

        public override bool UnlockUser(string userName)
        {
            using (var context = new DataContext())
            {
                User user = null;
                user = context.Users.FirstOrDefault(usr => usr.Username == userName);
                if (user != null)
                {
                    user.IsLockedOut = false;
                    user.PasswordFailuresSinceLastSuccess = 0;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            var dateActive = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(Convert.ToDouble(System.Web.Security.Membership.UserIsOnlineTimeWindow)));
            using (var context = new DataContext())
            {
                return context.Users.Count(user => user.LastActivityDate > dateActive);
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            using (var context = new DataContext())
            {
                var user = context.Users.FirstOrDefault(usr => usr.Username == username);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            using (var context = new DataContext())
            {
                var user = context.Users.FirstOrDefault(usr => usr.Email == email);
                return user != null ? user.Username : string.Empty;
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection membershipUsers = new MembershipUserCollection();
            using (var context = new DataContext())
            {
                totalRecords = context.Users.Count(usr => usr.Email == emailToMatch);
                IQueryable<User> users = context.Users.Where(usr => usr.Email == emailToMatch).OrderBy(usrn => usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
                foreach (var user in users)
                {
                    membershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
                }
            }
            return membershipUsers;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection membershipUsers = new MembershipUserCollection();
            using (var context = new DataContext())
            {
                totalRecords = context.Users.Count(usr => usr.Username == usernameToMatch);
                IQueryable<User> users = context.Users.Where(usr => usr.Username == usernameToMatch).OrderBy(usrn => usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
                foreach (var user in users)
                {
                    membershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
                }
            }
            return membershipUsers;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection membershipUsers = new MembershipUserCollection();
            using (var context = new DataContext())
            {
                totalRecords = context.Users.Count();
                IQueryable<User> users = context.Users.OrderBy(usrn => usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
                foreach (User user in users)
                {
                    membershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
                }
            }
            return membershipUsers;
        }

        #endregion

        #region Not Supported

        //CodeFirstMembershipProvider does not support password retrieval scenarios.
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support password reset scenarios.
        public override bool EnablePasswordReset
        {
            get { return false; }
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support question and answer scenarios.
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support UpdateUser because this method is useless.
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}