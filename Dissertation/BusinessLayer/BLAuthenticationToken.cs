using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;


namespace BusinessLayer {
    [Serializable()]
    public partial class AuthenticationToken {

        public static AuthenticationToken Populate(String tokenId) {
            try {
                return (from x in App.DbContext.AuthenticationTokens
                        where x.Token == tokenId
                        select x).First();
            } catch {
                throw new Exception("Token does not exist");
            }
        }

        public static AuthenticationToken Populate(int deviceId) {
            try {
                return (from x in App.DbContext.AuthenticationTokens
                        where x.DeviceId == deviceId
                        select x).First();
            } catch {
                throw new Exception("Token does not exist");
            }
        }

        private static String GenerateAuthTokenString(string username, int deviceId) {
            HashAlgorithm algorithm = MD5.Create();  // SHA1.Create()
            //   return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            StringBuilder sb = new StringBuilder();
            sb.Append(username);
            sb.Append(deviceId);
            sb.Append(DateTime.Now.ToShortDateString());
            sb.Append(DateTime.Now.ToShortTimeString());

            byte[] hashed = algorithm.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));

            sb.Clear();
            foreach (byte b in hashed)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static AuthenticationToken AddAuthenticationToken(int deviceId, string username) {


            if (App.DbContext.AuthenticationTokens.Count(x => x.UserDevice.User.Username == username) > 0) {
                throw new Exception("Device already has authentication token.");
            }

            AuthenticationToken at = new AuthenticationToken();
            at.DeviceId = deviceId;
            at.Token = GenerateAuthTokenString(username, deviceId);
            at.CreationDate = DateTime.Now;
            at.Username = username;

            at = App.DbContext.AuthenticationTokens.Add(at);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();
            return at;
        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }

        public void Delete() {
            App.DbContext.AuthenticationTokens.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }

    }
}
