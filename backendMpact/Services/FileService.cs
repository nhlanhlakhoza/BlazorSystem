public class FileService
{
    public async Task<string> SaveFileToDesktop(IFormFile file, int taskId, bool isPdf = false)
    {
        if (file == null)
            return string.Empty;

        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string baseFolder = Path.Combine(desktop, "MpactStorage");

        if (!Directory.Exists(baseFolder))
            Directory.CreateDirectory(baseFolder);

        string folder = isPdf
            ? Path.Combine(baseFolder, "TaskPDFs")
            : Path.Combine(baseFolder, "TaskImages");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string originalName = Path.GetFileNameWithoutExtension(file.FileName);
        string extension = Path.GetExtension(file.FileName);

        string finalName = $"{originalName}_task{taskId}{extension}";
        string filePath = Path.Combine(folder, finalName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }
}
