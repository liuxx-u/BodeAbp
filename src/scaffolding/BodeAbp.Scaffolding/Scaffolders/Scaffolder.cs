using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using EnvDTE;
using BodeAbp.Scaffolding.UI;
using BodeAbp.Scaffolding.Utils;
using Microsoft.AspNet.Scaffolding;

namespace BodeAbp.Scaffolding.Scaffolders
{
    // 此类包含基架生成的所有步骤:
    // 1) ShowUIAndValidate() - 显示一个Visual Studio的对话框用于设置生成参数
    // 2) Validate() - 确认提取的Model   validates the model collected from the dialog
    // 3) GenerateCode() - 根据模板生成代码文件 if all goes well, generates the scaffolding output from the templates
    public class Scaffolder : CodeGenerator
    {

        private GeneratorModuleViewModel _moduleViewModel;

        internal Scaffolder(CodeGenerationContext context, CodeGeneratorInformation information)
            : base(context, information)
        {

        }

        public override bool ShowUIAndValidate()
        {
            _moduleViewModel = new GeneratorModuleViewModel(Context);

            GeneratorModuleDialog window = new GeneratorModuleDialog(_moduleViewModel);
            bool? isOk = window.ShowModal();

            File.WriteAllText(@"d:/a.txt", _moduleViewModel.ModelTypeCollection.Count().ToString());

            if (isOk == true)
            {
                Validate();
            }
            return (isOk == true);
        }


        // Validates the model returned by the Visual Studio dialog.
        // We always force a Visual Studio build so we have a model
        private void Validate()
        {
            CodeType modelType = _moduleViewModel.ModelType.CodeType;

            if (modelType == null)
            {
                throw new InvalidOperationException("请选择一个有效的实体类。");
            }

            var visualStudioUtils = new VisualStudioUtils();
            visualStudioUtils.BuildProject(Context.ActiveProject);


            Type reflectedModelType = GetReflectionType(modelType.FullName);
            if (reflectedModelType == null)
            {
                throw new InvalidOperationException("不能加载的实体类型。如果项目没有编译，请编译后重试。");
            }
        }

        public override void GenerateCode()
        {
            if (_moduleViewModel == null)
            {
                throw new InvalidOperationException("需要先调用ShowUIAndValidate方法。");
            }

            Cursor currentCursor = Mouse.OverrideCursor;
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                generateCode();
            }
            finally
            {
                Mouse.OverrideCursor = currentCursor;
            }
        }


        private void generateCode()
        {
            var project = Context.ActiveProject;
            var entity = _moduleViewModel.ModelType.CodeType;
            var entityName = entity.Name;
            var entityNamespace = entity.Namespace.FullName;
            var moduleNamespace = getModuleNamespace(entityNamespace);
            var moduleName = getModuleName(moduleNamespace);
            var assemblySuffix = getAssemblySuffix(moduleNamespace);
            var functionName = _moduleViewModel.FunctionName;


            Dictionary<string, object> templateParams = new Dictionary<string, object>()
            {
                {"EntityNamespace", entityNamespace}
                ,
                {"ModuleNamespace", moduleNamespace}
                ,
                {"ModuleName", moduleName}
                ,
                {"AssemblySuffix", assemblySuffix}
                ,
                {"EntityName", entityName}
                ,
                {"FunctionName", functionName}
                ,
                {"DtoMetaTable", _moduleViewModel.DtoClassMetadataViewModel.DataModel}
            };

            var templates = new[]
            {
                @"{ModuleName}\Dtos\{Entity}Dto"
                , @"{ModuleName}\I{ModuleName}AppService"
                , @"{ModuleName}\{ModuleName}AppService"
                //, @"{ModuleName}\Views\{entity}s"
                , @"{ModuleName}\Views\{Entity}List"
                , @"{ModuleName}\ModelConfigs\{Entity}Configuration"
                , @"{ModuleName}\{ModuleName}Constants"
            };

            foreach (var template in templates)
            {
                string outputPath = Path.Combine(@"_Code\" + 
                    template.Replace("{Entity}", entityName)
                    .Replace("{ModuleName}", moduleName)
                    .Replace("{entity}",entityName.ToCamelCase()));
                try
                {
                    AddFileFromTemplate(project, outputPath, template, templateParams, false);
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }

        private string getModuleNamespace(string entityNamespace)
        {
            var list = entityNamespace.Split('.').ToList();
            list.RemoveAt(list.Count - 1);
            return string.Join(".", list);
        }

        private string getModuleName(string moduleNamespace)
        {
            return moduleNamespace.Split('.').Last();
        }

        private string getAssemblySuffix(string moduleNamespace)
        {
            var arr = moduleNamespace.Split('.');
            if (arr.Length > 2)
            {
                return arr[arr.Length - 2];
            }
            else
            {
                return arr.Last();
            }
        }


        #region function library


        // Called to ensure that the project was compiled successfully
        private Type GetReflectionType(string typeName)
        {
            return GetService<IReflectedTypesService>().GetType(Context.ActiveProject, typeName);
        }

        private TService GetService<TService>() where TService : class
        {
            return (TService)ServiceProvider.GetService(typeof(TService));
        }


        // Returns the relative path of the folder selected in Visual Studio or an empty 
        // string if no folder is selected.
        protected string GetSelectionRelativePath()
        {
            return Context.ActiveProjectItem == null ? String.Empty : ProjectItemUtils.GetProjectRelativePath(Context.ActiveProjectItem);
        }

        #endregion


    }
}
