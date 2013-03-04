using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    class App
    {
        //public static marcdissertation_dbEntities _DbContext;

        //public static marcdissertation_dbEntities DbContext {
        //    get {
        //       if (_DbContext == null) {
        //            _DbContext = new marcdissertation_dbEntities();
        //       }
                
        //        return _DbContext;
        //    }
        //}


        public static Exception ExceptionFormatter(IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors)
        {
           
            StringBuilder sb = new StringBuilder();

            foreach (System.Data.Entity.Validation.DbEntityValidationResult error in errors)
            {
                foreach (System.Data.Entity.Validation.DbValidationError vR in error.ValidationErrors)
                {
                    sb.AppendLine(vR.PropertyName + ": " + vR.ErrorMessage);
                }
            }
            return new Exception(sb.ToString());
            }

    }
}
