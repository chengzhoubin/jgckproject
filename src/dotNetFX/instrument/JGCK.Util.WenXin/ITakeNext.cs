using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSMY.Util.WeiXin
{
    public interface ITakeNext<TNextObject, TCurrentObject>
    {
        TNextObject WillDo(TCurrentObject req);
    }
}
