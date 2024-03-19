using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstPartTwoService
{
    public interface ICarApiService
    {
        Task<bool> IsModelAvailableAsync(string model, int year, string brand);
    }
}
