﻿// -----------------------------------------------------------------------
// <copyright file="Matter.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Matters
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class Matter : Core
    {
        [ColumnMapping(Name = "id")]
        public Guid Id { get; set; }

        [ColumnMapping(Name = "id_int")]
        public long IdInt { get; set; }

        [ColumnMapping(Name = "matter_type_id")]
        public int? MatterTypeId { get; set; }

        [ColumnMapping(Name = "title")]
        public string Title { get; set; }

        [ColumnMapping(Name = "parent_id")]
        public Guid? ParentId { get; set; }

        [ColumnMapping(Name = "synopsis")]
        public string Synopsis { get; set; }

        [ColumnMapping(Name = "active")]
        public bool Active { get; set; }

        [ColumnMapping(Name = "case_number")]
        public string CaseNumber { get; set; }

        [ColumnMapping(Name = "lead_attorney_contact_id")]
        public int? LeadAttorneyContactId { get; set; }

        [ColumnMapping(Name = "bill_to_contact_id")]
        public int? BillToContactId { get; set; }

        [ColumnMapping(Name = "minimum_charge")]
        public decimal? MinimumCharge { get; set; }

        [ColumnMapping(Name = "estimated_charge")]
        public decimal? EstimatedCharge { get; set; }

        [ColumnMapping(Name = "maximum_charge")]
        public decimal? MaximumCharge { get; set; }

        [ColumnMapping(Name = "default_billing_rate_id")]
        public int? DefaultBillingRateId { get; set; }

        [ColumnMapping(Name = "billing_group_id")]
        public int? BillingGroupId { get; set; }

        [ColumnMapping(Name = "override_matter_rate_with_employee_rate")]
        public bool OverrideMatterRateWithEmployeeRate { get; set; }

        [ColumnMapping(Name = "attorney_for_party_title")]
        public string AttorneyForPartyTitle { get; set; }

        [ColumnMapping(Name = "court_type_id")]
        public int? CourtTypeId { get; set; }

        [ColumnMapping(Name = "court_geographical_jurisdiction_id")]
        public int? CourtGeographicalJurisdictionId { get; set; }

        [ColumnMapping(Name = "court_sitting_in_city_id")]
        public int? CourtSittingInCityId { get; set; }

        [ColumnMapping(Name = "caption_plaintiff_or_subject_short")]
        public string CaptionPlaintiffOrSubjectShort { get; set; }

        [ColumnMapping(Name = "caption_plaintiff_or_subject_regular")]
        public string CaptionPlaintiffOrSubjectRegular { get; set; }

        [ColumnMapping(Name = "caption_plaintiff_or_subject_long")]
        public string CaptionPlaintiffOrSubjectLong { get; set; }

        [ColumnMapping(Name = "caption_other_party_short")]
        public string CaptionOtherPartyShort { get; set; }

        [ColumnMapping(Name = "caption_other_party_regular")]
        public string CaptionOtherPartyRegular { get; set; }

        [ColumnMapping(Name = "caption_other_party_long")]
        public string CaptionOtherPartyLong { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Matter), new ColumnAttributeTypeMapper<Matter>());
            Mapper.CreateMap<DBOs.Matters.Matter, Common.Models.Matters.Matter>()
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
                .ForMember(dst => dst.IdInt, opt => opt.MapFrom(src => src.IdInt))
                .ForMember(dst => dst.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dst => dst.MatterType, opt => opt.ResolveUsing(db =>
                {
                    if (!db.MatterTypeId.HasValue) return null;
                    return new Common.Models.Matters.MatterType()
                    {
                        Id = db.MatterTypeId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dst => dst.CaseNumber, opt => opt.MapFrom(src => src.CaseNumber))
                .ForMember(dst => dst.LeadAttorney, opt => opt.ResolveUsing(db =>
                {
                    if (!db.LeadAttorneyContactId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.LeadAttorneyContactId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.BillTo, opt => opt.ResolveUsing(db =>
                {
                    if (!db.BillToContactId.HasValue) return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = db.BillToContactId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.MinimumCharge, opt => opt.MapFrom(src => src.MinimumCharge))
                .ForMember(dst => dst.EstimatedCharge, opt => opt.MapFrom(src => src.EstimatedCharge))
                .ForMember(dst => dst.MaximumCharge, opt => opt.MapFrom(src => src.MaximumCharge))
                .ForMember(dst => dst.DefaultBillingRate, opt => opt.ResolveUsing(db =>
                {
                    if (!db.DefaultBillingRateId.HasValue) return null;
                    return new Common.Models.Billing.BillingRate()
                    {
                        Id = db.DefaultBillingRateId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.BillingGroup, opt => opt.ResolveUsing(db =>
                {
                    if (!db.BillingGroupId.HasValue) return null;
                    return new Common.Models.Billing.BillingGroup()
                    {
                        Id = db.BillingGroupId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.OverrideMatterRateWithEmployeeRate, opt => opt.MapFrom(src => src.OverrideMatterRateWithEmployeeRate))
                .ForMember(dst => dst.AttorneyForPartyTitle, opt => opt.MapFrom(src => src.AttorneyForPartyTitle))
                .ForMember(dst => dst.CourtType, opt => opt.ResolveUsing(db =>
                {
                    if (!db.CourtTypeId.HasValue) return null;
                    return new Common.Models.Billing.BillingGroup()
                    {
                        Id = db.CourtTypeId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.CourtGeographicalJurisdiction, opt => opt.ResolveUsing(db =>
                {
                    if (!db.CourtGeographicalJurisdictionId.HasValue) return null;
                    return new Common.Models.Billing.BillingGroup()
                    {
                        Id = db.CourtGeographicalJurisdictionId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.CourtSittingInCity, opt => opt.ResolveUsing(db =>
                {
                    if (!db.CourtSittingInCityId.HasValue) return null;
                    return new Common.Models.Billing.BillingGroup()
                    {
                        Id = db.CourtSittingInCityId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.CaptionPlaintiffOrSubjectShort, opt => opt.MapFrom(src => src.CaptionPlaintiffOrSubjectShort))
                .ForMember(dst => dst.CaptionPlaintiffOrSubjectRegular, opt => opt.MapFrom(src => src.CaptionPlaintiffOrSubjectRegular))
                .ForMember(dst => dst.CaptionPlaintiffOrSubjectLong, opt => opt.MapFrom(src => src.CaptionPlaintiffOrSubjectLong))
                .ForMember(dst => dst.CaptionOtherPartyShort, opt => opt.MapFrom(src => src.CaptionOtherPartyShort))
                .ForMember(dst => dst.CaptionOtherPartyRegular, opt => opt.MapFrom(src => src.CaptionOtherPartyRegular))
                .ForMember(dst => dst.CaptionOtherPartyLong, opt => opt.MapFrom(src => src.CaptionOtherPartyLong));

            Mapper.CreateMap<Common.Models.Matters.Matter, DBOs.Matters.Matter>()
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
                .ForMember(dst => dst.IdInt, opt => opt.MapFrom(src => src.IdInt))
                .ForMember(dst => dst.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dst => dst.MatterTypeId, opt => opt.ResolveUsing(model =>
                {
                    if (model.MatterType == null) return null;
                    return model.MatterType.Id;
                }))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dst => dst.CaseNumber, opt => opt.MapFrom(src => src.CaseNumber))
                .ForMember(dst => dst.LeadAttorneyContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.LeadAttorney == null) return null;
                    return model.LeadAttorney.Id;
                }))
                .ForMember(dst => dst.BillToContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.BillTo == null) return null;
                    return model.BillTo.Id;
                }))
                .ForMember(dst => dst.MinimumCharge, opt => opt.MapFrom(src => src.MinimumCharge))
                .ForMember(dst => dst.EstimatedCharge, opt => opt.MapFrom(src => src.EstimatedCharge))
                .ForMember(dst => dst.MaximumCharge, opt => opt.MapFrom(src => src.MaximumCharge))
                .ForMember(dst => dst.DefaultBillingRateId, opt => opt.ResolveUsing(model =>
                {
                    if (model.DefaultBillingRate == null) return null;
                    return model.DefaultBillingRate.Id;
                }))
                .ForMember(dst => dst.BillingGroupId, opt => opt.ResolveUsing(model =>
                {
                    if (model.BillingGroup == null) return null;
                    return model.BillingGroup.Id;
                }))
                .ForMember(dst => dst.OverrideMatterRateWithEmployeeRate, opt => opt.MapFrom(src => src.OverrideMatterRateWithEmployeeRate))
                .ForMember(dst => dst.AttorneyForPartyTitle, opt => opt.MapFrom(src => src.AttorneyForPartyTitle))
                .ForMember(dst => dst.CourtTypeId, opt => opt.ResolveUsing(model =>
                {
                    if (model.CourtType == null) return null;
                    return model.CourtType.Id;
                }))
                .ForMember(dst => dst.CourtGeographicalJurisdictionId, opt => opt.ResolveUsing(model =>
                {
                    if (model.CourtGeographicalJurisdiction == null) return null;
                    return model.CourtGeographicalJurisdiction.Id;
                }))
                .ForMember(dst => dst.CourtSittingInCityId, opt => opt.ResolveUsing(model =>
                {
                    if (model.CourtSittingInCity == null) return null;
                    return model.CourtSittingInCity.Id;
                }))
                .ForMember(dst => dst.CaptionPlaintiffOrSubjectShort, opt => opt.MapFrom(src => src.CaptionPlaintiffOrSubjectShort))
                .ForMember(dst => dst.CaptionPlaintiffOrSubjectRegular, opt => opt.MapFrom(src => src.CaptionPlaintiffOrSubjectRegular))
                .ForMember(dst => dst.CaptionPlaintiffOrSubjectLong, opt => opt.MapFrom(src => src.CaptionPlaintiffOrSubjectLong))
                .ForMember(dst => dst.CaptionOtherPartyShort, opt => opt.MapFrom(src => src.CaptionOtherPartyShort))
                .ForMember(dst => dst.CaptionOtherPartyRegular, opt => opt.MapFrom(src => src.CaptionOtherPartyRegular))
                .ForMember(dst => dst.CaptionOtherPartyLong, opt => opt.MapFrom(src => src.CaptionOtherPartyLong));
        }
    }
}