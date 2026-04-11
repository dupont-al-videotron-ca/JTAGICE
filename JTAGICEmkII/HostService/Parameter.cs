using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;

namespace JTAGICEmkII.HostService
{

    public class Parameter
    {
        #region Constructors 

        public Parameter(Master.ParameterEnum parameterId, ParameterOption option, int size = 1, bool notUsed = false)
        {
            this.ParameterId = parameterId;
            this.Value = 0;
            this.Option = option;
            this.Size = size;
            this.IsUsed = !notUsed;
        }

        public Master.ParameterEnum ParameterId { get; set; }
        public uint Value { get; set; }
        public ParameterOption Option { get; private set; }
        public int Size { get; private set; }
        public bool IsUsed { get; private set; }

        public bool IsRead => Option.HasFlag(ParameterOption.ReadOnly) || Option.HasFlag(ParameterOption.ReadWrite);
        
        public bool IsWrite => Option.HasFlag(ParameterOption.WriteOnly) || Option.HasFlag(ParameterOption.ReadWrite);

        #endregion
    }
}