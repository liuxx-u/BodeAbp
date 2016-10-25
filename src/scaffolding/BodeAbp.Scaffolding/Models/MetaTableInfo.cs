using System;
using System.Collections.Generic;
using System.Linq;

namespace BodeAbp.Scaffolding.Models
{
    [Serializable]
    public class MetaTableInfo
    {
        public List<MetaColumnInfo> Columns { get; set; }

        public MetaColumnInfo this[string name]
        {
            get { return this.Columns.FirstOrDefault(x => x.Name == name); }
        }
        public MetaColumnInfo this[int index]
        {
            get { return this.Columns[index]; }
        }

        public MetaTableInfo()
        {
            this.Columns = new List<MetaColumnInfo>();
        }


        //public static MetadataTableinfo ToDataModel(ModelMetadataViewModel vm)
        //{
        //    MetadataTableinfo model = new MetadataTableinfo();
        //    model.Columns = new List<MetadataFieldinfo>();
        //    foreach (MetadataFieldViewModel c1 in vm.Columns)
        //    {
        //        model.Columns.Add(c1.DataModel);
        //    }
        //    return model;
        //}

        //public ModelMetadataViewModel ToViewModel(MetadataTableinfo data)
        //{
        //    ModelMetadataViewModel vm = new ModelMetadataViewModel();
        //    vm.
        //}
    }
}
