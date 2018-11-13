using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSMY.Util.WeiXin
{
    public class NextExec<TNextObject>
    {
        public bool IsFinished { get; set; }

        public string ExecError { get; set; }

        public bool HasError { get; set; }

        public bool CanExecNextStep { get; set; }

        public TNextObject NextObject { get; set; }
    }
}
