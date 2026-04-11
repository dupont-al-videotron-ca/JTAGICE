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


        #region Fields 

        #endregion


        #region Properties 
        public ActivityInitial Initial { get; set; }

        public ActivityFinal? Final { get; set; }


        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override bool ActivityEntry()
        {
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
                    return this.ActivityStructure.CurrentActivity.ActivityEntry();
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
            if (lastRequest)
            {
                if (this.Final is not null)
                {
                    if(this.Final.IsExitPoint)
                    {
                        return true;
                    }
                    else if (!this.Final.ActivityExit(lastRequest))
                    {
                        Logger.Error($"Activity {this.GetType()} final activity exit returned false, so the composite activity exit will also return false.");
                        return false;
                    }

                    else if (this.HasChild)
                    {
                        this.ActivityStructure.CurrentActivity = this.Nexts[0];
                        return this.ActivityStructure.CurrentActivity.ActivityExit(lastRequest);
                    }
                    else
                    {
                        Logger.Error($"Activity {this.GetType()} has no final activity to exit.");
                        throw new InvalidOperationException($"Activity {this.GetType()} has no final activity to exit.");
                    }
                }
                else
                {
                    Logger.Error($"Activity {this.GetType()} has no final activity to exit.");
                    throw new InvalidOperationException($"Activity {this.GetType()} has no final activity to exit.");
                }
            }
            else
            {
                return true;
            }
        }

        #endregion

    }
}
