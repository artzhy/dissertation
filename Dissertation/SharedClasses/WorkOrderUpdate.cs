using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedClasses
{
    [Serializable]
    public class WorkOrderUpdate
    {
        public int WorkOrderId {
            get;
            set;
        }

        public int ModifiedBy {
            get;
            set;
        }

        public UpdateType WorkOrderUpdateType {
            get;
            set;
        }

        public String ResultJson {
            get;
            set;
        }

        public enum UpdateType {
            Cancel,
            Acknowledge,
            SubmitResult,
            MarkBeingComputed
        }

        public DateTime? ComputationStartTime {
            get;
            set;
        }

        public DateTime? ComputationEndTime {
            get;
            set;
        }

        public WorkOrderUpdate(int _workOrderId, UpdateType _updateType, int _modifiedBy, DateTime? _compStartTime, DateTime? _compEndTime, String _resultJson = "") {
            this.WorkOrderId = _workOrderId;
            this.WorkOrderUpdateType = _updateType;
            this.ModifiedBy = _modifiedBy;
            this.ResultJson = _resultJson;
            this.ComputationStartTime = _compStartTime;
            this.ComputationEndTime = _compEndTime;
        }
    }
}
