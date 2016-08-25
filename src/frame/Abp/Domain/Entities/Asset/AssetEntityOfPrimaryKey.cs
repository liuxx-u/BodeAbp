using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Domain.Entities.Asset
{
    /// <summary>
    /// 资产实体
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    [Serializable]
    public abstract class AssetEntity<TPrimaryKey> : Entity<TPrimaryKey>, IAsset, IHasCreationTime
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssetEntity()
        {
            CreationTime = Clock.Now;
        }

        /// <summary>
        /// 资产路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 资产类型(默认为图片)
        /// </summary>
        public AssetType AssetType { get; set; }
        
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

    }
}
