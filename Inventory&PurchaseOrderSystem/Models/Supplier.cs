namespace Inventory_PurchaseOrderSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ContactEmail { get; set; } = "";
        public string Phone { get; set; } = "";
        public bool IsActive { get; set; } = true;

        public List<PurchaseOrder> PurchaseOrders { get; set; } = new();
    }
}
