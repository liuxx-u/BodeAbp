using Abp.Domain.Entities;

namespace BodeAbp.Zero.Files.Domain
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfo : Entity<long>
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int Size { get; set; }
    }
}
