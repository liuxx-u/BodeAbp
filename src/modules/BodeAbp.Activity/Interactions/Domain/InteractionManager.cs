using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.UI;

namespace BodeAbp.Activity.Interactions.Domain
{
    /// <summary>
    /// 活动互动 领域服务
    /// </summary>
    public class InteractionManager : IDomainService
    {
        /// <summary>
        /// 评论  仓储
        /// </summary>
        public IRepository<Comment, long> commentRepository { protected get; set; }

        /// <summary>
        /// 回复  仓储
        /// </summary>
        public IRepository<Reply, long> replyRepository { protected get; set; }

        /// <summary>
        /// 顶/踩  仓储
        /// </summary>
        public IRepository<TopOrStep, long> topOrStepRepository { protected get; set; }

        /// <summary>
        /// 活动  仓储
        /// </summary>
        public IRepository<Activities.Domain.Activity, long> activityRepository { protected get; set; }

        /// <summary>
        /// 创建评论
        /// </summary>
        /// <param name="comment">评论</param>
        /// <returns></returns>
        public async Task CreateCommentAsync(Comment comment)
        {
            var activity = await activityRepository.GetAsync(comment.ActivityId);
            if (activity == null)
            {
                throw new UserFriendlyException("评论的活动不存在");
            }
            await commentRepository.InsertAsync(comment);
            activity.CommentNo++;
            await activityRepository.UpdateAsync(activity);
        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <param name="reply">回复</param>
        /// <returns></returns>
        public async Task ReplyCommentAsync(Reply reply)
        {
            var comment = await commentRepository.GetAsync(reply.CommentId);
            if (comment == null)
            {
                throw new UserFriendlyException("回复的评论不存在");
            }
            await replyRepository.InsertAsync(reply);
            comment.ReplyNo++;
            await commentRepository.UpdateAsync(comment);
        }

        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="replyId">回复Id</param>
        /// <returns></returns>
        public async Task DeleteReplyAsync(long replyId)
        {
            var reply = await replyRepository.GetAsync(replyId);
            await DeleteReplyAsync(reply);
        }

        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="reply">回复</param>
        /// <returns></returns>
        public async Task DeleteReplyAsync(Reply reply)
        {
            reply.CheckNotNull("reply");
            var comment = await commentRepository.GetAsync(reply.CommentId);
            comment.ReplyNo--;

            await replyRepository.DeleteAsync(reply);
            await commentRepository.UpdateAsync(comment);
        }

        /// <summary>
        /// 顶 评论
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="commentId">评论Id</param>
        /// <param name="isTop">是否顶</param>
        /// <returns></returns>
        public async Task TopOrStepComment(long userId, long commentId, bool isTop = false)
        {
            var comment = await commentRepository.GetAsync(commentId);
            await TopOrStepComment(userId, comment);
        }

        /// <summary>
        /// 顶 评论
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="comment">评论</param>
        /// <param name="isTop">是否顶</param>
        /// <returns></returns>
        public async Task TopOrStepComment(long userId, Comment comment, bool isTop = false)
        {
            comment.CheckNotNull("comment");
            userId.CheckGreaterThan("userId", userId);
            if (topOrStepRepository.CheckExists(p => p.CommentId == comment.Id && p.CreatorUserId == userId))
            {
                throw new UserFriendlyException("重复的操作");
            }

            var top = new TopOrStep
            {
                CommentId = comment.Id,
                CreatorUserId = userId,
                IsTop = isTop
            };

            if (isTop)
            {
                comment.TopNo++;
            }
            else
            {
                comment.StepNo++;
            }

            await topOrStepRepository.InsertAsync(top);
            await commentRepository.UpdateAsync(comment);
        }

        /// <summary>
        /// 删除 顶/踩
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task DeleteTopOrStep(long id)
        {
            var topOrStep = await topOrStepRepository.GetAsync(id);
            var comment = await commentRepository.GetAsync(topOrStep.CommentId);
            if (topOrStep.IsTop)
            {
                comment.TopNo--;
            }
            else
            {
                comment.StepNo--;
            }

            await topOrStepRepository.DeleteAsync(topOrStep);
            await commentRepository.UpdateAsync(comment);
        }
    }
}
