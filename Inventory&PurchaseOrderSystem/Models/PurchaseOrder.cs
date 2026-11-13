using InventoryApi.Models;

namespace Inventory_PurchaseOrderSystem.Models
{
    public static class PurchaseOrderStatus
    {
        public const string Draft = "Draft";
        public const string Submitted = "Submitted";
        public const string Received = "Received";
        public const string Cancelled = "Cancelled";
    }

    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string PoNumber { get; set; } = "";
        public int SupplierId { get; set; }
        public string Status { get; set; } = PurchaseOrderStatus.Draft;
        public string? Notes { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public Supplier? Supplier { get; set; }
        public User? CreatedByUser { get; set; }
        public List<PurchaseOrderLine> Lines { get; set; } = new();
    }
}
