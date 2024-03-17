using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstPartTwoService.Dto;

namespace CodeFirstPartTwoService
{
    public class EngineTypeService(ApplicationContext context) : IEngineTypeService
    {
        public IEnumerable<EngineType> GetAllEngineTypes()
        {
            return context.EngineTypes.ToList();
        }


        public EngineType GetEngineTypeById(int id)
        {
            return context.EngineTypes.Find(id);
        }

        public EngineType AddEngineType(CreateEngineTypeDto engineTypeDto)
        {
            var engineType = new EngineType
            {
                Model = engineTypeDto.Model,
                Name = engineTypeDto.Name
            };
            context.EngineTypes.Add(engineType);
            context.SaveChanges();

            return engineType;
        }

        public void UpdateEngineType(int id, CreateEngineTypeDto engineTypeDto)
        {
            var engineType = context.EngineTypes.Find(id);
            if (engineType != null)
            {
                engineType.Model = engineTypeDto.Model;
                engineType.Name = engineTypeDto.Name;
                context.SaveChanges();
            }
        }

        public void DeleteEngineType(int id)
        {
            var engineType = context.EngineTypes.Find(id);
            if (engineType != null)
            {
                context.EngineTypes.Remove(engineType);
                context.SaveChanges();
            }

            
        }
    }
}
