using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.UI;
using Abp.WebApi.Upload;

namespace WebDemo.Api.Controllers
{
    public class FileController : ApiController
    {
        [HttpPost]
        public async Task<string> UploadPic()
        {
            // 检查是否是 multipart/form-data
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new UserFriendlyException("请求数据格式不正确");
            }
            var paths = await ApiUploadHelper.Upload(Request.Content);
            if (!paths.Any())
            {
                throw new UserFriendlyException("上传失败");
            }
            return paths.First();
        }

        [HttpPost]
        public async Task<List<string>> UploadPics()
        {
            // 检查是否是 multipart/form-data
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new UserFriendlyException("请求数据格式不正确");
            }
            var paths = await ApiUploadHelper.Upload(Request.Content);
            if (!paths.Any())
            {
                throw new UserFriendlyException("上传失败");
            }
            return paths;
        }
    }
}
