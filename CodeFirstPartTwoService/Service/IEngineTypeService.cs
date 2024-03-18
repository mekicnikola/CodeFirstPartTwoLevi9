

using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoService.Dto;

namespace CodeFirstPartTwoService.Service
{
    public interface IEngineTypeService
    {
        IEnumerable<EngineType> GetAllEngineTypes();
        EngineType GetEngineTypeById(int id);
        EngineType AddEngineType(CreateEngineTypeDto engineTypeDto);
        void UpdateEngineType(int id, CreateEngineTypeDto engineTypeDto);
        void DeleteEngineType(int id);
    }
}
