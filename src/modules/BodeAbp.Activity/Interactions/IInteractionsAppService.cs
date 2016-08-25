using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using BodeAbp.Activity.Interactions.Dtos;

namespace BodeAbp.Activity.Interactions
{
    /// <summary>
    /// 活动互动服务
    /// </summary>
    [Description("活动互动接口")]
    public interface IInteractionsAppService : IApplicationService
    {
        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        Task Comment(CreateCommentInput input);

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="activityId">活动Id</param>
        /// <returns></returns>
        Task GetComments(long activityId);

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        Task Reply(CreateReplyInput input);

        /// <summary>
        /// 获取评论的回复
        /// </summary>
        /// <param name="commentId">评论Id</param>
        /// <returns></returns>
        Task GetReplies(long commentId);

        /// <summary>
        /// 顶/踩 评论
        /// </summary>
        /// <param name="commentId">评论Id</param>
        /// <param name="isTop">是否是顶</param>
        /// <returns></returns>
        Task TopOrStep(long commentId,bool isTop);
    }
}
