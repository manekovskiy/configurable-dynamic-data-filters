using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfigurableDynamicDataFilters.Helpers;

namespace ConfigurableDynamicDataFilters.Controls
{
	public partial class DynamicFilterForm : UserControl
	{
		public DynamicFilterRepeater FilterRepeater;
		public Type FitlerType { get; set; }

		[IDReferenceProperty(typeof(GridView))]
		public string GridViewID { get; set; }

		[IDReferenceProperty(typeof(QueryExtender))]
		public string QueryExtenderID { get; set; }

		private MetaTable MetaTable { get; set; }
		private GridView GridView { get; set; }
		protected QueryExtender GridQueryExtender { get; set; }

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			MetaTable = MetaTable.CreateTable(FitlerType);
			
			GridQueryExtender = this.FindChildControl<QueryExtender>(QueryExtenderID);
			GridView = this.FindChildControl<GridView>(GridViewID);
			GridView.SetMetaTable(MetaTable);

			// Tricky thing to retrieve list of filter columns directly from hidden field
			if (!string.IsNullOrEmpty(Request.Form[FilterColumns.UniqueID]))
				FilterRepeater.FilterColumns.AddRange(Request.Form[FilterColumns.UniqueID].Split(','));

			((IFilterExpressionProvider)FilterRepeater).Initialize(GridQueryExtender.DataSource);
		}

		// SelectMethod for ddlFilterableColumns
		public List<KeyValuePair<string, string>> GetFilterableColumns()
		{
			// adding empty item to list
			var filterableColumns = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>() };
			
			filterableColumns.AddRange(MetaTable.GetFilteredColumns()
				.Select(c => new KeyValuePair<string, string>(c.Name, c.DisplayName))
				.Where(c => !FilterRepeater.FilterColumns.Contains(c.Key)));
			return filterableColumns;
		}

		protected void ddlFilterableColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selectedValue = ddlFilterableColumns.SelectedValue;
			if (!string.IsNullOrEmpty(selectedValue) && !FilterRepeater.FilterColumns.Contains(selectedValue))
			{
				FilterRepeater.FilterColumns.Add(selectedValue);
				ddlFilterableColumns.DataBind();
			}
		}

		protected void lblDisplayName_PreRender(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			DynamicFilter dynamicFilter = (DynamicFilter)label.FindControl("DynamicFilter");
			QueryableFilterUserControl fuc = dynamicFilter.FilterTemplate as QueryableFilterUserControl;
			if (fuc != null && fuc.FilterControl != null)
			{
				label.AssociatedControlID = fuc.FilterControl.GetUniqueIDRelativeTo(label);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			FilterColumns.Value = string.Join(",", FilterRepeater.FilterColumns);
			base.OnPreRender(e);
		}
	}
}