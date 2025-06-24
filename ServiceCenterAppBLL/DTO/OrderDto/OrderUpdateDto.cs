namespace ServiceCenterAppBLL.DTO.OrderDto
{
    public class OrderUpdateDto
    {
        public int RepairTypeId { get; set; }
        public int? AdditionalServiceId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
