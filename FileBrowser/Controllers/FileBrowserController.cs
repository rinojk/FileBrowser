using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FileBrowser.Infrastructure;

namespace FileBrowser.Controllers
{
    public class FileBrowserController : ApiController
    {
         
        [HttpGet]
        public IEnumerable<string> AllDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            IEnumerable<string> result = allDrives.Select(p => p.Name);
            return result;
        }

        [HttpGet]
        public Answer GetFolder(string folderPath)
        {
            Answer result = new Answer();
            result.Folders = Directory.EnumerateDirectories(folderPath).Select(p=>p.Remove(0, p.LastIndexOf(@"\", StringComparison.Ordinal)+1)); //Getting only list of folder names
            result.Files = Directory.EnumerateFiles(folderPath).Select(p => p.Remove(0, p.LastIndexOf(@"\", StringComparison.Ordinal) + 1)); //Getting only list of file names
            result.CurrentFolder = folderPath;
            if(Directory.GetParent(folderPath)!=null)
                result.ParentFolder = Directory.GetParent(folderPath).FullName;
            return result;
        }

        [HttpGet]
        public object GetFilesCount(string FolderPath)
        {
            int[] result = new int[3]; //Result array of numbers. [0] - files count less 10mb; [1] - files count from 10 to 50mb; [2] - files count more 100mb
            CountFiles(ref result, FolderPath);
            return new
            {
                FileLess10MbCount = result[0],
                File10To50MbCount = result[1],
                FileMore100MbCount = result[2]
            };
        }

        private void CountFiles(ref int[] answer, string folder)
        {
            IEnumerable<string> folders;
            try //Checking for an System.UnathorizedException
            {
                folders = Directory.EnumerateDirectories(folder);
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(folder))
            {
                if (file.Length >= 260) continue; //Fix for System.IO.PathTooLongException. Skip file if full path > 260 symbols.
                double fileSize = (new FileInfo(file)).Length / 1048576; //Counting size of a file in Mb
                if (fileSize <= 10.0) answer[0]++;
                else if (fileSize > 10.0 && fileSize <= 50.0) answer[1]++;
                else if (fileSize >= 100.0) answer[2]++;
            }
            foreach (var currentFolder in folders)
            {
                CountFiles(ref answer, currentFolder);
            }
        }
    }
}
