using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Enum
{
    /// <summary>
    /// 课程文件类型
    /// </summary>
    public enum MicroClassFileType
    {
        /// <summary>
        /// 课程大图
        /// </summary>
        [Description("课程大图")] LargePic = 1,

        /// <summary>
        /// 课程小图
        /// </summary>
        [Description("课程小图")] ThumbnailPic,

        /// <summary>
        /// 二维码图片
        /// </summary>
        [Description("二维码图片")] QrCodePic,

        /// <summary>
        /// 音频文件
        /// </summary>
        [Description("音频文件")] AudioFile,

        /// <summary>
        /// 视频文件
        /// </summary>
        [Description("视频文件")] VideoFile
    }
}