using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.CollisionDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.LearningSpaces.Services;

public class LearningSpaceCollisionServiceTests
{
    private static LearningSpaceColor Color() => LearningSpaceColor.Create("#FFFFFF");
    private static LearningSpaceTexture Texture() => LearningSpaceTexture.Create("WallTex.png");
    private static LearningSpaceDimensions Dim(float w = 2, float l = 2, float h = 2) => LearningSpaceDimensions.Create(w, l, h);
    private static LearningSpaceCoordinates Coords(float x = 0, float y = 0, float z = 0) => LearningSpaceCoordinates.Create(x, y, z);

    private static Laboratory Lab(int id, int? buildingId = 1, int? floor = 1, string? roomId = null, float x = 0, float y = 0, float z = 0)
        => new Laboratory(id, buildingId, floor, roomId ?? $"LAB-{id}", Color(), Texture(), Dim(), Coords(x, y, z));

    private static Classroom Class(int id, int? buildingId = 1, int? floor = 1, string? roomId = null, float x = 0, float y = 0, float z = 0)
        => new Classroom(id, buildingId, floor, roomId ?? $"CLASS-{id}", Color(), Texture(), Dim(), Coords(x, y, z));

    [Fact]
    public async Task DetectCollisionAsync_CandidateIsNull_ThrowsArgumentNullException_AndDoesNotCallRepo()
    {
        // Arrange
        var repo = new FakeLearningSpaceRepository();
        var detector = new FakeLearningSpaceDetector();
        var sut = new LearningSpaceCollisionService(repo, detector);

        // Act
        Func<Task> act = () => sut.DetectCollisionAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("candidate");
        repo.LabsCalls.Should().Be(0);
        repo.ClassesCalls.Should().Be(0);
        detector.Calls.Should().Be(0);
    }

    [Fact]
    public async Task DetectCollisionAsync_WhenRepositoryReturnsNoLabsOrClasses_ReturnsFalse_AndSkipsDetector()
    {
        // Arrange
        var candidate = Lab(10);
        var repo = new FakeLearningSpaceRepository();
        var detector = new FakeLearningSpaceDetector();
        var sut = new LearningSpaceCollisionService(repo, detector);

        // Act
        var result = await sut.DetectCollisionAsync(candidate);

        // Assert
        result.Should().BeFalse();
        repo.LabsCalls.Should().Be(1);
        repo.ClassesCalls.Should().Be(1);
        detector.Calls.Should().Be(0);
    }

    [Fact]
    public async Task DetectCollisionAsync_WhenOnlyCandidateExists_ReturnsFalse_AndSkipsDetector()
    {
        // Arrange
        var candidate = Lab(1);
        var repo = new FakeLearningSpaceRepository(labs: new[] { candidate });
        var detector = new FakeLearningSpaceDetector();
        var sut = new LearningSpaceCollisionService(repo, detector);

        // Act
        var result = await sut.DetectCollisionAsync(candidate);

        // Assert
        result.Should().BeFalse();
        detector.Calls.Should().Be(0);
    }

    [Fact]
    public async Task DetectCollisionAsync_WhenDetectorReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var candidate = Lab(1);
        var otherLab = Lab(2);
        var otherClass = Class(3);
        var repo = new FakeLearningSpaceRepository(labs: new[] { candidate, otherLab }, classes: new[] { otherClass });
        var detector = new FakeLearningSpaceDetector { NextResult = true };
        var sut = new LearningSpaceCollisionService(repo, detector);

        // Act
        var result = await sut.DetectCollisionAsync(candidate);

