namespace CodeFirstPartTwoApp.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public string ChassisNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        public Engine Engine { get; set; }
    }
}
