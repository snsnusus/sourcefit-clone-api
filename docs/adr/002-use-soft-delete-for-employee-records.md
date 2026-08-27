# 2. Use soft delete for Employee records

## Status

Accepted

## Context

The initial `EmployeesController` DELETE endpoint performed a hard
delete (`_context.Employees.Remove(employee)`), permanently removing
the row from the database.

For an HR/MIS system, permanently deleting employee records is
undesirable: historical reporting (e.g. "who used to work in this
department"), audit trails, and accidental-deletion recovery all
depend on records remaining queryable even after an employee is
considered "removed" from active use.

An initial design used a separate `IsDeleted` boolean alongside a
`DeletedAt` timestamp, but this allowed the two fields to
theoretically disagree with each other (e.g. `IsDeleted = true` with
`DeletedAt = null`). A single nullable `DeletedAt`
