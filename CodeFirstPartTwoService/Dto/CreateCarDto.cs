namespace CodeFirstPartTwoService.Dto
{
    public class CreateCarDto
    {
        public string Color { get; set; }
        public int Year { get; set; }
        public string ChassisNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public CreateEngineDto Engine { get; set; }
    }
}
