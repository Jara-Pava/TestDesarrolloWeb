using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos
{
    public class StoreProcedure : ConfigurationSection
    {
        [ConfigurationProperty("Code", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Code
        {
            get
            {
                return (string)this["Code"];
            }
            set
            {
                this["Code"] = value;
            }
        }

        [ConfigurationProperty("Name", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Name
        {
            get
            {
                return (string)this["Name"];
            }
            set
            {
                this["Name"] = value;
            }
        }
    }
}
