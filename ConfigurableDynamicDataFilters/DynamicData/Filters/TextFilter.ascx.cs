using System;
using System.Linq;
using System.Web.UI;
using System.Web.DynamicData;
using System.Linq.Expressions;
using System.Diagnostics;

namespace ConfigurableDynamicDataFilters.DynamicData.Filters
{
	public partial class TextFilter : QueryableFilterUserControl
	{
		public override Control FilterControl
		{
			get
			{
				return tbFilter;
			}
		}

		public override IQueryable GetQueryable(IQueryable source)
		{
			if (string.IsNullOrEmpty(tbFilter.Text))
			{
				return source;
			}

			string filterValue = tbFilter.Text;
			ConstantExpression value = Expression.Constant(filterValue);

			ParameterExpression parameter = Expression.Parameter(source.ElementType);
			MemberExpression property = Expression.Property(parameter, Column.Name);
			if (Nullable.GetUnderlyingType(property.Type) != null)
			{
				property = Expression.Property(property, "Value");
			}


			Expression comparison;
			switch (ddlOperator.SelectedValue)
			{
				case "contains":
					comparison = Expression.Call(property, typeof(string).GetMethod("Contains", new [] { typeof(string) }), value);
					break;
				case "start":
					comparison = Expression.Call(property, typeof(string).GetMethod("StartsWith", new [] { typeof(string) }), value);
					break;
				case "end":
					comparison = Expression.Call(property, typeof(string).GetMethod("EndsWith", new [] { typeof(string) }), value);
					break;
				default:
					Debug.Fail("Unexpected operator");
				    return source;
			}

			LambdaExpression lambda = Expression.Lambda(comparison, parameter);

			MethodCallExpression where = Expression.Call(
							typeof(Queryable),
							"Where",
							new[] { source.ElementType },
							source.Expression,
							lambda);

			return source.Provider.CreateQuery(where);
		}

        protected void Page_Init(object sender, EventArgs e)
		{
			tbFilter.ToolTip = Column.Description;
		}

	    protected void FireFilterChanged(object sender, EventArgs e)
	    {
	        OnFilterChanged();
	    }
	}
}