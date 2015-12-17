﻿// -----------------------------------------------------------------------
// <copyright file="InvoiceTime.cs" company="Nodine Legal, LLC">
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

    public static class InvoiceTime
    {
        public static List<Common.Models.Billing.InvoiceTime> ListForMatter(
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Billing.InvoiceTime, DBOs.Billing.InvoiceTime>(
                "SELECT * FROM \"invoice_time\" WHERE \"time_id\" IN " +
                "   (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN " +
                "       (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId) " +
                "   ) AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Billing.InvoiceTime> ListForMatter(
            Transaction t,
            Guid matterId)
        {
            return ListForMatter(matterId, t.Connection, false);
        }

        public static List<Common.Models.Billing.InvoiceTime> ListForMatterAndInvoice(
            Guid invoiceId, 
            Guid matterId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Billing.InvoiceTime, DBOs.Billing.InvoiceTime>(
                "SELECT * FROM \"invoice_time\" WHERE \"time_id\" IN " +
                "   (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN " +
                "       (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId) " +
                "   ) AND " +
                "\"invoice_id\"=@InvoiceId AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { InvoiceId = invoiceId, MatterId = matterId }, conn, closeConnection);
        }

        public static List<Common.Models.Billing.InvoiceTime> ListForMatterAndInvoice(
            Transaction t,
            Guid invoiceId,
            Guid matterId)
        {
            return ListForMatterAndInvoice(invoiceId, matterId, t.Connection, false);
        }

        public static Common.Models.Billing.InvoiceTime Get(
            Guid invoiceId, 
            Guid timeId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Billing.InvoiceTime, DBOs.Billing.InvoiceTime>(
                "SELECT * FROM \"invoice_time\" WHERE \"invoice_id\"=@InvoiceId AND \"time_id\"=@TimeId",
                new { InvoiceId = invoiceId, TimeId = timeId }, conn, closeConnection);
        }

        public static Common.Models.Billing.InvoiceTime Get(
            Transaction t,
            Guid invoiceId,
            Guid timeId)
        {
            return Get(invoiceId, timeId, t.Connection, false);
        }

        public static Common.Models.Billing.InvoiceTime Create(
            Common.Models.Billing.InvoiceTime model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Billing.InvoiceTime dbo = Mapper.Map<DBOs.Billing.InvoiceTime>(model);
            
            conn = DataHelper.OpenIfNeeded(conn);

            Common.Models.Billing.InvoiceTime currentModel = Get(model.Invoice.Id.Value, model.Time.Id.Value);

            if (currentModel != null)
            { // Update
                conn.Execute("UPDATE \"invoice_time\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null WHERE \"id\"=@Id", dbo);
                model.Created = currentModel.Created;
                model.CreatedBy = currentModel.CreatedBy;
            }
            else
            { // Create
                conn.Execute("INSERT INTO \"invoice_time\" (\"id\", \"time_id\", \"invoice_id\", \"duration\", \"details\", \"price_per_hour\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @TimeId, @InvoiceId, @Duration, @Details, @PricePerHour, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Billing.InvoiceTime Create(
            Transaction t,
            Common.Models.Billing.InvoiceTime model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }
    }
}
