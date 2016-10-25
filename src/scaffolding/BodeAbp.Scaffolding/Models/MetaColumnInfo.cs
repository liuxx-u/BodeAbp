using EnvDTE;
using System;
using System.Linq;
using BodeAbp.Scaffolding.Utils;
using System.Collections.Generic;

namespace BodeAbp.Scaffolding.Models
{
    [Serializable]
    public class MetaColumnInfo
    {
        public string ShortTypeName { get; set; }
        public string strDataType { get; set; }
        public euColumnType DataType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Nullable { get; set; }
        public bool Required { get; set; }
        public int? MaxLength { get; set; }
        public int? RangeMin { get; set; }
        public int? RangeMax { get; set; }

        public bool IsDtoVisible { get; set; }

        public string strControlType
        {
            get
            {
                switch (this.DataType)
                {
                    case euColumnType.stringCT:
                        return "text";
                    case euColumnType.longCT:
                    case euColumnType.intCT:
                        return "number";
                    case euColumnType.decimalCT:
                    case euColumnType.floatCT:
                    case euColumnType.doubleCT:
                        return "decimal";
                    case euColumnType.datetimeCT:
                        return "datepicker";
                    case euColumnType.boolCT:
                        return "switch";
                    case euColumnType.RelatedModel:
                        return "dropdown";
                    default:
                        return "text";
                }
            }
        }
        
        public bool HasMetaAttribute
        {
            get
            {
                return (MetaAttribute != string.Empty);
            }
        }

        public string MetaAttribute
        {
            get
            {
                switch (this.DataType)
                {
                    case euColumnType.stringCT:
                        if (this.MaxLength.HasValue)
                            return string.Format("[MaxLength({0})]", this.MaxLength);
                        else
                            break;
                    case euColumnType.longCT:
                    case euColumnType.intCT:
                    case euColumnType.decimalCT:
                    case euColumnType.floatCT:
                    case euColumnType.doubleCT:
                        //if (this.RangeMin > 0 || this.RangeMax > 0)
                        if (this.RangeMin.HasValue || this.RangeMax.HasValue)
                            return string.Format("[Range({0}, {1})]", this.RangeMin, this.RangeMax);
                        else
                            break;
                    default:
                        break;
                }
                return string.Empty;
            }
        }


        public MetaColumnInfo(CodeProperty property)
        {
            string strName = property.Name;
            string strType = property.Type.AsString;
            string strDisplayName = VmUtils.getCName(property);
            this.Name = property.Name;
            this.ShortTypeName = property.Type.AsString;
            this.strDataType = strType.Split('.').Last();
            this.DataType = GetColumnType(strType);
            DisplayName = strDisplayName ?? this.Name;
            Nullable = true;
            Required = false;
            setPropWithAttributes(property);
            if (strDataType.ToLower() == "int" || strDataType.ToLower() == "guid")
            {
                Nullable = false;
            }

            IsDtoVisible = IsDtoVisibleMember();
        }

        private bool IsDtoVisibleMember()
        {
            if (DataType == euColumnType.RelatedModel) return false;
            if (VmUtils.DtoUnSelectFields.Contains(Name)) return false;
            return true;
        }

        private void setPropWithAttributes(CodeProperty property)
        {
            foreach (var ele in property.Attributes)
            {
                var prop = ele as CodeAttribute;
                if (prop.Name == "Required")
                {
                    this.Required = true;
                    this.Nullable = false;
                }
                if (prop.Name == "MaxLength")
                {
                    int v = 0;
                    int.TryParse(prop.Value, out v);
                    this.MaxLength = v;
                }
                if (prop.Name == "Range")
                {
                    var arr = prop.Value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int n1 = 0, n2 = 0;
                    int.TryParse(arr[0], out n1);
                    int.TryParse(arr[1], out n2);
                    this.RangeMin = n1;
                    this.RangeMax = n2;
                }
            }
        }

        public euColumnType GetColumnType(string shortTypeName)
        {
            return ParseEnum(shortTypeName);
        }

        private static euColumnType ParseEnum(string value)
        {
            value = value.Replace("?", "").Replace("System.", "").ToLower() + "CT";
            euColumnType result;
            if (Enum.TryParse<euColumnType>(value, true, out result))
            {
                return result;
            }
            else
                return euColumnType.RelatedModel;
        }
    }
}
