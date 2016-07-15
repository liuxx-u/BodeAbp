using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Domain.Entities.Asset
{
    /// <summary>
    /// 资产信息基础接口.
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// 路径
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// 资产类型(默认为图片)
        /// </summary>
        AssetType AssetType { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        string Cover { get; set; }
    }
}
