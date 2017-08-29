namespace Abp.Owin.Authentication
{
    public class ClientInfo
    {
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 客户端Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 登陆成功后token通知地址
        /// </summary>
        public string LoginNotifyUrl { get; set; }

        /// <summary>
        /// 注销成功后通知地址
        /// </summary>
        public string LogoutNotifyUrl { get; set; }
    }
}
