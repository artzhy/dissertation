//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkApplication
    {
        public WorkApplication()
        {
            this.DeviceAppInstallations = new HashSet<DeviceAppInstallation>();
            this.WorkOrders = new HashSet<WorkOrder>();
        }
    
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationCreator { get; set; }
        public string ApplicationPackageURL { get; set; }
        public string ApplicationWorkIntent { get; set; }
        public int ApplicationVersion { get; set; }
        public string ApplicationNamespace { get; set; }
    
        public virtual ICollection<DeviceAppInstallation> DeviceAppInstallations { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}
