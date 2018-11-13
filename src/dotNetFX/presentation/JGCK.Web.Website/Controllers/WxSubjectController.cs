using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
using HSMY.BizLogic.Ops;
using HSMY.BizLogic.Org;
using HSMY.Data.Advertisement.Models;
using HSMY.Data.Membership;
using HSMY.Data.MicroClass;
using HSMY.Util;
using HSMY.Web.General.WebAPI;
using HSMY_WxWeb.Models;

namespace HSMY_WxWeb.Controllers
{
    public class WxSubjectController : HSMY_ApiController
    {
        private SpecialTopicService MSubjectService { get; set; }
        private DoctorService MDoctorService { get; set; }
        private IClassService MClassService { get; set; }
        private HospitalService MHospitalService { get; set; }


        [HttpPost]
        public List<VM_Wx_Subject> GetWxSubjects(VM_Wx_SubjectFilter f)
        {
            var exp = PredicateBuilder.Create<subject>(sub => !sub.is_delete);
            if (f.subjectId > 0)
            {
                exp = exp.And(sub => sub.subject_id == f.subjectId);
            }
            var queryOfSubject = (IQueryable<subject>) MSubjectService.GetSubjects(exp);
            queryOfSubject = queryOfSubject
                .OrderByDescending(m => m.subject_id)
                .Skip((f.CurrentIndex - 1) * f.PageSize)
                .Take(f.PageSize);                
            return queryOfSubject.AsEnumerable().Select(m =>
            {
                var vmSubject = new VM_Wx_Subject();
                vmSubject.title = m.title;
                vmSubject.description = m.description;
                vmSubject.subjectId = m.subject_id;
                vmSubject.Images = m.subject_images.Select(img => new VM_Wx_SubjectImage
                {
                    imageurl = img.image_url ?.Replace("http://resource.bestdoctor1.com", "https://resource1.bestdoctor1.com"),
                    url = img.image_jump_url
                }).ToList();
                return vmSubject;
            }).ToList();
        }

        [HttpPost]
        public List<VM_Wx_ClassInfo> GetWxClassOfSubject(VM_Wx_SubjectFilter f)
        {
            var ret = new List<VM_Wx_ClassInfo>();
            var ent = MSubjectService.GetSubject(f.subjectId);
            if (ent == null)
                return ret;

            List<int> selectDiseaseIds = null;
            if (ent.subject_diseases != null && ent.subject_diseases.Count > 0)
            {
                selectDiseaseIds = ent.subject_diseases.Select(m => m.disease_id).ToList();
            }
            var selectedDoctorIds = MDoctorService.GetDoctors(GetDoctorIdExpBuilder(ent)).Select(doc => doc.id)
                .ToList();
            var expClass = PredicateBuilder.Create<microclass>(m => !m.is_deleted);
            var isAddConditionalFilter = false;
            if (selectDiseaseIds != null && selectDiseaseIds.Count > 0)
            {
                isAddConditionalFilter = true;
                expClass = expClass.And(s => selectDiseaseIds.Contains(s.DiseaseCategoryID));
            }
            if (selectedDoctorIds.Count > 0)
            {
                isAddConditionalFilter = true;
                expClass = expClass.And(s =>
                    s.microclass_livedoctor
                        .Select(d => d.doctor_id)
                        .Intersect(selectedDoctorIds).Any());
            }
            if (!isAddConditionalFilter)
            {
                expClass = PredicateBuilder.False<microclass>();
            }
            ret.AddRange(
                MClassService.GetMicroclasses(expClass)
                    .OrderByDescending(c => c.start_time)
                    .Select(m => new VM_Wx_ClassInfo(m,
                        id => MDoctorService.GetDoctor(id),
                        hid => MHospitalService.GetHospitalProfileByID(hid),
                        cid => MClassService.GetOnlineUserCountByType(cid, 0))
                    ));
            return ret;
        }

        private Expression<Func<member, bool>> GetDoctorIdExpBuilder(subject ent)
        {
            var exp = PredicateBuilder.True<member>();
            if (ent.subject_titles != null && ent.subject_titles.Count > 0)
            {
                var titleIds = ent.subject_titles.Select(t => t.title_id).ToList();
                exp = exp.And(m => titleIds.Contains(m.member_doctor_admintitle.title));
            }
            if (ent.subject_hospitals != null && ent.subject_hospitals.Count > 0)
            {
                var hospitalIds = ent.subject_hospitals.Select(h => h.hospital_id).ToList();
                exp = exp.And(m => m.member_doctor_hospital.Any(dh => hospitalIds.Contains(dh.hospital_id)));
            }
            if (ent.subject_sections != null && ent.subject_sections.Count > 0)
            {
                var sectionIds = ent.subject_sections.Select(m => m.section_id).ToList();
                exp = exp.And(m => m.member_doctor_hospital_section.Any(dhs => sectionIds.Contains(dhs.hospital_section_id)));
            }
            if (ent.subject_doctors != null && ent.subject_doctors.Count > 0)
            {
                var doctorIds = ent.subject_doctors.Select(d => d.doctor_id).ToList();
                exp = exp.And(m => doctorIds.Contains(m.member_doctor_profile.doctor_id));
            }
            return exp;

        }
    }
}