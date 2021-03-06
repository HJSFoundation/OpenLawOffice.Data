﻿// -----------------------------------------------------------------------
// <copyright file="EventMatter.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Events
{
    using System;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class EventMatter
    {
        public static Common.Models.Events.EventMatter Get(Guid id,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Events.EventMatter, DBOs.Events.EventMatter>(
                "SELECT * FROM \"event_matter\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Events.EventMatter Get(Guid eventId, Guid matterId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Events.EventMatter, DBOs.Events.EventMatter>(
                "SELECT * FROM \"event_matter\" WHERE \"event_id\"=@EventId AND \"matter_id\"=@MatterId AND \"utc_disabled\" is null",
                new { eventId = eventId, MatterId = matterId }, conn, closeConnection);
        }

        public static Common.Models.Events.EventMatter GetFor(Guid eventId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Events.EventMatter, DBOs.Events.EventMatter>(
                "SELECT * FROM \"event_matter\" WHERE \"event_id\"=@EventId AND \"utc_disabled\" is null",
                new { eventId = eventId }, conn, closeConnection);
        }

        public static List<Common.Models.Matters.Matter> ListForEvent(Guid eventId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT * FROM \"matter\" WHERE \"id\" IN (SELECT \"matter_id\" FROM \"event_matter\" WHERE \"event_id\"=@EventId AND \"utc_disabled\" is null)",
                new { EventId = eventId }, conn, closeConnection);
        }

        public static Common.Models.Events.EventMatter Create(Common.Models.Events.EventMatter model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, bool closeConnection = true)
        {
            DBOs.Events.EventMatter dbo;
            Common.Models.Events.EventMatter currentModel;

            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            currentModel = Get(model.Event.Id.Value, model.Matter.Id.Value);

            if (currentModel != null) 
                return currentModel;

            dbo = Mapper.Map<DBOs.Events.EventMatter>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            throw new Exception("this is broke");
            conn.Execute("UPDATE \"event_assigned_conttter\" (\"id\", \"event_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @EventId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo);
            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static void Delete(Common.Models.Events.EventMatter model, Common.Models.Account.Users deleter,
            IDbConnection conn = null, bool closeConnection = true)
        {
            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("DELETE FROM \"event_matter\" WHERE \"id\"=@Id",
                new { Id = model.Id.Value });

            DataHelper.Close(conn, closeConnection);
        }
    }
}
