//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SharedClasses
//{
//    [Serializable]
//    [Obsolete("Use Business Layer instead", true)]
//    public class CommunicationPackage1
//    {
//        public int WorkOrderId {
//            get;
//            set;
//        }

//        public int? UpdateFromDeviceId {
//            get;
//            set;
//        }

//        public UpdateType CommunicationType {
//            get;
//            set;
//        }

//        public String ResultJson {
//            get;
//            set;
//        }

//        public enum UpdateType {
//            Result,
//            UpdateRequest,
//            Error,
//            NewWorkOrder
//        }

//        public int TargetDeviceId {
//            get;
//            set;
//        }

//        public DateTime? ComputationStartTime {
//            get;
//            set;
//        }

//        public DateTime? ComputationEndTime {
//            get;
//            set;
//        }

//        public CommunicationPackage(int _workOrderId, UpdateType _updateType, int? _modifiedBy, DateTime? _compStartTime, DateTime? _compEndTime, int _targetDeviceId,String _resultJson = "") {
//            this.WorkOrderId = _workOrderId;
//            this.CommunicationType = _updateType;
//            this.UpdateFromDeviceId = _modifiedBy;
//            this.ResultJson = _resultJson;
//            this.ComputationEndTime = _compEndTime;
//            this.ComputationStartTime = _compStartTime;
//            this.TargetDeviceId = _targetDeviceId;
//        }

//        public String Serialize() {
//            //StringBuilder sb = new StringBuilder();

//            //sb.Append("{");
//            //sb.Append("WorkOrderId: '" + this.WorkOrderId.ToString() + "', ");
//            //sb.Append("CommunicationType: '" + this + "', ");
//            //sb.Append("WorkOrderId: '" + this.WorkOrderId.ToString() + "', ");


//            //sb.Append("}");
//            //return sb.ToString;

//            return Newtonsoft.Json.JsonConvert.SerializeObject(this);


//        }
//    }
//}
