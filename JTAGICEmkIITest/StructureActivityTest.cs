using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII.HostService;
using JTAGICEmkIITest.Moq;
using log4net;
using Xunit;
using Moq;

namespace JTAGICEmkIITest
{
    public class StructureActivityTest : XUnitTestBase
    {
        private HostDeviceServiceMoq _hostService;
        private Mock<IVisitorActivity> _visitorActivityMoq;

        public StructureActivityTest()
        {
            _hostService = new HostDeviceServiceMoq();

            _visitorActivityMoq = this.MockRepository.Create<IVisitorActivity>();
        }

        [Fact]
        public void StructureActivity__shall_Constructor()
        {
            var test = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            Assert.NotNull(test);
            Assert.Null(test.CurrentActivity);
            Assert.Same(_hostService, test.HostService);
            Assert.Empty(test);
            Assert.False(test.IsReadOnly);
        }

        [Fact]
        public void StructureActivity__shall_GetEnumerator()
        {
            //--- Setup
            var test = new JTAGICEmkII.HostService.StructureActivity(_hostService);

            //--- Expectations

            //--- Action

            //--- Verification
            Assert.NotNull(test.GetEnumerator());
            Assert.NotNull(((IEnumerable)test).GetEnumerator());
            Assert.False(test.Any());
        }


        [Fact]
        public void StructureActivity_Shall_Add_Item()
        {
            //--- Setup
            var test = new JTAGICEmkII.HostService.StructureActivity(_hostService);

            //--- Expectations

            //--- Action
            var item = new ActivityInitial(test);

            //--- Verification
            Assert.Single(test);
            Assert.Same(item, test.First());
        }

        [Fact]
        public void StructureActivity_Shall_Remove_Item()
        {
            //--- Setup
            var test = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var item = new ActivityInitial(test);
            Assert.NotEmpty(test);

            //--- Expectations

            //--- Action
            test.Remove(item);

            //--- Verification
            Assert.Empty(test);
        }

        [Fact]
        public void StructureActivity_Shall_Contains_Item()
        {
            //--- Setup
            var test = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var item = new ActivityInitial(test);
            Assert.NotEmpty(test);

            //--- Expectations

            //--- Action
            var contains = test.Contains(item);

            //--- Verification
            Assert.True(contains);
            Assert.Single(test);
        }

        [Fact]
        public void StructureActivity_Shall_Clear_Item()
        {
            //--- Setup
            var test = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var item = new ActivityInitial(test);
            Assert.NotEmpty(test);

            //--- Expectations

            //--- Action
            test.Clear();

            //--- Verification
            Assert.Empty(test);
        }

        [Fact]
        public void StructureActivity_Shall_CopyTo_Item()
        {
            //--- Setup
            var test = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var item = new ActivityInitial(test);
            Assert.NotEmpty(test);

            //--- Expectations

            //--- Action
            IActivityElement[] array = new IActivityElement[1];
            test.CopyTo(array, 0);

            //--- Verification
            Assert.Equal(item, array[0]);
        }



        [Fact]
        public void ActivityBaseComp_Shall_Constructor_with_parent_and_items()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityInitial(activityStructure);      
            var next1 = new ActivityInitial(activityStructure);
            var next2 = new ActivityInitial(activityStructure);
            var nextElements = new List<IActivityElement> { next1, next2 };
            //--- Expectations

            //--- Action
            var test = new ActivityBaseCompMoq(activityStructure, parent, nextElements);

            //--- Verification
            Assert.NotNull(test.Parent);
            Assert.True(test.HasChild);
            Assert.True(test.HasParent);
            Assert.Equal(2, test.Nexts.Count);

            Assert.Same(parent, test.Parent);
        }

        [Fact]
        public void ActivityBaseComp_Shall_Constructor_without_parent_and_items()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityInitial(activityStructure);
            var next1 = new ActivityInitial(activityStructure);
            var next2 = new ActivityInitial(activityStructure);
            var nextElements = new List<IActivityElement> { next1, next2 };
            //--- Expectations

