﻿
using ETicaretAPI.Infrastructure.Operations;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService
    {
        //readonly IWebHostEnvironment _webHostEnvironment;
        //public FileService(IWebHostEnvironment webHostEnvironment)
        //{
        //    _webHostEnvironment = webHostEnvironment;
        //}

        //public async Task<bool> CopyFileAsync(string path, IFormFile file)
        //{
        //    try
        //    {
        //        await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

        //        await file.CopyToAsync(fileStream);
        //        await fileStream.FlushAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //todo log!
        //        throw ex;
        //    }
        //}

        

    //    public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
    //    {
    //        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
    //        if (!Directory.Exists(uploadPath))
    //            Directory.CreateDirectory(uploadPath);

    //        List<(string fileName, string path)> datas = new();
    //        List<bool> results = new();
    //        foreach (IFormFile file in files)
    //        {
    //            string fileNewName = await FileRenameAsync(uploadPath, file.FileName);

    //            bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
    //            datas.Add((fileNewName, $"{path}\\{fileNewName}"));
    //            results.Add(result);
    //        }

    //        if (results.TrueForAll(r => r.Equals(true)))
    //            return datas;

    //        return null;

    //        //todo Eğer ki yukarıdaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığına dair uyarıcı bir exception oluşturulup fırlatılması gerekyior!
    //    }


    }
} 
