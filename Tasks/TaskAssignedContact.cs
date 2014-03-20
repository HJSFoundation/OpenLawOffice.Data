﻿// -----------------------------------------------------------------------
// <copyright file="TaskAssignedContact.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TaskAssignedContact
    {
        public static Common.Models.Tasks.TaskAssignedContact Get(Guid id)
        {
            return null;
            //DbModels.TaskAssignedContact dbo = DbModels.TaskAssignedContact.FirstOrDefault(
            //    "SELECT * FROM \"task_assigned_contact\" WHERE \"id\"=@0 AND \"utc_disabled\" is null",
            //    id);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(dbo);
        }

        public static Common.Models.Tasks.TaskAssignedContact Get(long taskId, int contactId)
        {
            return null;
            //DbModels.TaskAssignedContact dbo = DbModels.TaskAssignedContact.FirstOrDefault(
            //    "SELECT * FROM \"task_assigned_contact\" WHERE \"matter_id\"=@0 AND \"contact_id\"=@1 AND \"utc_disabled\" is null",
            //    taskId, contactId);
            //if (dbo == null) return null;
            //return Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(dbo);
        }

        public static List<Common.Models.Tasks.TaskAssignedContact> ListForTask(long taskId)
        {
            return null;
            //List<Common.Models.Tasks.TaskAssignedContact> list = new List<Common.Models.Tasks.TaskAssignedContact>();
            //IEnumerable<DbModels.TaskAssignedContact> ie = DbModels.TaskAssignedContact.Query(
            //    "SELECT * FROM \"task_assigned_contact\" WHERE \"task_id\"=@0 \"utc_disabled\" is null",
            //    taskId);
            //foreach (DbModels.TaskAssignedContact dbo in ie)
            //    list.Add(Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(dbo));
            //return list;
        }

        public static Common.Models.Tasks.TaskAssignedContact Create(Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Security.User creator)
        {
            return null;
            //if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            //model.CreatedBy = model.ModifiedBy = creator;
            //model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            //DbModels.TaskAssignedContact dbo = Mapper.Map<DbModels.TaskAssignedContact>(model);
            //dbo.Insert();
            //return model;
        }

        public static Common.Models.Tasks.TaskAssignedContact Edit(Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Security.User modifier)
        {
            return null;
            //model.ModifiedBy = modifier;
            //model.UtcModified = DateTime.UtcNow;
            //DbModels.TaskAssignedContact dbo = Mapper.Map<DbModels.TaskAssignedContact>(model);
            //dbo.Update(new string[] {
            //    "utc_modified",
            //    "modified_by_user_id",
            //    "task_id",
            //    "contact_id",
            //    "assignment_type"
            //});
            //return model;
        }

        public static Common.Models.Tasks.TaskAssignedContact Disable(Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Security.User disabler)
        {
            return null;
            //model.DisabledBy = disabler;
            //model.UtcDisabled = DateTime.UtcNow;
            //DbModels.MatterContact dbo = Mapper.Map<DbModels.MatterContact>(model);
            //dbo.Update(new string[] {
            //    "utc_disabled",
            //    "disabled_by_user_id"
            //});
            //return model;
        }

        public static Common.Models.Tasks.TaskAssignedContact Enable(Common.Models.Tasks.TaskAssignedContact model,
            Common.Models.Security.User enabler)
        {
            return null;
            //model.ModifiedBy = enabler;
            //model.UtcModified = DateTime.UtcNow;
            //model.DisabledBy = null;
            //model.UtcDisabled = null;
            //DbModels.MatterContact dbo = Mapper.Map<DbModels.MatterContact>(model);
            //dbo.Update(new string[] {
            //    "utc_modified",
            //    "modified_by_user_id",
            //    "utc_disabled",
            //    "disabled_by_user_id"
            //});
            //return model;
        }
    }
}
