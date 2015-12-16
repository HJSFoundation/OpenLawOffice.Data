﻿// -----------------------------------------------------------------------
// <copyright file="NoteTask.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Notes
{
    using System;
    using System.Data;
    using AutoMapper;
    using Dapper;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NoteTask
    {
        public static Common.Models.Notes.NoteTask Get(
            long taskId, 
            Guid noteId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Notes.NoteTask, DBOs.Notes.NoteTask>(
                "SELECT * FROM \"note_task\" WHERE \"task_id\"=@TaskId AND \"note_id\"=@NoteId AND \"utc_disabled\" is null",
                new { TaskId = taskId, NoteId = noteId }, conn, closeConnection);
        }

        public static Common.Models.Notes.NoteTask Get(
            Transaction t,
            long taskId,
            Guid noteId)
        {
            return Get(taskId, noteId, t.Connection, false);
        }

        public static Common.Models.Notes.NoteTask GetIgnoringDisable(
            long taskId, 
            Guid noteId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Notes.NoteTask, DBOs.Notes.NoteTask>(
                "SELECT * FROM \"note_task\" WHERE \"task_id\"=@TaskId AND \"note_id\"=@NoteId",
                new { TaskId = taskId, NoteId = noteId }, conn, closeConnection);
        }

        public static Common.Models.Notes.NoteTask GetIgnoringDisable(
            Transaction t,
            long taskId,
            Guid noteId)
        {
            return GetIgnoringDisable(taskId, noteId, t.Connection, false);
        }

        public static Common.Models.Tasks.Task GetRelatedTask(
            Guid noteId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT \"task\".* FROM \"note_task\" JOIN \"task\" ON \"note_task\".\"task_id\"=\"task\".\"id\" " +
                "WHERE \"note_task\".\"note_id\"=@NoteId " +
                "AND \"note_task\".\"utc_disabled\" is null " +
                "AND \"task\".\"utc_disabled\" is null ",
                new { NoteId = noteId }, conn, closeConnection);
        }

        public static Common.Models.Tasks.Task GetRelatedTask(
            Transaction t,
            Guid noteId)
        {
            return GetRelatedTask(noteId, t.Connection, false);
        }

        public static List<Common.Models.Notes.Note> ListForTask(
            long taskId,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            return DataHelper.List<Common.Models.Notes.Note, DBOs.Notes.Note>(
                "SELECT * FROM \"note\" WHERE \"id\" IN (SELECT \"note_id\" FROM \"note_task\" WHERE \"task_id\"=@TaskId) AND " +
                "\"utc_disabled\" is null ORDER BY \"timestamp\" DESC",
                new { TaskId = taskId }, conn, closeConnection);
        }

        public static List<Common.Models.Notes.Note> ListForTask(
            Transaction t,
            long taskId)
        {
            return ListForTask(taskId, t.Connection, false);
        }

        public static Common.Models.Notes.NoteTask Create(
            Common.Models.Notes.NoteTask model,
            Common.Models.Account.Users creator,
            IDbConnection conn = null, 
            bool closeConnection = true)
        {
            model.Created = model.Modified = DateTime.UtcNow;
            model.CreatedBy = model.ModifiedBy = creator;
            DBOs.Notes.NoteTask dbo = Mapper.Map<DBOs.Notes.NoteTask>(model);

            conn = DataHelper.OpenIfNeeded(conn);

            Common.Models.Notes.NoteTask currentModel = Get(model.Task.Id.Value, model.Note.Id.Value, conn, false);

            if (currentModel != null)
            { // Update
                conn.Execute("UPDATE \"note_task\" SET \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId, " +
                    "\"utc_disabled\"=null, \"disabled_by_user_pid\"=null WHERE \"id\"=@Id", dbo);
                model.Created = currentModel.Created;
                model.CreatedBy = currentModel.CreatedBy;
            }
            else
            { // Create
                if (conn.Execute("INSERT INTO \"note_task\" (\"id\", \"note_id\", \"task_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @NoteId, @TaskId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo) > 0)
                    model.Id = conn.Query<DBOs.Notes.NoteTask>("SELECT currval(pg_get_serial_sequence('event_assigned_contact', 'id')) AS \"id\"").Single().Id;
            }

            DataHelper.Close(conn, closeConnection);

            return model;
        }

        public static Common.Models.Notes.NoteTask Create(
            Transaction t,
            Common.Models.Notes.NoteTask model,
            Common.Models.Account.Users creator)
        {
            return Create(model, creator, t.Connection, false);
        }
    }
}