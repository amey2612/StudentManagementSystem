using StudentManagement.Core.DTOs;
using StudentManagement.Core.Entities;
using StudentManagement.Data.Repositories;
using StudentManagement.Service.Interfaces;

namespace StudentManagement.Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _repository.GetAllStudentsAsync();
            return students.Select(MapToDto).ToList();
        }

        public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
        {
            var student = await _repository.GetStudentByIdAsync(id);
            return student == null ? null : MapToDto(student);
        }

        public async Task<StudentResponseDto> CreateStudentAsync(CreateStudentDto dto)
        {
            // Validate email uniqueness
            if (await _repository.EmailExistsAsync(dto.Email))
            {
                throw new InvalidOperationException($"Email '{dto.Email}' is already registered.");
            }

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age,
                Course = dto.Course,
                CreatedDate = DateTime.UtcNow
            };

            var createdStudent = await _repository.AddStudentAsync(student);
            return MapToDto(createdStudent);
        }

        public async Task<StudentResponseDto?> UpdateStudentAsync(int id, UpdateStudentDto dto)
        {
            var student = await _repository.GetStudentByIdAsync(id);
            if (student == null) return null;

            // Check email uniqueness if email is being updated
            if (!string.IsNullOrEmpty(dto.Email) && 
                dto.Email != student.Email && 
                await _repository.EmailExistsAsync(dto.Email))
            {
                throw new InvalidOperationException($"Email '{dto.Email}' is already registered.");
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(dto.Name)) student.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Email)) student.Email = dto.Email;
            if (dto.Age.HasValue) student.Age = dto.Age.Value;
            if (!string.IsNullOrEmpty(dto.Course)) student.Course = dto.Course;

            var updatedStudent = await _repository.UpdateStudentAsync(student);
            return MapToDto(updatedStudent);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _repository.DeleteStudentAsync(id);
        }

        private static StudentResponseDto MapToDto(Student student)
        {
            return new StudentResponseDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Age = student.Age,
                Course = student.Course,
                CreatedDate = student.CreatedDate
            };
        }
    }
}
