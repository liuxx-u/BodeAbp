using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using EnvDTE80;
using System.Windows;

namespace BodeAbp.Scaffolding.UI
{
    internal class GeneratorModuleViewModel : ViewModel<GeneratorModuleViewModel>
    {
        private readonly CodeGenerationContext _context;

        public GeneratorModuleViewModel(CodeGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
        }

        #region tab事件

        private int CurrentStepIndex = 0;

        private ObservableCollection<Visibility> _StepVisibale
            = new ObservableCollection<Visibility>() { Visibility.Visible, Visibility.Collapsed};

        public ObservableCollection<Visibility> StepVisibale
        {
            get
            {
                return _StepVisibale;
            }
        }

        public void ShowStep()
        {
            for (int x = 0; x < StepVisibale.Count; x++)
            {
                StepVisibale[x] = (x == CurrentStepIndex ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        private DelegateCommand _NextStepCommand;
        public ICommand NextStepCommand
        {
            get
            {
                if (_NextStepCommand == null)
                {
                    _NextStepCommand = new DelegateCommand(_ =>
                    {
                        Validate(propertyName: null);
                        if (!HasErrors)
                        {
                            CurrentStepIndex += 1;
                            if (CurrentStepIndex == 1)
                            {
                                CreateMetadataViewModel();
                            }
                            ShowStep();
                        }
                    });
                }
                return _NextStepCommand;
            }
        }

        private DelegateCommand _BackStepCommand;
        public ICommand BackStepCommand
        {
            get
            {
                if (_BackStepCommand == null)
                {
                    _BackStepCommand = new DelegateCommand(_ =>
                    {
                        Validate(propertyName: null);
                        if (!HasErrors)
                        {
                            CurrentStepIndex -= 1;
                            ShowStep();
                        }
                    });
                }
                return _BackStepCommand;
            }
        }


        private DelegateCommand _okCommand;
        public ICommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new DelegateCommand(_ =>
                    {
                        Validate(propertyName: null);
                        if (!HasErrors)
                        {
                            OnClose(result: true);
                        }
                    });
                }
                return _okCommand;
            }
        }

        public event Action<bool> Close;

        private void OnClose(bool result)
        {
            if (Close != null)
            {
                Close(result);
            }
        }

        #endregion

        #region 属性
        
        private ModelType _modelType;
        public ModelType ModelType
        {
            get { return _modelType; }
            set
            {
                Validate();

                if (value == _modelType)
                {
                    return;
                }

                _modelType = value;

                OnPropertyChanged();

                if (!string.IsNullOrEmpty(_modelType.CName))
                {
                    FunctionName = _modelType.CName;
                    OnPropertyChanged(m => m.FunctionName);
                }
            }
        }
        
        private string _ModuleName = "";
        public string ModuleName
        {
            get { return _ModuleName; }
            set
            {
                if (value == _ModuleName)
                {
                    return;
                }
                _ModuleName = value;
                OnPropertyChanged();
            }
        }

        private string _functionName;
        public string FunctionName
        {
            get { return _functionName ?? ""; }
            set
            {
                if (value == _functionName)
                {
                    return;
                }
                _functionName = value;
                OnPropertyChanged();
            }
        }
        
        private MetadataSettingViewModel _dtoClassMetadataViewModel;
        public MetadataSettingViewModel DtoClassMetadataViewModel
        {
            get
            {
                return _dtoClassMetadataViewModel;
            }
            set
            {
                Validate();
                if (value == this._dtoClassMetadataViewModel) { return; }

                this._dtoClassMetadataViewModel = value;
                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<ModelType> _modelTypeCollection;
        public ObservableCollection<ModelType> ModelTypeCollection
        {
            get
            {
                if (_modelTypeCollection == null || !_modelTypeCollection.Any())
                {
                    ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
                    Project project = _context.ActiveProject;

                    var modelTypes = codeTypeService
                                        .GetAllCodeTypes(project)
                                        .Where(codeType => IsEntityClass(codeType))
                                        .OrderBy(x => x.Name)
                                        .Select(codeType => new ModelType(codeType));
                    _modelTypeCollection = new ObservableCollection<ModelType>(modelTypes);
                }
                return _modelTypeCollection;
            }
        }

        #endregion

        #region 私有方法
        private void CreateMetadataViewModel()
        {
            if (DtoClassMetadataViewModel == null || DtoClassMetadataViewModel.CodeType != ModelType.CodeType)
                DtoClassMetadataViewModel = new MetadataSettingViewModel(ModelType.CodeType);
        }

        private bool IsEntityClass(CodeType codeType)
        {
            CodeClass2 codeClass2 = codeType as CodeClass2;
            if (codeClass2 == null || codeClass2.IsAbstract) return false;

            if (codeType.Namespace == null) return false;
            if (!codeType.Namespace.FullName.EndsWith("Domain")
                && !codeType.Namespace.FullName.EndsWith("Core"))
            {
                return false;
            }

            Func<CodeType, bool> validateBaseStart = cType =>
            {
                var fullName = cType.FullName;
                return fullName.StartsWith("Abp.Domain.Entities.Entity")
                        || fullName.StartsWith("Abp.Domain.Entities.Auditing.AuditedEntity")
                        || fullName.StartsWith("Abp.Domain.Entities.Auditing.CreationAuditedEntity")
                        || fullName.StartsWith("Abp.Domain.Entities.Auditing.FullAuditedEntity")
                        || fullName.StartsWith("Abp.Domain.Entities.Asset.AssetEntity");
            };

            Func<CodeType, bool> validateInterfaceStart = cType =>
            {
                var fullName = cType.FullName;
                return fullName.StartsWith("Abp.Domain.Entities.IEntity")
                       || fullName.StartsWith("Abp.Domain.Entities.IPassivable")
                       || fullName.StartsWith("Abp.Domain.Entities.ISoftDelete")
                       || fullName.StartsWith("Abp.Domain.Entities.Auditing.IAudited")
                       || fullName.StartsWith("Abp.Domain.Entities.Auditing.ICreationAudited")
                       || fullName.StartsWith("Abp.Domain.Entities.Auditing.IModificationAudited")
                       || fullName.StartsWith("Abp.Domain.Entities.Auditing.IDeletionAudited")
                       || fullName.StartsWith("Abp.Domain.Entities.Auditing.IFullAudited")
                       || fullName.StartsWith("Abp.Domain.Entities.Auditing.IHasCreationTime")
                       || fullName.StartsWith("Abp.Domain.Entities.Auditing.IHasModificationTime")
                       || fullName.StartsWith("Abp.Domain.Entities.Auditing.IHasDeletionTime")
                       || fullName.StartsWith("Abp.Domain.Entities.Asset.IAsset");
            };

            return codeClass2.Bases.OfType<CodeType>().Any(p => validateBaseStart(p))
                || codeClass2.ImplementedInterfaces.OfType<CodeType>().Any(p => validateInterfaceStart(p));
        }

        private TService GetService<TService>() where TService : class
        {
            return (TService)_context.ServiceProvider.GetService(typeof(TService));
        }
        #endregion

        #region 重载

        protected override void Validate([CallerMemberName]string propertyName = "")
        {
            string currentPropertyName;

            // ModelType
            currentPropertyName = PropertyName(m => m.ModelType);
            if (ShouldValidate(propertyName, currentPropertyName))
            {
                ClearError(currentPropertyName);
                if (ModelType == null)
                {
                    AddError(currentPropertyName, "请从列表中选择一个实体类");
                }
            }

        }
        #endregion
        
    }
}
