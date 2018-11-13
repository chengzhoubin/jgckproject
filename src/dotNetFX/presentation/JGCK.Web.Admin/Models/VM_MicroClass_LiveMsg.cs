using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using HSMY.Data.MicroClass;
using HSMY.Web.General.ChatMsg;
using Newtonsoft.Json;

namespace HSMY_AdminWeb.Models
{
    /// <summary>
    /// 在线消息
    /// </summary>
    [DataContract]
    public class VM_MicroClass_LiveMsg
    {
        public long ID { get; set; }

        public long UserID { get; set; }

        public string IM_UserName { get; set; }

        public string IM_UserHeaderUrl { get; set; }

        public long CourseID { get; set; }

        public int UserType { get; set; }

        public int MsgType { get; set; }

        public string Msg { get; set; }

        public string Msg_Ext { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public DateTime SendTime { get; set; } = DateTime.Now;

        public VM_MicroClass_LiveMsg toBeRepliedMsg { get; set; }

        public int SendType { get; set; }

        public bool IsAllowSubmit { get; set; } = true;

        public bool IsHasDeleted { get; set; }

        /// <summary>
        /// 是否主持人模式
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public bool isHostPattern { get; set; }

        IDictionary<MicroClass_UserType, List<int>> UserTypeWithPosition => new
            Dictionary<MicroClass_UserType, List<int>>
            {
                {MicroClass_UserType.User, new List<int>() {2,3}},
                {MicroClass_UserType.Doctor, new List<int>() {1, 3}},
                {MicroClass_UserType.Host, new List<int>() {1, 4}}
            };

        private IDictionary<MicroClass_MsgSendType, List<int>> SendTypeWithPosition => new
            Dictionary<MicroClass_MsgSendType, List<int>>
            {
                {MicroClass_MsgSendType.DOCTOR_REPLY, new List<int>() {3}},
                {MicroClass_MsgSendType.PUBLISH, new List<int>() {4}},
                {MicroClass_MsgSendType.RESEND_DOCTOR, new List<int>() {2, 3}},
                {MicroClass_MsgSendType.USER_COMMENTS, new List<int>() {1, 2, 3}}
            };

        /// <summary>
        /// 显示位置1.直播间 2.互动位置 3.回复位置 4 置顶位置
        /// </summary>
        public List<int> DisplayPosition
        {
            get
            {
                var sendType = (MicroClass_MsgSendType) SendType;
                var userType = (MicroClass_UserType) UserType;
                var position = UserTypeWithPosition[userType].Intersect(SendTypeWithPosition[sendType]).ToList();
                //if (!isHostPattern)
                //{
                //    position.Remove(2);
                //}
                if (userType == MicroClass_UserType.Doctor && sendType == MicroClass_MsgSendType.USER_COMMENTS)
                {
                    position.Remove(3);
                }
                return position;
            }
            set
            {
                ;
            }
        }

        public microclass_livemsg ToCast(microclass_livemsg rpcLivemsg)
        {
            rpcLivemsg.id = ID;
            rpcLivemsg.from_user_type = UserType;
            rpcLivemsg.course_id = CourseID;
            rpcLivemsg.from_im_headpic = IM_UserHeaderUrl;
            rpcLivemsg.from_im_nickname = IM_UserName;
            rpcLivemsg.from_user_id = UserID;
            rpcLivemsg.message = Msg;
            rpcLivemsg.message_ext = Msg_Ext;
            rpcLivemsg.message_type = MsgType;
            rpcLivemsg.send_type = SendType;
            return rpcLivemsg;
        }

        public VM_MicroClass_LiveMsg(microclass_livemsg rpcLivemsg)
        {
            this.ID = rpcLivemsg.id;
            this.CourseID = rpcLivemsg.course_id;
            this.IM_UserHeaderUrl = rpcLivemsg.from_im_headpic;
            this.IM_UserName = rpcLivemsg.from_im_nickname;
            this.Msg = rpcLivemsg.message;
            this.Msg_Ext = rpcLivemsg.message_ext;
            this.MsgType = rpcLivemsg.message_type;
            this.UserID = rpcLivemsg.from_user_id;
            this.SendTime = rpcLivemsg.create_date;
            this.SendType = rpcLivemsg.send_type;
            this.UserType = rpcLivemsg.from_user_type;
            this.IsHasDeleted = rpcLivemsg.is_deleted;
            if (rpcLivemsg.reply_msg != null)
            {
                this.toBeRepliedMsg = new VM_MicroClass_LiveMsg(rpcLivemsg.reply_msg);
            }
        }

        public VM_MicroClass_LiveMsg()
        {
        }
    }
}