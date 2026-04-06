using Xunit;
using Moq;
using StudentManagement.Core.DTOs;
using StudentManagement.Core.Entities;
using StudentManagement.Data.Repositories;
using StudentManagement.Service.Services;

namespace StudentManagement.Tests
{
    public class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _mockRepository;
        private readonly StudentService _studentService;

        public StudentServiceTests()
        {
            _mockRepository = new Mock<IStudentRepository>();
            _studentService = new StudentService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllStudentsAsync_ReturnsEmptyList_WhenNoStudentsExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllStudentsAsync())
                .ReturnsAsync(new List<Student>());

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            Assert.Empty(result);
            _mockRepository.Verify(r => r.GetAllStudentsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllStudentsAsync_ReturnsStudentList()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" },
                new Student { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Age = 21, Course = "IT" }
            };
            _mockRepository.Setup(r => r.GetAllStudentsAsync())
                .ReturnsAsync(students);

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John Doe", result[0].Name);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ReturnsStudent_WhenStudentExists()
        {
            // Arrange
            var student = new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" };
            _mockRepository.Setup(r => r.GetStudentByIdAsync(1))
                .ReturnsAsync(student);

            // Act
            var result = await _studentService.GetStudentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john@example.com", result.Email);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ReturnsNull_WhenStudentDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetStudentByIdAsync(999))
                .ReturnsAsync((Student)null);

            // Act
            var result = await _studentService.GetStudentByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateStudentAsync_CreatesStudent_WhenEmailIsUnique()
        {
            // Arrange
            var dto = new CreateStudentDto { Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" };
            var student = new Student { Id = 1, Name = dto.Name, Email = dto.Email, Age = dto.Age, Course = dto.Course };

            _mockRepository.Setup(r => r.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddStudentAsync(It.IsAny<Student>()))
                .ReturnsAsync(student);

            // Act
            var result = await _studentService.CreateStudentAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john@example.com", result.Email);
            _mockRepository.Verify(r => r.AddStudentAsync(It.IsAny<Student>()), Times.Once);
        }

        [Fact]
        public async Task CreateStudentAsync_ThrowsException_WhenEmailExists()
        {
            // Arrange
            var dto = new CreateStudentDto { Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" };
            _mockRepository.Setup(r => r.EmailExistsAsync(dto.Email))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _studentService.CreateStudentAsync(dto));
            Assert.Contains("Email", exception.Message);
        }

        [Fact]
        public async Task UpdateStudentAsync_UpdatesStudent_WhenStudentExists()
        {
            // Arrange
            var student = new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "CS" };
            var updateDto = new UpdateStudentDto { Name = "Jane Doe" };

            _mockRepository.Setup(r => r.GetStudentByIdAsync(1))
                .ReturnsAsync(student);
            _mockRepository.Setup(r => r.UpdateStudentAsync(It.IsAny<Student>()))
                .ReturnsAsync(student);

            // Act
            var result = await _studentService.UpdateStudentAsync(1, updateDto);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(r => r.UpdateStudentAsync(It.IsAny<Student>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStudentAsync_ReturnsNull_WhenStudentDoesNotExist()
        {
            // Arrange
            var updateDto = new UpdateStudentDto { Name = "Jane Doe" };
            _mockRepository.Setup(r => r.GetStudentByIdAsync(999))
                .ReturnsAsync((Student)null);

            // Act
            var result = await _studentService.UpdateStudentAsync(999, updateDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteStudentAsync_DeletesStudent_WhenStudentExists()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteStudentAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _studentService.DeleteStudentAsync(1);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.DeleteStudentAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteStudentAsync_ReturnsFalse_WhenStudentDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteStudentAsync(999))
                .ReturnsAsync(false);

            // Act
            var result = await _studentService.DeleteStudentAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}