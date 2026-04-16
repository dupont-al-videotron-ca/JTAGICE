using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    public abstract class ActivityComposite : ActivityBaseComp
    {


        #region Constructors 
        internal ActivityComposite(StructureActivity activityStructure) : this(activityStructure, null)
        {
        }

        internal ActivityComposite(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
            Initial = new ActivityInitial(activityStructure);
            
        }

        #endregion

        #region Properties 

        public ActivityInitial Initial { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override bool ActivityEntry()
        {
            Logger.Debug($"ActivityComposite: ActivityEntry has been called.");
            if (!this.HasParent)
            {
                Logger.Error($"Activity {this.GetType()} has an invalid Final activity, which is not supported yet.");
                throw new InvalidOperationException($"Activity {this.GetType()} has an invalid Final activity, which is not supported yet.");
            }

            if (this.Initial is not null)
            {
                if (!this.Initial.HasChild)
                {
                    Logger.Error($"Activity {this.GetType()} has an initial activity that has no child activities, which is not supported yet.");
                    throw new InvalidOperationException($"Activity {this.GetType()} has an initial activity that has no child activities, which is not supported yet.");
                }
                else
                {
                    this.ActivityStructure.CurrentActivity = this.Initial;
                    return base.ActivityEntry();
                }
            }
            else
            {
                Logger.Error($"Activity {this.GetType()} has no initial activity to enter.");
                throw new InvalidOperationException($"Activity {this.GetType()} has no initial activity to enter.");
            }
        }

        public override bool ActivityExit(bool lastRequest)
        {
            return base.ActivityExit(lastRequest);

        }

        #endregion

    }
}
