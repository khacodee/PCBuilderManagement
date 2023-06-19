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

namespace PCBuilder.Services.Service
{
    public interface IUserServices
    {
        Task<ServiceResponse<List<UserDTO>>> GetUsersAsync();
        Task<ServiceResponse<UserDTO>> GetUserByIdAsync(int id);
        Task<ServiceResponse<UserDTO>> CreateUserAsync(UserDTO userDTO);
        Task<ServiceResponse<UserDTO>> UpdateUserAsync(int id, UserDTO userDTO);
        Task<ServiceResponse<bool>> DeleteUserAsync(int id);
    }

    public class UserServices : IUserServices
    {
        private readonly IUserRepository _iUserRepository;
        private readonly IMapper _mapper;

        public UserServices(IUserRepository iUserRepository, IMapper mapper)
        {
            _iUserRepository = iUserRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<UserDTO>>> GetUsersAsync()
        {
            ServiceResponse<List<UserDTO>> _response = new();

            try
            {

                var UsersList = await _iUserRepository.GetAllUsersAsync();

                var UserListDto = new List<UserDTO>();

                foreach (var item in UsersList)
                {
                    UserListDto.Add(_mapper.Map<UserDTO>(item));
                }

                //OR
                //UserListDto.AddRange(from item in CompaniesList select _mapper.Map<UserDTO>(item));
                _response.Success = true;
                _response.Message = "ok";
                _response.Data = UserListDto;

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

        public async Task<ServiceResponse<UserDTO>> GetUserByIdAsync(int id)
        {
            ServiceResponse<UserDTO> response = new ServiceResponse<UserDTO>();

            try
            {
                var user = await _iUserRepository.GetUserByIdAsync(id);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found.";
                    return response;
                }

                var userDto = _mapper.Map<UserDTO>(user);

                response.Data = userDto;
                response.Success = true;
                response.Message = "OK";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }
        public async Task<ServiceResponse<UserDTO>> CreateUserAsync(UserDTO userDTO)
        {
            ServiceResponse<UserDTO> response = new ServiceResponse<UserDTO>();

            try
            {
                var user = _mapper.Map<User>(userDTO);
                var createdUser = await _iUserRepository.CreateUserAsync(user);
                var createdUserDto = _mapper.Map<UserDTO>(createdUser);

                response.Data = createdUserDto;
                response.Success = true;
                response.Message = "User created successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }
        public async Task<ServiceResponse<UserDTO>> UpdateUserAsync(int id, UserDTO userDTO)
        {
            ServiceResponse<UserDTO> response = new ServiceResponse<UserDTO>();

            try
            {
                var existingUser = await _iUserRepository.GetUserByIdAsync(id);

                if (existingUser == null)
                {
                    response.Success = false;
                    response.Message = "User not found.";
                    return response;
                }

                // Update the properties of the existing user
                existingUser.Fullname = userDTO.Fullname;
                existingUser.Email = userDTO.Email;
                existingUser.Phone = userDTO.Phone;
                existingUser.Country = userDTO.Country;
                existingUser.Gender = userDTO.Gender;
                existingUser.Password = userDTO.Password;
                existingUser.Address = userDTO.Address;
                existingUser.Avatar = userDTO.Avatar;

                var updatedUser = await _iUserRepository.UpdateUserAsync(existingUser);
                var updatedUserDto = _mapper.Map<UserDTO>(updatedUser);

                response.Data = updatedUserDto;
                response.Success = true;
                response.Message = "User updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }
        public async Task<ServiceResponse<bool>> DeleteUserAsync(int id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();

            try
            {
                var isDeleted = await _iUserRepository.DeleteUserAsync(id);

                if (!isDeleted)
                {
                    response.Success = false;
                    response.Message = "User not found.";
                    response.Data = false;
                    return response;
                }

                response.Data = true;
                response.Success = true;
                response.Message = "User deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;

        }

    }
}
