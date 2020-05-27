using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Forms;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Dependency(typeof(CodeSpy.Droid.FileManaging))]
namespace CodeSpy.Droid
{
    public class FileManaging : IFileManaging
    {
        public Task<bool> ExistAsync(string fileName)
        {
            string filePath = GetFilePath(fileName);

            bool isExist = File.Exists(filePath);

            return Task<bool>.FromResult(isExist);
        }

        public async Task SaveTextAsync(string fileName, string text)
        {
            string filePath = GetFilePath(fileName);

            using (StreamWriter writer = File.CreateText(filePath))
            {
                await writer.WriteAsync(text);
            }
        }

        public async Task<string> LoadTextAsync(string fileName)
        {
            string filePath = GetFilePath(fileName);

            using (StreamReader reader = File.OpenText(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public Task<IEnumerable<string>> GetFilesAsync()
        {
            IEnumerable<string> filenames = 
                from filepath in Directory.EnumerateFiles(GetDocsPath())
                                            select Path.GetFileName(filepath);

            return Task<IEnumerable<string>>.FromResult(filenames);
        }

        public Task DeleteAsync(string fileName)
        {
            File.Delete(GetFilePath(fileName));

            return Task.FromResult(true);
        }

        string GetFilePath(string fileName)
        {
            return Path.Combine(GetDocsPath(), fileName);
        }

        string GetDocsPath()
        {
            return Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
        }
    }
}