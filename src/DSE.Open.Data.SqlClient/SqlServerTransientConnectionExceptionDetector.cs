// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Data.SqlClient;

namespace DSE.Open.Data.SqlClient;

/// <summary>
///     Detects the exceptions caused by SQL Server transient connection failures.
/// </summary>
public static class SqlServerTransientConnectionExceptionDetector
{
    private static readonly string[] s_messages = new[]
    {
        "TCP Provider, error: 40",
        "TCP Provider, error: 35",
        "Execution Timeout Expired",
        "Please retry the connection later"
    };

    public static bool ShouldRetryOn(Exception? ex)
    {
        if (ex is SqlException sqlException)
        {
            foreach (SqlError err in sqlException.Errors)
            {
                switch (err.Number)
                {
                    // https://learn.microsoft.com/en-us/azure/azure-sql/database/troubleshoot-common-errors-issues?view=azuresql-db

                    // Database 'replicatedmaster' cannot be opened. It has been marked SUSPECT by
                    // recovery. See the SQL Server errorlog for more information. This error may be
                    // logged on SQL Managed Instance errorlog, for a short period of time, during the
                    // last stage of a reconfiguration, while the old primary is shutting down its log.
                    case 926:

                    // Cannot open database "%.*ls" requested by the login. The login failed. 
                    case 4060:

                    // The service has encountered an error processing your request. Please try again.
                    // Error code %d. You receive this error when the service is down due to software or
                    // hardware upgrades, hardware failures, or any other failover problems. The error
                    // code (%d) embedded within the message of error 40197 provides additional
                    // information about the kind of failure or failover that occurred. Some examples of
                    // the error codes are embedded within the message of error 40197 are 40020, 40143,
                    // 40166, and 40540.
                    case 40197:

                    // The service is currently busy. Retry the request after 10 seconds.
                    // Incident ID: %ls. Code: %d. 
                    case 40501:

                    // Database '%.*ls' on server '%.*ls' is not currently available. Please retry the
                    // connection later. If the problem persists, contact customer support, and provide
                    // them the session tracing ID of '%.*ls'.
                    case 40613:

                    // Cannot process request. Not enough resources to process request. The service is
                    // currently busy. Please retry the request later. 
                    case 49918:

                    // Cannot process create or update request. Too many create or update operations in
                    // progress for subscription "%ld". The service is busy processing multiple create or
                    // update requests for your subscription or server. Requests are currently blocked
                    // for resource optimization. 
                    case 49919:

                    // Cannot process request. Too many operations in progress for subscription "%ld".
                    // The service is busy processing multiple requests for this subscription. Requests
                    // are currently blocked for resource optimization. 
                    case 49920:

                    // Login to read-secondary failed due to long wait on
                    // 'HADR_DATABASE_WAIT_FOR_TRANSITION_TO_VERSIONING'. The replica is not available for
                    // login because row versions are missing for transactions that were in-flight when
                    // the replica was recycled. The issue can be resolved by rolling back or committing
                    // the active transactions on the primary replica. Occurrences of this condition can
                    // be minimized by avoiding long write transactions on the primary.
                    case 4221:

                    // Could not find database ID %d, name '%.*ls'. Error Code 615. This means in-memory
                    // cache is not in-sync with SQL server instance and lookups are retrieving stale
                    // database ID. SQL logins use in-memory cache to get the database name to ID mapping.
                    // The cache should be in sync with backend database and updated whenever attach and
                    // detach of database to/from the SQL server instance occurs.
                    case 615:


                    // System.Data.SqlClient.SqlException: A network-related or instance-specific error
                    // occurred while establishing a connection to SQL Server. The server was not found
                    // or was not accessible. Verify that the instance name is correct and that SQL Server
                    // is configured to allow remote connections.(provider: SQL Network Interfaces,
                    // error: 26 – Error Locating Server/Instance Specified)
                    case 26:

                    // A network-related or instance-specific error occurred while establishing a
                    // connection to SQL Server. The server was not found or was not accessible. Verify
                    // that the instance name is correct and that SQL Server is configured to allow remote
                    // connections. (provider: Named Pipes Provider, error: 40 - Could not open a
                    // connection to SQL Server)
                    case 40:

                    // 10053: A transport-level error has occurred when receiving results from the server.
                    // (Provider: TCP Provider, error: 0 - An established connection was aborted by the
                    // software in your host machine)
                    case 10053:

                    // Resource ID: %d. The %s limit for the database is %d and has been reached
                    case 10928:

                    // Resource ID: %d. The %s limit for the elastic pool is %d and has been reached
                    case 10936:

                    // Resource ID: %d. The %s minimum guarantee is %d, maximum limit is %d, and the
                    // current usage for the database is %d. However, the server is currently too busy to
                    // support requests greater than %d for this database. The Resource ID indicates the
                    // resource that has reached the limit. For worker threads, the Resource ID = 1.
                    // For sessions, the Resource ID = 2.. 
                    case 10929:

                    // 40544: The database has reached its size quota. Partition or delete data, drop
                    // indexes, or consult the documentation for possible resolutions.
                    case 40544:

                    // The elastic pool has reached its storage limit. The storage usage for the elastic
                    // pool cannot exceed (%d) MBs. Attempting to write data to a database when the
                    // storage limit of the elastic pool has been reached
                    case 1132:

                    // https://github.com/dotnet/SqlClient/blob/main/src/Microsoft.Data.SqlClient/src/Microsoft/Data/SqlClient/Reliability/SqlConfigurableRetryFactory.cs

                    // The instance of the SQL Server Database Engine cannot obtain a LOCK resource at this
                    // time. Rerun your statement when there are fewer active users. Ask the database
                    // administrator to check the lock and memory configuration for this instance, or to
                    // check for long-running transactions.
                    case 1024:

                    // Transaction (Process ID) was deadlocked on resources with another process and has
                    // been chosen as the deadlock victim. Rerun the transaction.
                    case 1025:

                    // Lock request time out period exceeded.
                    case 1222:

                    // The service has encountered an error processing your request. Please try again.
                    case 40143:

                    // The service has encountered an error processing your request. Please try again.
                    case 40540:

                    // Can not connect to the SQL pool since it is paused. Please resume the SQL pool
                    // and try again.
                    case 42108:

                    // The SQL pool is warming up. Please try again.
                    case 42109:

                    // An error has occurred while establishing a connection to the server. When
                    // connecting to SQL Server, this failure may be caused by the fact that under the
                    // default settings SQL Server does not allow remote connections. (provider: TCP
                    // Provider, error: 0 - A connection attempt failed because the connected party did
                    // not properly respond after a period of time, or established connection failed
                    // because connected host has failed to respond.) (Microsoft SQL Server, Error: 10060)
                    case 10060:

                    // A connection was successfully established with the server, but then an error
                    // occurred during the login process. (provider: Named Pipes Provider, error: 0 -
                    // Overlapped I/O operation is in progress).
                    case 997:

                    // A connection was successfully established with the server, but then an error
                    // occurred during the login process. (provider: Shared Memory Provider, error: 0 -
                    // No process is on the other end of the pipe.) (Microsoft SQL Server, Error: 233)
                    case 233:

                        return true;
                }
            }

            if (s_messages.Any(m => sqlException.Message.Contains(m, StringComparison.Ordinal)))
            {
                return true;
            }
        }

        return ex is TimeoutException;
    }
}
