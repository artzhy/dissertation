using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestApp {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {

            try {


                List<WorkOrderService.CommPackageParamListItem> parameters = new List<WorkOrderService.CommPackageParamListItem>();

                parameters.Add(new WorkOrderService.CommPackageParamListItem() {
                    ParameterNamek__BackingField = "a",
                    ParameterValuek__BackingField = 3
                });

                parameters.Add(new WorkOrderService.CommPackageParamListItem() {
                    ParameterNamek__BackingField = "b",
                    ParameterValuek__BackingField = 9
                });

                WorkOrderService.WorkOrderSvcClient wsc = new WorkOrderService.WorkOrderSvcClient();
                wsc.CreateWorkOrder("C091EF5A93B7DC4BCF0BB8D43AAF62B9", 6, 4, parameters.ToArray(), "WorkerFunction");

            } catch {

            }

            
        }
    }
}
