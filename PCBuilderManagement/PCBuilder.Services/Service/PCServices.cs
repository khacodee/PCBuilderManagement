using PCBuilder.Repository.Repository;
using PCBuilder.Repository.Model;
using PCBuilder.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCBuilder.Services.DTO;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel;

namespace PCBuilder.Services.Service
{
    public interface IPCServices
    {
        /// <summary>
        /// Return list of companies which are not marked as deleted.
        /// </summary>
        /// <returns>List Of PcDTO</returns>
        /// 
        Task<ServiceResponse<List<PcDTO>>> GetPCList();
        Task<ServiceResponse<List<PcDTO>>> GetPCListByAdmin();
        Task<ServiceResponse<List<PcDTO>>> GetPCListByCustomer();
        Task<ServiceResponse<PcDTO>> GetPCByID(int ID);
        Task<ServiceResponse<PcDTO>> CreatePC(PcDTO pcDTO);
        Task<ServiceResponse<PcDTO>> UpdatePC(int ID, PcDTO pcDTO);
        Task<ServiceResponse<bool>> DeletePC(int ID);
        Task<ServiceResponse<List<PcDTO>>> SearchPCsByName(String name);
        Task<ServiceResponse<List<PCInformationDTO>>> GetPCComponent();
        Task<ServiceResponse<PCInformationDTO>> GetPCComponentByID(int pcId);
        Task<ServiceResponse<PCInformationDTO>> AddComponentsToPC(int pcId,List<int> componentIds);

    }

    public class PCServices : IPCServices
    {
        private readonly IPCRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPcComponentRepository _pcComponentRepository;
        private readonly IComponentRepository _componentRepository;

