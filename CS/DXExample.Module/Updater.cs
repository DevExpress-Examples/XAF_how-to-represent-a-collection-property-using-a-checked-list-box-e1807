using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;

namespace DXExample.Module {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            Master master = ObjectSpace.FindObject<Master>(new BinaryOperator("MasterName", "TestMaster"));
            if (master == null) {
                master = ObjectSpace.CreateObject<Master>();
                master.MasterName = "TestMaster";
                CreateDetail("1");
                CreateDetail("2");
                master.Details.Add(CreateDetail("3"));
                CreateDetail("4");
                CreateDetail(null);
                master.Save();
            }
        }
        private Detail CreateDetail(string name) {
            Detail detail = ObjectSpace.CreateObject<Detail>();
            detail.DetailName = name;
            detail.Save();
            return detail;
        }
    }
}
