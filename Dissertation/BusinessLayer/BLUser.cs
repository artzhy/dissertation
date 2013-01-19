using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable()]
    public partial class User {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;
        public static User Populate(string username) {
            try {
                return (from x in App.DbContext.Users
                        where x.Username.Equals(username)
                        select x).First();
            } catch {
                throw new Exception("User does not exist");
            }

        }

        public static User Populate(int userId) {
            try {
                return (from x in App.DbContext.Users
                        where x.UserId.Equals(userId)
                        select x).First();
            } catch {
                throw new Exception("User does not exist");
            }

        }

        public static Boolean AuthenticateUser(string username, string password) {
            return App.DbContext.Users.Count(x => x.Username.Equals(username) && x.Password.Equals(password)) == 1;
        }

        public static User CreateUser(string forename, string surname, string username, string password) {
           
                User u = new User();
                u.Forename = forename;
                u.Surname = surname;
                u.Username = username;
                u.Password = password;
                u = App.DbContext.Users.Add(u);

                if (App.DbContext.Users.Count(x => x.Username.Equals(username)) > 0) {
                    throw new Exception("Username must be unique.");
                }

                errors = App.DbContext.GetValidationErrors();
            try {
                App.DbContext.SaveChanges();
            } catch {
                throw App.ExceptionFormatter(errors);
            }
            return u;

        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }

        public void Delete() {
            App.DbContext.Users.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }

    }
}
