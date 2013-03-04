using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;


namespace BusinessLayer {
    [Serializable()]
    public partial class AuthenticationToken {
        [NonSerialized]
        private marcdissertation_dbEntities context;

        public static AuthenticationToken Populate(String tokenId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                AuthenticationToken at = (from x in ctxt.AuthenticationTokens
                        where x.Token == tokenId
                        select x).First();
                at.context = ctxt;
                return at;
            } catch {
                throw new Exception("Token does not exist");
            }
        }

        public static AuthenticationToken Populate(int deviceId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                AuthenticationToken at = (from x in ctxt.AuthenticationTokens
                        where x.DeviceId == deviceId
                        select x).First();
                at.context = ctxt;
                return at;
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
            marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();

            if (ctxt.AuthenticationTokens.Count(x => x.UserDevice.User.Username == username  && x.UserDevice.DeviceId == deviceId) > 0) {
               throw new Exception("Device already has authentication token.");
           //     return AuthenticationToken.Populate(deviceId);
            }

            AuthenticationToken at = new AuthenticationToken();
            at.DeviceId = deviceId;
            at.Token = GenerateAuthTokenString(username, deviceId);
            at.CreationDate = DateTime.Now;
            at.Username = username;
            at.context = ctxt;

            at = at.context.AuthenticationTokens.Add(at);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = at.context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            at.context.SaveChanges();
            return at;
        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }

        public void Delete() {
            context.AuthenticationTokens.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }

    }
}
