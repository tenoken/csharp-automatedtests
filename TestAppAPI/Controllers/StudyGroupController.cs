using Microsoft.AspNetCore.Mvc;
using TestAppAPI.Data.Respositories.Interfaces;
using TestAppAPI.Models;

namespace TestAppAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudyGroupController : ControllerBase//ControllerBase
    {
        private readonly IStudyGroupRepository _studyGroupRepository;

        public StudyGroupController(IStudyGroupRepository studyGroupRepository)
        {
            _studyGroupRepository = studyGroupRepository;
        }

        [HttpPost("/createstudygroup")]
        public async Task<IActionResult> CreateStudyGroup(StudyGroup studyGroup)
        {
            if (!ModelState.IsValid)
            {
                //handle error                
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                var group = await _studyGroupRepository.CreateStudyGroup(studyGroup);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new { message = e.Message });
            }

            return new OkObjectResult(studyGroup);
        }

        [HttpGet("/getstudygroups")]
        public async Task<IActionResult> GetStudyGroups()
        {
            var studyGroups = await _studyGroupRepository.GetStudyGroups();
            return new OkObjectResult(studyGroups);
        }

        [HttpGet("/searchstudygroups/{subject}")]
        public async Task<IActionResult> SearchStudyGroups(string subject)
        {
            var studyGroups = await _studyGroupRepository.SearchStudyGroups(subject);
            return new OkObjectResult(studyGroups);
        }

        [HttpGet("/searchstudygroups")]
        public async Task<IActionResult> SearchStudyGroups([FromQuery] DateTime from, bool orderByMostRecent = false)
        {
            //var coverted = DateTime.ParseExact(dateTime.ToString("dd-MM-yyyy"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var studyGroups = await _studyGroupRepository.SearchStudyGroups(from);
            if (orderByMostRecent)
                return new OkObjectResult(studyGroups.OrderByDescending(c => c.CreationDate));

            return new OkObjectResult(studyGroups.OrderBy(c => c.CreationDate));
        }

        [HttpPost("/joinstudygroup")]
        public async Task<IActionResult> JoinStudyGroup(int studyGroupId, int userId)
        {
            await _studyGroupRepository.JoinStudyGroup(studyGroupId, userId);
            return new OkResult();
        }

        [HttpPost("/leavestudygroup")]
        public async Task<IActionResult> LeaveStudyGroup(int studyGroupId, int userId)
        {
            await _studyGroupRepository.LeaveStudyGroup(studyGroupId, userId);
            return new OkResult();
        }
    }
}