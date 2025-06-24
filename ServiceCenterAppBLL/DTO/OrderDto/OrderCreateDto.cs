namespace ServiceCenterAppBLL.DTO.OrderDto
{
    public class OrderCreateDto
    {
        public int ClientId { get; set; }
        public int RepairTypeId { get; set; }
        public int? AdditionalServiceId { get; set; }
        public string Description { get; set; }
    }
}
