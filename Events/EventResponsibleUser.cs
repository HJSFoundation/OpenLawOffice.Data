﻿// -----------------------------------------------------------------------
// <copyright file="EventResponsibleUser.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class EventResponsibleUser
    {
        public static Common.Models.Events.EventResponsibleUser Get(Guid id,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Events.EventResponsibleUser, DBOs.Events.EventResponsibleUser>(
                "SELECT * FROM \"event_responsible_user\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id }, conn, closeConnection);
        }

        public static Common.Models.Events.EventResponsibleUser Get(Guid eventId, Guid userPId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Events.EventResponsibleUser, DBOs.Events.EventResponsibleUser>(
                "SELECT * FROM \"event_responsible_user\" WHERE \"event_id\"=@EventId AND \"user_pid\"=@userPId AND \"utc_disabled\" is null",
                new { EventId = eventId, UserPId = userPId }, conn, closeConnection);
        }

        public static Common.Models.Events.EventResponsibleUser GetIgnoringDisable(Guid eventId, Guid userPId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Events.EventResponsibleUser, DBOs.Events.EventResponsibleUser>(
                "SELECT * FROM \"event_responsible_user\" WHERE \"event_id\"=@EventId AND \"user_pid\"=@userPId",
                new { EventId = eventId, UserPId = userPId }, conn, closeConnection);
        }

        public static List<Common.Models.Events.EventResponsibleUser> ListForEvent(Guid eventId,
            IDbConnection conn = null, bool closeConnection = true)
        {
            List<Common.Models.Events.EventResponsibleUser> list =
                DataHelper.List<Common.Models.Events.EventResponsibleUser, DBOs.Events.EventResponsibleUser>(
                "SELECT * FROM \"event_responsible_user\" WHERE \"event_id\"=@EventId AND \"utc_disabled\" is null",
                new { EventId = eventId }, conn, false);

            list.ForEach(x =>
            {
                x.User = Account.Users.Get(x.User.PId.Value, conn, false);
            });

            DataHelper.Close(conn, closeConnection);

            return list;
        }

        public static Common.Models.Events.EventResponsibleUser Create(Common.Models.Events.EventResponsibleUser model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, bool closeConnection = true)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;

            DBOs.Events.EventResponsibleUser dbo = Mapper.Map<DBOs.Events.EventResponsibleUser>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            if (conn.Execute("INSERT INTO \"event_responsible_user\" (\"id\", \"event_id\", \"user_pid\", \"responsibility\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                "VALUES (@Id, @EventId, @UserPId, @Responsibility, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                dbo) > 0)
                model.Id = conn.Query<DBOs.Events.EventResponsibleUser>("SELECT currval(pg_get_serial_sequence('event_responsible_user', 'id')) AS \"id\"").Single().Id;

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Events.EventResponsibleUser Edit(Common.Models.Events.EventResponsibleUser model,
            Common.Models.Account.Users modifier,
            IDbConnection conn = null, bool closeConnection = true)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            Common.Models.Events.EventResponsibleUser currentModel = Data.Events.EventResponsibleUser.Get(model.Id.Value);
            model.Event = currentModel.Event;
            DBOs.Tasks.TaskResponsibleUser dbo = Mapper.Map<DBOs.Tasks.TaskResponsibleUser>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            conn.Execute("UPDATE \"event_responsible_user\" SET " +
                "\"event_id\"=@EventId, \"user_pid\"=@UserPId, \"responsibility\"=@Responsibility, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                "WHERE \"id\"=@Id", dbo);

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Events.EventResponsibleUser Disable(Common.Models.Events.EventResponsibleUser model,
            Common.Models.Account.Users disabler,
            IDbConnection conn = null, bool closeConnection = true)
        {
            model.DisabledBy = disabler;
            model.Disabled = DateTime.UtcNow;

            DataHelper.Disable<Common.Models.Events.EventResponsibleUser,
                DBOs.Events.EventResponsibleUser>("event_responsible_user", disabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }

        public static Common.Models.Events.EventResponsibleUser Enable(Common.Models.Events.EventResponsibleUser model,
            Common.Models.Account.Users enabler,
            IDbConnection conn = null, bool closeConnection = true)
        {
            model.ModifiedBy = enabler;
            model.Modified = DateTime.UtcNow;
            model.DisabledBy = null;
            model.Disabled = null;

            DataHelper.Enable<Common.Models.Events.EventResponsibleUser,
                DBOs.Events.EventResponsibleUser>("event_responsible_user", enabler.PId.Value, model.Id, conn, closeConnection);

            return model;
        }
    }
}
