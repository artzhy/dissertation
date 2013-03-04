using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable()]
    public partial class User {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;
        [NonSerialized]
        public marcdissertation_dbEntities context;

        public static User Populate(string username) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                User usr = (from x in ctxt.Users
                        where x.Username.Equals(username)
                        select x).First();
                usr.context = ctxt;
                return usr;
            } catch {
                throw new Exception("User does not exist");
            }

        }

        public static User Populate(int userId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                User usr =  (from x in ctxt.Users
                        where x.UserId.Equals(userId)
                        select x).First();
                usr.context = ctxt;
                return usr;
            } catch {
                throw new Exception("User does not exist");
            }

        }

        public static Boolean AuthenticateUser(string username, string password) {
            marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
            return ctxt.Users.Count(x => x.Username.Equals(username) && x.Password.Equals(password)) == 1;
        }

        public static User CreateUser(string forename, string surname, string username, string password) {
           
                User u = new User();
                u.Forename = forename;
                u.Surname = surname;
                u.Username = username;
                u.Password = password;
                u.context = new marcdissertation_dbEntities();
                u = u.context.Users.Add(u);

                if (u.context.Users.Count(x => x.Username.Equals(username)) > 0) {
                    throw new Exception("Username must be unique.");
                }

                errors = u.context.GetValidationErrors();
            try {
                u.context.SaveChanges();
            } catch (Exception e) {
                throw App.ExceptionFormatter(errors);
            }
            return u;

        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }

        public void Delete() {
            context.Users.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }

    }
}
