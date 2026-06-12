using Microsoft.Kiota.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.LearningSpaces.Mappers;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.LearningSpaces.Repositories 
{
    /// <summary>
    /// Repository implementation for managing Learning Spaces.
    /// </summary>
    /// <remarks>
    /// Uses Kiota-generated API client to perform operations.
    /// </remarks>
    internal class KiotaLearningSpaceRepository : ILearningSpaceRepository
    {
        /// <summary>
        /// Represents the API client used to interact with external services.
        /// </summary>
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="apiClient">The API client used to interact with the remote service.</param>
        public KiotaLearningSpaceRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Laboratory> GetLaboratoryByIdAsync(int laboratoryId)
        {
            if (laboratoryId <= 0)
                throw new ArgumentException("A valid laboratory ID is required.", nameof(laboratoryId));

            // Endpoint call that returns the wrapper
            var response = await _apiClient.Laboratories[laboratoryId].GetAsync();

            // Accessing the Laboratory DTO from the wrapper
            var dto = response?.Laboratory;

            // Null validation
            if (dto == null)
                return null;

            // Convert DTO to entity
            return LaboratoryDtoMapper.ToEntity(dto);
        }

        public async Task<IEnumerable<Laboratory>> ListLaboratoriesAsync()
        {
            // Asynchronously lists all laboratories from the repository through the API.
            var response = await _apiClient.Laboratories.GetAsync();

            var laboratories = response?.Laboratories?.Select(LaboratoryDtoMapper.ToEntity)
                ?? Enumerable.Empty<Laboratory>();
            return laboratories;
        }

        public async Task<Classroom> GetClassroomByIdAsync(int classroomId)
        {
            if (classroomId <= 0)
                throw new ArgumentException("A valid classroom ID is required.", nameof(classroomId));

            // Endpoint call that returns the wrapper
            var response = await _apiClient.Classrooms[classroomId].GetAsync();

            // Accessing the Classroom DTO from the wrapper
            var dto = response?.Classroom;

            // Null validation
            if (dto == null)
                return null;

            // Convert DTO to entity
            return ClassroomDtoMapper.ToEntity(dto);
        }

        public async Task<IEnumerable<Classroom>> ListClassroomsAsync()
        {
            // Asynchronously lists all classrooms from the repository through the API.
            var response = await _apiClient.Classrooms.GetAsync();

            var classrooms = response?.Classrooms?.Select(ClassroomDtoMapper.ToEntity)
                ?? Enumerable.Empty<Classroom>();
            return classrooms;
        }
    }
}