using System.Collections.Generic;

namespace WebDemo.Web.Areas.Admin.ViewModels
{
    public class NavigationViewModel
    {
        public NavigationViewModel()
        {
            Children = new List<NavigationViewModel>();
        }

        /// <summary>
        /// 菜单名称（唯一标识码）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 样式类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public ICollection<NavigationViewModel> Children { get; set; }
    }
}