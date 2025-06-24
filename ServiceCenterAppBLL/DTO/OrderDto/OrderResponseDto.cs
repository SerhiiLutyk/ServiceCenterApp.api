namespace ServiceCenterAppBLL.DTO.OrderDto
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime? OrderDate { get; set; }

        public int ClientId { get; set; }
        public int RepairTypeId { get; set; }
        public int? AdditionalServiceId { get; set; }
    }
}
