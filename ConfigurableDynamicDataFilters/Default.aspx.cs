using System;
using System.Web.UI;

namespace ConfigurableDynamicDataFilters
{
    public partial class _Default : Page
    {
        protected void btnApply_Click(object sender, EventArgs e)
        {
            gvCustomer.PageIndex = 0;
            gvCustomer.DataBind();
        }
    }
}