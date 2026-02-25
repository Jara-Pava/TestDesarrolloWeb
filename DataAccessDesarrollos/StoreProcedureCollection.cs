using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos
{
    public class StoreProcedureCollection : ConfigurationElementCollection
    {
        public StoreProcedureCollection()
        {
        }

        public StoreProcedure this[int index]
        {
            get { return (StoreProcedure)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(StoreProcedure depConfig)
        {
            BaseAdd(depConfig);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new StoreProcedure();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StoreProcedure)element).Code;
        }

        public void Remove(StoreProcedure depConfig)
        {
            BaseRemove(depConfig.Code);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }
}
