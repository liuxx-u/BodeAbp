using Abp.Dependency;
using System.Collections.Generic;

namespace BodeAbp.Plugins.Core
{
    public interface IPush : ITransientDependency
    {
        /// <summary>
        /// 推送全部用户
        /// </summary>
        /// <param name="tiker">通知栏提示文字</param>
        /// <param name="title">通知标题</param>
        /// <param name="text">通知文字描述</param>
        /// <param name="objs">自定义推送内容</param>
        /// <returns>是否推送成功</returns>
        bool PushAll(string tiker, string title, string text, Dictionary<string, object> objs);

        /// <summary>
        /// 推送指定用户
        /// </summary>
        /// <param name="pushKey">推送键</param>
        /// <param name="tiker">通知栏提示文字</param>
        /// <param name="title">通知标题</param>
        /// <param name="text">通知文字描述</param>
        /// <param name="objs">自定义推送内容</param>
        /// <returns>是否推送成功</returns>
        bool PushByKey(string pushKey, string tiker, string title, string text, Dictionary<string, object> objs);

        /// <summary>
        /// 推送指定用户
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="tiker">通知栏提示文字</param>
        /// <param name="title">通知标题</param>
        /// <param name="text">通知文字描述</param>
        /// <param name="objs">自定义推送内容</param>
        /// <returns>是否推送成功</returns>
        bool PushByAlias(List<string> alias, string tiker, string title, string text, Dictionary<string, object> objs);

        /// <summary>
        /// 推送指定用户
        /// </summary>
        /// <param name="tag">标签</param>
        /// <param name="tiker">通知栏提示文字</param>
        /// <param name="title">通知标题</param>
        /// <param name="text">通知文字描述</param>
        /// <param name="objs">自定义推送内容</param>
        /// <returns>是否推送成功</returns>
        bool PushByTagWithinAnd(string tag, string tiker, string title, string text, Dictionary<string, object> objs);

        /// <summary>
        /// 推送指定用户
        /// </summary>
        /// <param name="tag">标签</param>
        /// <param name="tiker">通知栏提示文字</param>
        /// <param name="title">通知标题</param>
        /// <param name="text">通知文字描述</param>
        /// <param name="objs">自定义推送内容</param>
        /// <returns>是否推送成功</returns>
        bool PushByTagWithinOr(string tag, string tiker, string title, string text, Dictionary<string, object> objs);
    }
}
