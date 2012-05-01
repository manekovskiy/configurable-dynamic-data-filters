using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace ConfigurableDynamicDataFilters.Controls
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public class DynamicFilterRepeater : Control, IFilterExpressionProvider
	{
	    private static readonly MethodInfo DynamicFilterEnsureInit;

		private bool loadCompleted;
		private bool initComleted;
		private readonly List<string> addedOnInitCompleteFilters;
		int itemIndex;

		private readonly List<IFilterExpressionProvider> filters = new List<IFilterExpressionProvider>();
		private IQueryableDataSource dataSource;
		
        public List<string> FilterColumns
        {
            get; private set;
        }

		private MetaTable metaTable;
		public MetaTable MetaTable
		{
			get
			{
				return metaTable ?? (metaTable = dataSource.GetMetaTable());
			}
		}

		private const string DynamicFilterContainerIdKey = "DynamicFilterRepeater_DynamicFilterContainerId";
		[Category("Behavior")]
		[DefaultValue("DynamicFilter")]
		[Themeable(false)]
		[IDReferenceProperty(typeof(QueryableFilterUserControl))]
		public string DynamicFilterContainerId
		{
			get
			{
				string id = ViewState[DynamicFilterContainerIdKey] as string;
				return string.IsNullOrEmpty(id) ? "DynamicFilter" : id;
			}
			set
			{
				ViewState[DynamicFilterContainerIdKey] = value;
			}
		}

		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(INamingContainer))]
		public virtual ITemplate ItemTemplate { get; set; }

        static DynamicFilterRepeater()
        {
            DynamicFilterEnsureInit = typeof(DynamicFilter).GetMethod("EnsureInit", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public DynamicFilterRepeater()
        {
            FilterColumns = new List<string>();
            addedOnInitCompleteFilters = new List<string>();
        }

		#region IFilterExpressionProvider implementation

		IQueryable IFilterExpressionProvider.GetQueryable(IQueryable source)
		{
			return filters.Aggregate(source, (current, filter) => filter.GetQueryable(current));
		}

		void IFilterExpressionProvider.Initialize(IQueryableDataSource queryableDataSource)
		{
			Contract.Assert(queryableDataSource is IDynamicDataSource);
			Contract.Assert(queryableDataSource != null);

			if (ItemTemplate == null)
				return;
			dataSource = queryableDataSource;

			Page.InitComplete += InitComplete;
			Page.LoadComplete += LoadCompeted;
		}

		#endregion

        private void InitComplete(object sender, EventArgs e)
        {
            if (initComleted)
                return;

            addedOnInitCompleteFilters.AddRange(FilterColumns);
            AddFilterControls(addedOnInitCompleteFilters);

            initComleted = true;
        }

		private void LoadCompeted(object sender, EventArgs eventArgs)
		{
			if (loadCompleted)
				return;

			AddFilterControls(FilterColumns.Except(addedOnInitCompleteFilters));

			loadCompleted = true;
		}

		private void AddFilterControls(IEnumerable<string> columnNames)
		{
			foreach (MetaColumn column in GetFilteredMetaColumns(columnNames))
			{
				DynamicFilterRepeaterItem item = new DynamicFilterRepeaterItem { DataItemIndex = itemIndex, DisplayIndex = itemIndex };
				itemIndex++;
				ItemTemplate.InstantiateIn(item);
				Controls.Add(item);

				DynamicFilter filter = item.FindControl(DynamicFilterContainerId) as DynamicFilter;
				if (filter == null)
				{
					throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
						"DynamicFilterRepeater '{0}' does not contain a control of type '{1}' with ID '{2}' in its item templates",
						ID,
						typeof(DynamicFilter).FullName,
						DynamicFilterContainerId));
				}
				filter.DataField = column.Name;
				
				item.DataItem = column;
				item.DataBind();
				item.DataItem = null;

				filters.Add(filter);
			}

		    filters.ForEach(f => DynamicFilterEnsureInit.Invoke(f, new object[] { dataSource }));
		}

		private IEnumerable<MetaColumn> GetFilteredMetaColumns(IEnumerable<string> filterColumns)
		{
			return MetaTable.GetFilteredColumns()
				.Where(column => filterColumns.Contains(column.Name))
				.OrderBy(column => column.Name);
		}

        private class DynamicFilterRepeaterItem : Control, IDataItemContainer
		{
			public object DataItem { get; internal set; }
			public int DataItemIndex { get; internal set; }
			public int DisplayIndex { get; internal set; }
		}
	}
}