﻿// -----------------------------------------------------------------------
// <copyright file="InvoiceExpense.cs" company="Nodine Legal, LLC">
// Licensed to Nodine Legal, LLC under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Nodine Legal, LLC licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    public static class InvoiceExpense
    {
        public static List<Common.Models.Billing.InvoiceExpense> ListForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Billing.InvoiceExpense, DBOs.Billing.InvoiceExpense>(
                "SELECT * FROM \"invoice_expense\" WHERE \"expense_id\" IN (SELECT \"expense_id\" FROM \"expense_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Billing.InvoiceExpense> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static List<Common.Models.Billing.InvoiceExpense> ListForMatterAndInvoice(
            Guid invoiceId, 
            Guid matterId, 
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Billing.InvoiceExpense, DBOs.Billing.InvoiceExpense>(
                "SELECT * FROM \"invoice_expense\" WHERE \"expense_id\" IN (SELECT \"expense_id\" FROM \"expense_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "\"invoice_id\"=@InvoiceId AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { InvoiceId = invoiceId, MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Billing.InvoiceExpense> ListForMatterAndInvoice(
            Transaction t,
            Guid invoiceId, 
            Guid matterId)
        {
            return ListForMatterAndInvoice(invoiceId, matterId, t.Connection, false);
        }

        public static Common.Models.Billing.InvoiceExpense Get(
            Guid invoiceId, 
            Guid expenseId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Billing.InvoiceExpense, DBOs.Billing.InvoiceExpense>(
                "SELECT * FROM \"invoice_expense\" WHERE \"invoice_id\"=@InvoiceId AND \"expense_id\"=@ExpenseId",
                new { InvoiceId = invoiceId, ExpenseId = expenseId }, conn, closeConnection);
        }

        public static Common.Models.Billing.InvoiceExpense Get(
            Transaction t,
            Guid invoiceId,
            Guid expenseId)
        {
            return Get(invoiceId, expenseId, t.Connection, false);
        }

        public static Common.Models.Billing.InvoiceExpense Create(
            Common.Models.Billing.InvoiceExpense model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Billing.InvoiceExpense dbo = Mapper.Map<DBOs.Billing.InvoiceExpense>(model);
            
            conn = DataHelper.OpenIfNeeded(conn);

            Common.Models.Billing.InvoiceExpense currentModel = Get(model.Invoice.Id.Value, model.Expense.Id.Value, conn, closeConnection);

            if (currentModel != null)
            { // Update
                conn.Execute("UPDATE \"invoice_expense\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null WHERE \"id\"=@Id", dbo);
                model.Created = currentModel.Created;
                model.CreatedBy = currentModel.CreatedBy;
            }
            else
            { // Create
                conn.Execute("INSERT INTO \"invoice_expense\" (\"id\", \"expense_id\", \"invoice_id\", \"amount\", \"details\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @ExpenseId, @InvoiceId, @Amount, @Details, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Billing.InvoiceExpense Create(
            Transaction t,
            Common.Models.Billing.InvoiceExpense model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }
    }
}
