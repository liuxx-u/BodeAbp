using System.Collections.ObjectModel;
using System.Linq;
using BodeAbp.Scaffolding.Models;
using System.Windows;
using EnvDTE;
using BodeAbp.Scaffolding.Utils;


namespace BodeAbp.Scaffolding.UI
{
    internal class MetadataSettingViewModel : ViewModel<MetadataSettingViewModel>
    {
        public MetadataSettingViewModel(CodeType codeType, bool IsHiddenFields = true)
        {
            MetaTableInfo dataModel = new MetaTableInfo();
            fillCodeTypeMembers(dataModel, codeType, IsHiddenFields);

            Init(codeType, dataModel);
        }

        private void fillCodeTypeMembers(MetaTableInfo dataModel, CodeType codeType, bool IsHiddenFields = true)
        {
            foreach (CodeElement ce in codeType.Members)
            {
                var prop = ce as CodeProperty;
                if (prop == null) continue;
                if (IsHiddenFields && VmUtils.DefaultHiddenFields.Contains(prop.Name)) continue; //默认不显示的字段
                if (dataModel.Columns.Any(d=>d.Name == prop.Name)) continue; //已存在
                if (prop.Type.AsFullName.StartsWith("System.Collections")) continue;//不显示集合类
                dataModel.Columns.Add(new MetaColumnInfo(prop));
            }

            foreach (CodeElement c in codeType.Bases)
            {
                if (c.IsCodeType)
                {
                    fillCodeTypeMembers(dataModel, (CodeType)c);
                }
            }

        }

        private void Init(CodeType codeType, MetaTableInfo dataModel)
        {
            this.CodeType = codeType;
            this.DataModel = dataModel;
            this.Columns = new ObservableCollection<MetadataFieldViewModel>();
            foreach (MetaColumnInfo f1 in dataModel.Columns)
            {
                this.Columns.Add(new MetadataFieldViewModel(f1));
            }
        }

        public MetadataFieldViewModel this[string name]
        {
            get { return this.Columns.FirstOrDefault(x => x.Name == name); }
        }

        public MetadataFieldViewModel this[int index]
        {
            get { return this.Columns[index]; }
        }

        public MetaTableInfo DataModel { get; private set; }
        public CodeType CodeType { get; private set; }

        private ObservableCollection<MetadataFieldViewModel> m_Columns = null;

        public ObservableCollection<MetadataFieldViewModel> Columns
        {
            get { return m_Columns; }
            private set { this.m_Columns = value; }
        }

    }

    internal class MetadataFieldViewModel : ViewModel<MetadataFieldViewModel>
    {
        public MetadataFieldViewModel(MetaColumnInfo data)
        {
            this.DataModel = data;
        }

        public MetaColumnInfo DataModel { get; private set; }

        public string Name
        {
            get { return DataModel.Name; }
            private set
            {
                DataModel.Name = value;
            }
        }

        public string DisplayName
        {
            get { return DataModel.DisplayName; }
            set
            {
                if (value == DataModel.DisplayName)
                {
                    return;
                }
                DataModel.DisplayName = value;
                OnPropertyChanged();
            }
        }

        public bool Required
        {
            get { return DataModel.Required; }
            set
            {
                if (value == DataModel.Required)
                {
                    return;
                }
                DataModel.Required = value;
                OnPropertyChanged();
            }
        }

        public bool IsDtoVisible
        {
            get { return DataModel.IsDtoVisible; }
            set
            {
                if (value == DataModel.IsDtoVisible)
                {
                    return;
                }
                DataModel.IsDtoVisible = value;
                OnPropertyChanged();
            }
        }

        public string strDataType
        {
            get { return DataModel.strDataType; }
        }

        public int? MaxLength
        {
            get { return DataModel.MaxLength; }
            set
            {
                if (value == DataModel.MaxLength)
                {
                    return;
                }
                DataModel.MaxLength = value;
                OnPropertyChanged();
            }
        }

        public int? RangeMin
        {
            get { return DataModel.RangeMin; }
            set
            {
                if (value == DataModel.RangeMin)
                {
                    return;
                }
                DataModel.RangeMin = value;
                OnPropertyChanged();
            }
        }

        public int? RangeMax
        {
            get { return DataModel.RangeMax; }
            set
            {
                if (value == DataModel.RangeMax)
                {
                    return;
                }
                DataModel.RangeMax = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowEditorMaxLength
        {
            get
            {
                if (DataModel.DataType == euColumnType.stringCT)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility ShowEditorRange
        {
            get
            {
                if (DataModel.DataType == euColumnType.intCT 
                    || DataModel.DataType == euColumnType.longCT
                    || DataModel.DataType == euColumnType.floatCT
                    || DataModel.DataType == euColumnType.decimalCT)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

    }
}
