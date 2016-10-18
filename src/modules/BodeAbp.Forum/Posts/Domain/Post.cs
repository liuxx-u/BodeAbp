using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Forum.Posts.Domain
{
    public class Post : FullAuditedEntity<long>
    {
        #region 常量



        #endregion

        #region 属性

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否关闭
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否精华
        /// </summary>
        public bool IsEssence { get; set; }

        /// <summary>
        /// 状态，根据不同需求约定
        /// </summary>
        public int Status { get; set; }

        #endregion
    }
}
