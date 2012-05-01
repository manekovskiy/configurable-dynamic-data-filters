using System.Web;
using System.Web.UI;

namespace ConfigurableDynamicDataFilters.Helpers
{
	internal static class ControlExtensions
	{
		/// <summary>
		/// Assumes that startingControl is NOT the control you are searching for.    
		/// </summary>
		public static T FindChildControl<T>(this Control startingControl, string id) 
            where T: Control
		{
			Control currentContainer = startingControl;
			Control found = null;

			if (startingControl == startingControl.Page)
			{
				return (T)startingControl.FindControl(id);
			}

			while (found == null && currentContainer != startingControl.Page)
			{
				currentContainer = currentContainer.NamingContainer;
				if (currentContainer == null)
				{
					throw new HttpException(string.Format("Naming container was not found for {0}", startingControl.GetType().Name));
				}
				found = currentContainer.FindControl(id);
			}

			return (T)found;
		}

        public static void RegisterExpandoAttribute(this Control control, string attributeName, string attributeValue, bool encode = false)
        {
            var scriptManager = ScriptManager.GetCurrent(control.Page);
            if (scriptManager != null && scriptManager.IsInAsyncPostBack)
                ScriptManager.RegisterExpandoAttribute(control, control.ClientID, attributeName, attributeValue, encode);
            else
                control.Page.ClientScript.RegisterExpandoAttribute(control.ClientID, attributeName, attributeValue, encode);
        }

	}

}