using System.ComponentModel.DataAnnotations;

namespace ConfigurableDynamicDataFilters.Model
{
    public class CustomerMetadata
    {
        [Display(Name = "Customer ID")]
        [FilterUIHint("TextFilter")]
        public string CustomerID { get; set; }

        [Display(Name = "Company Name")]
        [FilterUIHint("TextFilter")]
        public string CompanyName { get; set; }
        
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}