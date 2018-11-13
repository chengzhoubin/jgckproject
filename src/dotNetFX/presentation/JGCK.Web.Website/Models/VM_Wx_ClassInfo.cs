using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HSMY.Data.BasicMeta;
using HSMY.Data.Membership;
using HSMY.Data.MicroClass;

namespace HSMY_WxWeb.Models
{
    /// <summary>
    /// 微信端课程
    /// </summary>
    public class VM_Wx_ClassInfo
    {
        public long id { get; set; }
        public string title { get; set; }

        public string time { get; set; }

        public int status { get; set; }

        public int viewcount { get; set; }

        public string desc { get; set; }

        public List<VM_Wx_Doctor> doctors { get; set; }

        public List<VM_Wx_ClassImage> images { get; set; }

        public VM_Wx_ClassInfo(microclass m,
            Func<long, member> getDoctorEntityFunc,
            Func<long, admin_hospital_profile> getHospitalEntityFunc,
            Func<long, int> getEnterIntoLiveroomCountFunc)
        {
            id = m.id;
            title = m.title;
            time = m.start_time.ToString("yyyy-MM-dd HH:mm");
            desc = m.description;
            images = m.microclass_file.Select(file => new VM_Wx_ClassImage
            {
                imagetype = file.file_type,
                imageurl = file.file_path?.Replace("http://resource.bestdoctor1.com","https://resource1.bestdoctor1.com")
            }).ToList();
            doctors = m.microclass_livedoctor.Select(doc =>
            {
                var doctorProfile = getDoctorEntityFunc(doc.doctor_id);
                var hospitalProfile = getHospitalEntityFunc(doctorProfile.member_doctor_hospital
                            .Select(h => h.hospital_id)
                            .FirstOrDefault());
                var vmDoctor = new VM_Wx_Doctor
                {
                    name = doctorProfile.name,
                    desc = doctorProfile.member_doctor_profile?.background,
                    iconSrc = doctorProfile.member_doctor_profile?.headpic?.Replace("http://resource.bestdoctor1.com", "https://resource1.bestdoctor1.com"),
                    url = "",
                    jobtitle = doctorProfile.member_doctor_admintitle?.admintitle?.title,
                    jobtitle_ext = doctorProfile.member_doctor_profile?.good_field,
                    firstWorkHospital = hospitalProfile?.hospital_name,
                    firstWorkHospitalshort = hospitalProfile?.Aliasname.FirstOrDefault()?.alias_name,
                };
                return vmDoctor;
            }).ToList();
            viewcount = Convert.ToInt32(m.view_count); //getEnterIntoLiveroomCountFunc(m.id);
            if (!m.is_fine)
                status = 0;
            else if (m.is_fine && !m.end_time.HasValue)
                status = 1;
            else if (m.end_time.HasValue)
                status = 2;
        }
    }

    public class VM_Wx_ClassImage
    {
        public string imageurl { get; set; }

        public int imagetype { get; set; }
    }
}