using System.Drawing;
using System.Drawing.Imaging;
using System;

namespace Abp.Helper
{
    /// <summary>  
    /// 图片处理类  
    /// </summary>  
    public static class ImageHelper
    {
        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>缩略图</returns>
        public static Image GetThumbnail(string path, int width, int height)
        {
            Image image = Image.FromFile(path);//获取原图
            if (image == null || width < 1 || height < 1)
                return null;

            // 新建一个bmp图片
            Image img = new Bitmap(width, height);

            // 新建一个画板
            using (Graphics g = Graphics.FromImage(img))
            {
                // 设置高质量插值法
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 设置高质量,低速度呈现平滑程度
                //g.SmoothingMode = SmoothingMode.HighQuality;

                // 高质量、低速度复合
                //g.CompositingQuality = CompositingQuality.HighQuality;
                // 清空画布并以透明背景色填充
                g.Clear(Color.Transparent);

                // 在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(image, new Rectangle(0, 0, width, height));
                return img;
            }
        }

        /// <summary>
        /// 按比例获取缩略图
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="percent">比例，基数100</param>
        /// <returns>缩略图</returns>
        public static Image GetThumbnail(string path, int percent)
        {
            Image image = Image.FromFile(path);//获取原图
            if (image == null || percent<=0)
                return null;

            Size imageSize = GetImageSize(image, percent);
            return GetThumbnail(path, imageSize.Width, imageSize.Height);
        }
        
        /// <summary>
        /// 生成缩略图，并保持纵横比
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>生成缩略图后对象</returns>
        public static Image GetThumbnailKeepRatio(string path, int width, int height)
        {
            Image image = Image.FromFile(path);
            if (image == null) return null;
            Size imageSize = GetImageSize(image, width, height);
            return GetThumbnail(path, imageSize.Width, imageSize.Height);
        }

        /// <summary>
        /// 根据百分比获取图片的尺寸
        /// </summary>
        /// <param name="picture">图片</param>
        /// <param name="percent">比例，基数100</param>
        /// <returns>图片尺寸</returns>
        public static Size GetImageSize(Image picture, int percent)
        {
            if (picture == null || percent < 1)
                return Size.Empty;

            int width = picture.Width * percent / 100;
            int height = picture.Height * percent / 100;

            return GetImageSize(picture, width, height);
        }

        /// <summary>
        /// 根据设定的大小返回图片的大小，考虑图片长宽的比例问题
        /// </summary>
        /// <param name="picture">图片</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>图片尺寸</returns>
        public static Size GetImageSize(Image picture, int width, int height)
        {
            if (picture == null || width < 1 || height < 1)
                return Size.Empty;
            Size imageSize;
            imageSize = new Size(width, height);
            double heightRatio = (double)picture.Height / picture.Width;
            double widthRatio = (double)picture.Width / picture.Height;
            int desiredHeight = imageSize.Height;
            int desiredWidth = imageSize.Width;
            imageSize.Height = desiredHeight;
            if (widthRatio > 0)
                imageSize.Width = Convert.ToInt32(imageSize.Height * widthRatio);
            if (imageSize.Width > desiredWidth)
            {
                imageSize.Width = desiredWidth;
                imageSize.Height = Convert.ToInt32(imageSize.Width * heightRatio);
            }
            return imageSize;
        }

        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        public static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /// <summary>
        /// xx
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo icf in encoders)
            {
                if (icf.FormatID == format.Guid)
                {
                    return icf;
                }
            }
            return null;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="savePath">保存地址</param>
        /// <param name="format"></param>
        public static void SaveImage(Image image, string savePath, ImageFormat format)
        {
            SaveImage(image, savePath, GetImageCodecInfo(format));
        }

        /// <summary>
        /// 高质量保存图片
        /// </summary>
        private static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            // 设置 原图片 对象的 EncoderParameters 对象
            EncoderParameters parms = new EncoderParameters(1);
            EncoderParameter parm = new EncoderParameter(Encoder.Quality, ((long)95));
            parms.Param[0] = parm;
            image.Save(savePath, ici, parms);
            parms.Dispose();
        }
        
    }
}