namespace FinalProject.Helpers.Extentions
{
    public static class FileExtension
    {
        public static bool CheckFileTpe(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }

        public static bool CheckFileSize(this IFormFile file, long size)
        {
            return file.Length / 1024 < size;
        }

        public static string GenerateFilePath(this IWebHostEnvironment env, string folder, string fileName)
        {
            return Path.Combine(env.WebRootPath, folder, fileName);
        }


        public static string GenereteFileNmae(this IFormFile file)
        {
            return Guid.NewGuid() + "-" + file.FileName;
        }
    }
}
