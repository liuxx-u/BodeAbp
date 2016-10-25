using Microsoft.AspNet.Scaffolding;
using System;
using System.ComponentModel.Composition;
using System.Runtime.Versioning;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using BodeAbp.Scaffolding.Scaffolders;

namespace BodeAbp.Scaffolding
{

    [Export(typeof(CodeGeneratorFactory))]
    public class ScaffolderFactory : CodeGeneratorFactory
    {
        public ScaffolderFactory()
            : base(CreateCodeGeneratorInformation())
        {

        }

        public override ICodeGenerator CreateInstance(CodeGenerationContext context)
        {
            return new Scaffolder(context, Information);
        }

        // We support CSharp WAPs targetting at least .Net Framework 4.5 or above.
        // We DON'T currently support VB
        public override bool IsSupported(CodeGenerationContext codeGenerationContext)
        {
            if (ProjectLanguage.CSharp.Equals(codeGenerationContext.ActiveProject.GetCodeLanguage()))
            {
                FrameworkName targetFramework = codeGenerationContext.ActiveProject.GetTargetFramework();
                return (targetFramework != null) &&
                        String.Equals(".NetFramework", targetFramework.Identifier, StringComparison.OrdinalIgnoreCase) &&
                        targetFramework.Version >= new Version(4, 5);
            }

            return false;
        }

        private static CodeGeneratorInformation CreateCodeGeneratorInformation()
        {
            return new CodeGeneratorInformation(
                displayName: "[BodeAbp.Scaffolding]项目文件",
                description: "通过实体类，生成相应模块的代码",
                author: "Liuxx",
                version: new Version(0, 1, 0, 0),
                id: "BodeAbp_Scaffolding",
                icon: ToImageSource(Resources.Application),
                gestures: new[] { "BodeAbp" },
                categories: new[] { "BodeAbp", Categories.Common, Categories.Other }
            );
        }

        /// <summary>
        /// Helper method to convert Icon to Imagesource.
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <returns>Imagesource</returns>
        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
