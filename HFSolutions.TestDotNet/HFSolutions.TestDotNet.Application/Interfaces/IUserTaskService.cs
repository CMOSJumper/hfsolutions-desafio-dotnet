﻿using HFSolutions.TestDotNet.Application.Dtos.UserTaskDto;
using HFSolutions.TestDotNet.Application.QueryParams;
using HFSolutions.TestDotNet.Application.Responses;

namespace HFSolutions.TestDotNet.Application.Interfaces
{
    public interface IUserTaskService
    {
        Task<UserTaskDto?> CreateAsync(CreateUserTaskDto createUserTaskDto);
        Task<IEnumerable<UserTaskDto>> ReadAllAsync(int userId);
        Task<PagedResponse<UserTaskDto>> ReadAllAsync(int userId, UserTaskQueryParams? userTaskQueryParams = null, PaginationQueryParams? paginationQueryParams = null);
        Task<UserTaskDto?> ReadAsync(int userId, int id);
        Task<UserTaskDto?> UpdateAsync(int id, UpdateUserTaskDto updateUserTaskDto);
        Task<int> DeleteAsync(int userId, int id);
    }
}
