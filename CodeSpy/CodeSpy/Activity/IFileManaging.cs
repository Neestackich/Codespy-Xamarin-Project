using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeSpy
{
    public interface IFileManaging
    {
        Task<bool> ExistAsync(string fileName);
        Task SaveTextAsync(string fileName, string text);
        Task<string> LoadTextAsync(string fileName);
        Task<IEnumerable<string>> GetFilesAsync();
        Task DeleteAsync(string fileName);
    }
}
