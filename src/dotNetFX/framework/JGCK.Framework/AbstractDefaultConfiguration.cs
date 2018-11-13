using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework
{
    public abstract class AbstractDefaultConfiguration<TConfigurationSection> where TConfigurationSection : class, new()
    {
        public static TConfigurationSection Instance { get; private set; }

        static AbstractDefaultConfiguration()
        {
            Instance = new TConfigurationSection();
        }

        protected AbstractDefaultConfiguration()
        {
            
        }

        protected virtual string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
