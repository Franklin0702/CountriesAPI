namespace App.Access.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public required string CountryName { get; set; }
        public required string CountryCode { get; set; }
        public string? Coordinates { get; set; }
        public IList<City>? Cities { get; set; }
    }
}
