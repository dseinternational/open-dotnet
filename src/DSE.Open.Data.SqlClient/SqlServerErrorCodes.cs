// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Data.SqlClient;

public static class SqlServerErrorCodes
{
    // https://learn.microsoft.com/en-us/sql/relational-databases/errors-events/database-engine-events-and-errors-3000-to-3999?view=sql-server-ver16

    /// <summary>
    /// The error number for the error "Snapshot isolation transaction aborted due to update conflict. You cannot use
    /// snapshot isolation to access table '%.*ls' directly or indirectly in database '%.*ls' to update, delete, or
    /// insert the row that has been modified or deleted by another transaction. Retry the transaction or change the
    /// isolation level for the update/delete statement."
    /// </summary>
    public const int SnapshotIsolationTransactionAbortedUpdateConflict = 3960;

    /// <summary>
    /// The error number for the error "Snapshot isolation transaction failed in database '%.*ls' because the object
    /// accessed by the statement has been modified by a DDL statement in another concurrent transaction since the start
    /// of this transaction. It is disallowed because the metadata is not versioned. A concurrent update to metadata can
    /// lead to inconsistency if mixed with snapshot isolation."
    /// </summary>
    public const int SnapshotIsolationTransactionFailedObjectModified = 3961;
}
