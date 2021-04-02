namespace TemplateNBL.ViewModel
{
    public class OrderProductViewModel
    {
        public int ProductId { get; set; }
        public string Product { get; set; }
        public string ProductType { get; set; }
        public int? Stock { get; set; }
        public int Quantity { get; set; }//Pedidos
        public int QuantityDelivered { get; set; }//Entregados 
        public bool? OutStock { get; set; }

    }
}