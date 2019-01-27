

namespace Sales.Backend.Models
{
    using Sales.Common.Models;
    using System.Web;
    public class ProducView : Product
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}