        public PCServices(IPCRepository pCRepository, IMapper mapper, IPcComponentRepository pcComponentRepository, IComponentRepository componentRepository )
        {
            this._repository = pCRepository;
            this._mapper = mapper;
            this._componentRepository = componentRepository;
            this._pcComponentRepository = pcComponentRepository;
        }
        public async Task<ServiceResponse<List<PcDTO>>> GetPCList()
        {
            ServiceResponse<List<PcDTO>> _response = new();

            try
            {

                var PCList = await _repository.GetAllPcsAsync();

                var PcListDTO = new List<PcDTO>();

                foreach (var item in PCList)
                {
                    if (item.IsPublic == true)
                    {
                        PcListDTO.Add(_mapper.Map<PcDTO>(item));
                    }
                }

                //OR 
                //PcListDTO.AddRange(from item in PCList select _mapper.Map<PcDTO>(item));
                _response.Success = true;
                _response.Message = "ok";
                _response.Data = PcListDTO;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }
        public async Task<ServiceResponse<List<PcDTO>>> GetPCListByAdmin()
        {
            ServiceResponse<List<PcDTO>> _response = new();

            try
            {

                var PCList = await _repository.GetAllPcsAsync();

                var PcListDTO = new List<PcDTO>();

                foreach (var item in PCList)
                {
                   
                        PcListDTO.Add(_mapper.Map<PcDTO>(item));
                    
                }

                //OR 
                //PcListDTO.AddRange(from item in PCList select _mapper.Map<PcDTO>(item));
                _response.Success = true;
                _response.Message = "ok";
                _response.Data = PcListDTO;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        public async Task<ServiceResponse<List<PcDTO>>> GetPCListByCustomer()
        {
            ServiceResponse<List<PcDTO>> _response = new();

            try
            {
                
                var PCList = await _repository.GetAllPcsAsync();

                var PcListDTO = new List<PcDTO>();

                foreach (var item in PCList)
                {
                    if (item.IsPublic == true) {
                        PcListDTO.Add(_mapper.Map<PcDTO>(item)); 
                    }
                }

                //OR 
                //PcListDTO.AddRange(from item in PCList select _mapper.Map<PcDTO>(item));
                _response.Success = true;
                _response.Message = "ok";
                _response.Data = PcListDTO;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }
        public async Task<ServiceResponse<List<PCInformationDTO>>> GetPCComponent()
        {
            ServiceResponse<List<PCInformationDTO>> response = new ServiceResponse<List<PCInformationDTO>>();

            try
            {
                var pcList = await _repository.GetPcsWithComponentsAsync();

                List<PCInformationDTO> pcDTOList = new List<PCInformationDTO>();

                foreach (var pc in pcList)
                {
                    var pcDTO = _mapper.Map<PCInformationDTO>(pc);
                    pcDTO.Components = pc.PcComponents.Select(pcComp => _mapper.Map<ComponentDTO>(pcComp.Component)).ToList();
                    pcDTOList.Add(pcDTO);
                }
                var pcComponentDTOList = new List<PCInformationDTO>();

                foreach (var pcDTO in pcDTOList)
                {
                    var pcComponentDTO = new PCInformationDTO
                    {
                        Id = pcDTO.Id,
                        Name = pcDTO.Name,
                        Description = pcDTO.Description,
                        Price = pcDTO.Price,
                        Discount = pcDTO.Discount,
                        Components = new List<ComponentDTO>()
                    };

                    foreach (var componentDTO in pcDTO.Components)
                    {
                        var component = new ComponentDTO
                        {
                            Id = componentDTO.Id,
                            Name = componentDTO.Name,
                            Image = componentDTO.Image,
                            Price = componentDTO.Price,
                            Description = componentDTO.Description,
                            CategoryId = componentDTO.CategoryId,
                            BrandId = componentDTO.BrandId

                        };

                        pcComponentDTO.Components.Add(component);
                    }

                    pcComponentDTOList.Add(pcComponentDTO);
                }

                response.Success = true;
                response.Message = "PCs with components retrieved successfully";
                response.Data = pcComponentDTOList;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving PCs with components";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }


        public async Task<ServiceResponse<PCInformationDTO>> GetPCComponentByID(int PcId)
        {
            ServiceResponse<PCInformationDTO> response = new ServiceResponse<PCInformationDTO>();

            try
            {
                var pc = await _repository.GetPcsWithComponentByIdAsync(PcId);

                if (pc == null)
                {
                    response.Success = false;
                    response.Message = "PC not found";
                    return response;
                }

                var pcDTO = _mapper.Map<PCInformationDTO>(pc);
                pcDTO.Components = pc.PcComponents.Select(pcComp => _mapper.Map<ComponentDTO>(pcComp.Component)).ToList();

                
                var pcComponentDTO = new PCInformationDTO
                {
                    Id = pcDTO.Id,
                    Name = pcDTO.Name,
                    Description = pcDTO.Description,
                    Price = pcDTO.Price,
                    Discount = pcDTO.Discount,
                    Image = pcDTO.Image,
                    Components = new List<ComponentDTO>()
                };

                foreach (var componentDTO in pcDTO.Components)
                {
                    var component = new ComponentDTO
                    {
                        Id = componentDTO.Id,
                        Name = componentDTO.Name,
                        Image = componentDTO.Image,
                        Price = componentDTO.Price,
                        Description = componentDTO.Description,
                        CategoryId = componentDTO.CategoryId,
                        BrandId = componentDTO.BrandId
                    };

                    pcComponentDTO.Components.Add(component);
                }     
               response.Success = true;
                response.Message = "PC with components retrieved successfully";
                response.Data = pcComponentDTO;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving PC with components";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }





        public async Task<ServiceResponse<PcDTO>> GetPCByID(int ID)
        {
            ServiceResponse<PcDTO> _response = new();
            try
            {
                var pc = await _repository.GetPcsByIdAsync(ID);



                var pcDTO = _mapper.Map<PcDTO>(pc);

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = pcDTO;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }
        public async Task<ServiceResponse<PcDTO>> CreatePC(PcDTO pcDTO)
        {
            ServiceResponse<PcDTO> _response = new();

            try
            {
                pcDTO.IsPublic = false;
                var pc = _mapper.Map<Pc>(pcDTO);
               

                var createdPc = await _repository.CreatePcAsync(pc);

                var createdPcDTO = _mapper.Map<PcDTO>(createdPc);

                _response.Success = true;
                _response.Message = "PC created successfully";
                _response.Data = createdPcDTO;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }
        public async Task<ServiceResponse<PcDTO>> UpdatePC(int ID, PcDTO pcDTO)
        {
            ServiceResponse<PcDTO> _response = new();

            try
            {
                var pc = await _repository.GetPcsByIdAsync(ID);

                if (pc == null)
                {

                    _response.Success = false;
                    _response.Message = "PC not found";
                    return _response;
                }
                
                _mapper.Map(pcDTO, pc);
                var updatedPc = await _repository.UpdatePcAsync(pc);

                var updatedPcDTO = _mapper.Map<PcDTO>(updatedPc);
                _response.Success = true;
                _response.Message = "PC updated successfully";
                _response.Data = updatedPcDTO;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        public async Task<ServiceResponse<bool>> DeletePC(int ID)
        {
            ServiceResponse<bool> _response = new();

            try
            {
                var pc = await _repository.DeletePcAsync(ID);

                if (pc == null)
                {
                    _response.Success = false;
                    _response.Message = "PC not found";
                    return _response;
                }

               

                _response.Success = true;
                _response.Message = "PC deleted successfully";
                _response.Data = true;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }
        public async Task<ServiceResponse<List<PcDTO>>> SearchPCsByName(string name)
        {
            ServiceResponse<List<PcDTO>> _response = new();

            try
            {
                var searchResult = await _repository.SearchPcsByNameAsync(name);

                var pcListDTO = searchResult.Select(pc => _mapper.Map<PcDTO>(pc)).ToList();

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = pcListDTO;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        public async Task<ServiceResponse<PCInformationDTO>> AddComponentsToPC(int pcId, List<int> componentIds)
        {
            ServiceResponse<PCInformationDTO> response = new ServiceResponse<PCInformationDTO>();

            try
            {
                // Fetch the PC from the database
                var pc = await _repository.GetPcsByIdAsync(pcId);
                if (pc == null)
                {
                    response.Success = false;
                    response.Message = "PC not found";
                    return response;
                }

                // Fetch the existing components from the database based on the IDs
                var listComponents = await _pcComponentRepository.GetComponentsByIdsAsync(componentIds);
                if (pc == null || listComponents == null)
                {
                    response.Success = false;
                    response.Message = "PC or component not found.";
                    return response;
                }
                var newComponents = new List<ComponentDTO>();
                // Create a PC_Component entity for each existing component
                foreach (var component in listComponents)
                {
                    var pcComponent = new PcComponent
                    {
                        ComponentId = component.Id,
                        PcId = pc.Id,
                        Quantity = 1
                    };
                   

                    await _pcComponentRepository.AddPcComponentsAsync(pcComponent);
                    var componentDTO = new ComponentDTO
                    {
                        Id = component.Id,
                        Name = component.Name,
                        Image = component.Image,
                        Price = (decimal)component.Price,
                        Description = component.Description,
                        BrandId = component.BrandId,
                        CategoryId = component.CategoryId
                    };

                    newComponents.Add(componentDTO);
                }

                // Add the PC_Component entities to the database
                // Update the response with success message and PC DTO
                var pcDTO = _mapper.Map<PCInformationDTO>(pc);
                pcDTO.Components = newComponents;
                response.Success = true;
                response.Message = "Components added to PC successfully";
                response.Data = pcDTO;

                
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error adding components to PC";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
