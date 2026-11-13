namespace Inventory_PurchaseOrderSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }
        public bool IsActive { get; set; } = true;

        public List<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new();
    }
}
