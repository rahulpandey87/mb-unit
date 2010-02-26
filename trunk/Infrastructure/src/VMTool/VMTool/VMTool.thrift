# Type definitions for VMTool services.

namespace csharp VMTool.Thrift

# Exception
exception OperationFailedException
{
	1: required string why
	2: optional string details
}

# Start
struct StartRequest
{
	1: required string vm,
	2: optional string snapshot
}

struct StartResponse { }

# PowerOff
struct PowerOffRequest
{
	1: required string vm
}

struct PowerOffResponse { }

# Shutdown
struct ShutdownRequest
{
	1: required string vm
}

struct ShutdownResponse { }

# Pause
struct PauseRequest
{
	1: required string vm
}

struct PauseResponse { }

# Resume
struct ResumeRequest
{
	1: required string vm
}

struct ResumeResponse { }

# Take Snapshot
struct TakeSnapshotRequest
{
	1: required string vm,
	2: required string snapshotName
}

struct TakeSnapshotResponse { }

# Get IP
struct GetIPRequest
{
	1: required string vm
}

struct GetIPResponse
{
	1: required string ip
}

# ToolService
service VMToolService
{
	StartResponse Start(1: StartRequest request) throws(1: OperationFailedException ex),
	PowerOffResponse PowerOff(1: PowerOffRequest request) throws(1: OperationFailedException ex),
	ShutdownResponse Shutdown(1: ShutdownRequest request) throws(1: OperationFailedException ex),
	PauseResponse Pause(1: PauseRequest request) throws(1: OperationFailedException ex),
	ResumeResponse Resume(1: ResumeRequest request) throws(1: OperationFailedException ex),
	TakeSnapshotResponse TakeSnapshot(1: TakeSnapshotRequest request) throws(1: OperationFailedException ex),
	GetIPResponse GetIP(1: GetIPRequest request) throws(1: OperationFailedException ex),
}