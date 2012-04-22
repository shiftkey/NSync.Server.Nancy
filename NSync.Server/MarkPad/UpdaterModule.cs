using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using NSync.Core;
using Nancy;
using Nancy.Responses;

namespace NSync.Server.MarkPad
{
    public class UpdaterModule : NancyModule
    {
        private const string RelativeFolder = "markpad";

        public UpdaterModule() : base(RelativeFolder)
        {
            Get["/releases"] = _ => GenerateFileContents(RelativeFolder);
            Get["/{file}"] = r => DownloadFile(RelativeFolder, r.file.ToString());
        }

        private static Response DownloadFile(string relativeFolder, string fileName)
        {
            //var root = HttpRuntime.AppDomainAppPath;
            var fullPath = Path.Combine(relativeFolder, fileName);

            //if (!File.Exists(fullPath))
            //    return new Response { StatusCode = HttpStatusCode.NotFound };

            return new GenericFileResponse(fullPath, "application/zip");
        }

        private static Response GenerateFileContents(string relativeFolder)
        {
            var root = HttpRuntime.AppDomainAppPath;
            var folder = Path.Combine(root, relativeFolder);
            var files = Directory.GetFiles(folder, "*.nupkg*");

            // TODO: compare versions and generate new diffs JIT

            return new Response { Contents = WriteResponseContents(files) };
        }

        private static Action<Stream> WriteResponseContents(IEnumerable<string> files)
        {
            return s =>
            {
                using (var writer = new StreamWriter(s))
                foreach (var f in files)
                {
                    var file = File.OpenRead(f);
                    var release = ReleaseEntry.GenerateFromFile(file, Path.GetFileName(f));
                    writer.WriteLine(release.EntryAsString);
                }
            };
        }
    }
}