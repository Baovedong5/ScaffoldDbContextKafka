namespace ProductServices.DTOs
{
    public class UpdateQuantityDto
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public bool Increase { get; set; }
    }
}
