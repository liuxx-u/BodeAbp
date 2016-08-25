using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.AutoMapper;
using BodeAbp.Activity.Interactions.Domain;
using BodeAbp.Activity.Interactions.Dtos;

namespace BodeAbp.Activity.Interactions
{
    /// <summary>
    /// 活动互动服务
    /// </summary>
    public class InteractionsAppService : ApplicationService, IInteractionsAppService
    {
        /// <summary>
        /// 领域服务
        /// </summary>
        public InteractionManager interactionManager { get; set; }

        /// <inheritdoc/>
        [AbpAuthorize]
        public async Task Comment(CreateCommentInput input)
        {
            var comment = input.MapTo<Comment>();
            await interactionManager.CreateCommentAsync(comment);
        }

        /// <inheritdoc/>
        public Task GetComments(long activityId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task GetReplies(long commentId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        [AbpAuthorize]
        public async Task Reply(CreateReplyInput input)
        {
            var reply = input.MapTo<Reply>();
            await interactionManager.ReplyCommentAsync(reply);
        }

        /// <inheritdoc/>
        [AbpAuthorize]
        public async Task TopOrStep(long commentId,bool isTop)
        {
            await interactionManager.TopOrStepComment(AbpSession.UserId.Value, commentId, isTop);
        }
    }
}
