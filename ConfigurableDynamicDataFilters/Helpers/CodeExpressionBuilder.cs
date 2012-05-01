using System.CodeDom;
using System.Web.Compilation;
using System.Web.UI;

namespace ConfigurableDynamicDataFilters.Helpers
{
	public class CodeExpressionBuilder : ExpressionBuilder
	{
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			return new CodeSnippetExpression(entry.Expression.Trim());
		}
	}
}