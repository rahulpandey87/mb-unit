using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMTool.Thrift;
using System.Diagnostics;
using System.IO;
using log4net;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using VMTool.Core;

namespace VMTool.Slave
{
    public class SlaveServiceHandler : VMToolSlaveCustom.Handler
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(SlaveServiceHandler));

        public override ExecuteResponse Execute(ExecuteRequest request, Action<ExecuteStream> stream)
        {
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Execute:\n  Executable: {0}", request.Executable);
            if (request.__isset.arguments)
                message.AppendFormat("\n  Arguments: {0}", request.Arguments);
            if (request.__isset.timeout)
                message.AppendFormat("\n  Timeout: {0} seconds", request.Timeout);
            if (request.__isset.workingDirectory)
                message.AppendFormat("\n  WorkingDirectory: {0}", request.WorkingDirectory);
            if (request.__isset.environmentVariables)
            {
                message.Append("\n  EnvironmentVariables:");
                foreach (KeyValuePair<string, string> entry in request.EnvironmentVariables)
                    message.AppendFormat("\n    {0}={1}", entry.Key, entry.Value);
            }
            log.Info(message.ToString());

            int exitCode;
            try
            {
                exitCode = ProcessUtil.Execute(request.Executable,
                    request.__isset.arguments ? request.Arguments : "",
                    request.__isset.workingDirectory ? request.WorkingDirectory : null,
                    request.__isset.environmentVariables ? request.EnvironmentVariables : null,
                    line => stream(new ExecuteStream() { StdoutLine = line }),
                    line => stream(new ExecuteStream() { StderrLine = line }),
                    request.__isset.timeout ? TimeSpan.FromSeconds(request.Timeout) : (TimeSpan?) null);
            }
            catch (Exception ex)
            {
                throw OperationFailed(string.Format("Failed to execute process '{0}'.", request.Executable), ex.Message);
            }

            return new ExecuteResponse() { ExitCode = exitCode };
        }

        public override ReadFileResponse ReadFile(ReadFileRequest request)
        {
            log.InfoFormat("ReadFile:\n  Path: {0}", request.Path);

            byte[] contents;
            try
            {
                contents = File.ReadAllBytes(request.Path);
            }
            catch (Exception ex)
            {
                throw OperationFailed(string.Format("Could not read file '{0}'.", request.Path), ex.Message);
            }

            return new ReadFileResponse() { Contents = contents };
        }

        public override WriteFileResponse WriteFile(WriteFileRequest request)
        {
            log.InfoFormat("WriteFile:\n  Path: {0}\n  Overwrite: {1}", request.Path, request.Overwrite);

            try
            {
                if (! request.Overwrite && File.Exists(request.Path))
                    throw OperationFailed(string.Format("File already exists and did not request to overwrite '{0}'.", request.Path), null);

                File.WriteAllBytes(request.Path, request.Contents);
            }
            catch (OperationFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw OperationFailed(string.Format("Could not read file '{0}'.", request.Path), ex.Message);
            }

            return new WriteFileResponse();
        }

        public override CreateDirectoryResponse CreateDirectory(CreateDirectoryRequest request)
        {
            log.InfoFormat("CreateDirectory:\n  Path: {0}", request.Path);

            try
            {
                if (!Directory.Exists(request.Path))
                    Directory.CreateDirectory(request.Path);
            }
            catch (Exception ex)
            {
                throw OperationFailed(string.Format("Could not read file '{0}'.", request.Path), ex.Message);
            }

            return new CreateDirectoryResponse();
        }

        public override EnumerateResponse Enumerate(EnumerateRequest request)
        {
            log.InfoFormat("Enumerate:\n  Path: {0}\n  Recursive: {1}", request.PathGlob, request.Recursive);

            EnumerateResponse response = new EnumerateResponse();
            response.Items = new List<EnumerateItem>();

            FileUtil.TraverseFiles(request.PathGlob, request.Recursive,
                (file, relativePath) => response.Items.Add(new EnumerateItem()
                {
                    Kind = EnumerateItemKind.FILE,
                    RelativePath = relativePath,
                    FullPath = file.FullName
                }),
                (directory, relativePath) => response.Items.Add(new EnumerateItem()
                {
                    Kind = EnumerateItemKind.DIRECTORY,
                    RelativePath = relativePath,
                    FullPath = directory.FullName
                }));

            return response;
        }

        private static OperationFailedException OperationFailed(string why, string details)
        {
            log.ErrorFormat("Operation failed: {0}", why);

            var ex = new OperationFailedException() { Why = why };
            if (details != null)
                ex.Details = details;
            return ex;
        }
    }
}