            //--- Action
            var test = new ActivityBaseCompMoq(activityStructure, null, nextElements);

            //--- Verification
            Assert.Null(test.Parent);
            Assert.True(test.HasChild);
            Assert.False(test.HasParent);
            Assert.Equal(2, test.Nexts.Count);
        }

        [Fact]
        public void ActivityBase_Shall_Constructor()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityBaseMoq(activityStructure);
            //--- Expectations

            //--- Action
            //--- Verification
            Assert.NotNull(test.LoggerTest);
            Assert.Same(activityStructure, test.ActivityStructureTest);
        }

        [Fact]
        public void ActivityBase_Shall_Constructor_withParent()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityBaseMoq(activityStructure);
            //--- Expectations

            //--- Action
            //--- Verification
            Assert.NotNull(test.LoggerTest);
            Assert.Same(activityStructure, test.ActivityStructureTest);
        }

        [Fact]
        public void ActivityBaseComp_Shall_Test_for_empty_Nexts_and_empty_Parents()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            //--- Expectations

            //--- Action
            var test = new ActivityBaseCompMoq(activityStructure);

            //--- Verification
            Assert.Empty(test.Nexts);
            Assert.Empty(test.Parents);
            Assert.Null(test.Parent);
            Assert.False(test.HasChild);
            Assert.False(test.HasParent);
        }

        [Fact]
        public void ActivityBaseComp_Shall_Test_for_2_Nexts_and_empty_Parents()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityBaseCompMoq(activityStructure);
            var initial = new ActivityInitial(activityStructure);
            var initial2 = new ActivityInitial(activityStructure);
            Assert.Equal(3, activityStructure.Count);

            //--- Expectations

            //--- Action
            test.AddNext(initial);
            test.AddNext(initial2);

            Assert.Equal(2, test.Nexts.Count);
            Assert.Same(initial, test.Nexts[0]);
            Assert.Same(initial2, test.Nexts[1]);
            Assert.Empty(test.Parents);
            Assert.Null(test.Parent);
            Assert.True(test.HasChild);
            Assert.False(test.HasParent);

            //--- Verification
        }

        [Fact]
        public void ActivityBaseComp_Shall_Test_for_2_Nexts_and_2_Parents()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var initial = new ActivityInitial(activityStructure);
            var initial2 = new ActivityInitial(activityStructure);
            var test = new ActivityBaseCompMoq(activityStructure, initial);
            Assert.Equal(3, activityStructure.Count);

            //--- Expectations

            //--- Action
            test.AddParent(initial2);
            test.AddNext(initial);
            test.AddNext(initial2);

            Assert.Equal(2, test.Nexts.Count);
            Assert.Equal(2, test.Parents.Count);
            Assert.NotEmpty(test.Parents);

            Assert.Same(initial, test.Nexts[0]);
            Assert.Same(initial2, test.Nexts[1]);

            Assert.Same(initial, test.Parents[0]);
            Assert.Same(initial2, test.Parents[1]);

            Assert.Same(initial, test.Parent);
            Assert.True(test.HasChild);
            Assert.True(test.HasParent);

            //--- Verification
        }

        [Fact]
        public void ActivityBaseComp_Shall_Test_ActivityExit_with_lastRequest_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var initial = new ActivityInitial(activityStructure);
            var initial2 = new ActivityInitial(activityStructure);
            
            var test = new ActivityBaseCompMoq(activityStructure, initial);
            test.AddNext(initial);
            test.AddNext(initial2);

            Assert.Equal(3, activityStructure.Count);
            initial.AddNext(test);
            initial2.AddNext(test);
            test.NextActivity = initial2;

            //--- Expectations

            //--- Action
            var result = test.ActivityExit(true);

            //--- Verification
            Assert.True(result);

        }

        [Fact]
        public void ActivityBaseComp_Shall_Test_ActivityExit_shall_return_true_without_nexts_LastRequest_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var initial = new ActivityInitial(activityStructure);
            var initial2 = new ActivityInitial(activityStructure);

            var test = new ActivityBaseCompMoq(activityStructure, initial);

            Assert.Equal(3, activityStructure.Count);
            initial.AddNext(test);
            initial2.AddNext(test);

            //--- Expectations

            //--- Action
            var result = test.ActivityExit(true);

            //--- Verification
            Assert.True(result);
            Assert.Null(activityStructure.CurrentActivity);
        }
        
        [Fact]
        public void ActivityBaseComp_ActivityExit_shall_return_false_without_nexts_LastRequest_false()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var initial = new ActivityInitial(activityStructure);
            var initial2 = new ActivityInitial(activityStructure);

            var test = new ActivityBaseCompMoq(activityStructure, initial);

            Assert.Equal(3, activityStructure.Count);
            initial.AddNext(test);
            initial2.AddNext(test);
            //--- Expectations

            //--- Action
            var result = test.ActivityExit(false);

            //--- Verification
            Assert.False(result);
            Assert.Null(activityStructure.CurrentActivity);

        }

        [Fact]
        public void ActivityBaseComp_Shall_AddNext()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var initial = new ActivityInitial(activityStructure);
            var initial2 = new ActivityInitial(activityStructure);

            var test = new ActivityBaseCompMoq(activityStructure);

            Assert.Equal(3, activityStructure.Count);

            //--- Expectations

            //--- Action
            test.AddNext(initial);
            test.AddNext(initial2);

            //--- Verification
            Assert.True(test.HasChild);
            Assert.False(test.HasParent);
            Assert.Equal(2, test.Nexts.Count);

            Assert.Same(initial, test.Nexts[0]);
            Assert.Same(initial2, test.Nexts[1]);
            Assert.Null(activityStructure.CurrentActivity);

        }

        [Fact]
        public void ActivityBaseComp_Shall_AddNextRange()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var initial = new ActivityInitial(activityStructure);
            var initial2 = new ActivityInitial(activityStructure);

            var test = new ActivityBaseCompMoq(activityStructure);

            Assert.Equal(3, activityStructure.Count);

            //--- Expectations

            //--- Action
            test.AddNextRange(new[] { initial, initial2 });

            //--- Verification
            Assert.True(test.HasChild);
            Assert.False(test.HasParent);

            Assert.Equal(2, test.Nexts.Count);

            Assert.Same(initial, test.Nexts[0]);
            Assert.Same(initial2, test.Nexts[1]);
            Assert.Null(activityStructure.CurrentActivity);
        }

        [Fact]
        public void ActivityBaseComp_Shall_AddParent()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var initial = new ActivityInitial(activityStructure);
            var initial2 = new ActivityInitial(activityStructure);

            var test = new ActivityBaseCompMoq(activityStructure);

            Assert.Equal(3, activityStructure.Count);

            //--- Expectations

            //--- Action
            test.AddParent(initial);
            test.AddParent(initial2);

            //--- Verification
            Assert.False(test.HasChild);
            Assert.True(test.HasParent);

            Assert.Equal(2, test.Parents.Count);

            Assert.Same(initial, test.Parents[0]);
            Assert.Same(initial2, test.Parents[1]);
            Assert.Null(activityStructure.CurrentActivity);

        }

        [Fact]
        public void ActivityBaseComp_Shall_AddParentRange()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var initial = new ActivityInitial(activityStructure);
            var initial2 = new ActivityInitial(activityStructure);

            var test = new ActivityBaseCompMoq(activityStructure);

            Assert.Equal(3, activityStructure.Count);

            //--- Expectations

            //--- Action
            test.AddParentRange(new[] { initial, initial2 });

            //--- Verification
            Assert.False(test.HasChild);
            Assert.True(test.HasParent);

            Assert.Equal(2, test.Parents.Count);

            Assert.Same(initial, test.Parents[0]);
            Assert.Same(initial2, test.Parents[1]);
            Assert.Null(activityStructure.CurrentActivity);

        }
    }


}
