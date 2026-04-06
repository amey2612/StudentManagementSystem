using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.DTOs;
using StudentManagement.Core.Models;
using StudentManagement.Service.Interfaces;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>List of all students</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<StudentResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<StudentResponseDto>>>> GetAllStudents()
        {
            _logger.LogInformation("Fetching all students");
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(ApiResponse<List<StudentResponseDto>>.SuccessResponse(students, "Students retrieved successfully."));
        }

        /// <summary>
        /// Get student by ID
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Student details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> GetStudentById(int id)
        {
            _logger.LogInformation("Fetching student with ID: {StudentId}", id);
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
            {
                _logger.LogWarning("Student with ID: {StudentId} not found", id);
                return NotFound(ApiResponse.ErrorResponse($"Student with ID {id} not found."));
            }

            return Ok(ApiResponse<StudentResponseDto>.SuccessResponse(student, "Student retrieved successfully."));
        }

        /// <summary>
        /// Create a new student
        /// </summary>
        /// <param name="dto">Student creation details</param>
        /// <returns>Created student</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StudentResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> CreateStudent([FromBody] CreateStudentDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(ApiResponse.ErrorResponse("Validation failed.", errors));
            }

            try
            {
                _logger.LogInformation("Creating new student with email: {Email}", dto.Email);
                var createdStudent = await _studentService.CreateStudentAsync(dto);
                return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, 
                    ApiResponse<StudentResponseDto>.SuccessResponse(createdStudent, "Student created successfully."));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Student creation failed: {Message}", ex.Message);
                return BadRequest(ApiResponse.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Update student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="dto">Student update details</param>
        /// <returns>Updated student</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> UpdateStudent(int id, [FromBody] UpdateStudentDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(ApiResponse.ErrorResponse("Validation failed.", errors));
            }

            try
            {
                _logger.LogInformation("Updating student with ID: {StudentId}", id);
                var updatedStudent = await _studentService.UpdateStudentAsync(id, dto);

                if (updatedStudent == null)
                {
                    _logger.LogWarning("Student with ID: {StudentId} not found for update", id);
                    return NotFound(ApiResponse.ErrorResponse($"Student with ID {id} not found."));
                }

                return Ok(ApiResponse<StudentResponseDto>.SuccessResponse(updatedStudent, "Student updated successfully."));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Student update failed: {Message}", ex.Message);
                return BadRequest(ApiResponse.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Delete student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Success or error message</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteStudent(int id)
        {
            _logger.LogInformation("Deleting student with ID: {StudentId}", id);
            var result = await _studentService.DeleteStudentAsync(id);

            if (!result)
            {
                _logger.LogWarning("Student with ID: {StudentId} not found for deletion", id);
                return NotFound(ApiResponse.ErrorResponse($"Student with ID {id} not found."));
            }

            return Ok(ApiResponse.SuccessResponse("Student deleted successfully."));
        }
    }
}
