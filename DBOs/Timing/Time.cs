﻿// -----------------------------------------------------------------------
// <copyright file="Time.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Timing
{
    using System;
    using AutoMapper;

    /// <summary>
    /// Represents a quantity of time consumed by a specific contact
    /// </summary>
    [Common.Models.MapMe]
    public class Time : Core
    {
        [ColumnMapping(Name = "id")]
        public Guid Id { get; set; }

        [ColumnMapping(Name = "start")]
        public DateTime Start { get; set; }

        [ColumnMapping(Name = "stop")]
        public DateTime? Stop { get; set; }

        [ColumnMapping(Name = "worker_contact_id")]
        public int WorkerContactId { get; set; }

        [ColumnMapping(Name = "time_category_id")]
        public int? TimeCategoryId { get; set; }

        [ColumnMapping(Name = "details")]
        public string Details { get; set; }

        [ColumnMapping(Name = "billable")]
        public bool Billable { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Time), new ColumnAttributeTypeMapper<Time>());
            Mapper.CreateMap<DBOs.Timing.Time, Common.Models.Timing.Time>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Created, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcCreated.ToSystemTime();
                }))
                .ForMember(dst => dst.Modified, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcModified.ToSystemTime();
                }))
                .ForMember(dst => dst.Disabled, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcDisabled.ToSystemTime();
                }))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Account.Users()
                    {
                        PId = db.CreatedByUserPId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Account.Users()
                    {
                        PId = db.ModifiedByUserPId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (!db.DisabledByUserPId.HasValue) return null;
                    return new Common.Models.Account.Users()
                    {
                        PId = db.DisabledByUserPId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Start, opt => opt.ResolveUsing(db =>
                {
                    return db.Start.ToSystemTime();
                }))
                .ForMember(dst => dst.Stop, opt => opt.ResolveUsing(db =>
                {
                    return db.Stop.ToSystemTime();
                }))
                .ForMember(dst => dst.Worker, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.WorkerContactId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.TimeCategory, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Timing.TimeCategory()
                    {
                        Id = db.TimeCategoryId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Details, opt => opt.MapFrom(src => src.Details))
                .ForMember(dst => dst.Billable, opt => opt.MapFrom(src => src.Billable));

            Mapper.CreateMap<Common.Models.Timing.Time, DBOs.Timing.Time>()
                .ForMember(dst => dst.UtcCreated, opt => opt.ResolveUsing(db =>
                {
                    return db.Created.ToDbTime();
                }))
                .ForMember(dst => dst.UtcModified, opt => opt.ResolveUsing(db =>
                {
                    return db.Modified.ToDbTime();
                }))
                .ForMember(dst => dst.UtcDisabled, opt => opt.ResolveUsing(db =>
                {
                    return db.Disabled.ToDbTime();
                }))
                .ForMember(dst => dst.CreatedByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.CreatedBy == null || !model.CreatedBy.PId.HasValue)
                        return Guid.Empty;
                    return model.CreatedBy.PId.Value;
                }))
                .ForMember(dst => dst.ModifiedByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.ModifiedBy == null || !model.ModifiedBy.PId.HasValue)
                        return Guid.Empty;
                    return model.ModifiedBy.PId.Value;
                }))
                .ForMember(dst => dst.DisabledByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.DisabledBy == null) return null;
                    return model.DisabledBy.PId;
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Start, opt => opt.ResolveUsing(db =>
                {
                    return db.Start.ToDbTime();
                }))
                .ForMember(dst => dst.Stop, opt => opt.ResolveUsing(db =>
                {
                    return db.Stop.ToDbTime();
                }))
                .ForMember(dst => dst.WorkerContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Worker == null) return null;
                    return model.Worker.Id;
                }))
                .ForMember(dst => dst.TimeCategoryId, opt => opt.ResolveUsing(model =>
                {
                    if (model.TimeCategory == null) return null;
                    return model.TimeCategory.Id;
                }))
                .ForMember(dst => dst.Details, opt => opt.MapFrom(src => src.Details))
                .ForMember(dst => dst.Billable, opt => opt.MapFrom(src => src.Billable));
        }
    }
}