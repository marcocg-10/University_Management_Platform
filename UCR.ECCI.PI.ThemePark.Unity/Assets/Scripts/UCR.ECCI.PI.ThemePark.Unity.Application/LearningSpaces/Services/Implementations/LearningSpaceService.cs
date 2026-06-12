using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Repositories;
using Zenject;

namespace UCR.ECCI.PI.ThemePark.Unity.Application.LearningSpaces.Services.Implementations
{
    public class LearningSpaceService : ILearningSpaceService
    {
        private readonly ILearningSpaceRepository _learningSpaceRepository;

        [Inject]
        public LearningSpaceService(ILearningSpaceRepository learningSpaceRepository)
        {
            _learningSpaceRepository = learningSpaceRepository;
        }

        public async Task<Laboratory?> GetLaboratoryByIdAsync(int laboratoryId)
        {
            return await _learningSpaceRepository.GetLaboratoryByIdAsync(laboratoryId);
        }

        public async Task<IEnumerable<Laboratory>> ListLaboratoriesAsync()
        {
            return await _learningSpaceRepository.ListLaboratoriesAsync();
        }

        public async Task<IEnumerable<Laboratory>> ListLaboratoriesByBuildingAsync(int buildingId)
        {
            var all = await ListLaboratoriesAsync();
            return all.Where(l => l.BuildingId == buildingId);
        }


        public async Task<IEnumerable<Classroom>> ListClassroomsAsync()
        {
            return await _learningSpaceRepository.ListClassroomsAsync();
        }
        public async Task<Classroom?> GetClassroomByIdAsync(int classroomId)
        {
            return await _learningSpaceRepository.GetClassroomByIdAsync(classroomId);
        }
        public async Task<IEnumerable<Classroom>> ListClassroomsByBuildingAsync(int buildingId)
        {
            var all = await ListClassroomsAsync();
            return all.Where(l => l.BuildingId == buildingId);
        }

    }
}
