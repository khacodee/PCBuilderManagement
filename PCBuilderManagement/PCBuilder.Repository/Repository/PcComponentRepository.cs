using Microsoft.EntityFrameworkCore;
using PCBuilder.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBuilder.Repository.Repository
{
    public interface IPcComponentRepository
    {
        Task AddPcComponentsAsync(PcComponent pcComponents);
        Task<List<Component>> GetComponentsByIdsAsync(List<int> componentIds);
    }

    public class PcComponentRepository : IPcComponentRepository
    {
        private readonly PcBuildingContext _dataContext;

        public PcComponentRepository(PcBuildingContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddPcComponentsAsync(PcComponent pcComponents)
        {
            await _dataContext.PcComponents.AddRangeAsync(pcComponents);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<Component>> GetComponentsByIdsAsync(List<int> componentIds)
        {
            return await _dataContext.Components
                .Where(c => componentIds.Contains(c.Id))
                .ToListAsync();
        }
    }
}
