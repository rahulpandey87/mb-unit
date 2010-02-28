namespace csharp VMTool.Thrift

# Exception
exception OperationFailedException
{
	1: required string why
	2: optional string details
}

### MASTER ###

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

# Save State
struct SaveStateRequest
{
	1: required string vm
}

struct SaveStateResponse { }

# Take Snapshot
struct TakeSnapshotRequest
{
	1: required string vm,
	2: required string snapshotName
}

struct TakeSnapshotResponse { }

# Get Status
enum Status
{
	UNKNOWN,
	OFF,
	RUNNING,
	PAUSED,
	SAVED
}

struct GetStatusRequest
{
	1: required string vm
}

struct GetStatusResponse
{
	1: required Status status
}

# Get IP
struct GetIPRequest
{
	1: required string vm
}

struct GetIPResponse
{
	1: required string ip
}

# Master Service
service VMToolMaster
{
	StartResponse Start(1: StartRequest request) throws(1: OperationFailedException ex),
	PowerOffResponse PowerOff(1: PowerOffRequest request) throws(1: OperationFailedException ex),
	ShutdownResponse Shutdown(1: ShutdownRequest request) throws(1: OperationFailedException ex),
	PauseResponse Pause(1: PauseRequest request) throws(1: OperationFailedException ex),
	ResumeResponse Resume(1: ResumeRequest request) throws(1: OperationFailedException ex),
	SaveStateResponse SaveState(1: SaveStateRequest request) throws(1: OperationFailedException ex),
	TakeSnapshotResponse TakeSnapshot(1: TakeSnapshotRequest request) throws(1: OperationFailedException ex),
	GetStatusResponse GetStatus(1: GetStatusRequest request) throws(1: OperationFailedException ex),
	GetIPResponse GetIP(1: GetIPRequest request) throws(1: OperationFailedException ex),
}

### SLAVE ###

# Execute
struct ExecuteRequest
{
	1: required string executable,
	2: optional string arguments,
	3: optional string workingDirectory,
	4: optional map<string, string> environmentVariables
}

struct ExecuteResponse
{	
	1: required i32 exitCode
}

struct ExecuteStream
{
	1: optional string stdoutLine,
	2: optional string stderrLine
}

# Read File
struct ReadFileRequest
{
	1: required string path
}

struct ReadFileResponse
{
	1: required binary contents
}

# Write File
struct WriteFileRequest
{
	1: required string path,
	2: required binary contents,
	3: optional bool overwrite
}

struct WriteFileResponse
{
}

# Create Directory
struct CreateDirectoryRequest
{
	1: required string path,
}

struct CreateDirectoryResponse
{
}

# Enumerate
struct EnumerateRequest
{
	1: required string pathGlob,
	2: optional bool recursive
}

enum EnumerateItemKind
{
    FILE,
	DIRECTORY
}

struct EnumerateItem
{
	1: required string fullPath,
    2: required string relativePath,
	3: required EnumerateItemKind kind
}

struct EnumerateResponse
{
	1: required list<EnumerateItem> items
}

# Slave Service
service VMToolSlave
{
	// Execute streams back multiple responses until the exitCode field is set.
	ExecuteResponse Execute(1: ExecuteRequest request) throws(1: OperationFailedException ex),
	ReadFileResponse ReadFile(1: ReadFileRequest request) throws(1: OperationFailedException ex),
	WriteFileResponse WriteFile(1: WriteFileRequest request) throws(1: OperationFailedException ex),
	CreateDirectoryResponse CreateDirectory(1: CreateDirectoryRequest request) throws(1: OperationFailedException ex),
	EnumerateResponse Enumerate(1: EnumerateRequest request) throws(1: OperationFailedException ex)
}