        // Assert
        result.Should().BeTrue();
        detector.Calls.Should().Be(1);
        detector.LastCandidate.Should().Be(candidate);
        detector.LastCompared.Select(ls => ls.Id).Should().BeEquivalentTo(new[] { otherLab.Id, otherClass.Id });
    }

    [Fact]
    public async Task DetectCollisionAsync_WhenDetectorReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var candidate = Lab(1);
        var otherLab = Lab(2);
        var otherClass = Class(3);
        var repo = new FakeLearningSpaceRepository(labs: new[] { candidate, otherLab }, classes: new[] { otherClass });
        var detector = new FakeLearningSpaceDetector { NextResult = false };
        var sut = new LearningSpaceCollisionService(repo, detector);

        // Act
        var result = await sut.DetectCollisionAsync(candidate);

        // Assert
        result.Should().BeFalse();
        detector.Calls.Should().Be(1);
    }

    [Fact]
    public async Task DetectCollisionAsync_ExcludesSelf_ByReference_AndById()
    {
        // Arrange
        var candidate = Lab(10);
        var sameIdDifferentInstance = Lab(10);
        var neighborLab = Lab(11);
        var repo = new FakeLearningSpaceRepository(labs: new[] { candidate, sameIdDifferentInstance, neighborLab });
        var detector = new FakeLearningSpaceDetector { NextResult = true };
        var sut = new LearningSpaceCollisionService(repo, detector);

        // Act
        var result = await sut.DetectCollisionAsync(candidate);

        // Assert
        result.Should().BeTrue();
        detector.LastCompared.Should().ContainSingle().Which.Should().Be(neighborLab);
    }

    [Fact]
    public async Task DetectCollisionAsync_UnpersistedCandidate_IdZero_ShouldExcludeOnlyByReference()
    {
        // Arrange (Id = 0 candidate)
        var candidate = new Laboratory(buildingId: 1, floorLevel: 1, roomId: "LAB-X", Color(), Texture(), Dim(), Coords());
        var differentInstanceSameIdZero = new Laboratory(buildingId: 1, floorLevel: 1, roomId: "LAB-Y", Color(), Texture(), Dim(), Coords());
        var repo = new FakeLearningSpaceRepository(labs: new[] { candidate, differentInstanceSameIdZero });
        var detector = new FakeLearningSpaceDetector { NextResult = false };
        var sut = new LearningSpaceCollisionService(repo, detector);

        // Act
        var result = await sut.DetectCollisionAsync(candidate);

        // Assert
        result.Should().BeFalse();
        detector.LastCompared.Should().ContainSingle().Which.Should().Be(differentInstanceSameIdZero);
    }

    [Fact]
    public async Task DetectCollisionAsync_DoesNotFilterByBuildingOrFloor()
    {
        // Arrange
        var candidate = Lab(1, buildingId: 100, floor: 5);
        var otherSameBuildingDifferentFloor = Lab(2, buildingId: 100, floor: 6);
        var otherDifferentBuilding = Class(3, buildingId: 200, floor: 1);
        var repo = new FakeLearningSpaceRepository(labs: new[] { candidate, otherSameBuildingDifferentFloor }, classes: new[] { otherDifferentBuilding });
        var detector = new FakeLearningSpaceDetector { NextResult = false };
        var sut = new LearningSpaceCollisionService(repo, detector);

        // Act
        var _ = await sut.DetectCollisionAsync(candidate);

        // Assert
        detector.LastCompared.Select(ls => ls.Id).Should().BeEquivalentTo(new[] { otherSameBuildingDifferentFloor.Id, otherDifferentBuilding.Id },
            "service merges all laboratories and classrooms without scoping by building/floor");
    }

    [Fact]
    public async Task DetectCollisionAsync_WhenAfterExclusion_NoRemainingItems_ReturnsFalse()
    {
        // Arrange
        var candidate = Lab(50);
        var sameIdDifferentRef = Lab(50);
        var repo = new FakeLearningSpaceRepository(labs: new[] { candidate, sameIdDifferentRef });
        var detector = new FakeLearningSpaceDetector { NextResult = true };
        var sut = new LearningSpaceCollisionService(repo, detector);

        // Act
        var result = await sut.DetectCollisionAsync(candidate);

        // Assert
        result.Should().BeFalse();
        detector.Calls.Should().Be(0);
    }

    private sealed class FakeLearningSpaceRepository : ILearningSpaceRepository
    {
        private readonly List<Laboratory> _labs;
        private readonly List<Classroom> _classes;

        public int LabsCalls { get; private set; }
        public int ClassesCalls { get; private set; }

        public FakeLearningSpaceRepository(IEnumerable<Laboratory>? labs = null, IEnumerable<Classroom>? classes = null)
        {
            _labs = labs?.ToList() ?? new List<Laboratory>();
            _classes = classes?.ToList() ?? new List<Classroom>();
        }

        public Task<IEnumerable<Laboratory>> ListLaboratoriesAsync()
        {
            LabsCalls++;
            return Task.FromResult<IEnumerable<Laboratory>>(_labs);
        }

        public Task<IEnumerable<Classroom>> ListClassroomsAsync()
        {
            ClassesCalls++;
            return Task.FromResult<IEnumerable<Classroom>>(_classes);
        }

        public Task<(IReadOnlyList<Laboratory> Laboratories, int TotalCount)> ListLaboratoriesPagedAsync(int pageNumber, int pageSize, string keyword)
        {
            var paged = _labs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return Task.FromResult<(IReadOnlyList<Laboratory>, int)>((paged, _labs.Count));
        }

        public Task<(IReadOnlyList<Classroom> Classrooms, int TotalCount)> ListClassroomsPagedAsync(int pageNumber, int pageSize, string keyword)
        {
            var paged = _classes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return Task.FromResult<(IReadOnlyList<Classroom>, int)>((paged, _classes.Count));
        }

        // Unused interface members for these tests
        public Task AddLearningSpaceAsync(LearningSpace learningSpace) => throw new NotImplementedException();
        public Task DeleteLearningSpaceAsync(int learningSpaceId) => throw new NotImplementedException();
        public Task<IEnumerable<LearningSpace>> ListLearningSpacesByBuildingIdAsync(int buildingId) => throw new NotImplementedException();
        public Task<Laboratory?> GetLaboratoryByIdAsync(int laboratoryId) => throw new NotImplementedException();
        public Task UpdateLaboratoryAsync(Laboratory laboratory) => throw new NotImplementedException();
        public Task<Classroom?> GetClassroomByIdAsync(int classroomId) => throw new NotImplementedException();
        public Task<IEnumerable<LearningSpaceTexture>> ListLearningSpaceTexturesAsync() => throw new NotImplementedException();
        public Task<LearningSpace?> GetLearningSpaceByIdAsync(int learningSpaceId) => throw new NotImplementedException();
        public Task UpdateClassroomAsync(Classroom classroom) => throw new NotImplementedException();

    }

    private sealed class FakeLearningSpaceDetector : ILearningSpaceCollisionDetector
    {
        public int Calls { get; private set; }
        public bool NextResult { get; set; }
        public LearningSpace? LastCandidate { get; private set; }
        public IReadOnlyList<LearningSpace> LastCompared { get; private set; } = Array.Empty<LearningSpace>();

        public bool DetectCollision(LearningSpace candidate, IEnumerable<LearningSpace> existing)
        {
            Calls++;
            LastCandidate = candidate;
            LastCompared = existing.ToList();
            return NextResult;
        }

        public bool HasCollision(LearningSpace a, LearningSpace b) => throw new NotImplementedException();
    }
}
