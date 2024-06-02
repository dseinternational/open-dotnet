// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using Microsoft.Data.SqlClient;

namespace DSE.Open.Data.SqlClient;

/// <summary>
///     Detects the exceptions caused by SQL Server transient connection failures.
/// </summary>
public static class SqlServerTransientConnectionExceptionDetector
{
    private static readonly string[] s_messages =
    [
        "TCP Provider, error: 40",
        "TCP Provider, error: 35",
        "Execution Timeout Expired",
        "Please retry the connection later"
    ];

    public static bool ShouldRetryOn(Exception? ex)
    {
        if (ex is SqlException sqlException)
        {
            foreach (SqlError err in sqlException.Errors)
            {
                switch (err.Number)
                {
                    // https://github.com/dotnet/efcore/pull/31612/files
                    // and
                    // https://github.com/dotnet/efcore/blob/main/src/EFCore.SqlServer/Storage/Internal/SqlServerTransientExceptionDetector.cs

                    // SQL Error Code: 49983
                    // The '%ls' operation failed to complete. Retry the operation. Create a support request if the retry attempts do not succeed.
                    case 49983:
                    // SQL Error Code: 49977
                    // The elastic pool '%ls' is busy with another operation. Please wait until the ongoing operation finishes and try again.
                    case 49977:
                    // SQL Error Code: 49802
                    // Database is unavailable at the moment, please retry connection at later time.
                    case 49802:
                    // SQL Error Code: 49510
                    // Managed instance is busy with another operation. Please try your operation later.
                    case 49510:
                    // SQL Error Code: 47139
                    // Join contained availability group '%.*ls' failed to create group master '%.*ls' Database ID. Please retry the operation again.
                    case 47139:
                    // SQL Error Code: 47137
                    // Cannot create contained system databases in contained availability group '%.*ls' It might be caused by
                    // temporary condition. Retry the operation.
                    case 47137:
                    // SQL Error Code: 47132
                    // Joining availability group '%.*ls' with rebuilding contained system DB has failed because rebuilding contained MSDB
                    // has failed. This is caused by contained MSDB is still used. Retry the operation later.
                    case 47132:
                    // SQL Error Code: 45547
                    // The create operation has timed out in one of the backend workflows. Please retry the operation.
                    case 45547:
                    // SQL Error Code: 45319
                    // The service objective assignment for database '%.*ls' on server '%.*ls' could not be completed as the database
                    // is too busy. Reduce the workload before initiating another service objective update.
                    case 45319:
                    // SQL Error Code: 45182
                    // Database '%ls' is busy with another operation. Please try your operation later.
                    case 45182:
                    // SQL Error Code: 45179
                    // The operation timed out and automatically rolled back. Please retry the operation.
                    case 45179:
                    // SQL Error Code: 45161
                    // Managed instance '%.*ls' is busy with another operation. Please try your operation later.
                    case 45161:
                    // SQL Error Code: 45157
                    // Server '%.*ls' is busy with another operation. Please try your operation later.
                    case 45157:
                    // SQL Error Code: 45156
                    // Subscription '%.*ls' is busy with another operation. Please try your operation later.
                    case 45156:
                    // SQL Error Code: 45153
                    // Management Service is not currently available. Please retry the operation later. If the problem persists,
                    // contact customer support, and provide them the session tracing ID of '%ls'.
                    case 45153:
                    // SQL Error Code: 42029
                    // An internal error happened while generating a new DBTS for database %.*ls. Please retry the operation.
                    case 42029:
                    // SQL Error Code: 41840
                    // Could not perform the operation because the elastic pool or managed instance has reached its quota for in-memory tables.
                    // This error may be transient. Please retry the operation. See 'http://go.microsoft.com/fwlink/?LinkID=623028' for more information.
                    case 41840:
                    // SQL Error Code: 41823
                    // Could not perform the operation because the database has reached its quota for in-memory tables. This error may be transient.
                    // Please retry the operation. See 'http://go.microsoft.com/fwlink/?LinkID=623028' for more information.
                    case 41823:
                    // SQL Error Code: 41701
                    // The Activation Context is unavailable at this time. The Windows Fabric Runtime is unavailable at this time,
                    // retry later. Wait for the activation context to become available, then retry.
                    case 41701:
                    // SQL Error Code: 41700
                    // System views related to Windows Fabric partitions and replicas are not available at this time,
                    // because replica manager has not yet started. Wait for replica manager to start, then retry the system view query.
                    case 41700:
                    // SQL Error Code: 41640
                    // Database '%ls' encountered a transient error (error code: 0x%08X) while performing task '%ls'.
                    // Refer to the SQL Server error log for information about the errors that were encountered.
                    // If this condition persists, contact the system administrator.
                    case 41640:
                    // SQL Error Code: 41619
                    // Windows Fabric '%ls' (partition ID '%ls')encountered transient error %d while waiting for build replica operation
                    // on database '%ls' (ID %d). Refer to the SQL Server error log for information about the errors that were encountered.
                    // If this condition persists, contact the system administrator.
                    case 41619:
                    // SQL Error Code: 41614
                    // Fabric Service '%ls' encountered a transient error while performing Windows Fabric operation on '%ls' database
                    // (ID %d). Refer to the SQL Server error log for information about the errors that were encountered.
                    // If this condition persists, contact the system administrator.
                    case 41614:
                    // SQL Error Code: 41383
                    // An internal error occurred while running the DMV query. This was likely caused by concurrent DDL operations. Please retry the query.
                    case 41383:
                    // SQL Error Code: 41339
                    // The table '%.*ls' has been created or altered after the start of the current transaction. The transaction was aborted.
                    // Please retry the transaction.
                    case 41339:
                    // SQL Error Code: 40938
                    // The Server DNS Alias '%.*ls' is busy with another operation and cannot perform the '%.*ls' operation. Please try again later.
                    case 40938:
                    // SQL Error Code: 40918
                    // The Failover Group '%.*ls' is busy with another operation and cannot perform the '%.*ls' operation. Please try again later
                    case 40918:
                    // SQL Error Code: 40903
                    // The server '%.*ls' is currently busy. Please wait a few minutes before trying again.
                    case 40903:
                    // SQL Error Code: 40890
                    // The elastic pool is busy with another operation.
                    case 40890:
                    // SQL Error Code: 40675
                    // The service is currently too busy. Please try again later.
                    case 40675:
                    // SQL Error Code: 40671
                    // Unable to '%.*ls' '%.*ls' on server '%.*ls'. Please retry the connection later.
                    case 40671:
                    // SQL Error Code: 40648
                    // Too many requests have been performed. Please retry later.
                    case 40648:
                    // SQL Error Code: 40642
                    // The server is currently too busy. Please try again later.
                    case 40642:
                    // SQL Error Code: 40623
                    // Reauthentication failed for login %.*ls. Within the past reauthentification interval, the login has become invalid
                    // due to a password change, a dropped login, or other cause. Please retry login.
                    case 40623:
                    // SQL Error Code: 40540
                    // The service has encountered an error processing your request. Please try again.
                    // case 40540:
                    // SQL Error Code: 40189
                    // The resource quota for the current database has been exceeded and this request has been aborted.
                    // Please rerun your request in the next quota window. %s
                    case 40189:
                    // SQL Error Code: 40143
                    // The service has encountered an error processing your request. Please try again.
                    case 40143:
                    // SQL Error Code: 40106
                    // The schema scope set in the session is not the current schema scope for the current partition. Please rerun your query.
                    case 40106:
                    // SQL Error Code: 39152
                    // Transient error communicating with the streaming runtime due to unfinished stop streaming job operation. Please retry the operation.
                    case 39152:
                    // SQL Error Code: 39151
                    // Transient error communicating with the streaming runtime. Please retry the operation.
                    case 39151:
                    // SQL Error Code: 39110
                    // Maximum number of concurrent external script queries for this user has been reached. Limit is %d.
                    // Please retry the operation. External script request id is %ls.
                    case 39110:
                    // SQL Error Code: 39108
                    // Maximum number of concurrent external script users has been reached. Limit is %d. Please retry the operation. External script request id is %ls.
                    case 39108:
                    // SQL Error Code: 39025
                    // External script execution failed as extensibility environment is not ready yet. Retry the operation when the server is fully started.
                    case 39025:
                    // SQL Error Code: 37327
                    // Maximum number of concurrent DBCC commands running in the enclave has been reached. The maximum number of
                    // concurrent DBCC queries is %d. Try rerunning the query.
                    case 37327:
                    // SQL Error Code: 37202
                    // An instance pool with name '%.*ls' is busy with another ongoing operation.
                    case 37202:
                    // SQL Error Code: 35293
                    // Error in retrieving extended recovery forks from the primary replica. The extended-recovery-fork stack changed
                    // while being retrieved by the secondary replica. Retry the operation.
                    case 35293:
                    // SQL Error Code: 35256
                    // The session timeout value was exceeded while waiting for a response from the other availability replica in the
                    // session. That replica or the network might be down, or the command might be misconfigured. If the partner is running
                    // and visible over the network, retry the command using correctly configured partner-connection parameters.
                    case 35256:
                    // SQL Error Code: 35218
                    // An error occurred while trying to set the initial Backup LSN of database '%.*ls'. Primary database is temporarily
                    // offline due to restart or other transient condition. Retry the operation.
                    case 35218:
                    // SQL Error Code: 35216
                    // An error occurred while adding or removing a log truncation holdup to build secondary replica from primary availability
                    // database '%.*ls'. Primary database is temporarily offline due to restart or other transient condition. Retry the operation.
                    case 35216:
                    // SQL Error Code: 33123
                    // Cannot drop or alter the database encryption key since it is currently in use on a mirror or secondary availability replica.
                    // Retry the command after all the previous reencryption scans have propagated to the mirror or secondary availability replicas
                    // or after availability relationship has been disabled.
                    case 33123:
                    // SQL Error Code: 30085
                    // A stoplist cache cannot be generated while processing a full-text query or performing full-text indexing. There is
                    // not enough memory to load the stoplist cache. Rerun the query or indexing command when more resources are available.
                    case 30085:
                    // SQL Error Code: 30080
                    // The full-text population on table '%ls' cannot be started because the full-text catalog is importing data from
                    // existing catalogs. After the import operation finishes, rerun the command.
                    case 30080:
                    // SQL Error Code: 25740
                    // Unable to start event session '%.*ls' because system is busy. Please try again later.
                    case 25740:
                    // SQL Error Code: 25738
                    // Event session '%.*ls' could not be started because system is currently busy. Please try again later.
                    case 25738:
                    // SQL Error Code: 25003
                    // Upgrade of the distribution database MSmerge_subscriptions table failed. Rerun the upgrade procedure in order
                    // to upgrade the distribution database.
                    case 25003:
                    // SQL Error Code: 22984
                    // An error occurred while waiting on the log reader history cache event. This error is reported by the internal task
                    // scheduling and might be transient. Retry the operation.
                    case 22984:
                    // SQL Error Code: 22760
                    // Aborting Synapse Link Publish task for partition %ld timed out. Retry this operation later.
                    case 22760:
                    // SQL Error Code: 22759
                    // Aborting Synapse Link Snapshot task for table %ld timed out. Retry this operation later.
                    case 22759:
                    // SQL Error Code: 22758
                    // Aborting Synapse Link Commit task for table group '%s' timed out. Retry this operation later.
                    case 22758:
                    // SQL Error Code: 22754
                    // Aborting Synapse Link Capture task for this database timed out. Retry this operation later.
                    case 22754:
                    // SQL Error Code: 22498
                    // There is not enough resources to perform the operation. Please retry your operation later.
                    case 22498:
                    // SQL Error Code: 22493
                    // The database '%.*ls' failed to sync. Please retry the operation again. If the problem persists contact
                    // Microsoft Azure Customer Support.
                    case 22493:
                    // SQL Error Code: 22491
                    // The DDL statement failed due to an internal error. Please retry the operation again. If the problem persists contact
                    // Microsoft Azure Customer Support.
                    case 22491:
                    // SQL Error Code: 22430
                    // Operation failed as the Database '%.*ls' is shutting down. Please retry the operation again. If the problem persists
                    // contact Microsoft Azure Customer Support.
                    case 22430:
                    // SQL Error Code: 22427
                    // Operation failed due to an error in a background task. Please retry the operation again. If the problem persists
                    // contact Microsoft Azure Customer Support.
                    case 22427:
                    // SQL Error Code: 22358
                    // The Database Controller required for this operation was not found. Please retry the operation again. If the problem
                    // persists contact Microsoft Azure Customer Support.
                    case 22358:
                    // SQL Error Code: 22355
                    // Lock manager does not exist. Please retry the operation again. If the problem persists contact
                    // Microsoft Azure Customer Support.
                    case 22355:
                    // SQL Error Code: 22353
                    // The SQL instances has not been correctly setup to allow this operation. Please retry the operation again.
                    // If the problem persists contact Microsoft Azure Customer Support.
                    case 22353:
                    // SQL Error Code: 22335
                    // Cannot obtain a LOCK resource at this time due to internal error. Rerun your statement when there are fewer active users.
                    case 22335:
                    // SQL Error Code: 22226
                    // An internal error (%d, %d) occured. Please retry the operation again. If the problem persists contact
                    // Microsoft Azure Customer Support.
                    case 22226:
                    // SQL Error Code: 22225
                    // An internal error (%d, %d) occured. Please retry the operation again. If the problem persists contact
                    // Microsoft Azure Customer Support.
                    case 22225:
                    // SQL Error Code: 21503
                    // Cleanup of merge meta data cannot be performed while merge processes are running. Retry this operation after
                    // the merge processes have completed.
                    case 21503:
                    // SQL Error Code: 19494
                    // Automatic seeding of availability database '%ls' in availability group '%.*ls' failed with a transient error.
                    // The operation will be retried.
                    case 19494:
                    // SQL Error Code: 19416
                    // One or more databases in availability group '%.*ls' are not synchronized. On a synchronous-commit availability
                    // replica, ALTER AVAILABILITY GROUP <group_name> OFFLINE is not allowed when one or more databases are not
                    // synchronized. Wait for all databases to reach the SYNCHRONIZED state, and retry the command.
                    case 19416:
                    // SQL Error Code: 19413
                    // An attempt to switch Always On Availability Groups to the local Windows Server Failover Clustering (WSFC) cluster
                    // context failed. This attempt failed because switching the cluster context back to the local cluster at this time
                    // might cause data loss because one or more secondary databases on synchronous-commit replicas are not in the
                    // SYNCHRONIZED state. Wait until all synchronous-commit secondary databases are synchronized, and then retry the
                    // ALTER SERVER CONFIGURATION SET HADR CLUSTER LOCAL command.
                    case 19413:
                    // SQL Error Code: 18858
                    // Could not read data from replication table '%s'. If retrying does not fix the issue, drop and reconfigure replication.
                    case 18858:
                    // SQL Error Code: 18401
                    // Login failed for user '%s'. Reason: Server is in script upgrade mode. Only administrator can connect at this time.
                    // Devnote: this can happen when SQL is going through recovery (e.g. after failover)
                    case 18401:
                    // SQL Error Code: 17889
                    // A new connection was rejected because the maximum number of connections on session ID %d has been reached.
                    // Close an existing connection on this session and retry.%.*ls
                    case 17889:
                    // SQL Error Code: 17067
                    // SQL Server Assertion: File: <%s>, line = %d %s. This error may be timing-related. If the error persists after
                    // rerunning the statement, use DBCC CHECKDB to check the database for structural integrity, or restart the server
                    // to ensure in-memory data structures are not corrupted.
                    case 17067:
                    // SQL Error Code: 17066
                    // SQL Server Assertion: File: <%s>, line=%d Failed Assertion = '%s'. This error may be timing-related. If the error
                    // persists after rerunning the statement, use DBCC CHECKDB to check the database for structural integrity, or restart
                    // the server to ensure in-memory data structures are not corrupted.
                    case 17066:
                    // SQL Error Code: 17065
                    // SQL Server Assertion: File: <%s>, line = %d Failed Assertion = '%s' %s. This error may be timing-related.
                    // If the error persists after rerunning the statement, use DBCC CHECKDB to check the database for structural integrity,
                    // or restart the server to ensure in-memory data structures are not corrupted.
                    case 17065:
                    // SQL Error Code: 16555
                    // Operation failed due to an error while processing rejected rows. Intermediate results, if any, should be discarded
                    // as such results may not be complete. Please retry the operation. If the problem persists, contact
                    // Microsoft Azure Customer Support.
                    case 16555:
                    // SQL Error Code: 16554
                    // Operation failed due to an error in a background task. Please retry the operation again. If the problem persists
                    // contact Microsoft Azure Customer Support.
                    case 16554:
                    // SQL Error Code: 16528
                    // Operation '%ls %ls' failed. Retry the operation later.
                    case 16528:
                    // SQL Error Code: 14906
                    // The database '%s' is not accessible. Ensure that the remote database exists. If you believe that your database should
                    // be accessible please retry the command.
                    case 14906:
                    // SQL Error Code: 14868
                    // Inbound migration is in progress or paused. Migration direction outbound cannot be set at this time. Please retry
                    // after inbound migration is complete.
                    case 14868:
                    // SQL Error Code: 14817
                    // The server '%s' is not accessible. Ensure that the remote server exists and the Azure SQL DB Firewall Rules permit
                    // access to the server. If you believe that your server should be accessible please retry the command.
                    case 14817:
                    // SQL Error Code: 11539
                    // One of the types specified in WITH RESULT SETS clause has been modified after the EXECUTE statement started running.
                    // Please rerun the statement.
                    case 11539:
                    // SQL Error Code: 11001
                    // An error has occurred while establishing a connection to the server. When connecting to SQL Server,
                    // this failure may be caused by the fact that under the default settings SQL Server does not allow remote connections.
                    // (provider: TCP Provider, error: 0 - No such host is known.) (.Net SqlClient Data Provider)
                    case 11001:
                    // SQL Error Code: 10930
                    // The service is currently too busy. Please try again later.
                    case 10930:
                    // SQL Error Code: 9985
                    // 	There is not enough memory to generate a search property list cache. Rerun your full-text indexing statement when more resources are available.
                    case 9985:
                    // SQL Error Code: 9020
                    // The log for database '%ls' failed to grow while shrink in progress. Please retry.
                    case 9020:
                    // SQL Error Code: 7951
                    // Warning: Could not complete filestream consistency checks due to an operating system error. Any consistency errors
                    // found in the filestream subsystem will be silenced. Please refer to other errors for more information.
                    // This condition is likely transient; try rerunning the command.
                    case 7951:
                    // SQL Error Code: 6292
                    // The transaction that is associated with this operation has been committed or rolled back. Retry with a different transaction.
                    case 6292:
                    // SQL Error Code: 5529
                    // Failed to remove a FILESTREAM file. The database is a primary database in an availability group. Wait for the
                    // FILESTREAM data files to be hardened on every secondary availability replica. Then retry the drop file operation.
                    case 5529:
                    // SQL Error Code: 5280
                    // An unexpected protocol element was recevied during the execution of a consistency check command. Retry the operation.
                    case 5280:
                    // SQL Error Code: 3980
                    // The request failed to run because the batch is aborted, this can be caused by abort signal sent from client,
                    // or another request is running in the same session, which makes the session busy.
                    case 3980:
                    // SQL Error Code: 4184
                    // Cannot retrieve table data for the query operation because the table ""%.*ls"" schema is being altered too frequently.
                    // Because the table ""%.*ls"" contains a filtered index or filtered statistics, changes to the table schema require
                    // a refresh of all table data. Retry the query operation, and if the problem persists, use SQL Server Profiler to identify
                    // what schema-altering operations are occurring.
                    case 4184:
                    // SQL Error Code: 4117
                    // Cannot retrieve table data for the query operation because the table ""%.*ls"" schema is being altered too frequently.
                    // Because the table ""%.*ls"" contains a computed column, changes to the table schema require a refresh of all table data.
                    // Retry the query operation, and if the problem persists, use SQL Server Profiler to identify what schema-altering operations are occurring.
                    case 4117:
                    // SQL Error Code: 3957
                    // Snapshot isolation transaction failed in database '%.*ls' because the database did not allow snapshot isolation when
                    // the current transaction started. It may help to retry the transaction.
                    case 3957:
                    // SQL Error Code: 3966
                    // Transaction is rolled back when accessing version store. It was earlier marked as victim when the version store
                    // was shrunk due to insufficient space in tempdb. This transaction was marked as a victim earlier because it may need
                    // the row version(s) that have already been removed to make space in tempdb. Retry the transaction
                    case 3966:
                    // SQL Error Code: 3960
                    // Snapshot isolation transaction aborted due to update conflict. You cannot use snapshot isolation to access table '%.*ls'
                    // directly or indirectly in database '%.*ls' to update, delete, or insert the row that has been modified or deleted
                    // by another transaction. Retry the transaction or change the isolation level for the update/delete statement.
                    case 3960:
                    // SQL Error Code: 3953
                    // Snapshot isolation transaction failed in database '%.*ls' because the database was not recovered when the current
                    // transaction was started. Retry the transaction after the database has recovered.
                    case 3953:
                    // SQL Error Code: 3950
                    // Version store scan timed out when attempting to read the next row. Please try the statement again later when the system is not as busy.
                    case 3950:
                    // SQL Error Code: 3948
                    // The transaction was terminated because of the availability replica config/state change or because ghost records
                    // are being deleted on the primary and the secondary availability replica that might be needed by queries running under
                    // snapshot isolation. Retry the transaction.
                    case 3948:
                    // SQL Error Code: 3947
                    // The transaction was aborted because the secondary compute failed to catch up redo. Retry the transaction.
                    case 3947:
                    // SQL Error Code: 3941
                    // The transaction cannot modify an object that is published for replication or has Change Data Capture enabled
                    // because the transaction started before replication or Change Data Capture was enabled on the database. Retry the transaction.
                    case 3941:
                    // SQL Error Code: 3635
                    // An error occurred while processing '%ls' metadata for database id %d, file id %d, and transaction='%.*ls'.
                    // Additional Context='%ls'. Location='%hs'(%d). Retry the operation; if the problem persists,
                    // contact the database administrator to review locking and memory configurations.
                    // Review the application for possible deadlock conflicts.
                    case 3635:
                    // SQL Error Code: 3429
                    // Recovery could not determine the outcome of a cross-database transaction %S_XID, named '%.*ls',
                    // in database '%.*ls' (database ID %d:%d). The coordinating database (database ID %d:%d) was unavailable.
                    // The transaction was assumed to be committed. If the transaction was not committed, you can retry recovery
                    // when the coordinating database is available.
                    case 3429:
                    // SQL Error Code: 2816
                    // The metadata for object with id %d has changed. Retry the statement.
                    case 2816:
                    // SQL Error Code: 2021
                    // The referenced entity '%.*ls' was modified during DDL execution. Please retry the operation.
                    case 2021:
                    // SQL Error Code: 1535
                    // Cannot share extent %S_PGID. Shared extent directory is full. Retry the transaction. If the problem persists, contact Technical Support.
                    case 1535:
                    // SQL Error Code: 1534
                    // Extent %S_PGID not found in shared extent directory. Retry the transaction. If the problem persists, contact Technical Support.
                    case 1534:
                    // SQL Error Code: 1533
                    // Cannot share extent %S_PGID. The correct extents could not be identified. Retry the transaction.
                    case 1533:
                    // SQL Error Code: 1532
                    // New sort run starting on page %S_PGID found an extent not marked as shared. Retry the transaction.
                    // If the problem persists, contact Technical Support.
                    case 1532:
                    // SQL Error Code: 1438
                    // The server instance %ls rejected configure request; read its error log file for more information.
                    // The reason %u, and state %u, can be of use for diagnostics by Microsoft.
                    // This is a transient error hence retrying the request is likely to succeed. Correct the cause if any and retry.
                    case 1438:
                    // SQL Error Code: 1421
                    // Communications to the remote server instance '%.*ls' failed to complete before its timeout.
                    // The ALTER DATABASE command may have not completed. Retry the command.
                    case 1421:
                    // SQL Error Code: 1413
                    // Communications to the remote server instance '%.*ls' failed before database mirroring was fully started.
                    // The ALTER DATABASE command failed. Retry the command when the remote database is started.
                    case 1413:
                    // SQL Error Code: 1404
                    // The command failed because the database mirror is busy. Reissue the command later.
                    case 1404:
                    // SQL Error Code: 1232
                    // Failed to acquire lock with lock manager service, it could be due to many reasons including transient service failure.
                    case 1232:
                    // SQL Error Code: 615
                    // Could not find database ID %d, name '%.*ls'. The database may be offline. Wait a few minutes and try again.
                    case 615:
                    // SQL Error Code: 539
                    // Schema changed after the target table was created. Rerun the Select Into query.
                    case 539:



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
                    // case 615:


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
                    // case 40143:

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

                    // SQL Error Code: 203
                    // A connection was successfully established with the server, but then an error occurred during the pre-login handshake.
                    // (provider: TCP Provider, error: 0 - 20) ---> System.ComponentModel.Win32Exception (203): Unknown error: 203
                    case 203:
                        if (ex.InnerException is Win32Exception)
                        {
                            return true;
                        }
                        continue;

                    // SQL Error Code: 121
                    // The semaphore timeout period has expired
                    case 121:

                    // SQL Error Code: 64
                    // A connection was successfully established with the server, but then an error occurred during the login process.
                    // (provider: TCP Provider, error: 0 - The specified network name is no longer available.)
                    case 64:

                    // DBNETLIB Error Code: 20
                    // The instance of SQL Server you attempted to connect to does not support encryption.
                    case 20:

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

    // https://github.com/dotnet/efcore/pull/31612/files

    private static bool IsMemoryOptimizedError(Exception exception)
    {
        if (exception is SqlException sqlException)
        {
            foreach (SqlError err in sqlException.Errors)
            {
                switch (err.Number)
                {
                    case 41301:
                    case 41302:
                    case 41305:
                    case 41325:
                    case 41839:
                        return true;
                }
            }
        }

        return false;
    }

    // https://github.com/dotnet/efcore/pull/31612/files

    private static bool IsThrottlingError(Exception exception)
    {
        if (exception is SqlException sqlException)
        {
            foreach (SqlError err in sqlException.Errors)
            {
                switch (err.Number)
                {
                    case 49977:
                    case 49920:
                    case 49919:
                    case 49918:
                    case 45319:
                    case 45182:
                    case 45161:
                    case 45157:
                    case 45156:
                    case 41840:
                    case 41823:
                    case 40903:
                    case 40890:
                    case 40675:
                    case 40648:
                    case 40642:
                    case 40613:
                    case 40501:
                    case 40189:
                    case 39110:
                    case 39108:
                    case 37327:
                    case 30085:
                    case 25740:
                    case 25738:
                    case 22498:
                    case 22335:
                    case 17889:
                    case 14355:
                    case 10930:
                    case 10929:
                    case 9985:
                    case 3950:
                    case 3935:
                    case 1404:
                    case 1204:
                    case 233:
                    case -2:
                        return true;
                }
            }
        }

        return false;
    }
}
