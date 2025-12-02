namespace Sales.API.Dtos
{
    public class InventoryReserveRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}