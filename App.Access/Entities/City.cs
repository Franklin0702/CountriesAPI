namespace App.Access.Entities
{
    public class City
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int CountryId { get; set; }
        public Country? Country { get; set; }
    }
}
