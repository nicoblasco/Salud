using TemplateNBL.Models;

namespace TemplateNBL.ViewModel
{
    public class ProductStockViewModel
    {
        public int ProductId { get; set; }
        public int Stock { get; set; }
        public virtual Product Product { get; set; }
    }
}