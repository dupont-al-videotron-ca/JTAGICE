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

        public IActivityElement? Child
        {
            get
            {
                if (_nexts.Count > 0)
                {
                    return _nexts[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public bool HasParent => Parent is not null;

        public bool HasChild => _nexts.Count != 0;


        #endregion


        #region Properties 

        protected List<IActivityElement> Nexts => _nexts;

        protected List<IActivityElement> Parents => _parents;

        public IActivityElement? NextActivity { get; set; }
        #endregion

        #region Public Methods 

        protected IActivityElement? Find<T>() where T : IActivityElement
        {
            if (this.GetType() is T)
                return this;

            var nextIndex = this.Nexts.FindIndex(n => n.GetType() is T);
            if (nextIndex == -1)
            {
                IActivityElement? found = this.ActivityStructure.Find<T>();
                return found;
            }
            else
            {
                return _nexts[nextIndex];
            }
        }

        public override bool ActivityEntry()
        {
            Logger.Debug($"{this.GetType()} has {this._nexts.Count} next activities.");
            Logger.Debug($"{this.GetType()} has {this._parents.Count} parent activities.");
            return base.ActivityEntry();
        }

        public override bool ActivityExit(bool lastRequest)
        {
            if (lastRequest)
            {
                if (NextActivity == null)
                {
                    Logger.Error($"{this.GetType()} ActivityExit called with lastRequest true but no next activity to transition to.");
                    return false;
                }
                else
                {
                    this.ActivityStructure.CurrentActivity = this.NextActivity;
                    return true;
                }
            }
            else
            {
                // keep _nextIndex until last request is done.;
                Logger.Debug($"{this.GetType()} ActivityExit called with lastRequest false.");
                return true;
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
