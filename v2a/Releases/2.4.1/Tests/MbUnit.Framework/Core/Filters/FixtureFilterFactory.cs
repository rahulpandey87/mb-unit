using System;

namespace MbUnit.Framework.Tests.Core.Filters
{
	using MbUnit.Core.Filters;
	using MbUnit.Framework;

	public class FixtureFilterFactory
	{
        [Factory]
		public AnyFixtureFilter Any
		{
			get
			{
				return FixtureFilters.Any;
			}
		}

        [Factory]
        public AuthorFixtureFilter Author
        {
			get
			{
				return FixtureFilters.Author("Jonathan de Halleux");
			}
		}

        [Factory]
        public AuthorFixtureFilter MultipleAuthor
        {
            get
            {
                return FixtureFilters.Author("Bob,Mike,Jonathan de Halleux");
            }
        }

        [Factory]
        public CategoryFixtureFilter Category
        {
			get
			{
				return FixtureFilters.Category("FixtureFilters.Tests");
			}
		}

        [Factory]
        public CategoryFixtureFilter MultipleCategory
        {
            get
            {
                return FixtureFilters.Category("A,B,FixtureFilters.Tests");
            }
        }

        [Factory]
        public NamespaceFixtureFilter Namespace
        {
			get
			{
				return FixtureFilters.Namespace("MbUnit.Framework.Tests.Core.Filters");
			}
		}

        [Factory]
        public NamespaceFixtureFilter MultipleNamespace
        {
            get
            {
                return FixtureFilters.Namespace("MbUnit.Foo,MbUnit.Bar,MbUnit.Framework.Tests.Core.Filters");
            }
        }

        [Factory]
        public NotFixtureFilter Not
        {
			get
			{
                return FixtureFilters.Not(FixtureFilters.Author("Sonko"));
			}
		}

        [Factory]
        public TestImportanceFixtureFilter Importance
        {
			get
			{
				return FixtureFilters.Importance(TestImportance.Critical);
			}
		}

        [Factory]
        public TypeFixtureFilter Type
        {
			get
			{
				return FixtureFilters.Type("MbUnit.Framework.Tests.Core.Filters.DummyFixture");
			}
		}

        [Factory]
        public AndFixtureFilter And
        {
			get
			{
				return FixtureFilters.And(this.Author, this.Category);
			}
		}

        [Factory]
        public OrFixtureFilter Or
        {
			get
			{
				return FixtureFilters.Or(this.Author, this.Category);
			}
		}

        [Factory]
        public CurrentFixtureFilter Current
        {
			get
			{
				return FixtureFilters.Current;
			}
		}
	}
}
