using Newtonsoft.Json;

namespace Velocity.Models;
public class LogEvent
{
    [JsonProperty("timeStamp")]
    public required string TimeStamp
    {
        get; set;
    }

    [JsonProperty("eventId")]
    public required EventIds EventId
    {
        get; set;
    }

    [JsonProperty("logger")]
    public required string Logger
    {
        get; set;
    }

    [JsonProperty("level")]
    public required string Level
    {
        get; set;
    }

    [JsonProperty("message")]
    public required string Message
    {
        get; set;
    }

    [JsonProperty("exception")]
    public required string Exception
    {
        get; set;
    }

    public enum EventIds
    {
        Startup = 1,
        Shutdown = 2,
        OperationStarted = 3,
        OperationCompleted = 4,

        AvailableUpdatesRetrieved = 10,
        AvailableUpdatesFailedToRetrieve = 11,
        UpdateDownloadStarted = 12,
        UpdateDownloadCompleted = 13,
        UpdateDownloadFailed = 14,
        UpdateInstallStarted = 15,
        UpdateInstallCompleted = 16,
        UpdateInstallFailed = 17,

        ServiceStarted = 20,
        ServiceCompleted = 21,
        ServiceFailed = 22,

        GeneralError = 30,
        DatabaseError = 31,
        NetworkError = 32,
        IoError = 33,
        UnhandledException = 34,

        BackDropChanged = 40,
        BackDropFailedToChange = 41,

        DebugInformation = 50
    }
}
