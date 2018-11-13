using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

//using Happy.ExtentionMethods;
//using Happy.Utils.IO;

namespace Happy.Web.Mvc.FileUpload
{
    /// <summary>
    /// 文件上传器。
    /// </summary>
    public sealed class FileUploader
    {
        private readonly ISavedFileNameProvider _fileNameProvider;
        private long _maximalFileSize = long.MaxValue;
        private string _allowedFileExtensionReg = @"\..*";
        private readonly List<IFileUploadPreprocessor> _preprocessors = new List<IFileUploadPreprocessor>();

        /// <summary>
        /// 构造方法。
        /// </summary>
        public FileUploader(ISavedFileNameProvider fileNameProvider)
        {
            //fileNameProvider.MustNotNull("fileNameProvider");

            _fileNameProvider = fileNameProvider;
        }

        /// <summary>
        /// 最大的文件大小。
        /// </summary>
        public long MaximalFileSize
        {
            get { return _maximalFileSize; }
            set { _maximalFileSize = value; }
        }

        /// <summary>
        /// 运行的文件后缀名正则表达式。
        /// </summary>
        public string AllowedFileExtensionReg
        {
            get { return _allowedFileExtensionReg; }
            set { _allowedFileExtensionReg = value; }
        }

        /// <summary>
        /// 上传文件。
        /// </summary>
        public FileUploadResult Upload(HttpPostedFileBase file)
        {
            this.CheckFileExtension(file);
            this.CheckFileSize(file);

            var stream = file.InputStream;
            this.ExecutePreprocessors(file, ref stream);

            var filename = _fileNameProvider.GetSavedFileName(file);

            //StreamUtil.SaveAs(stream, filename);
            return new FileUploadResult(file.FileName, filename);
        }

        private void CheckFileExtension(HttpPostedFileBase file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (Regex.IsMatch(extension, this.AllowedFileExtensionReg, RegexOptions.IgnoreCase))
            {
                return;
            }

            //var error = string.Format(Resource.Messages.Error_UploadFileExtensionNotMatch, file.FileName, this.AllowedFileExtensionReg);
            //throw new FileUploadException(error);
        }

        private void CheckFileSize(HttpPostedFileBase file)
        {
            if (file.InputStream.Length <= this.MaximalFileSize)
            {
                return;
            }

            //var error = string.Format(Resource.Messages.Error_UploadFileSizeNotMatch, file.FileName, this.MaximalFileSize);
            //throw new FileUploadException(error);
        }

        private void ExecutePreprocessors(HttpPostedFileBase file, ref Stream stream)
        {
            foreach (var preprocessor in _preprocessors)
            {
                preprocessor.Process(file, ref stream);
            }
        }
    }
}
