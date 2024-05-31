using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bill_API.Models
{
    public class CustomersNames
    {
        public int Id { get; set; }
        public List<SelectListItem> CNameList { get; set; }
    }
}
