using Microsoft.EntityFrameworkCore;
using PCBuilder.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBuilder.Repository.Repository
{
    public interface IComponentRepository
    {
        Task<ICollection<Component>> GetAllComponentsAsync();
        Task<Component> GetComponentByIdAsync(int id);
        Task<Component> CreateComponentAsync(Component component);
        Task<Component> UpdateComponentAsync(Component component);
        Task<bool> DeleteComponentAsync(int id);
        Task<List<Component>> GetComponentsByIdsAsync(List<int> componentIds);
        Task<ICollection<Component>> SearchComponentsByNameAsync(string name);
    }

    public class ComponentRepository : IComponentRepository
    {
        private readonly PcBuildingContext _dataContext;

        public ComponentRepository(PcBuildingContext context)
        {
            _dataContext = context;
        }

        public async Task<ICollection<Component>> GetAllComponentsAsync()
        {
            return await _dataContext.Components.ToListAsync();
        }

        public async Task<Component> GetComponentByIdAsync(int id)
        {
            return await _dataContext.Components.FindAsync(id);
        }

        public async Task<List<Component>> GetComponentsByIdsAsync(List<int> componentIds)
        {
            return await _dataContext.Components
                .Where(c => componentIds.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<Component> CreateComponentAsync(Component component)
        {
            _dataContext.Components.Add(component);
            await _dataContext.SaveChangesAsync();
            return component;
        }

        public async Task<Component> UpdateComponentAsync(Component component)
        {
            _dataContext.Components.Update(component);
            await _dataContext.SaveChangesAsync();
            return component;
        }

        public async Task<bool> DeleteComponentAsync(int id)
        {
            var component = await _dataContext.Components.FindAsync(id);
            if (component == null)
            {
                return false;
            }

            _dataContext.Components.Remove(component);
            await _dataContext.SaveChangesAsync();
            return true;
        }
        public async Task<ICollection<Component>> SearchComponentsByNameAsync(string name)
        {
            return await _dataContext.Components
                .Where(C => C.Name.Contains(name))
                .ToListAsync();
        }
    }

}
