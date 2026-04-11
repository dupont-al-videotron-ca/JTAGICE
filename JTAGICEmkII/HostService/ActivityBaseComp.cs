using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    public abstract class ActivityBaseComp : ActivityBase
    {

        #region Constructors 

        internal ActivityBaseComp(StructureActivity activityStructure) : base(activityStructure)
        {
            _nexts = new List<IActivityElement>();
            _parents = new List<IActivityElement>();

        }

        internal ActivityBaseComp(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure)
        {
            _nexts = new List<IActivityElement>();
            _parents = new List<IActivityElement>();

            if (parent is not null)
                _parents.Add(parent);
        }

        internal ActivityBaseComp(StructureActivity activityStructure, IActivityElement? parent, IEnumerable<IActivityElement> nextElements) : this(activityStructure, parent)
        {
            ArgumentNullException.ThrowIfNull(nextElements);
            _nexts.AddRange(nextElements);

        }

        #endregion


        #region Fields 

        private readonly List<IActivityElement> _nexts;
        private readonly List<IActivityElement> _parents;

        public IActivityElement? Parent
        {
            get
            {
                if (_parents.Count > 0)
                {
                    return _parents[0];
                }
                else
                {
                    return null;
                }
            }
        }
        public bool HasParent => Parent is not null;

        public bool HasChild => _nexts.Count != 0;

        protected internal int _nextIndex = -1;


        #endregion


        #region Properties 

        public IReadOnlyList<IActivityElement> Nexts => _nexts;

        public IReadOnlyList<IActivityElement> Parents => _parents;

        #endregion


        #region Public Methods 


        public override bool ActivityExit(bool lastRequest)
        {
            Logger.Debug($"{this.GetType()} Executing activity exit called with lastRequest: {lastRequest}.");
            if (lastRequest)
            {
                if (this.HasChild && _nextIndex != -1 && this.Nexts.Count > _nextIndex)
                {
                    this.ActivityStructure.CurrentActivity = this.Nexts[_nextIndex];
                    _nextIndex = -1;
                    var result = this.ActivityStructure.CurrentActivity.ActivityEntry();
                    return result;
                }
                else
                {
                    Logger.Error($"{this.GetType()} ActivityExit called with lastRequest true but no next activity to transition to.");
                    _nextIndex = -1;
                    return false;
                }
            }
            else
            {
                // keep _nextIndex until last request is done.;
                return false;
            }
        }

        public virtual void AddNext(IActivityElement element)
        {
            _nexts.Add(element);
        }

        public virtual void AddNextRange(IEnumerable<IActivityElement> elements)
        {
            _nexts.AddRange(elements);
        }

        public virtual void AddParent(IActivityElement element)
        {
            _parents.Add(element);
        }

        public virtual void AddParentRange(IEnumerable<IActivityElement> elements)
        {
            _parents.AddRange(elements);
        }

        #endregion
    }
}
