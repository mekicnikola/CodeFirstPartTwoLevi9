using System.Collections.Generic;

namespace CodeFirstPartTwoApp.Models
{
    public class EngineType
    {
        public int EngineTypeId { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Engine> Engines { get; set; }

        public EngineType()
        {
            Engines = new HashSet<Engine>();
        }
    }
}
