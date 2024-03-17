namespace CodeFirstPartTwoService.Dto
{
    public class CreateEngineDto
    {
        public int Year { get; set; }
        public string Brand { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public int EngineTypeId { get; set; }
    }
}
