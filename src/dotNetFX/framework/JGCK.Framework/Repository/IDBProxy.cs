﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework
{
    public interface IDBProxy : IDisposable
    {
        int Commit();

        Task<int> CommitAsync();
    }
}
