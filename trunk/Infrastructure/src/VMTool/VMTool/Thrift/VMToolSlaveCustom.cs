using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Protocol;
using Thrift;

namespace VMTool.Thrift
{
    public class VMToolSlaveCustom : VMToolSlave
    {
        private const TMessageType StreamMessageType = (TMessageType)5;

        new public class Client : VMToolSlave.Client
        {
            public Client(TProtocol prot)
                : this(prot, prot)
            {
            }

            public Client(TProtocol iprot, TProtocol oprot)
                : base(iprot, oprot)
            {
            }

            public ExecuteResponse Execute(ExecuteRequest request, Action<ExecuteStream> handleStream)
            {
                send_Execute(request);

                for (; ; )
                {
                    TMessage msg = iprot_.ReadMessageBegin();
                    if (msg.Type == TMessageType.Exception)
                    {
                        TApplicationException x = TApplicationException.Read(iprot_);
                        iprot_.ReadMessageEnd();
                        throw x;
                    }

                    if (msg.Type == StreamMessageType)
                    {
                        ExecuteStream stream = new ExecuteStream();
                        stream.Read(iprot_);
                        iprot_.ReadMessageEnd();
                        handleStream(stream);
                        continue;
                    }

                    Execute_result result = new Execute_result();
                    result.Read(iprot_);
                    iprot_.ReadMessageEnd();
                    if (result.__isset.success)
                    {
                        return result.Success;
                    }
                    if (result.__isset.ex)
                    {
                        throw result.Ex;
                    }
                    throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "Execute failed: unknown result");
                }
            }
        }

        [ThreadStatic]
        private static TProtocol CurrentOutputProtocol;

        [ThreadStatic]
        private static int CurrentSeqId;

        new public class Processor : VMToolSlave.Processor
        {
            public Processor(Handler iface)
                : base(iface)
            {
                processMap_["Execute"] = Execute_Process;
            }

            new public void Execute_Process(int seqid, TProtocol iprot, TProtocol oprot)
            {
                try
                {
                    CurrentOutputProtocol = oprot;
                    CurrentSeqId = seqid;
                    base.Execute_Process(seqid, iprot, oprot);
                }
                finally
                {
                    CurrentOutputProtocol = null;
                }
            }
        }

        public abstract class Handler : Iface
        {
            ExecuteResponse Iface.Execute(ExecuteRequest request)
            {
                TProtocol oprot = CurrentOutputProtocol;
                int seqId = CurrentSeqId;

                return Execute(request, stream =>
                {
                    oprot.WriteMessageBegin(new TMessage("Execute", StreamMessageType, seqId));
                    stream.Write(oprot);
                    oprot.WriteMessageEnd();
                });
            }

            public abstract ExecuteResponse Execute(ExecuteRequest request, Action<ExecuteStream> sendStream);
            public abstract ReadFileResponse ReadFile(ReadFileRequest request);
            public abstract WriteFileResponse WriteFile(WriteFileRequest request);
            public abstract CreateDirectoryResponse CreateDirectory(CreateDirectoryRequest request);
            public abstract EnumerateResponse Enumerate(EnumerateRequest request);
        }
    }
}
