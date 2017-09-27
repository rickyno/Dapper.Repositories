namespace Dapper.Repositories.UnitTests.Mocks
{
    using System.Collections;
    using System.Collections.Generic;

    public class TestEntityComparer : IComparer<TestEntity>, IComparer
    {
        public int Compare(TestEntity left, TestEntity right)
        {
            var leftId = left?.Id;
            var rightId = left?.Id;
            return leftId.GetValueOrDefault().CompareTo(rightId.GetValueOrDefault());
        }

        public int Compare(object left, object right)
        {
            return this.Compare(left as TestEntity, right as TestEntity);
        }
    }
}