namespace ProductApi.Domain.Common
{
    public class Filter
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Order { get; set; }
        //If not Ascending will be Descending
        public bool Ascending { get; set; }
    }
}
